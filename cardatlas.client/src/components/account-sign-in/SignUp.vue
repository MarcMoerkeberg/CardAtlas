<script setup lang="ts">
  import { useValidationRules } from '@/composeables/useValidationRules'
import { useUserStore } from '@/stores/userStore'
import { User } from '@/types/User'

  const emit = defineEmits(['showSignIn', 'signUpSuccess'])

  const email = ref('')
  const name = ref('')
  const password = ref('')
  const confirmedPassword = ref('')
  const loading = ref(false)
  const formRef = ref<HTMLFormElement | null>(null)
  const errorMessage = ref('')
  const userStore = useUserStore()
  const user = ref<User>({ 
    Email: email.value, 
    FirstName: name.value, 
    Password: password.value 
  })

  const { required, minLength, equalTo, validEmailFormat } = useValidationRules()

  const form = computed(() => !!name.value && !!email.value && !!password.value && !!confirmedPassword.value)

  const createAccount = async () => {
    const rulesAreValid = formRef.value?.checkValidity()

    if (rulesAreValid) {
      loading.value = true
      errorMessage.value = ''

      const signUpResponse = await userStore.signUp(user.value)
      if (signUpResponse.success) {
        emit('signUpSuccess')
      } else {
        errorMessage.value = 'signUpResponse.message'
      }

      loading.value = false
    }
  }
</script>

<template>
  <v-card class="mx-auto px-6 py-8" max-width="344">
    <v-card-title>Create account</v-card-title>
    <v-card-subtitle>
      Already have an account?
      <a href="#" @click.prevent="$emit('showSignIn')">Sign in</a>
    </v-card-subtitle>
    <br>

    <v-alert
      v-if="errorMessage"
      class="mb-5"
      color="error"
      icon="mdi-alert-outline"
      :text="errorMessage"
    />

    <v-form ref="formRef" v-model="form" @submit.prevent="createAccount">
      <v-text-field
        v-model="name"
        label="Full name"
        placeholder="John Doe"
        :readonly="loading"
        :rules="[required]"
        type="text"
      />

      <v-text-field
        v-model="email"
        label="Email"
        :readonly="loading"
        :rules="[required, validEmailFormat]"
        type="email"
        validate-on="submit"
      />

      <v-text-field
        v-model="password"
        label="Password"
        placeholder="Enter your password"
        :readonly="loading"
        :rules="[required, minLength(6)]"
        type="password"
        validate-on="blur"
      />

      <v-text-field
        v-model="confirmedPassword"
        label="Confirm password"
        :readonly="loading"
        :rules="[equalTo(password, 'Passwords do not match.')]"
        type="password"
        validate-on="blur"
      />

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
        Create account
      </v-btn>
    </v-form>
  </v-card>
</template>
