<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'
import Swal from 'sweetalert2'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'

export default {
  name: 'JobsAdminDetails',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const route = useRoute()
    const jwtToken = localStorage.getItem('jwtToken')
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
      Admin: '',

      isSaving: false
    })
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
        const AdminInfo = response.data
        state.Admin = AdminInfo.userName

        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const currentAdmin = fetchUser()

    const fetchOfferData = async () => {
      try {
        const id = route.params.id
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
        console.log("job",state.job)
      } catch (error) {
        Swal.fire({
          icon: 'error',
          title: 'Error fetching job offer!',
          showConfirmButton: false,
          timer: 1500
        })
        console.error('Error fetching offer:', error)
      }
    }
    onMounted(async () => {
      await fetchOfferData()
    })
    return {
      state,
      formatDate,
      fetchOfferData,
      currentAdmin
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
          <h2 class="text-dark font-weight-bold mb-2">Job details</h2>
        </div>
        <div class="row">
          <div class="col-md-6 grid-margin stretch-card">
            <div class="card">
              <div class="card-body left-align">
                <h2 class="card-description">{{ state.job.Titre }}</h2>
                <h4 class="card-title text-primary">Description</h4>
                <p class="card-description">{{ state.job.Description }}</p>
                <h4 class="card-title text-primary ">Qualifications</h4>
                <p class="card-description">{{ state.job.Qualifications }}</p>
                <h4 class="card-title text-primary">Start Date</h4>
                <p class="card-description">{{ formatDate(state.job.DateDebut) }}</p>
              </div>
              <div class="card-body"></div>
            </div>
          </div>
          <div class="col-md-6 grid-margin stretch-card">
            <div class="card">
              <div class="card-body left-align">
                <h4 class="card-title text-primary">Responsibilities</h4>
                <p class="card-description">{{ state.job.Responsibilities }}</p>

                <h4 class="card-title text-primary">Required Skills</h4>
                <ul class="list-unstyled">
                  <li v-for="skill in state.job.RequiredSkills" :key="skill">
                    <i class="fa fa-angle-right text-primary me-2"></i>{{ skill }}
                  </li>
                </ul>
                <h4 class="card-title text-primary">Required Langages</h4>
                <ul class="list-unstyled">
                  <li v-for="langage in state.job.RequiredLanguages" :key="langage">
                    <i class="fa fa-angle-right text-primary me-2"></i>{{ langage }}
                  </li>
                </ul>
                <h4 class="card-title text-primary">Contract Duration</h4>
                <ul class="list-unstyled">
                  <li>
                    <i class="fa fa-angle-right text-primary me-2"></i>{{ state.job.ContractDuration }}
                  </li>
                </ul>
              </div>
            
                <router-link class="btn btn-link btn-fw" to="/jobs-admin">Back</router-link>
          
             
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
.left-align {
  text-align: left;
}

</style>
