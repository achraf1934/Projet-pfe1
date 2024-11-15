<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'
import router from '../../../router'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'

export default {
  name: 'CandidatesList',
  components: {
    SidebarShow,
    NavbarAdmin
  },
  setup() {
    const state = reactive({
      Admin: ''
    })
    const data = reactive({
      username: '',
      email: '',
      password: '',
      phoneNumber: ''
    })
    const jwtToken = localStorage.getItem('jwtToken')

    const Candidates = ref<any[]>([])
    const fetchUsersList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Account/GetAllUsers')
        Candidates.value = response.data
        return response
      } catch (error) {
        return error
      }
    }
    const fetchUser = async () => {
      try {
        const response = await axios.get(`http://localhost:5094/api/Account/user`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        const AdminInfo = response.data
        state.Admin = AdminInfo.userName

        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const handleDelete = async (id: string) => {
      Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
      }).then(async (result) => {
        if (result.isConfirmed) {
          try {
            const response = await axios.delete(
              `http://localhost:5094/api/Account/DeleteUser/${id}`
            )
            Swal.fire({
              icon: 'success',
              title: 'User supprimée avec success!',
              showConfirmButton: false,
              timer: 1500
            })
            fetchUsersList()
            return response
          } catch (error) {
            Swal.fire({
              icon: 'error',
              title: 'An Error Occured!',
              showConfirmButton: false,
              timer: 1500
            })
            return error
          }
        }
      })
    }
    const submit = async () => {
      await fetch('http://localhost:5094/api/Account/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      })
      await router.push('/candidate-list')
    }
    const formatDate = (date: Date) => {
      // Méthode pour formater la date
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    onMounted(async () => {
      await fetchUsersList()
    })

    const currentAdmin = fetchUser()
    return {
      currentAdmin,
      formatDate,
      handleDelete,
      Candidates,
      submit,
      data
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
          <h2 class="text-dark font-weight-bold mb-2">Candidates overview</h2>
        </div>  <hr>
        <div class="row">
          <div class="col-md-12">
            <div
              class="d-sm-flex justify-content-between align-items-center transaparent-tab-border {"
            >
              <ul class="nav nav-tabs tab-transparent" role="tablist">
                <li class="nav-item">
                  <a
                    class="nav-link"
                    id="business-tab"
                    data-toggle="tab"
                    href="#business-1"
                    role="tab"
                    aria-selected="false"
                    >Candidates list</a
                  >
                </li>
                <li class="nav-item">
                  <a
                    class="nav-link"
                    id="performance-tab"
                    data-toggle="tab"
                    href="#performance-1"
                    role="tab"
                    aria-selected="false"
                    >Add new candidate</a
                  >
                </li>
              </ul>
            </div>
            <div class="tab-content tab-transparent-content">
              <div
                class="tab-pane fade show active"
                id="business-1"
                role="tabpanel"
                aria-labelledby="business-tab"
              >
                <div class="row">
                  <div class="col-xl-12 col-lg-6 col-sm-6 grid-margin stretch-card">
                    <div class="card">
                      <div class="card-body">
                        <h4 class="card-title">Candidates list</h4>
                        <table class="table">
                          <thead>
                            <tr>
                              <th>Name</th>
                              <th>Email</th>
                              <th>Joined</th>
                              <th>Status</th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr v-for="candidate in Candidates" :key="candidate.id">
                              <td>{{ candidate.userName }}</td>
                              <td>{{ candidate.email }}</td>
                              <td>{{ formatDate(candidate.dateCreated) }}</td>
                              <td>
                                <button
                                  @click="handleDelete(candidate.id)"
                                  class="badge badge-danger"
                                >
                                  Delete
                                </button>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div
                class="tab-pane fade show"
                id="performance-1"
                role="tabpanel"
                aria-labelledby="performance-tab"
              >
                <div class="row">
                  <div class="col-md-8 mx-auto">
                    <div class="card">
                      <div class="card-body">
                        <h4 class="card-title">Add a new Candidate</h4>
                        <p class="card-description">Fill the form with informations</p>
                        <form class="forms-sample" @submit.prevent="submit">
                          <div class="form-group">
                            <label for="exampleInputName1">Username</label>
                            <input
                              type="text"
                              class="form-control"
                              id="exampleInputName1"
                              v-model="data.username"
                            />
                          </div>
                          <div class="form-group">
                            <label for="exampleInputCity">Email</label>
                            <input
                              type="text"
                              class="form-control"
                              id="exampleInputCity"
                              name="email"
                              v-model="data.email"
                            />
                          </div>
                          <div class="form-group">
                            <label for="exampleInputEmail3">Password</label>
                            <input
                              type="password"
                              class="form-control"
                              id="exampleInputEmail3"
                              rows="4"
                              v-model="data.password"
                            />
                          </div>

                          <button type="submit" class="btn btn-primary mr-2">Submit</button>
                        </form>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
