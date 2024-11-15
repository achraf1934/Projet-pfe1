import { defineStore } from 'pinia'
export const useAuthStore = defineStore('auth', () => {
  const state = {
    isLoggedIn: sessionStorage.getItem('jwtToken') !== null,
    isAdmin: sessionStorage.getItem('isAdmin') === 'true'  }
  return { state }
})
