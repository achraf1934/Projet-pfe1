<script lang="ts">
import SidebarShow from '../Sidebar/SidebarShow.vue'
import { onMounted, reactive, ref } from 'vue'
import axios from 'axios'
import NavbarAdmin from '../../navbar-admin/NavbarAdmin.vue'
import router from '../../../router';

export default {
  name: 'JobsAdmin',
  components: {
    NavbarAdmin,
    SidebarShow
  },
  setup() {
    const state = reactive({
      Admin: ''
    })
    const Jobs = ref<any[]>([])
    const jwtToken = localStorage.getItem('jwtToken')

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
    const fetchJobsList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetAllOffre')
        Jobs.value = response.data
      } catch (error) {
        return error
      }
    }
    const show = ( jobId: number) => {
      // Redirection vers une autre page avec les détails de la candidature
      router.push(`/jobAdminDetail/${jobId}`)
    }
    onMounted(async () => {
      await fetchJobsList()
      console.log('All jobs : ', Jobs)
    })
    const currentAdmin = fetchUser()

    return {
      fetchJobsList,
      Jobs,
      fetchUser,
      show,
      currentAdmin
    }
  }
}
</script>
<template>
  <div class="container-scroller">
    <link rel="stylesheet" href="/src/assets/admin/css/style.css" />

    <NavbarAdmin :currentAdmin="currentAdmin" />
    <div class="container-fluid page-body-wrapper">
      <SidebarShow :currentAdmin="currentAdmin" />
      <div class="main-panel">
        <div class="content-wrapper">
          <div class="d-xl-flex justify-content-between align-items-start">
            <h2 class="text-dark font-weight-bold mb-2">Job overview</h2>
          </div>
          <hr />
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
                      >Job</a
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
                      >Intership</a
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
                  <div class="row">
                    <div
                      class="col-xl-3 col-lg-6 col-sm-6 grid-margin stretch-card"
                      v-for="job in Jobs"
                      :key="job.id"
                      
                    >
                      <div class="card">
                        <div class="card-body text-center">
                          <h2 class="mb-4 text-dark font-weight-bold">{{ job.titre }}</h2>

                         <i class="mdi mdi-eye btn-icon-append"  @click="show(job.id)"  ></i>
                        </div>
                        <p class="card-description ">
                            <router-link :to="`/job-applications/${job.id}`" class="btn btn-link"
                              >Candidates</router-link
                            >
                          </p>
                          <p  class="card-description">
                            <router-link :to="`/ShowInterviewByOffer/${job.id}`" class="btn btn-link"
                              >Interviews</router-link
                            >
                          </p>
                      <hr>
                        <router-link
                          :to="`/editJob/${job.id}`"
                          type="button"
                          class="btn btn-primary btn-fw"
                        >
                          <i class="mdi mdi-file-check btn-icon-append"></i> Edit </router-link
                        >
                        <router-link
                          :to="`/jobAdminDetail/${job.id}`"
                          type="button"
                          class="btn custom-btn btn-fw"
                          ><i class="mdi mdi-delete">Delete</i>
                        </router-link>
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
                  <div class="row">
                    <div
                      class="col-xl-6 col-lg-6 col-sm-6 grid-margin stretch-card"
                      v-for="job in Jobs"
                      :key="job.id"
                    >
                      <div class="card">
                        <div class="card-body text-center">
                          <h2 class="mb-4 text-dark font-weight-bold">{{ job.titre }}</h2>
                          <div
                            class="dashboard-progress dashboard-progress-1 d-flex align-items-center justify-content-center item-parent"
                          ></div>
                          <p class="mt-4 mb-0">Candidates</p>
                          <h3 class="mb-0 font-weight-bold mt-2 text-dark">5443</h3>
                          <router-link to="/Edit-Job" class="btn purchase-button">Edit</router-link>
                        </div>
                      </div>
                    </div>
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
<style scooped>
.custom-btn {
  background-color: #181824; /* Couleur de fond */
  color: #181824 ; /* Couleur du texte */
  border-color: #181824; /* Bordure, si nécessaire */
}

.custom-btn:hover {
  background-color: #212130; /* Couleur de fond au survol */
}

.description {
  /* Spécifiez la hauteur maximale à afficher */
  max-height: 3 * 1.2em; /* 1.2em est généralement la hauteur d'une ligne de texte */

  /* Permet au texte supplémentaire de dépasser la hauteur maximale spécifiée */
  overflow: hidden;

  /* Garantit que le texte supplémentaire est masqué */
  text-overflow: ellipsis;

  /* Assurez-vous que le texte débordant est correctement coupé */
  white-space: nowrap;

  /* Facultatif : ajoutez une marge inférieure pour plus de clarté */
  margin-bottom: 60px;
}
</style>
