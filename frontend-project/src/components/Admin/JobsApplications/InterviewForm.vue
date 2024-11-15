<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

  <NavbarAdmin :currentAdmin="{}" />
  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="{}" />

    <div class="main-panel">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Schedule Interview</h2>
        </div><hr>
        <div class="row">
          <div class="col-md-12 grid-margin stretch-card">
            <div class="card">
              <div class="card-body">
                <div class="schedule-interview">
                  <h2 class="card-title">Schedule an interview appointment</h2>
                  <form @submit.prevent="submitAppointment">
                    <div>
                      <label class="card-description" for="date">Choose a date </label>&nbsp;
                      <input
                        class="card-description"
                        v-model="appointmentDateTime"
                        type="datetime-local"
                        id="appointmentDateTime"
                        required
                      />
                    </div>

                    <button type="submit" :disabled="isButtonDisabled">Fixer l'entretien</button>
                  </form>
                  <p class="card-description" v-if="appointmentStatus">{{ appointmentStatus }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import axios from 'axios'
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import SidebarShow from '../Sidebar/SidebarShow.vue'
import router from '../../../router'
export default {
  name: 'InterviewForm',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const route = useRoute()
    const appUserId = route.params.appUserId
    const offreId = route.params.offreId
    const appointmentDateTime = ref('')

    const appointmentStatus = ref('')
    const isButtonDisabled = ref(false)

    const jwtToken = sessionStorage.getItem('jwtToken')

    // Envoi de la demande de rendez-vous au backend
    const submitAppointment = async () => {
      try {
        isButtonDisabled.value = true
        const response = await axios.post(
          'http://localhost:5094/api/Interview/schedule', // Remplacez par votre URL de backend
          {
            appUserId,
            offreId,
            appointmentDateTime: appointmentDateTime.value
          },
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`,
              'Content-Type': 'application/json'
            }
          }
        )
        appointmentStatus.value = 'Rendez-vous fixé avec succès !'
        router.push(`/ShowInterviewByOffer/${offreId}`)
        return response
      } catch (error) {
        appointmentStatus.value = 'Erreur lors de la fixation du rendez-vous.'
        console.error(error)
      } finally {
        isButtonDisabled.value = false
      }
    }

    return {
      appointmentDateTime,
      appointmentStatus,
      isButtonDisabled,
      submitAppointment
    }
  }
}
</script>

<style scoped>
.schedule-interview {
  max-width: 600px;
  margin: 50px auto;
  padding: 20px;
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

h2 {
  color: #333;
}

form {
  display: flex;
  flex-direction: column;
}

label {
  margin: 10px 0 5px;
  font-weight: bold;
}

input {
  padding: 8px;
  margin-bottom: 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

button {
  background-color: #007bff;
  color: white;
  padding: 10px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:disabled {
  background-color: #ccc;
}

p {
  margin-top: 20px;
  font-size: 16px;
}
</style>
