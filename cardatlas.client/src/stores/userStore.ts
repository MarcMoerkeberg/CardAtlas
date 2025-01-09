import { defineStore } from 'pinia'
import { ResponseInfo } from '../types/ResponseInfo'
import { User } from '@/types/User';

type userStoreState = {
  currentUser: User | null;
}

export const useUserStore = defineStore('userStore', {
  state: (): userStoreState => ({
    currentUser: null,
  }),
  actions: {
    async signIn (email: string, password: string): Promise<ResponseInfo> {
      const signInResponse: ResponseInfo = { success: true }

      try {
        await fetch('baseUrl' + 'controller' + 'endpoint', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: email, password: password }),
        })
      } catch (error: any) {
        handleError(error)
      }

      return signInResponse
    },
    async signOut (): Promise<void> {
        try {
            await fetch('baseUrl' + 'controller' + 'endpoint', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }
            })
          } catch (error: any) {
            handleError(error)
          }
    },
    async signUp (user: User): Promise<ResponseInfo> {
      const createUserResponse: ResponseInfo = { success: true }

      try {
        await fetch('baseUrl' + 'controller' + 'endpoint', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user),
        })
      } catch (error: any) {
        handleError(error)
      }

      return createUserResponse
    },
    async resetPassword (email: string): Promise<ResponseInfo> {
      const resetPasswordResponse: ResponseInfo = { success: true }

      try {
        await fetch('baseUrl' + 'controller' + 'endpoint', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: email }),
        })
      } catch (error: any) {
        handleError(error)
      }

      return resetPasswordResponse
    },
  },
  getters: {
    userIsLoggedIn (): boolean {
      return this.currentUser !== null
    },
  },
})

const handleError = (error: any) => {
    console.error(error)
}