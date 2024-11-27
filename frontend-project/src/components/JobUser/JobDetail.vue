<script lang="ts">
import FooterShow from '../footer/FooterShow.vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import { onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'
import { useRoute } from 'vue-router'
import { jwtDecode } from 'jwt-decode'
import router from '../../router'
interface Data1 {
  Cv: File | null
  LettreMotivation: File | null
}
export default {
  name: 'JobDetail',
  components: {
    NavbarShow,
    FooterShow
  },

  setup() {
    const route = useRoute()
    const jwtToken = sessionStorage.getItem('jwtToken')
    const hasApplied = ref(false)

    const state = reactive({
      job: {
        Titre: '',
        Description: '',
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
        ContractDuration: '',
        Gender: '',
        NiveauEtudesRequis: '',
        EstActive: ''
      },

      data1: {
        Cv: null,
        LettreMotivation: null
      } as Data1,
      isSaving: false
    })
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    const handleFileChange = (event: Event, fieldName: keyof Data1) => {
      const input = event.target as HTMLInputElement
      if (input.files && input.files.length > 0) {
        state.data1[fieldName] = input.files[0]
      }
    }
    const checkIfApplied = async () => {
      if (jwtToken) {
        const decodedToken: any = jwtDecode(jwtToken)
        const userId = decodedToken.nameid
        const offreId = route.params.id

        try {
          const response = await axios.get(
            `http://localhost:5094/api/Candidature/HasApplied/${userId}/${offreId}`,
            {
              headers: {
                Authorization: `Bearer ${jwtToken}`
              }
            }
          )
          hasApplied.value = response.data
          console.log('hasapplied', hasApplied.value)
        } catch (error) {
          console.error('Error checking application status:', error)
        }
      }
    }
    const fetchOfferData = async () => {
      try {
        const id = route.params.id
        console.log(route.params)
        const response = await axios.get(`http://localhost:5094/api/Offre/${id}`)
        const jobInfo = response.data
        state.job.Titre = jobInfo.titre
        state.job.Description = jobInfo.description
        state.job.Qualifications = jobInfo.qualifications
        state.job.Responsibilities = jobInfo.responsibilities
        state.job.DateDebut = jobInfo.dateDebut
        state.job.PostedDate = jobInfo.postedDate
        state.job.RequiredSkills = jobInfo.requiredSkills
        state.job.RequiredLanguages = jobInfo.requiredLanguages
        state.job.Benefits = jobInfo.benefits
        state.job.EstStage = jobInfo.estStage
        state.job.Departement = jobInfo.departement
        state.job.Lieu = jobInfo.lieu
        state.job.ContractType = jobInfo.contractType
        state.job.ContractDuration = jobInfo.contractDuration

        state.job.Gender = jobInfo.gender
        state.job.NiveauEtudesRequis = jobInfo.niveauEtudesRequis
        state.job.EstActive = jobInfo.estActive
      } catch (error) {
        Swal.fire({
          icon: 'error',
          title: "Une erreur s'est produite!",
          showConfirmButton: false,
          timer: 1500
        })
        console.error('Error fetching offer:', error)
      }
    }

    const handleSave = () => {
      if (jwtToken) {
        const decodedToken: any = jwtDecode(jwtToken)
        if (decodedToken && decodedToken.role !== 'admin') {
          state.isSaving = true

          // Affichage d'un Swal de chargement
          Swal.fire({
            title: 'Veuillez patienter...',
            text: 'Envoi en cours de votre candidature.',
            allowOutsideClick: false,
            didOpen: () => {
              Swal.showLoading()
            }
          })
          const formData = new FormData()
          formData.append('titre', state.job.Titre)
          if (state.data1.Cv && state.data1.LettreMotivation) {
            formData.append('Cv', state.data1.Cv)
            formData.append('LettreMotivation', state.data1.LettreMotivation)
          }
          axios
            .post('http://localhost:5094/api/Candidature/Postuler', formData, {
              headers: {
                'Content-Type': 'multipart/form-data',
                Authorization: `Bearer ${jwtToken}`
              },
              withCredentials: true
            })
            .then(() => {
              Swal.close()
              Swal.fire({
                icon: 'success',
                title: 'Candidature envoyée!',
                showConfirmButton: false,
                timer: 1500
              })
            })
            .catch((error) => {
              Swal.close()
              Swal.fire({
                icon: 'error',
                title: 'Erreur!',
                text: 'Error while applying for this job.',
                showConfirmButton: false,
                timer: 1500
              })
              return error
            })
            .finally(() => {
              state.isSaving = false
            })
        } else {
          Swal.close()
          Swal.fire({
            icon: 'error',
            title: 'Acces denied!',
            text: 'Admin account',
            showConfirmButton: false,
            timer: 1500
          })
        }
      } else {
        Swal.close()
        //  JWT n'est pas disponible
        Swal.fire({
          icon: 'error',
          title: 'Acess denied!',
          text: 'Sign in to apply this job.',
          showConfirmButton: false,
          timer: 1500
        })
        router.push('/login')
      }
    }

    onMounted(async () => {
      await fetchOfferData()
      await checkIfApplied()
    })
    return {
      state,
      formatDate,
      fetchOfferData,
      handleFileChange,
      checkIfApplied,
      handleSave,
      hasApplied
    }
  }
}
</script>
<template>
  <div class="container-xxl bg-white p-0">
    <NavbarShow :currentAdmin="{}" />
    <div
      class="container-xxl py-5 wow fadeInUp"
      data-wow-delay="0.1s"
      style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
    >
      <div class="container">
        <div class="row gy-5 gx-4">
          <div class="col-lg-8">
            <div class="d-flex align-items-center mb-5">
              <div class="text-start ps-4">
                <h3 class="mb-3">{{ state.job.Titre }}</h3>
                <span class="text-truncate me-3"
                  ><i class="fa fa-map-marker text-primary me-2"></i>{{ state.job.Lieu }}</span
                >
                <span class="text-truncate me-3"
                  ><i class="far fa-clock text-primary me-2"></i>{{ state.job.ContractType }}</span
                >
                <span class="text-truncate me-3"
                  ><i class="far fa-map text-primary me-2"></i
                  >{{ state.job.ContractDuration }}</span
                >
              </div>
            </div>
            <hr />
            <div class="mb-5">
              <h4 class="mb-3 text-left">Job Description</h4>
              <p v-if="state.job.Description" class="text-left">{{ state.job.Description }}</p>
              <p v-else>No description available.</p>

              <h4 class="mb-3 text-left">Qualifications</h4>
              <p v-if="state.job.Qualifications" class="text-left">
                {{ state.job.Qualifications }}
              </p>
              <p v-else>No qualifications listed.</p>

              <h4 class="mb-3 text-left">Responsibilities</h4>
              <p v-if="state.job.Responsibilities" class="text-left">
                {{ state.job.Responsibilities }}
              </p>
              <p v-else>No responsibilities outlined.</p>
            </div>
            <hr />
            <div v-if="hasApplied">
              <h1 class="text-primary">You have already applied</h1>
            </div>
            <div v-else>
              <h4 class="mb-4">Apply For The Job</h4>
              <form @submit.prevent="handleSave">
                <div class="row g-3">
                  <div class="col-12 col-sm-6">
                    <label>Cv</label>

                    <input
                      type="file"
                      class="form-control bg-white"
                      @change="handleFileChange($event, 'Cv')"
                    />
                  </div>
                  <div class="col-12 col-sm-6">
                    <label>Cover letter</label>

                    <input
                      type="file"
                      class="form-control bg-white"
                      @change="handleFileChange($event, 'LettreMotivation')"
                    />
                  </div>

                  <div class="col-12">
                    <button class="btn btn-primary w-100" type="submit">Apply Now</button>
                  </div>
                </div>
              </form>
            </div>
          </div>

          <div class="col-lg-4">
            <div
              class="bg-light rounded p-5 mb-4 wow slideInUp"
              data-wow-delay="0.1s"
              style="visibility: visible; animation-delay: 0.1s; animation-name: slideInUp"
            >
              <h4 class="mb-4">Job Summery</h4>
              <p class="text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Published On :
                {{ formatDate(state.job.PostedDate) }}
              </p>
              <p class="text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Job Nature :
                {{ state.job.ContractType }}
              </p>
              <p class="text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Location : {{ state.job.Lieu }}
              </p>
              <p class="m-0 text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Start Date :
                {{ formatDate(state.job.DateDebut) }}
              </p>
            </div>
            <div
              class="bg-light rounded p-5 wow slideInUp"
              data-wow-delay="0.1s"
              style="visibility: visible; animation-delay: 0.1s; animation-name: slideInUp"
            >
              <h4 class="mb-3 text-left">Required Skills</h4>
              <ul class="list-unstyled">
                <li v-for="skill in state.job.RequiredSkills" :key="skill" class="text-left">
                  <i class="fa fa-angle-right text-primary me-2 text-left" aria-hidden="true"></i
                  >{{ skill }}
                </li>
              </ul>

              <h4 class="mb-3 text-left">Required Languages</h4>
              <ul class="list-unstyled">
                <li
                  v-for="language in state.job.RequiredLanguages"
                  :key="language"
                  class="text-left"
                >
                  <i class="fa fa-angle-right text-primary me-2 text-left" aria-hidden="true"></i
                  >{{ language }}
                </li>
              </ul>

              <h4 class="mb-3 text-left">Benefits</h4>
              <ul class="list-unstyled">
                <li v-for="benefit in state.job.Benefits" :key="benefit" class="text-left">
                  <i class="fa fa-angle-right text-primary me-2" aria-hidden="true"></i
                  >{{ benefit }}
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
    <FooterShow />
  </div>
</template>
<style scoped>
.text-left {
  text-align: left;
}
</style>
