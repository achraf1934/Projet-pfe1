<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import axios from 'axios'
import NavbarAdmin from '../../../components/navbar-admin/NavbarAdmin.vue'
import { reactive } from 'vue'

export default {
  name: 'ProfileAdmin',
  components: {
    SidebarShow,
    NavbarAdmin
  },
  setup() {
    const state = reactive({
      Admin: {
        userName: '',
        email: '',
        dateCreated: new Date()
      }
    })
    const jwtToken = localStorage.getItem('jwtToken')
    const fetchUser = async () => {
      try {
        const response = await axios.get(`http://localhost:5094/api/Account/user`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        const AdminInfo = response.data
        state.Admin.userName = AdminInfo.userName
        state.Admin.email = AdminInfo.email
        state.Admin.dateCreated = AdminInfo.dateCreated

        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const currentAdmin = fetchUser()
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    return {
      currentAdmin,
      state,
      fetchUser,
      formatDate
    }
  }
}
</script>
<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />
  <NavbarAdmin :currentAdmin="currentAdmin" />

  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="currentAdmin" />
    <div class="main-panel">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Admin profile</h2>
        </div>
        <div class="row">
          <div class="col-md-12">
            <div
              class="d-sm-flex justify-content-between align-items-center transaparent-tab-border {"
            ></div>
          </div>

          <div class="col-md-10 mx-auto">
            <div class="card text-left ">
              <div class="card-body">
                <h4 class="card-title">Welcome {{ state.Admin.userName }}</h4>
                <form class="forms-sample">
                  <div class="form-group">
                    <label for="exampleInputUsername1">Username</label>
                    <h5>{{ state.Admin.userName }}</h5>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputEmail1">Email address</label>
                    <h5>{{ state.Admin.email }}</h5>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputPassword1">Joined date</label>
                    <h5> {{ formatDate(state.Admin.dateCreated) }}</h5>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputPassword1">Password</label>
                    <h5>***********</h5>
                  </div>

                  <button type="submit" class="btn btn-primary mr-2">Edit</button>
                </form>
              </div>
            </div>
          </div>
          
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
.text-left {
  text-align: left;
}
</style>