<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { ref, onMounted, reactive } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'
import { useRoute, useRouter } from 'vue-router'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
export default {
  name: 'EditJob',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const jwtToken = sessionStorage.getItem('jwtToken')

    const offer = reactive({
      Titre: '',
      Description: '',
      Qualifications: '',
      Responsibilities: '',
      DateDebut: '',
      RequiredSkills: [] as string[],
      RequiredLanguages: [] as string[],
      Benefits: [] as string[],
      EstStage: '',
      Departement: '',
      Lieu: '',
      ContractType: '',
      Gender: '',
      NiveauEtudesRequis: '',
      EstActive: ''
    })
    const state = reactive({
      Admin: ''
    })
    const isSaving = ref(false)
    const route = useRoute()
    const router = useRouter()

    const newSkill = ref('')
    const newLanguage = ref('')
    const newBenefit = ref('')
    const addSkill = () => {
      if (newSkill.value.trim() !== '') {
        offer.RequiredSkills.push(newSkill.value.trim())
        newSkill.value = ''
      }
    }
    const removeSkill = (index: number) => {
      offer.RequiredSkills.splice(index, 1)
    }
    const addLanguage = () => {
      if (newLanguage.value.trim() !== '') {
        offer.RequiredLanguages.push(newLanguage.value.trim())
        newLanguage.value = ''
      }
    }
    const removeLanguage = (index: number) => {
      offer.RequiredLanguages.splice(index, 1)
    }
    const addBenefit = () => {
      if (newBenefit.value.trim() !== '') {
        offer.Benefits.push(newBenefit.value.trim())
        newBenefit.value = ''
      }
    }
    const removeBenefit = (index: number) => {
      offer.Benefits.splice(index, 1)
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
        offer.Titre = jobInfo.titre
        offer.Description = jobInfo.description
        offer.Qualifications = jobInfo.qualifications
        offer.Responsibilities = jobInfo.responsibilities
        offer.DateDebut = jobInfo.dateDebut
        offer.RequiredSkills = jobInfo.requiredSkills
        offer.RequiredLanguages = jobInfo.requiredLanguages
        offer.Benefits = jobInfo.benefits
        offer.EstStage = jobInfo.estStage
        offer.Departement = jobInfo.departement
        offer.Lieu = jobInfo.lieu
        offer.ContractType = jobInfo.contractType
        offer.Gender = jobInfo.gender
        offer.NiveauEtudesRequis = jobInfo.niveauEtudesRequis
        offer.EstActive = jobInfo.estActive
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
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    const handleSave = () => {
      isSaving.value = true

      const id = route.params.id
      axios
        .put(`http://localhost:5094/api/Offre/${id}`, offer)
        .then((response) => {
          Swal.fire({
            icon: 'success',
            title: ' updated !',
            showConfirmButton: false,
            timer: 1500
          })
          isSaving.value = false
          const jobInfo = response.data
          offer.Titre = jobInfo.titre
          offer.Description = jobInfo.description
          offer.Qualifications = jobInfo.qualifications
          offer.Responsibilities = jobInfo.responsabilites
          offer.DateDebut = jobInfo.dateDebut
          offer.RequiredSkills = jobInfo.requiredSkills
          offer.RequiredLanguages = jobInfo.requiredLanguages
          offer.Benefits = jobInfo.benefits
          offer.EstStage = jobInfo.estStage
          offer.Departement = jobInfo.departement
          offer.Lieu = jobInfo.lieu
          offer.ContractType = jobInfo.contractType
          offer.Gender = jobInfo.gender
          offer.NiveauEtudesRequis = jobInfo.niveauEtudesRequis
          offer.EstActive = jobInfo.estActive
          resetOffer()
          router.push(`/jobAdminDetail/${id}`)
        })
        .catch((error) => {
          isSaving.value = false
          Swal.fire({
            icon: 'error',
            title: 'Une erreur est survenue !',
            showConfirmButton: false,
            timer: 1500
          })
          console.error('Error updating offer:', error)
        })
    }
    const resetOffer = () => {
      offer.Titre = ''
      offer.Description = ''
      offer.Qualifications = ''
      offer.Responsibilities = ''
      offer.DateDebut = ''
      offer.RequiredSkills = []
      offer.RequiredLanguages = []
      offer.Benefits = []
      offer.EstStage = ''
      offer.Departement = ''
      offer.Lieu = ''
      offer.ContractType = ''
      offer.Gender = ''
      offer.NiveauEtudesRequis = ''
      offer.EstActive = ''
    }
    onMounted(async () => {
      await fetchOfferData()
    })
    return {
      offer,
      isSaving,
      formatDate,
      handleSave,
      fetchOfferData,
      addSkill,
      removeSkill,
      addLanguage,
      removeLanguage,
      addBenefit,
      removeBenefit,
      newSkill,
      newLanguage,
      newBenefit,
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
          <h2 class="text-dark font-weight-bold mb-2">Edit job offer</h2>
        </div>
        <div class="row">
          <br />
          &nbsp;
          <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
                <p class="card-description">Edit the form with informations</p>
                <form class="forms-sample">
                  <div class="d-flex justify-content-center">
                    <!-- Div contenant les 7 premiers inputs -->
                    <div class="first-seven-inputs col-md-5">
                      <div class="form-group">
                        <label for="exampleInputName1">Title</label>
                        <input type="text" class="form-control" v-model="offer.Titre" />
                      </div>
                      <div class="form-group">
                        <label for="exampleInputEmail3">Description</label>
                        <textarea
                          class="form-control"
                          id="exampleInputEmail3"
                          v-model="offer.Description"
                          rows="4"
                        ></textarea>
                      </div>
                      <div class="form-group">
                        <label for="exampleInputQualifications">Qualifications</label>
                        <textarea
                          class="form-control"
                          id="exampleInputQualifications"
                          rows="4"
                          v-model="offer.Qualifications"
                        ></textarea>
                      </div>
                      <div class="form-group">
                        <label>Responsibilities</label>
                        <textarea
                          class="form-control"
                          rows="4"
                          v-model="offer.Responsibilities"
                        ></textarea>
                      </div>
                      <div class="form-group">
                        <label for="exampleInputPassword4">Start Date</label>
                        <input
                          class="form-control"
                          type="date"
                          id="exampleInputPassword4"
                          name="start_date"
                          v-model="offer.DateDebut"
                        />
                      </div>
                      <div class="form-group">
                        <label for="exampleInputSkills">Compétences requises</label>
                        <input
                          type="text"
                          class="form-control"
                          id="exampleInputSkills"
                          v-model="newSkill"
                        />
                        <button type="button" class="btn btn-link" @click="addSkill">
                          Ajouter
                        </button>
                        <ul>
                          <li v-for="(skill, index) in offer.RequiredSkills" :key="index">
                            {{ skill }}
                            <button class="btn btn-outline text-danger" @click="removeSkill(index)">
                              Supprimer
                            </button>
                          </li>
                        </ul>
                      </div>
                      <div class="form-group">
                        <label for="exampleInputLanguages">Langues requises</label>
                        <input
                          type="text"
                          class="form-control"
                          id="exampleInputLanguages"
                          v-model="newLanguage"
                        />
                        <button type="button" class="btn btn-link" @click="addLanguage">
                          Ajouter
                        </button>
                        <ul>
                          <li v-for="(language, index) in offer.RequiredLanguages" :key="index">
                            {{ language }}
                            <button
                              class="btn btn-outline text-danger"
                              @click="removeLanguage(index)"
                            >
                              Supprimer
                            </button>
                          </li>
                        </ul>
                      </div>
                    </div>

                    <!-- Séparateur vertical -->
                    <div class="vertical-separator"></div>

                    <!-- Div contenant le reste des inputs -->
                    <div class="remaining-inputs col-md-5">
                      <div class="form-group">
                        <label for="exampleInputBenefits">Avantages</label>
                        <input
                          type="text"
                          class="form-control"
                          id="exampleInputBenefits"
                          v-model="newBenefit"
                        />
                        <button type="button" class="btn btn-link" @click="addBenefit">
                          Ajouter
                        </button>
                        <ul>
                          <li v-for="(benefit, index) in offer.Benefits" :key="index">
                            {{ benefit }}
                            <button
                              class="btn btn-outline text-danger"
                              @click="removeBenefit(index)"
                            >
                              Supprimer
                            </button>
                          </li>
                        </ul>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectTypeOffer">Offer Type</label>
                        <select
                          v-model="offer.EstStage"
                          class="form-control"
                          id="exampleSelectTypeOffer"
                        >
                          <option :value="true">Intership</option>
                          <option :value="false">Job</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectDepartement">Departement</label>
                        <select class="form-control" v-model="offer.Departement">
                          <option value="Développement web">Développement web</option>
                          <option value="Développement mobile">Développement mobile</option>
                          <option value="Intégration système">Intégration système</option>
                          <option value="RH/Recrutement">RH/Recrutement</option>
                          <option value="Marketing digital">Marketing digital</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectLocation">Location</label>
                        <select class="form-control" v-model="offer.Lieu">
                          <option value="Zarzis">Zarzis</option>
                          <option value="Tunis">Tunis</option>
                          <option value="Lille">Lille</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectContractType">Contract Type </label>
                        <select
                          class="form-control"
                          id="exampleSelectContractType"
                          v-model="offer.ContractType"
                        >
                          <option value="On-site">On site</option>
                          <option value="Remote">Remote</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectGender">Gender</label>
                        <select
                          class="form-control"
                          id="exampleSelectGender"
                          v-model="offer.Gender"
                        >
                          <option>Male</option>
                          <option>Female</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectGrade">Education level required</label>
                        <select
                          class="form-control"
                          id="exampleSelectGrade"
                          v-model="offer.NiveauEtudesRequis"
                        >
                          <option value="Bac+3">Bac + 3</option>
                          <option value="Bac+5">Bac + 5</option>
                          <option value="Master">Master</option>
                        </select>
                      </div>
                      <div class="form-group">
                        <label for="exampleSelectActive">Active</label>
                        <select
                          v-model="offer.EstActive"
                          class="form-control"
                          id="exampleSelectActive"
                        >
                          <option :value="true">Oui</option>
                          <option :value="false">Non</option>
                        </select>
                      </div>
                    </div>
                  </div>
                  <button
                    @click="handleSave()"
                    :disabled="isSaving"
                    type="button"
                    class="btn btn-link mt-3"
                  >
                    Save changes
                  </button>
                  <router-link to="/jobs-admin" type="button" class="btn btn-link mt-3">
                    Cancel
                  </router-link>
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
.d-flex {
  display: flex;
}
.first-seven-inputs {
  margin-right: 20px;
}
.vertical-separator {
  width: 0.5px;
  background-color: #e5e4e4; /* Changez la couleur selon vos besoins */
  margin: 0 20px; /* Espacement de chaque côté du séparateur */
}
select.form-control {
  background-color: #fff; /* Couleur de fond blanche */
  color: #000; /* Couleur du texte noir, changez selon vos besoins */
}
</style>
