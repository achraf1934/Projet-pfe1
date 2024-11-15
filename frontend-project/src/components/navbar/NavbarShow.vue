<script>
import { computed, ref, watchEffect } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'NavbarShow',
  props: {
    currentAdmin: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    const adminName = ref('')
    watchEffect(async () => {
      try {
        const adminData = await props.currentAdmin
        adminName.value = adminData.userName
        console.log(' username:', adminName.value)
      } catch (error) {
        console.error("Erreur lors de la récupération de l'administrateur:", error)
      }
    })

    const authStore = useAuthStore()
    const router = useRouter()
    const isLAdmin = computed(() => authStore.state.isAdmin)
    const isLoggedIn = computed(() => authStore.state.isLoggedIn)

    const logout = () => {
      authStore.state.isLoggedIn = false
      router.push('/login')

      sessionStorage.removeItem('jwtToken')
      sessionStorage.removeItem('isAdmin')
    }

    return {
      isLoggedIn,
      adminName,
      isLAdmin,
      logout
    }
  }
})
</script>
<template>
  <nav class="navbar navbar-expand-lg bg-white navbar-light shadow sticky-top p-0" style="top: 0px">
    <router-link
      to="/"
      class="navbar-brand d-flex align-items-center text-center py-0 px-4 px-lg-5"
    >
      <template v-if="isLoggedIn && isLAdmin">
        <h1 class="m-0 text-primary">Admin Dashboard</h1>
      </template>
      <template v-else> <h1 class="m-0 text-primary">Job Recruiter</h1> </template>
    </router-link>
    <button
      type="button"
      class="navbar-toggler me-4"
      data-bs-toggle="collapse"
      data-bs-target="#navbarCollapse"
    >
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarCollapse">
      <div class="navbar-nav ms-auto p-4 p-lg-0">
        <template v-if="isLoggedIn && !isLAdmin">
          <router-link to="/" class="nav-item nav-link active">Home</router-link>
          <router-link to="/about" class="nav-item nav-link">About</router-link>
          <router-link to="/jobs" class="nav-item nav-link">Jobs</router-link>
          <router-link to="/createResume" class="nav-item nav-link">Resume</router-link>
          <router-link to="/profile-candidat" class="nav-item nav-link">Profile</router-link>
          <router-link to="/contact" class="nav-item nav-link">Contact</router-link>
          <router-link to="/login" @click="logout" class="nav-item nav-link active"
            >Logout</router-link
          >
        </template>
        <template v-if="isLoggedIn && isLAdmin">
          <router-link to="/" class="nav-item nav-link">Profile</router-link>

          <router-link to="/" class="nav-item nav-link">Contact</router-link>
          <div class="navbar-menu-wrapper d-flex align-items-stretch">
            <ul class="navbar-nav navbar-nav-right">
              <li class="nav-item nav-profile dropdown">
                <a
                  class="nav-link dropdown-toggle"
                  id="profileDropdown"
                  href="#"
                  data-toggle="dropdown"
                  aria-expanded="false"
                >
                  <p class="mb-1 text-black">{{ adminName }}</p>
                </a>
                <div
                  class="dropdown-menu navbar-dropdown dropdown-menu-right p-0 border-0 font-size-sm"
                  aria-labelledby="profileDropdown"
                  data-x-placement="bottom-end"
                >
                  <div class="p-3 text-center bg-primary"></div>
                  <div class="p-2">
                    <h5 class="dropdown-header text-uppercase pl-2 text-dark">ADMIN Options</h5>
                    <router-link
                      class="dropdown-item py-1 d-flex align-items-center justify-content-between"
                      to="/profile-admin"
                    >
                      <span>Profile</span>
                      <span class="p-0">
                        <i class="mdi mdi-account-outline ml-1"></i>
                      </span>
                    </router-link>
                    <a
                      class="dropdown-item py-1 d-flex align-items-center justify-content-between"
                      href="#"
                    >
                      <span>Inbox</span>
                      <span class="p-0">
                        <i class="mdi mdi-email-open-outline ml-1"></i>
                      </span>
                    </a>

                    <a
                      class="dropdown-item py-1 d-flex align-items-center justify-content-between"
                      href="javascript:void(0)"
                    >
                      <span>Settings</span>
                      <i class="mdi mdi-settings"></i>
                    </a>
                    <div role="separator" class="dropdown-divider"></div>
                    <h5 class="dropdown-header text-uppercase pl-2 text-dark mt-2">Actions</h5>

                    <router-link
                      class="dropdown-item py-1 d-flex align-items-center justify-content-between"
                      to="/login"
                      @click="logout"
                    >
                      <span>Log Out</span>
                      <i class="mdi mdi-logout ml-1"></i>
                    </router-link>
                  </div>
                </div>
              </li>
            </ul>
          </div>
        </template>

        <div class="ms-4"></div>

        <template v-if="!isLoggedIn">
          <router-link to="/" class="nav-item nav-link active">Home</router-link>
          <router-link to="/about" class="nav-item nav-link">About</router-link>
          <div class="nav-item dropdown">
            <a href="#" class="nav-link dropdown-toggle" data-bs-toggle="dropdown">Jobs</a>
            <div class="dropdown-menu rounded-0 m-0">
              <router-link to="/jobs" class="dropdown-item">Jobs</router-link>
              <router-link to="/" class="dropdown-item">Interships</router-link>
            </div>
          </div>
          <router-link to="/login" class="nav-item nav-link active">Login</router-link>
        </template>
      </div>
    </div>
  </nav>
</template>
<style></style>
