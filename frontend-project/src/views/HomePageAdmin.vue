<script lang="ts">
import SidebarShow from '../components/Admin/Sidebar/SidebarShow.vue'
import DashboardShow from '../components/Admin/dashboard/DashboardShow.vue'
import LinearChart from '../components/Admin/dashboard/LinearChart.vue'
import NavbarAdmin from '../components/navbar-admin/NavbarAdmin.vue'
import axios from 'axios'

export default {
  name: 'HomePageAdmin',
  components: {
    SidebarShow,
    NavbarAdmin,
    DashboardShow,
    LinearChart
  },
  setup() {
    const jwtToken = localStorage.getItem('jwtToken')

    const fetchUser = async () => {
      try {
        const response = await axios.get(`http://localhost:5094/api/Account/user`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const currentAdmin = fetchUser()
    console.log('CurrentAdmin', currentAdmin)
    return {
      currentAdmin
    }
  }
}
</script>
<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

  <!--<NavbarAdmin :currentAdmin="currentAdmin" />-->
  <NavbarAdmin :currentAdmin="currentAdmin" />
  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="currentAdmin" />

    <div class="main-panel">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Statistic overview</h2>
        </div>
        <hr />
        <div class="row">
          <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
                <DashboardShow />
                <hr />
              </div>
            </div>
          </div>
          <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
                <LinearChart />
              </div>
            </div>
          </div>
        </div>

        <RouterView />
      </div>
    </div>
  </div>
</template>
<style scoped></style>
