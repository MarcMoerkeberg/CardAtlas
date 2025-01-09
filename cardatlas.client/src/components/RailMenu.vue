<script lang="ts" setup>
  import router from '@/router'

  const signOut = () => {
    // firebaseStore.signOut()
    router.push('/application-sign-in')
  }

  const userImage = ref<string|undefined>(undefined)// 'https://randomuser.me/api/portraits/men/85.jpg')

  const displayDrawer = computed(() => {
    return false //firebaseStore.userIsLoggedIn && router.currentRoute.value.path !== '/application-sign-in'
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
      <v-list-item prepend-icon="mdi-chart-line" title="View reports" :to="'/reports'" />
      <v-list-item prepend-icon="mdi-chart-bar" title="Generate report" :to="'/generate-report'" />
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
