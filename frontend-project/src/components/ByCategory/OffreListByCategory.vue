<template>
  <navbar-show />
  <div class="container-xxl py-5">
    <div class="container">
      <h1
        class="text-center mb-5 wow fadeInUp"
        data-wow-delay="0.1s"
        style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
      >
        Offers in {{ departement }}
      </h1>
      <div
        class="tab-class text-center wow fadeInUp"
        data-wow-delay="0.3s"
        style="visibility: visible; animation-delay: 0.3s; animation-name: fadeInUp"
      >
        <ul class="nav nav-pills d-inline-flex justify-content-center border-bottom mb-5">
          <li class="nav-item">
            <a
              class="d-flex align-items-center text-start mx-3 ms-0 pb-3 active"
              data-bs-toggle="pill"
              href="#tab-1"
            >
              <h6 class="mt-n1 mb-0">{{ departement }}</h6>
            </a>
          </li>
        </ul>
        <div class="tab-content">
          <div id="tab-1" class="tab-pane fade show p-0 active">
            <div class="job-item p-4 mb-4" v-for="offre in offres" :key="offre.id">
              <div class="row g-4">
                <div class="col-sm-12 col-md-8 d-flex align-items-center">
                  <div class="text-start ps-4">
                    <h5 class="mb-3">{{ offre.titre }}</h5>
                    <span class="text-truncate me-3"
                      ><i class="fa fa-map-marker text-primary me-2"></i>{{ offre.lieu }}</span
                    >
                    <span class="text-truncate me-3"
                      ><i class="far fa-clock text-primary me-2"></i>{{ offre.contractType }}
                    </span>
                  </div>
                </div>
                <div
                  class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                >
                  <div class="d-flex mb-3">
                    <a class="btn btn-light btn-square me-3" href=""
                      ><i class="far fa-heart text-primary"></i
                    ></a>
                    <router-link class="btn btn-primary" :to="`/jobDetail/${offre.id}`"
                        >Apply Now</router-link
                      >
                  </div>
                  <small class="text-truncate"
                    ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                    {{ formatDate(offre.dateDebut) }}</small
                  >
                </div>
              </div>
            </div>

            <a class="btn btn-primary py-3 px-5" href="">Browse More Jobs</a>
          </div>
        </div>
      </div>
    </div>
  </div>
  <footer-show />
</template>

<script lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import NavbarShow from '../navbar/NavbarShow.vue'
import FooterShow from '../footer/FooterShow.vue'

export default {
  name: 'OffreListByCategory',
  components: {
    NavbarShow,
    FooterShow
  },
  setup() {
    const route = useRoute()
    const departement = route.params.departement as string
    const offres = ref<any[]>([])
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    onMounted(async () => {
      try {
        const response = await fetch(`http://localhost:5094/api/Offre/ByDepartement/${departement}`)
        if (!response.ok) throw new Error('Network response was not ok')
        offres.value = await response.json()
      } catch (error) {
        console.error('Error fetching offers:', error)
        offres.value = []
      }
    })

    return {
      departement,
      offres,
      formatDate
    }
  }
}
</script>
