<script lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import FooterShow from '../footer/FooterShow.vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import axios from 'axios'
import Swal from 'sweetalert2'
import { useRoute, useRouter } from 'vue-router'

export default {
  name: 'InterviewUserList',
  components: {
    NavbarShow,
    FooterShow
  },
  setup() {
    const Interviews = ref<any[]>([])
    const jwtToken = sessionStorage.getItem('jwtToken')
    const router = useRouter()
    const route = useRoute()

    const currentPage = ref(1) // Page actuelle
    const itemsPerPage = 5 // Nombre d'offres par page
    const paginatedJobs = computed(() =>
      Interviews.value.slice(
        (currentPage.value - 1) * itemsPerPage,
        currentPage.value * itemsPerPage
      )
    )
    const changePage = (page: number) => {
      currentPage.value = page
    }
    const totalPages = computed(() => Math.ceil(Interviews.value.length / itemsPerPage))

    const fetchUser = async () => {
      try {
        const response = await axios.get(`http://localhost:5094/api/Account/user`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`
          },
          withCredentials: true
        })
        const UserInfo = response.data
        state.currentUser.Id = UserInfo.id
        state.currentUser.userName = UserInfo.userName
        state.currentUser.normalizedUserName = UserInfo.normalizedUserName
        state.currentUser.email = UserInfo.email
        state.currentUser.phoneNumber = UserInfo.phoneNumber
        state.currentUser.dateCreated = UserInfo.dateCreated
        console.log('current user : ', UserInfo)
        return response.data
      } catch (error) {
        console.error("Erreur lors de la récupération de l'utilisateur:", error)
        throw error
      }
    }
    const state = reactive({
      currentUser: {
        Id: '',
        userName: '',
        normalizedUserName: '',
        phoneNumber: '',
        email: '',
        dateCreated: new Date()
      }
    })
    const appUserId = route.params.appUserId

    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }

    const handleRowClick = (appUserId: string, offreId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/Jobapplication-details/${appUserId}/${offreId}`)
    }
    const fetchInterviewsList = async () => {
      try {
        console.log('id', state.currentUser)
        const response = await axios.get(
          `http://localhost:5094/api/Interview/get-interviews-byUser/${appUserId}`,
          {
            headers: {
              Authorization: `Bearer ${jwtToken}`
            },
            withCredentials: true
          }
        )
        Interviews.value = response.data
        return response
      } catch (error) {
        console.log(error)
        return error
      }
    }

    const handleDelete = async (appUserId: string, offreId: number) => {
      Swal.fire({
        title: 'Êtes-vous sûr ?',
        text: 'Cette action est irréversible!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Oui, supprimer!'
      }).then(async (result) => {
        if (result.isConfirmed) {
          try {
            // Appel à l'API avec les paramètres nécessaires
            const response = await axios.delete(
              `http://localhost:5094/api/Candidature/Delete/${appUserId}/${offreId}`,
              {
                headers: {
                  Authorization: `Bearer ${jwtToken}`
                },
                withCredentials: true
              }
            )
            Swal.fire({
              icon: 'success',
              title: 'Candidature supprimée avec succès!',
              showConfirmButton: false,
              timer: 1500
            })
            await fetchInterviewsList() // Mise à jour de la liste après suppression
            return response
          } catch (error) {
            Swal.fire({
              icon: 'error',
              title: 'Une erreur est survenue lors de la suppression!',
              showConfirmButton: false,
              timer: 1500
            })
            console.error('Error deleting application:', error)
            return error
          }
        }
      })
    }
    const interview = (appUserId: string) => {
      router.push(`/interview-user/${appUserId}`)
    }
    onMounted(() => {
      fetchUser()
      fetchInterviewsList()
    })
    return {
      Interviews,
      formatDate,
      state,
      interview,
      fetchInterviewsList,
      handleDelete,
      handleRowClick,
      fetchUser,
      currentPage,
      paginatedJobs,
      changePage,
      totalPages
    }
  }
}
</script>
<template>
  <div class="container-xxl bg-white p-0">
    <NavbarShow :currentAdmin="state.currentUser" />
    <div
      class="container-xxl py-5 wow fadeInUp"
      data-wow-delay="0.1s"
      style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
    >
      <div class="container">
        <h1
          class="text-center mb-5 wow fadeInUp"
          data-wow-delay="0.1s"
          style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
        >
          Your Interviews
        </h1>
        <hr />
        <div class="row gy-5 gx-4">
          <div class="col-lg-8">
            <div class="d-flex align-items-center mb-5">
              <img
                class="flex-shrink-0 img-fluid border rounded"
                src="/src/assets/img/user.png"
                alt=""
                style="width: 80px; height: 80px"
              />
              <div class="text-start ps-4">
                <h3 class="mb-4">{{ state.currentUser.userName }}</h3>
              </div>
            </div>
            <hr />
            <div class="mb-5">
              <div
                class="bg-light rounded p-5 mb-4 wow slideInUp"
                data-wow-delay="0.1s"
                style="visibility: visible; animation-delay: 0.1s; animation-name: slideInUp"
              >
                <div class="col" v-for="interview in Interviews" :key="interview.id">
                  {{ console.log(interview) }}
                  <div class="job-item p-4 mb-4">
                    <div class="d-flex align-items-center">
                      <div class="text-start ps-1 flex-grow-1">
                        <h5 class="mb-3">{{ interview.offre.titre }}</h5>

                        <span class="text-truncate me-3"
                          ><i class="far fa-calendar-alt text-primary me-2"></i>Date:
                          {{ new Date(interview.appointmentDateTime).toLocaleString() }}
                        </span>
                        <div class="text-start ps-1 flex-grow-1 text-left" v-if="interview.link">
                          <a
                            class="btn btn-link"
                            :href="interview.link"
                            target="_blank"
                            rel="noopener noreferrer"
                          >
                            <h6 class="text-primary text-left"><br /></h6>
                            {{ interview.link }}
                          </a>
                        </div>
                        <div class="text-start ps-1 flex-grow-1" v-else>
                          <a class="text-truncate me-3">
                            <h6 class="text-primary text-left"><br /></h6>

                            <h6 class="text-primary text-left">No link yet !</h6>
                          </a>
                        </div>
                      </div>

                      <span class="text-truncate me-3">
                        <i class="far fa fa-eye icon-eye text-primary me-2"></i>
                      </span>

                      <span
                        class="text-truncate me-3"
                        @click="handleDelete(interview.appUserId, interview.offreId)"
                      >
                        <i class="far fa-trash-alt text-primary me-2"></i>
                      </span>
                    </div>
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
          </div>
          <div class="col-lg-4">
            <div
              class="bg-light rounded p-5 wow slideInUp"
              data-wow-delay="0.1s"
              style="visibility: visible; animation-delay: 0.1s; animation-name: slideInUp"
            >
              <h4 class="mb-4 text-left">Actions</h4>
              <p class="m-0 text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>
                <router-link class="btn btn-link" to="/job-applicationsC"
                  >Job applications</router-link
                >
              </p>
              <p class="m-0 text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>
                <button class="btn btn-link" @click="interview(state.currentUser.Id)">
                  Interviews
                </button>
              </p>
            </div>
            <div class="col-lg-12"><hr /></div>
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
