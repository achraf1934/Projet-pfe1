<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import axios from 'axios'
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import Swal from 'sweetalert2'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
export default {
  name: 'CreateJob',
  components: {
    SidebarShow,
    NavbarAdmin
  },
  setup() {
    const jwtToken = localStorage.getItem('jwtToken')
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
      ContractDuration: '',
      Gender: '',
      NiveauEtudesRequis: '',
      EstActive: ''
    })
    const isSaving = ref(false)
    const newSkill = ref('')
    const newLanguage = ref('')
    const newBenefit = ref('')
    const currentStep = ref(1) // Étape actuelle
    const totalSteps = 4 // Nombre total d'étapes
    const nextStep = () => {
      if (currentStep.value < totalSteps) currentStep.value++
    }
    const prevStep = () => {
      if (currentStep.value > 1) currentStep.value--
    }

    const isLastStep = computed(() => currentStep.value === totalSteps)
    const isFirstStep = computed(() => currentStep.value === 1)
    const addSkill = () => {
      if (newSkill.value.trim() !== '') {
        offer.RequiredSkills.push(newSkill.value.trim())
        newSkill.value = ''
      }
    }

    const addLanguage = () => {
      if (newLanguage.value.trim() !== '') {
        offer.RequiredLanguages.push(newLanguage.value.trim())
        newLanguage.value = ''
      }
    }

    const addBenefit = () => {
      if (newBenefit.value.trim() !== '') {
        offer.Benefits.push(newBenefit.value.trim())
        newBenefit.value = ''
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
        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const router = useRouter()
    const handleSave = async () => {
      isSaving.value = true
      try {
        const response = await axios.post('http://localhost:5094/api/Offre/AddOffre', offer)
        Swal.fire({
          icon: 'success',
          title: 'Offer created!',
          showConfirmButton: false,
          timer: 1500
        })
        isSaving.value = false
        resetOffer()
        router.push('/jobs-admin')
        return response
      } catch (error) {
        isSaving.value = false
        Swal.fire({
          icon: 'error',
          title: 'Erreur creation offer !',
          showConfirmButton: false,
          timer: 3000
        })
        return error
      }
    }

    const resetOffer = () => {
      offer.Titre = ''
      offer.Description = ''
      offer.Qualifications = ''
      offer.Responsibilities = ''
      offer.DateDebut = ''
      offer.EstStage = ''
      offer.Departement = ''
      offer.Lieu = ''
      offer.ContractType = ''
      offer.ContractDuration = ''
      offer.NiveauEtudesRequis = ''
      offer.EstActive = ''
    }

    const currentAdmin = fetchUser()
    return {
      offer,
      isSaving,
      handleSave,
      currentAdmin,
      newSkill,
      newLanguage,
      newBenefit,
      addSkill,
      addLanguage,
      addBenefit,
      currentStep,
      nextStep,
      prevStep,
      isLastStep,
      isFirstStep
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
          <h2 class="text-dark font-weight-bold mb-2">Create a job / intership offer</h2>
        </div>
        <hr />
        <div class="row">
          <div
            class="d-sm-flex justify-content-between align-items-center transaparent-tab-border {"
          ></div>
          <br />
          <div class="col-md-11 mx-auto">
            <div class="card text-left">
              <div class="card-body">
                <p class="card-description">Fill the form with informations</p>

                <form class="forms-control" @submit.prevent="handleSave">
                  <div v-if="currentStep === 1">
                    <div class="form-group">
                      <label for="exampleInputName1">Title</label>
                      <input
                        type="text"
                        class="form-control"
                        id="exampleInputName1"
                        v-model="offer.Titre"
                      />
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
                  </div>
                  <!-- Étape 2 -->
                  <div v-if="currentStep === 2">
                    <div class="form-group">
                      <label for="exampleInputPassword4">Start Date</label>
                      &nbsp;
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
                      <button class="btn btn text-success" type="button" @click="addSkill">
                        Ajouter
                      </button>
                      <ul>
                        <li v-for="(skill, index) in offer.RequiredSkills" :key="index">
                          {{ skill }}
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
                      <button class="btn btn text-success" type="button" @click="addLanguage">
                        Ajouter
                      </button>
                      <ul>
                        <li v-for="(language, index) in offer.RequiredLanguages" :key="index">
                          {{ language }}
                        </li>
                      </ul>
                    </div>

                    <div class="form-group">
                      <label for="exampleInputBenefits">Avantages</label>
                      <input
                        type="text"
                        class="form-control"
                        id="exampleInputBenefits"
                        v-model="newBenefit"
                      />
                      <button class="btn btn text-success" type="button" @click="addBenefit">
                        Ajouter
                      </button>
                      <ul>
                        <li v-for="(benefit, index) in offer.Benefits" :key="index">
                          {{ benefit }}
                        </li>
                      </ul>
                    </div>
                  </div>

                  <!-- Étape 3 -->
                  <div v-if="currentStep === 3">
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
                        <option value="Web Developement">Web Developement</option>
                        <option value="Mobile Developement">Mobile Developement</option>
                        <option value="Integration systeme">Integration systeme</option>
                        <option value="HR/Recrutement">HR/Recrutement</option>
                        <option value="Digital Marketing">Digital Marketing</option>
                      </select>
                    </div>

                    <div class="form-group">
                      <label for="exampleSelectLocation">Location</label>
                      <input
                        type="text"
                        class="form-control"
                        id="exampleInputLocation"
                        v-model="offer.Lieu"
                      />
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
                  </div>
                  <!-- Étape 4 -->
                  <div v-if="currentStep === 4">
                    <div class="form-group">
                      <label for="exampleSelectGender">Gender</label>
                      <select class="form-control" id="exampleSelectGender" v-model="offer.Gender">
                        <option>Male</option>
                        <option>Female</option>
                        <option>Both</option>
                      </select>
                    </div>

                    <div class="form-group">
                      <label for="exampleSelectGrade">Education level required</label>
                      <input
                        type="text"
                        class="form-control"
                        id="exampleInputGrade"
                        v-model="offer.NiveauEtudesRequis"
                      />
                    </div>
                    <div class="form-group">
                      <label for="exampleSelectActive">Active</label>
                      <select
                        v-model="offer.EstActive"
                        class="form-control"
                        id="exampleSelectActive"
                      >
                        <option :value="true">yes</option>
                        <option :value="false">no</option>
                      </select>
                    </div>
                    <div class="form-group">
                      <label for="exampleSelectContractDuration">Contract Duration </label>
                      <select
                        class="form-control"
                        id="exampleSelectContractDuration"
                        v-model="offer.ContractDuration"
                      >
                        <option value="CDI">CDI</option>
                        <option value="CDD">CDD</option>
                        <option value="Intership">Intership</option>
                      </select>
                    </div>
                  </div>

                  <!-- Navigation -->
                  <div class="navigation-buttons">
                    <button
                      type="button"
                      class="btn btn-secondary"
                      @click="prevStep"
                      :disabled="isFirstStep"
                    >
                      Previous</button
                    >&nbsp;
                    <button
                      type="button"
                      class="btn btn-primary"
                      @click="isLastStep ? handleSave() : nextStep()"
                    >
                      {{ isLastStep ? 'Submit' : 'Next' }}
                    </button>
                  </div>
                  <br />
                  <hr />
                  <button type="submit" class="btn btn-primary btn-icon-text">
                    <i class="mdi mdi-file-check btn-icon-prepend"></i>
                    Submit
                  </button>
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
.text-center {
  text-align: center;
}
</style>
