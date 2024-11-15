<script lang="ts">
import { useRoute } from 'vue-router'
import axios from 'axios'
import { onMounted, ref } from 'vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import FooterShow from '../footer/FooterShow.vue'
export default {
  name: 'JobApplicationDetail',
  components: {
    NavbarShow,
    FooterShow
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
    const getCandidatureByUserName = async () => {
      try {
        const appUserId = route.params.appUserId
        const offreId = route.params.offreId
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

    const jobApp = getCandidatureByUserName()
    onMounted(async () => {
      await getCandidatureByUserName()
    })
    console.log('jobApp', candidature)
    return {
      candidature,
      showIframe,
      showIframeCV,
      toggleIframe,
      toggleIframeCV,
      getCandidatureByUserName,
      jobApp,
      formatDate,
      fetchUser
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
                <h3 class="mb-3">{{ candidature.offre.titre }}</h3>
                <span class="text-truncate me-3"
                  ><i class="fa fa-map-marker text-primary me-2"></i
                  >{{ candidature.offre.lieu }}</span
                >
                <span class="text-truncate me-3"
                  ><i class="far fa-clock text-primary me-2"></i
                  >{{ candidature.offre.contractType }}</span
                >
              </div>
            </div>
<hr>
            <div class="mb-5">
              <h4 class="mb-3 text-left">Job Description</h4>
              <p v-if="candidature.offre.description" class="text-left">
                {{ candidature.offre.description }}
              </p>
              <p v-else>No description available.</p>

              <h4 class="mb-3 text-left">Qualifications</h4>
              <p v-if="candidature.offre.qualifications" class="text-left">
                {{ candidature.offre.qualifications }}
              </p>
              <p v-else>No qualifications listed.</p>

              <h4 class="mb-3 text-left">Responsibilities</h4>
              <p v-if="candidature.offre.responsibilities" class="text-left">
                {{ candidature.offre.responsibilities }}
              </p>
              <p v-else>No responsibilities outlined.</p>
            </div>
            <h4 class="mb-3 text-left">Decision</h4>
            <p v-if="candidature.decision" class="text-left">
              {{ candidature.decision }}
            </p>
            <p v-else>No responsibilities outlined.</p>
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
                {{ formatDate(candidature.offre.postedDate) }}
              </p>
              <p class="text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Job Nature :
                {{ candidature.offre.contractType }}
              </p>
              <p class="text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Location :
                {{ candidature.offre.lieu }}
              </p>
              <p class="m-0 text-left">
                <i class="fa fa-angle-right text-primary me-2"></i>Start Date :
                {{ formatDate(candidature.offre.dateDebut) }}
              </p>
            </div>
            <div
              class="bg-light rounded p-5 wow slideInUp"
              data-wow-delay="0.1s"
              style="visibility: visible; animation-delay: 0.1s; animation-name: slideInUp"
            >
              <h4 class="mb-3 text-left">Required Skills</h4>
              <ul class="list-unstyled">
                <li
                  v-for="skill in candidature.offre.requiredSkills"
                  :key="skill"
                  class="text-left"
                >
                  <i class="fa fa-angle-right text-primary me-2 text-left" aria-hidden="true"></i
                  >{{ skill }}
                </li>
              </ul>

              <h4 class="mb-3 text-left">Required Languages</h4>
              <ul class="list-unstyled">
                <li
                  v-for="language in candidature.offre.requiredLanguages"
                  :key="language"
                  class="text-left"
                >
                  <i class="fa fa-angle-right text-primary me-2 text-left" aria-hidden="true"></i
                  >{{ language }}
                </li>
              </ul>

              <h4 class="mb-3 text-left">Benefits</h4>
              <ul class="list-unstyled">
                <li v-for="benefit in candidature.offre.benefits" :key="benefit" class="text-left">
                  <i class="fa fa-angle-right text-primary me-2" aria-hidden="true"></i
                  >{{ benefit }}
                </li>
              </ul>
            </div>
          </div>
          <hr>
          <div class="mb-5">
            <h4 class="mb-4">Application Details</h4>

            <div class="row g-3">
              <div class="col-12 col-sm-6">
                <div class="detail">
                  <button
                    @click="toggleIframeCV"
                    type="button"
                    class="btn btn-link btn-rounded btn-fw"
                  >
                    <i class="fa fa-eye icon-eye"></i>
                    Cv
                  </button>
                  <div v-if="showIframeCV">
                    <iframe
                      :src="'http://localhost:5094/api/Candidature/cv/' + candidature.cvFileName"
                      style="width: 100%; height: 600px"
                    ></iframe>
                  </div>
                </div>
              </div>
              <div class="col-12 col-sm-6">
                <div class="detail">
                  <button
                    @click="toggleIframe"
                    type="button"
                    class="btn btn-link btn-rounded btn-fw"
                  >
                    <i class="fa fa-eye icon-eye"></i> Cover letter
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
              </div>
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
