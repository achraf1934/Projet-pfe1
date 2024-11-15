<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { useRoute } from 'vue-router'
import axios from 'axios'
import { onMounted, ref } from 'vue'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import router from '../../../router'
import Swal from 'sweetalert2'
export default {
  name: 'ApplicationDetail',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const showIframe = ref(false)
    const showIframeCV = ref(false)

    const jwtToken = sessionStorage.getItem('jwtToken')
    const route = useRoute()
    const candidature = ref<any>()
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
    const toggleIframe = () => {
      showIframe.value = !showIframe.value
    }
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    const toggleIframeCV = () => {
      showIframeCV.value = !showIframeCV.value
    }
    const appUserId = route.params.appUserId
    const offreId = route.params.offreId
    const getCandidatureByUserName = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5094/api/Candidature/${appUserId}/${offreId}`,
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`
            },
            withCredentials: true
          }
        )
        const jobInfo = response.data
        candidature.value = jobInfo
      } catch (error) {
        console.error(
          "Erreur lors de la récupération de la candidature par nom d'utilisateur:",
          error
        )
        throw error
      }
    }
    // Méthode pour mettre à jour la décision de candidature
    const updateDecision = async (decision: string) => {
      try {
        await axios.put(
          `http://localhost:5094/api/Candidature/update-decision/${appUserId}/${offreId}`,
          { decision },
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`
            }
          }
        )
        candidature.value.decision = decision
        Swal.fire({
          icon: 'success',
          title: `Application ${decision}`, // Utilisation de backticks pour interpolation
          showConfirmButton: false,
          timer: 2500
        }) // Mise à jour locale de la décision
      } catch (error: any) {
        Swal.fire({
          icon: 'error',
          title: ' failed !',
          showConfirmButton: false,
          text: error.message,
          timer: 3000
        })
        console.error(`Erreur lors de la mise à jour de la décision : ${error}`)
        alert('Erreur lors de la mise à jour de la décision')
      }
    }

    const acceptApplication = () => updateDecision('Accepted')
    const rejectApplication = () => updateDecision('Rejected')

    const scheduleInterview = (appUserId: string, offreId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/interview/${appUserId}/${offreId}`)
    }
    const jobApp = getCandidatureByUserName()
    onMounted(async () => {
      await getCandidatureByUserName()
    })
    console.log('jobApp', candidature)
    return {
      currentAdmin,
      candidature,
      showIframe,
      showIframeCV,
      toggleIframe,
      toggleIframeCV,
      scheduleInterview,
      getCandidatureByUserName,
      jobApp,
      formatDate,
      acceptApplication,
      rejectApplication
    }
  }
}
</script>
<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

  <NavbarAdmin :currentAdmin="currentAdmin" />
  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="currentAdmin" />
    <div class="main-panel" v-if="candidature">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Job Application overview</h2>
        </div>
        <hr />

        <div class="row">
          <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
              <div class="card-body align-right">
                <div class="job-details">
                  <div class="detail">
                    <h1>{{ candidature.offre.titre }}</h1>
                    {{ console.log('candidature : ', candidature) }}
                    <p></p>
                    <br />
                    <div class="info-row">
                      <div class="info-item">
                        <h6 class="text-secondary">AI Decision</h6>
                        <i class="fa fa-angle-right">&nbsp;{{ candidature.status }}</i>
                      </div>
                      <div class="info-item">
                        <h6 class="text-secondary">Similarity score</h6>
                        <i class="fa fa-angle-right">&nbsp;{{ candidature.score }}% </i>
                      </div>
                      <div class="info-item">
                        <h6 class="text-secondary">Decision</h6>
                    <i class="fa fa-angle-right"
                      >&nbsp;{{ candidature.decision }}</i
                    >
                        </div>
                    </div>
                  </div>
                  <hr />

                  <div class="detail">
                    <h2 class="card-title">Resume</h2>
                    <button @click="toggleIframeCV" type="button" class="btn btn-link">
                      <i class="fa fa-angle-right text-primary me-2"></i> click to show resume
                    </button>
                    <div v-if="showIframeCV">
                      <iframe
                        :src="'http://localhost:5094/api/Candidature/cv/' + candidature.cvFileName"
                        style="width: 100%; height: 600px"
                      ></iframe>
                    </div>
                  </div>
                  <div class="detail">
                    <h2 class="card-title">Cover letter</h2>
                    <button @click="toggleIframe" type="button" class="btn btn-link">
                      <i class="fa fa-angle-right text-primary me-2"></i> click to show cover letter
                    </button>
                    <div v-if="showIframe">
                      <iframe
                        :src="
                          'http://localhost:5094/api/Candidature/lm/' +
                          candidature.lettreMotivationName
                        "
                        style="width: 100%; height: 600px"
                      ></iframe>
                    </div>
                  </div>
                 

                  <div class="detail">
                    <h2 class="card-title">Decision</h2>
                    
                    <button
                      @click="acceptApplication"
                      type="button"
                      :disabled="candidature.decision === 'Accepted'"
                      class="btn btn-secondary"
                    >
                      Accept</button
                    >&nbsp;
                    <button
                      @click="rejectApplication"
                      type="button"
                      :disabled="candidature.decision === 'Rejected'"
                      class="btn btn-danger"
                    >
                      Reject
                    </button>
                  </div>
                  <button
                    v-if="candidature.decision === 'Accepted'"
                    @click="scheduleInterview(candidature.appUserId, candidature.offreId)"
                    class="btn btn-link"
                  >
                    Schedule Interview
                  </button>
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
.header h2 {
  margin-bottom: 30px;
}

.job-details {
  margin-top: 20px;
}

.detail {
  margin-bottom: 20px;
  padding-bottom: 15px;
}

.iframe-container {
  width: 100%;
  height: 600px;
  margin-top: 15px;
}
.align-right {
  text-align: left;
}
h3 {
  color: #555;
  margin-bottom: 8px;
}

p {
  color: #666;
  margin-bottom: 10px;
}

.btn {
  margin-right: 10px;
}

.card {
  padding: 20px;
}
.info-row {
  display: flex;
  gap: 20px; /* Espace entre chaque info-item */
  margin-bottom: 20px;
}

.info-item {
  flex: 1;
  display: flex;
  flex-direction: column;
}
</style>
