<script lang="ts" setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'

  const router = useRouter()
  const activeComponent = ref<'signIn' | 'signUp' | 'resetPassword'>('signIn')

  const updateView = (showView: 'signIn' | 'signUp' | 'resetPassword') => {
    activeComponent.value = showView
  }
</script>

<template>
  <v-container class="fill-height">
    <v-responsive
      class="align-centerfill-height mx-auto"
      max-width="900"
    >

      <div class="text-center">
        <div class="text-body-2 font-weight-light mb-n1">Welcome to</div>
        <h1 class="text-h3 font-weight-bold">Card Atlas</h1>
      </div>

      <div class="py-4" />

      <SignIn
        v-if="activeComponent === 'signIn'"
        @show-reset-password="updateView('resetPassword')"
        @show-sign-up="updateView('signUp')"
        @sign-in-success="router.push('/userDashboard')"
      />

      <SignUp
        v-if="activeComponent === 'signUp'"
        @show-sign-in="updateView('signIn')"
        @sign-up-success="router.push('/userDashboard')"
      />

      <ResetPassword
        v-if="activeComponent === 'resetPassword'"
        @show-sign-in="updateView('signIn')"
      />
    </v-responsive>
  </v-container>

</template>
