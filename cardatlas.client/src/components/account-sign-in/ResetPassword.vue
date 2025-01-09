<script setup lang="ts">
  import { useValidationRules } from '@/composeables/useValidationRules'
import { useUserStore } from '@/stores/userStore'

  defineEmits(['showSignIn'])

  const email = ref('')
  const loading = ref(false)
  const formRef = ref<HTMLFormElement | null>(null)
  const emailInputRef = ref<HTMLInputElement | null>(null)
  const errorMessage = ref('')
  const successfullyResetPassword = ref(false)
  const userStore = useUserStore()

  const { required, validEmailFormat } = useValidationRules()
  const form = computed(() => !!email.value)

  const resetPassword = async () => {
    const rulesAreValid = formRef.value?.checkValidity()
    if (!rulesAreValid) {
      emailInputRef.value?.focus()
      return
    }

    loading.value = true
    errorMessage.value = ''

    const passwordResetResponse = await userStore.resetPassword(email.value)
    loading.value = false

    if (passwordResetResponse.success) {
      successfullyResetPassword.value = true
      window.alert('Password reset email sent. Please check your inbox.')
    } else {
      errorMessage.value = 'An error occurred. Please try again.'
    }
  }
</script>

<template>
  <v-card class="mx-auto px-6 py-8" max-width="344">
    <v-card-title>Reset password</v-card-title>
    <v-card-subtitle>
      Go back to
      <a href="#" @click.prevent="$emit('showSignIn')">sign in</a>
    </v-card-subtitle>
    <br>

    <v-form ref="formRef" v-model="form" @submit.prevent="resetPassword">
      <v-text-field
        ref="emailInputRef"
        v-model="email"
        :disabled="successfullyResetPassword"
        :error-messages="errorMessage"
        label="Email"
        :readonly="loading"
        :rules="[required, validEmailFormat]"
        type="email"
        validate-on="submit"
      />

      <br>

      <v-btn
        block
        color="success"
        :disabled="!form || successfullyResetPassword"
        :loading="loading"
        size="large"
        type="submit"
        variant="elevated"
      >
        Reset password
      </v-btn>
    </v-form>
  </v-card>
</template>
