<script setup lang="ts">
  import { useValidationRules } from '@/composeables/useValidationRules'
  import { useUserStore } from '@/stores/userStore'

  const emit = defineEmits(['showSignUp', 'signInSuccess', 'showResetPassword'])

  const email = ref('')
  const password = ref('')
  const loading = ref(false)
  const emailInputRef = ref<HTMLInputElement | null>(null)
  const errorMessage = ref('')
  const userStore = useUserStore()

  const { validEmailFormat } = useValidationRules()

  const form = computed(() => !!email.value && !!password.value)

  const signIn = async () => {
    errorMessage.value = ''
    if (!validEmailFormat(email.value)) {
      emailInputRef.value?.focus()
      return
    }

    loading.value = true

    const loginResponse = await userStore.signIn(email.value, password.value)
    if (loginResponse.success) {
      emit('signInSuccess')
    } else {
      errorMessage.value = loginResponse.message ?? 'An error occurred while signing in. Please try again later.'
      emailInputRef.value?.focus()
    }

    loading.value = false
  }
</script>

<template>
  <v-card class="mx-auto px-6 py-8" max-width="344">
    <v-card-title>Sign in</v-card-title>
    <v-card-subtitle>
      Don't have an account?
      <a href="#" @click.prevent="$emit('showSignUp')">Sign up</a>
    </v-card-subtitle>
    <br>

    <v-form v-model="form" @submit.prevent="signIn">
      <v-text-field
        ref="emailInputRef"
        v-model="email"
        :error-messages="errorMessage"
        label="Email"
        prepend-inner-icon="mdi-account"
        :readonly="loading"
        :rules="[validEmailFormat]"
        type="email"
        validate-on="submit"
      />

      <v-text-field
        v-model="password"
        :error-messages="errorMessage"
        label="Password"
        placeholder="Enter your password"
        prepend-inner-icon="mdi-lock"
        :readonly="loading"
        type="password"
      />

      <v-card-subtitle>
        <a href="#" @click.prevent="$emit('showResetPassword')">Forgot password?</a>
      </v-card-subtitle>

      <br>

      <v-btn
        block
        color="success"
        :disabled="!form"
        :loading="loading"
        size="large"
        type="submit"
        variant="elevated"
      >
        Sign In
      </v-btn>
    </v-form>
  </v-card>
</template>
