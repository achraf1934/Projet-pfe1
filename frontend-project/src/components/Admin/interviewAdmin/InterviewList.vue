<template>
  <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

  <NavbarAdmin :currentAdmin="{}" />
  <div class="container-fluid page-body-wrapper">
    <SidebarShow :currentAdmin="{}" />

    <div class="main-panel">
      <div class="content-wrapper">
        <div class="d-xl-flex justify-content-between align-items-start">
          <h2 class="text-dark font-weight-bold mb-2">Interview List</h2>
        </div>
        <hr />
        <div class="row">
          <div class="col-md-12 grid-margin stretch-card">
            <div
              class="col-xl-3 col-lg-6 col-sm-6 grid-margin stretch-card"
              v-for="interview in interviews"
              :key="interview.interviewId"
            >
              <div class="card">
                <div class="card-body" v-if="interviews">
                  <h1 class="card-title">Offre: {{ interview.offre.titre }}</h1>
                  <i
                    class="mdi mdi-eye btn-icon-append"
                    @click="handleRowClick(interview.appUserId, interview.offreId)"
                  ></i>

                  <p class="card-description">Candidat: {{ interview.appUser.userName }}</p>
                  <p class="card-description">Email: {{ interview.appUser.email }}</p>
                  <h4 ass="card-title">
                    Date: {{ new Date(interview.appointmentDateTime).toLocaleString() }}
                  </h4>

                  <hr />
                  <a
                    v-if="interview.link"
                    :href="interview.link"
                    target="_blank"
                    rel="noopener noreferrer"
                    class="btn btn-link"
                  >
                    {{ interview.link }}
                  </a>
                  <form action="forms-sample">
                    <input type="text" class="form-control" v-model="interview.link" />&nbsp;<br />
                    <button
                      @click="handleSave(interview.interviewId, interview.link)"
                      :disabled="isSaving"
                      type="submit"
                      class="btn btn-primary"
                    >
                      Send link
                    </button>
                  </form>
                </div>
                <p v-else>Aucun entretien programmé pour le moment.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useRoute } from 'vue-router'
import SidebarShow from '../Sidebar/SidebarShow.vue'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import router from '../../../router'
import Swal from 'sweetalert2'

export default {
  name: 'InterviewList',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const route = useRoute()
    const interviews = ref<any>()
    const isSaving = ref(false)

    const jwtToken = sessionStorage.getItem('jwtToken')
    const offreId = route.params.offreId
    const fetchInterviews = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5094/api/Interview/get-interviews-byOffer/${offreId}`,
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`
            },
            withCredentials: true
          }
        )
        interviews.value = response.data
        return response.data
      } catch (error) {
        console.error('Erreur lors de la récupération des entretiens:', error)
      }
    }
    const handleSave = (interviewId: number, link: string) => {
      isSaving.value = true

      axios
        .put(`http://localhost:5094/api/Interview/${interviewId}`, { link })
        .then((response) => {
          Swal.fire({
            icon: 'success',
            title: ' updated !',
            showConfirmButton: false,
            timer: 1500
          })
          isSaving.value = false
          link=''
          return response
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

    const handleRowClick = (appUserId: string, offreId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/application-details/${appUserId}/${offreId}`)
    }
    onMounted(() => {
      fetchInterviews()
    })

    return {
      interviews,
      fetchInterviews,
      handleRowClick,
      isSaving,
      handleSave
    }
  }
}
</script>

<style scoped>
h2 {
  color: #333;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  margin-bottom: 20px;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

p {
  margin: 5px 0;
}
</style>
