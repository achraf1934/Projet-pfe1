<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { computed, onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'
import router from '../../../router'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import { debounce } from 'lodash'

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
    const searchQuery = ref<string>('')
    const jwtToken = localStorage.getItem('jwtToken')

    const Candidates = ref<any[]>([])
    const currentPage = ref(1)
    const itemsPerPage = 4
    // Calculer les candidats paginés
    const paginatedCandidates = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage
      const endIndex = startIndex + itemsPerPage
      return Candidates.value.slice(startIndex, endIndex)
    })
    const filterCandidates = () => {
  if (searchQuery.value.trim() === '') {
    // Si le champ est vide, afficher tous les candidats
    fetchUsersList()
  } else {
    Candidates.value = Candidates.value.filter((candidate) =>
      candidate.userName.toLowerCase().includes(searchQuery.value.toLowerCase())
    )
  }
}
    const totalPages = computed(() => Math.ceil(Candidates.value.length / itemsPerPage))
    const debounceFilter = debounce(filterCandidates, 300)
    const onSearchInput = (e: Event) => {
  searchQuery.value = (e.target as HTMLInputElement).value
  debounceFilter()
}
    const changePage = (page: number) => {
      if (page >= 1 && page <= totalPages.value) {
        currentPage.value = page
      }
    }
    const fetchUsersList = async (username: string = '') => {
      try {
        const response = await axios.get(`http://localhost:5094/api/Account/GetAllUsers?username=${username}`)
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
      data,
      currentPage,
      paginatedCandidates,
      changePage,
      totalPages,
      searchQuery,
      filterCandidates,
      onSearchInput
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
        </div>

        <hr />
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
                <div class="form-group">
                  <label for="searchInput">Search by Username</label>
                  <input
                    type="text"
                    style="width: 300px;"
                    id="searchInput"
                    class="form-control"
                    placeholder="Enter username"
                    v-model="searchQuery"
                     @input="onSearchInput"
                  />
                </div>

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
                            <tr v-for="candidate in paginatedCandidates" :key="candidate.id">
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
                        <!--pagination---->
                        <div class="pagination">
                          <button
                            @click="changePage(currentPage - 1)"
                            :disabled="currentPage === 1"
                          >
                            Previous
                          </button>
                          <button
                            v-for="page in totalPages"
                            :key="page"
                            :class="{ active: page === currentPage }"
                            @click="changePage(page)"
                          >
                            {{ page }}
                          </button>
                          <button
                            @click="changePage(currentPage + 1)"
                            :disabled="currentPage === totalPages"
                          >
                            Next
                          </button>
                        </div>
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
<style scoped>
.pagination {
  display: flex;
  gap: 10px;
  margin-top: 20px;
  justify-content: center;
}

.pagination button {
  padding: 5px 10px;
  border: 1px solid #ccc;
  background: #fff;
  cursor: pointer;
}

.pagination button.active {
  background: #007bff;
  color: #fff;
  font-weight: bold;
}

.pagination button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
