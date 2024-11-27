<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { computed, onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import router from '../../../router'
import Swal from 'sweetalert2'

export default {
  name: 'JobsAdmin',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const state = reactive({
      Admin: ''
    })
    const Jobs = ref<any[]>([])
    const interships = ref<any[]>([])

    const Departments = ref<string[]>([])
    const selectedDepartment = ref<string>('')
    const jwtToken = sessionStorage.getItem('jwtToken')
    // Page actuelle
    const currentPage = ref(1) 
    const currentPageI = ref(1)

    const itemsPerPage = 5 // Nombre d'offres par page
    const paginatedJobs = computed(() =>
      filteredJobs.value.slice(
        (currentPage.value - 1) * itemsPerPage,
        currentPage.value * itemsPerPage
      )
    )
    const paginatedIntership = computed(() =>
      filteredInterships.value.slice(
        (currentPage.value - 1) * itemsPerPage,
        currentPage.value * itemsPerPage
      )
    )
    const changePage = (page: number) => {
      currentPage.value = page
    }
    const changePageI = (page: number) => {
      currentPageI.value = page
    }
    const totalPages = computed(() => Math.ceil(Jobs.value.length / itemsPerPage))
    const totalPagesI = computed(() => Math.ceil(interships.value.length / itemsPerPage))

    const searchKeyword = ref<string>('')
   
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
    const fetchDepartments = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/departements')
        Departments.value = response.data
      } catch (error) {
        console.error('Erreur lors de la récupération des départements:', error)
      }
    }
    const filteredJobs = computed(() =>
      Jobs.value.filter((jobsOffer) => {
        const matchesDepartment =
          !selectedDepartment.value || jobsOffer.departement === selectedDepartment.value
        const matchesSearch =
          !searchKeyword.value ||
          jobsOffer.titre.toLowerCase().includes(searchKeyword.value.toLowerCase())
        return matchesDepartment && matchesSearch
      })
    )
    const filteredInterships = computed(() =>
      interships.value.filter((intership) => {
        const matchesDepartment =
          !selectedDepartment.value || intership.departement === selectedDepartment.value

        const matchesSearch =
          !searchKeyword.value ||
          intership.titre.toLowerCase().includes(searchKeyword.value.toLowerCase())
        return matchesDepartment && matchesSearch
      })
    )
    const fetchJobsList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetOffreEmplois')
        Jobs.value = response.data
      } catch (error) {
        return error
      }
    }
    const fetchIntershipList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetOffreStage')
        interships.value = response.data
        return response
      } catch (error) {
        return error
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
            const response = await axios.delete(`http://localhost:5094/api/Offre/DeleteOffre/${id}`)
            Swal.fire({
              icon: 'success',
              title: 'Offer deleted!',
              showConfirmButton: false,
              timer: 1500
            })
            fetchJobsList()
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
    const show = (jobId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/jobAdminDetail/${jobId}`)
    }
    onMounted(async () => {
      await fetchJobsList()
      console.log('All jobs : ', Jobs)
      await fetchDepartments()
      await fetchIntershipList()
    })
    const currentAdmin = fetchUser()

    return {
      fetchJobsList,
      Jobs,
      interships,
      Departments,
      fetchUser,
      show,
      handleDelete,
      paginatedJobs,
      paginatedIntership,
      currentAdmin,
      changePage,
      changePageI,
      currentPage,
      currentPageI,
      totalPages,
      totalPagesI,
      selectedDepartment,
      filteredJobs,
      fetchDepartments,
      searchKeyword
    }
  }
}
</script>
<template>
  <div class="container-scroller">
    <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

    <NavbarAdmin :currentAdmin="currentAdmin" />
    <div class="container-fluid page-body-wrapper">
      <SidebarShow :currentAdmin="currentAdmin" />
      <div class="main-panel">
        <div class="content-wrapper">
          <div class="d-xl-flex justify-content-between align-items-start">
            <h2 class="text-dark font-weight-bold mb-2">Job overview</h2>
          </div>
          <div class="d-sm-flex justify-content-xl-between align-items-center mb-2">
            <input
              type="text"
              style="width: 300px"
              id="searchInput"
              class="form-control"
              placeholder="Search"
              v-model="searchKeyword"
            />
            <div class="dropdown ml-0 ml-md-4 mt-2 mt-lg-0">
              <button
                class="btn bg-white dropdown-toggle p-3 d-flex align-items-center"
                type="button"
                id="dropdownMenuButton1"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="mdi mdi-domain mr-1"></i> {{ selectedDepartment || 'Select Department' }}
              </button>
              <div
                class="dropdown-menu dropdown-menu-right"
                aria-labelledby="dropdownMenuButton1"
                x-placement="bottom-end"
                style="
                  position: absolute;
                  will-change: transform;
                  top: 0px;
                  left: 0px;
                  transform: translate3d(218px, 47px, 0px);
                "
              >
                <a
                  class="dropdown-item"
                  href="#"
                  v-for="department in Departments"
                  :key="department"
                  @click="selectedDepartment = department"
                  >{{ department }}
                </a>

                <div class="dropdown-divider"></div>
                <a class="dropdown-item" href="#" @click="selectedDepartment = ''">View all</a>
              </div>
            </div>
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
                      >Job</a
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
                      >Intership</a
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
                  &nbsp;
                  <div class="row">
                    <div
                      class="col-xl-3 col-lg-6 col-sm-6 grid-margin stretch-card"
                      v-for="job in paginatedJobs"
                      :key="job.id"
                    >
                      <div class="card">
                        <div class="card-body text-center">
                          <h2 class="mb-4 text-dark font-weight-bold">{{ job.titre }}</h2>

                          <i class="mdi mdi-eye btn-icon-append" @click="show(job.id)"></i>
                        </div>
                        <p class="card-description">
                          <router-link :to="`/job-applications/${job.id}`" class="btn btn-link"
                            >Candidates</router-link
                          >
                        </p>
                        <p class="card-description">
                          <router-link :to="`/ShowInterviewByOffer/${job.id}`" class="btn btn-link"
                            >Interviews</router-link
                          >
                        </p>
                        <hr />
                        <router-link
                          :to="`/editJob/${job.id}`"
                          type="button"
                          class="btn btn-primary btn-fw"
                        >
                          <i class="mdi mdi-file-check btn-icon-append"></i> Edit
                        </router-link>
                        <button
                          @click="handleDelete(job.id)"
                          type="button"
                          class="btn custom-btn btn-fw"
                        >
                          <i class="mdi mdi-delete">Delete</i>
                        </button>
                      </div>
                    </div>
                    <!--pagination-->
                    <div class="pagination">
                      <button :disabled="currentPage === 1" @click="changePage(currentPage - 1)">
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
                        :disabled="currentPage === totalPages"
                        @click="changePage(currentPage + 1)"
                      >
                        Next
                      </button>
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


                    <div
                      class="col-xl-3 col-lg-6 col-sm-6 grid-margin stretch-card"
                      v-for="intership in paginatedIntership"
                      :key="intership.id"
                    >
                      <div class="card">
                        <div class="card-body text-center">
                          <h2 class="mb-4 text-dark font-weight-bold">{{ intership.titre }}</h2>

                          <i class="mdi mdi-eye btn-icon-append" @click="show(intership.id)"></i>
                        </div>
                        <p class="card-description">
                          <router-link :to="`/job-applications/${intership.id}`" class="btn btn-link"
                            >Candidates</router-link
                          >
                        </p>
                        <p class="card-description">
                          <router-link :to="`/ShowInterviewByOffer/${intership.id}`" class="btn btn-link"
                            >Interviews</router-link
                          >
                        </p>
                        <hr />
                        <router-link
                          :to="`/editJob/${intership.id}`"
                          type="button"
                          class="btn btn-primary btn-fw"
                        >
                          <i class="mdi mdi-file-check btn-icon-append"></i> Edit
                        </router-link>
                        <button
                          @click="handleDelete(intership.id)"
                          type="button"
                          class="btn custom-btn btn-fw"
                        >
                          <i class="mdi mdi-delete">Delete</i>
                        </button>
                      </div>
                    </div>
                 
                    <!--pagination-->
                    <div class="pagination">
                      <button :disabled="currentPageI === 1" @click="changePageI(currentPageI - 1)">
                        Previous
                      </button>
                      <button
                        v-for="page in totalPagesI"
                        :key="page"
                        :class="{ active: page === currentPageI }"
                        @click="changePageI(page)"
                      >
                        {{ page }}
                      </button>
                      <button
                        :disabled="currentPageI === totalPagesI"
                        @click="changePageI(currentPageI + 1)"
                      >
                        Next
                      </button>
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
<style scooped>
.custom-btn {
  background-color: #181824; /* Couleur de fond */
  color: #181824; /* Couleur du texte */
  border-color: #181824; /* Bordure, si nécessaire */
}

.custom-btn:hover {
  background-color: #212130; /* Couleur de fond au survol */
}

.description {
  /* Spécifiez la hauteur maximale à afficher */
  max-height: 3 * 1.2em; /* 1.2em est généralement la hauteur d'une ligne de texte */

  /* Permet au texte supplémentaire de dépasser la hauteur maximale spécifiée */
  overflow: hidden;

  /* Garantit que le texte supplémentaire est masqué */
  text-overflow: ellipsis;

  /* Assurez-vous que le texte débordant est correctement coupé */
  white-space: nowrap;

  /* Facultatif : ajoutez une marge inférieure pour plus de clarté */
  margin-bottom: 60px;
}
.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
  margin-block-end: 40px;
}
.pagination button {
  margin: 0 5px;
  padding: 5px 10px;
  border: none;
  background-color: #f0f0f0;
  cursor: pointer;
}
.pagination button.active {
  font-weight: bold;
  background-color: #007bff;
  color: #fff;
}
.pagination button:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}
</style>
