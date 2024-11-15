<script lang="ts">
import { useAuthStore } from '../../../stores/auth'
import { ref, watchEffect } from 'vue'

export default {
  name: 'SidebarShow',
  props: {
    currentAdmin: {
      type: Object,
      required: true
    }
  },
  setup(props, { emit }) {
    const adminName = ref('')
    const authStore = useAuthStore()
    const showJobCreationPopup = () => {
      emit('show-job-creation-popup')
    }
    watchEffect(async () => {
      try {
        const adminData = await props.currentAdmin
        adminName.value = adminData.userName
      } catch (error) {
        console.error("Erreur lors de la récupération de l'administrateur:", error)
      }
    })
    const logout = () => {
      authStore.state.isLoggedIn = false

      sessionStorage.removeItem('jwtToken')
      sessionStorage.removeItem('isAdmin')
    }

    return {
      adminName,
      logout,
      showJobCreationPopup,
    }
  }
}
</script>
<template>
  <button class="menu-toggle">
  <i class="mdi mdi-menu"></i>
</button>
  <nav class="sidebar sidebar-offcanvas" id="sidebar">
    <ul class="nav">
      <li class="nav-item nav-category">Main</li>
      <li class="nav-item">
        <router-link class="nav-link" to="/dashboard">
          <span class="icon-bg"><i class="mdi mdi-cube menu-icon"></i></span>
          <span class="menu-title">Dashboard</span>
        </router-link>
      </li>
      <li class="nav-item">
        <a
          class="nav-link"
          data-toggle="collapse"
          href="#ui-basic"
          aria-expanded="false"
          aria-controls="ui-basic"
        >
          <span class="icon-bg"><i class="mdi mdi-animation menu-icon"></i></span>
          <span class="menu-title">Jobs</span>
          <i class="menu-arrow"></i>
        </a>
        <div class="collapse" id="ui-basic">
          <ul class="nav flex-column sub-menu">
            <li class="nav-item">
              <router-link class="nav-link" to="/jobs-admin">Jobs listing</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/createJob">Create a Job offer</router-link>
            </li>
          </ul>
        </div>
      </li>

      <li class="nav-item">
        <router-link to="/candidate-list" class="nav-link">
          <span class="icon-bg"><i class="mdi mdi-account-multiple menu-icon"></i></span>
          Candidates</router-link
        >
      </li>
      <li class="nav-item sidebar-user-actions">
        <div class="user-details">
          <div class="d-flex justify-content-between align-items-center">
            <div>
              <div class="d-flex align-items-center">
                <div class="sidebar-profile-img">
                  <img src="/src/assets/admin/images/faces/face28.png" alt="image" />
                </div>
                <div class="sidebar-profile-text">
                  <router-link to="/profile-admin"
                    ><p class="mb-1">{{ adminName }}</p></router-link
                  >
                </div>
              </div>
            </div>
          </div>
        </div>
      </li>
      <li class="nav-item sidebar-user-actions">
        <div class="sidebar-user-menu">
          <a href="#" class="nav-link"
            ><i class="mdi mdi-settings menu-icon"></i>
            <span class="menu-title">Settings</span>
          </a>
        </div>
      </li>

      <li class="nav-item sidebar-user-actions">
        <div class="sidebar-user-menu">
          <router-link to="/login" @click="logout" class="nav-link"
            ><i class="mdi mdi-logout menu-icon"></i>
            <span class="menu-title">Log Out</span></router-link
          >
        </div>
      </li>
    </ul>
  </nav>
</template>
<style scoped>
/* Styles de base pour la sidebar */
.sidebar {
  width: 250px;
  /* Autres styles */
}

/* Cacher la sidebar en mode mobile */
@media (max-width: 768px) {
  .sidebar {
    display: none; /* Cacher par défaut */
  }

  .sidebar-show {
    display: block; /* Afficher quand la classe est présente */
    position: fixed;
    top: 0;
    left: 0;
    height: 100%;
    width: 250px;
    background-color: #fff; /* Change selon le thème */
    z-index: 1000;
  }
}

/* Styles pour le bouton de menu */
.menu-toggle {
  display: none; /* Masquer sur grands écrans */
}

@media (max-width: 768px) {
  .menu-toggle {
    display: block; /* Afficher sur petits écrans */
    position: fixed;
    top: 10px;
    left: 10px;
    z-index: 1001;
  }
}
</style>
