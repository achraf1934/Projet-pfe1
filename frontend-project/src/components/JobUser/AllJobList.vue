<script lang="ts">
import FooterShow from '../footer/FooterShow.vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'

export default {
  name: 'AllJobList',
  components: {
    NavbarShow,
    FooterShow
  },
  setup() {
    const jobs = ref<any[]>([])
    const jobsOffer = ref<any[]>([])
    const interships = ref<any[]>([])
    //Page actuelle
    const currentPage = ref(1)
    const currentPageI = ref(1)
    const currentPageJ = ref(1)
    //filtreDepartment
    const Departments = ref<string[]>([])
    const selectedDepartment = ref<string>('')

    const selectedContractDuration = ref<string>('')
    const ContractDurations = ref<string[]>(['CDD', 'CDI', 'Internship'])

    const itemsPerPage = 3 // Nombre d'offres par page
    const searchKeyword = ref<string>('')
    const applySearch = () => {
      currentPage.value = 1 // Réinitialiser à la première page
    }

    const paginatedJobs = computed(() =>
      filteredAlls.value.slice(
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
    const paginatedJ = computed(() =>
      filteredJobs.value.slice(
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
    const changePageJ = (page: number) => {
      currentPageJ.value = page
    }
    const totalPages = computed(() => Math.ceil(jobs.value.length / itemsPerPage))
    const totalPagesI = computed(() => Math.ceil(interships.value.length / itemsPerPage))
    const totalPagesJ = computed(() => Math.ceil(jobsOffer.value.length / itemsPerPage))

    const fetchDepartments = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/departements')
        Departments.value = response.data
      } catch (error) {
        console.error('Erreur lors de la récupération des départements:', error)
      }
    }

    const filteredJobs = computed(() =>
      jobsOffer.value.filter((jobsOffer) => {
        const matchesDepartment =
          !selectedDepartment.value || jobsOffer.departement === selectedDepartment.value

        const matchesContractDuration =
          !selectedContractDuration.value ||
          jobsOffer.contractDuration === selectedContractDuration.value

        const matchesSearch =
          !searchKeyword.value ||
          jobsOffer.titre.toLowerCase().includes(searchKeyword.value.toLowerCase())
        return matchesDepartment && matchesSearch && matchesContractDuration
      })
    )
    const filteredInterships = computed(() =>
      interships.value.filter((intership) => {
        const matchesDepartment =
          !selectedDepartment.value || intership.departement === selectedDepartment.value

        const matchesContractDuration =
          !selectedContractDuration.value ||
          intership.contractDuration === selectedContractDuration.value  

        const matchesSearch =
          !searchKeyword.value ||
          intership.titre.toLowerCase().includes(searchKeyword.value.toLowerCase())
        return matchesDepartment && matchesSearch &&matchesContractDuration
      })
    )
    const filteredAlls = computed(() => {
      return jobs.value.filter((job) => {
        const matchesDepartment =
          !selectedDepartment.value || job.departement === selectedDepartment.value

        const matchesContractDuration =
          !selectedContractDuration.value ||
          job.contractDuration === selectedContractDuration.value  

        const matchesSearch =
          !searchKeyword.value ||
          job.titre.toLowerCase().includes(searchKeyword.value.toLowerCase())
        return matchesDepartment && matchesSearch && matchesContractDuration
      })
    })
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    const fetchjobList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetAllOffre')
        jobs.value = response.data
        return response
      } catch (error) {
        return error
      }
    }
    const fetchjobOfferList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetOffreEmplois')
        jobsOffer.value = response.data
        return response
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
              title: 'Offre supprimée avec success!',
              showConfirmButton: false,
              timer: 1500
            })
            fetchjobList()
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
    onMounted(async () => {
      await fetchjobList()
      await fetchjobOfferList()
      await fetchIntershipList()
      await fetchDepartments()
    })

    return {
      jobs,
      jobsOffer,
      interships,
      formatDate,
      fetchjobList,
      handleDelete,
      fetchjobOfferList,
      fetchIntershipList,
      paginatedJobs,
      changePage,
      changePageI,
      changePageJ,
      totalPages,
      totalPagesI,
      totalPagesJ,
      currentPage,
      currentPageI,
      currentPageJ,
      paginatedIntership,
      paginatedJ,
      Departments,
      selectedDepartment,
      searchKeyword,
      applySearch,
      selectedContractDuration,
      ContractDurations
    }
  }
}
</script>
<template>
  <div class="container-xxl bg-white p-0">
    <NavbarShow :currentAdmin="{}" />

    <div class="container-xxl py-5">
      <div class="container">
        <h1
          class="text-center mb-5 wow fadeInUp"
          data-wow-delay="0.1s"
          style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
        >
          Job Listing
        </h1>
        <!--recherche-->
        <div
          class="container-fluid bg-primary mb-5 wow fadeIn"
          data-wow-delay="0.1s"
          style="padding: 35px; visibility: visible; animation-delay: 0.1s; animation-name: fadeIn"
        >
          <div class="container">
            <div class="row g-2">
              <div class="col-md-10">
                <div class="row g-2">
                  <div class="col-md-4">
                    <input 
                      style="width: 300px"
                      id="searchInput"
                      class="form-control"
                      placeholder="Keyword"
                      v-model="searchKeyword"
                    />
                  </div>
                  <div class="col-md-4">
                    <select class="form-select border-0" v-model="selectedDepartment">
                      <option value="">All Departments</option>
                      <option
                        v-for="department in Departments"
                        :key="department"
                        :value="department"
                      >
                        {{ department }}
                      </option>
                    </select>
                  </div>
                  <div class="col-md-4">
                    <select class="form-select border-0" v-model="selectedContractDuration">
                      <option value="">Contract Duration</option>
                      <option
                        v-for="contractDuration in ContractDurations"
                        :key="contractDuration"
                        :value="contractDuration"
                      >
                        {{ contractDuration }}
                      </option>
                    </select>
                  </div>
                </div>
              </div>
              
            </div>
          </div>
        </div>
        <!-- End recherche-->
        <hr />
        <div
          class="tab-class text-center wow fadeInUp"
          data-wow-delay="0.3s"
          style="visibility: visible; animation-delay: 0.3s; animation-name: fadeInUp"
        >
          <ul class="nav nav-pills d-inline-flex justify-content-center border-bottom mb-5">
            <li class="nav-item">
              <a
                class="d-flex align-items-center text-start mx-3 ms-0 pb-3 active"
                data-bs-toggle="pill"
                href="#tab-1"
              >
                <h6 class="mt-n1 mb-0">Featured</h6>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="d-flex align-items-center text-start mx-3 pb-3"
                data-bs-toggle="pill"
                href="#tab-2"
              >
                <h6 class="mt-n1 mb-0">Jobs</h6>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="d-flex align-items-center text-start mx-3 me-0 pb-3"
                data-bs-toggle="pill"
                href="#tab-3"
              >
                <h6 class="mt-n1 mb-0">Interships</h6>
              </a>
            </li>
          </ul>
          <div class="tab-content">
            <div id="tab-1" class="tab-pane fade show p-0 active">
              <div class="job-item p-4 mb-4" v-for="job in paginatedJobs" :key="job.id">
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ job.titre }}</h5>

                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i>{{ job.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="fa fa-book text-primary me-2"></i
                        >{{ job.niveauEtudesRequis }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i>{{ job.contractType }}</span
                      >
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                      <router-link class="btn btn-primary" :to="`/jobDetail/${job.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                      {{ formatDate(job.dateDebut) }}</small
                    >
                  </div>
                </div>
              </div>
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
                <button :disabled="currentPage === totalPages" @click="changePage(currentPage + 1)">
                  Next
                </button>
              </div>
            </div>
            <div id="tab-2" class="tab-pane fade show p-0">
              <div class="job-item p-4 mb-4" v-for="jobOffer in paginatedJ" :key="jobOffer.id">
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ jobOffer.titre }}</h5>
                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i>{{ jobOffer.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i
                        >{{ jobOffer.contractType }}</span
                      >
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                      <router-link class="btn btn-primary" :to="`/jobDetail/${jobOffer.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                      {{ formatDate(jobOffer.dateDebut) }}</small
                    >
                  </div>
                </div>
              </div>
              <div class="pagination">
                <button :disabled="currentPageJ === 1" @click="changePageJ(currentPageJ - 1)">
                  Previous
                </button>
                <button
                  v-for="page in totalPagesJ"
                  :key="page"
                  :class="{ active: page === currentPageJ }"
                  @click="changePageJ(page)"
                >
                  {{ page }}
                </button>
                <button
                  :disabled="currentPageJ === totalPagesJ"
                  @click="changePageJ(currentPageJ + 1)"
                >
                  Next
                </button>
              </div>
            </div>
            <div id="tab-3" class="tab-pane fade show p-0">
              <div
                class="job-item p-4 mb-4"
                v-for="intership in paginatedIntership"
                :key="intership.id"
              >
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ intership.titre }}</h5>
                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i
                        >{{ intership.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i
                        >{{ intership.contractType }}</span
                      >
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                      <router-link class="btn btn-primary" :to="`/jobDetail/${intership.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                      {{ formatDate(intership.dateDebut) }}</small
                    >
                  </div>
                </div>
              </div>
              <div class="pagination">
                <button :disabled="currentPageI === 1" @click="changePage(currentPageI - 1)">
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
                  @click="changePage(currentPageI + 1)"
                >
                  Next
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <FooterShow />
  </div>
</template>
<style scoped>
.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
  margin-block-end: 80px;
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
  background-color: #01b075;
  color: #fff;
}
.pagination button:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}
</style>
