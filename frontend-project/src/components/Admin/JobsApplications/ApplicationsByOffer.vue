<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { computed, onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import { useRoute, useRouter } from 'vue-router'
import NavbarShow from '../../navbar/NavbarShow.vue'

export default {
  name: 'ApplicationsByOffer',
  components: {
    SidebarShow,
    NavbarShow
  },
  setup() {
    const state = reactive({
      job: {
        titre: '',
        description: '',
        Qualifications: '',
        Responsibilities: '',
        DateDebut: new Date(),
        PostedDate: new Date(),
        RequiredSkills: [],
        RequiredLanguages: [],
        Benefits: [],
        EstStage: '',
        Departement: '',
        Lieu: '',
        ContractType: '',
        Gender: '',
        NiveauEtudesRequis: '',
        EstActive: ''
      }
    })
    const JobApplications = ref<any[]>([])
    
    const jwtToken = sessionStorage.getItem('jwtToken')
    const route = useRoute()
    const router = useRouter()

    const currentPage = ref(1)
    const itemsPerPage = 4
    // Calculer les candidats paginés
    const paginatedJobApplications = computed(() => {
      const startIndex = (currentPage.value - 1) * itemsPerPage
      const endIndex = startIndex + itemsPerPage
      return JobApplications.value.slice(startIndex, endIndex)
    })

    const totalPages = computed(() => Math.ceil(JobApplications.value.length / itemsPerPage))

    const changePage = (page: number) => {
      if (page >= 1 && page <= totalPages.value) {
        currentPage.value = page
      }
    }

    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
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
    const fetchOfferData = async () => {
      try {
        const id = route.params.offerId
        const response = await axios.get(`http://localhost:5094/api/Offre/${id}`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        const jobInfo = response.data
        state.job.titre = jobInfo.titre
        state.job.description = jobInfo.description
        state.job.Qualifications = jobInfo.qualifications
        state.job.Responsibilities = jobInfo.responsabilites
        state.job.DateDebut = jobInfo.dateDebut
        state.job.PostedDate = jobInfo.postedDate
        state.job.RequiredSkills = jobInfo.requiredSkills
        state.job.RequiredLanguages = jobInfo.requiredLanguages
        state.job.Benefits = jobInfo.benefits
        state.job.EstStage = jobInfo.estStage
        state.job.Departement = jobInfo.departement
        state.job.Lieu = jobInfo.lieu
        state.job.ContractType = jobInfo.contractType
        state.job.Gender = jobInfo.gender
        state.job.NiveauEtudesRequis = jobInfo.niveauEtudesRequis
        state.job.EstActive = jobInfo.estActive
        return response.data
      } catch (error) {
        console.error('Erreur fetching offerData:', error)
        throw error
      }
    }
    console.log('miaw', state.job)
    const fetchJobApplications = async () => {
      try {
        const id = route.params.offerId
        console.log('dia id', route.params.offerId)
        const response = await axios.get(`http://localhost:5094/api/Candidature/${id}`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        JobApplications.value = response.data
        return response.data
      } catch (error) {
        console.error('Erreur fetching job applications:', error)
        throw error
      }
    }

    const handleRowClick = (appUserId: string, offreId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/application-details/${appUserId}/${offreId}`)
    }
    onMounted(async () => {
      await fetchJobApplications()
      await fetchOfferData()
    })

    const currentAdmin = fetchUser()
    return {
      currentAdmin,
      state,
      fetchOfferData,
      JobApplications,
      fetchJobApplications,
      handleRowClick,
      formatDate,
      currentPage,
      paginatedJobApplications,
      totalPages,
      changePage
    }
  }
}
</script>
<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

  <NavbarShow :currentAdmin="currentAdmin" />
  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="currentAdmin" />
    <div class="main-panel">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Job Applications overview</h2>
        </div>
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
                    >Candidate info</a
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
                    >Job info</a
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
                <div class="card">
                  <div class="card-body">
                    <h4 class="card-title">Candidate applied to {{ state.job.titre }}</h4>
                    <table class="table table-striped">
                      <thead>
                        <tr>
                          <th>Username</th>
                          <th>Email</th>
                          <th>Phone number</th>
                          <th>Status</th>
                          <th>Applied date</th>
                          <th>Similarity score</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr
                          v-for="jobApplication in paginatedJobApplications"
                          :key="jobApplication.id"
                          @click="handleRowClick(jobApplication.appUserId, jobApplication.offreId)"
                        >
                          <td class="py-1">
                            {{ jobApplication.appUser.userName }}
                          </td>
                          <td>{{ jobApplication.appUser.email }}</td>
                          <td>
                            {{ jobApplication.appUser.phoneNumber }}
                          </td>
                          <td>{{ jobApplication.decision }}</td>
                          {{ console.log("decision",jobApplication) }}
                          <td>{{ formatDate(jobApplication.aplliedDate) }}</td>
                          <td>
                            {{ jobApplication.score  }}%
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
              <div
                class="tab-pane fade show"
                id="performance-1"
                role="tabpanel"
                aria-labelledby="performance-tab"
              >
                <div class="card">
                  <div class="card-body">
                    <h4 class="card-title">{{ state.job.titre }}</h4>
                    <h4 class="card-title text-success">Description</h4>
                    <p class="card-description">{{ state.job.description }}</p>
                    <h4 class="card-title text-success">Qualifications</h4>
                    <p class="card-description">{{ state.job.Qualifications }}</p>
                    <h4 class="card-title text-success">Responsibilities</h4>
                    <p class="card-description">{{ state.job.Responsibilities }}</p>
                    <h4 class="card-title text-success">Required Languages</h4>
                    <p class="card-description">{{ state.job.RequiredLanguages }}</p>
                    <h4 class="card-title text-success">Required Skills</h4>
                    <p class="card-description">{{ state.job.RequiredSkills }}</p>
                    <h4 class="card-title text-success">Gender</h4>
                    <p class="card-description">{{ state.job.Gender }}</p>
                    <h4 class="card-title text-success">Contract Type</h4>
                    <p class="card-description">{{ state.job.ContractType }}</p>
                    <h4 class="card-title text-success">Required Languages</h4>
                    <p class="card-description">{{ state.job.RequiredLanguages }}</p>
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
<style></style>
