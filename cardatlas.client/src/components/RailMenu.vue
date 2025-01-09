<script lang="ts" setup>
  import { useRouter } from 'vue-router'
  import { useUserStore } from '@/stores/userStore'

  const router = useRouter()
  const userStore = useUserStore()
  const userImage = ref<string|undefined>(undefined)// 'https://randomuser.me/api/portraits/men/85.jpg')
  
  const signOut = () => {
    userStore.signOut()
    router.push('/ApplicationSignIn')
  }

  const displayDrawer = computed(() => {
    return userStore.userIsLoggedIn && router.currentRoute.value.path !== '/ApplicationSignIn'
  })
</script>

<template>
  <v-navigation-drawer
    v-if="displayDrawer"
    expand-on-hover
    rail
  >
    <v-list nav>
      <v-list-item
        nav
        title="User name"
      >
        <template #prepend>
          <v-avatar v-if="userImage" :image="userImage" />
          <v-icon v-else icon="mdi-account" />
        </template>
      </v-list-item>
    </v-list>

    <v-divider />

    <v-list density="compact" nav>
      <v-list-item prepend-icon="mdi-view-dashboard" title="Dashboard" :to="'/UserDashboard'" />
    </v-list>

    <template #append>
      <v-divider />

      <v-list nav>
        <v-list-item
          nav
          prepend-icon="mdi-logout"
          title="Logout"
          @click="signOut"
        />
      </v-list>
    </template>

  </v-navigation-drawer>
</template>
