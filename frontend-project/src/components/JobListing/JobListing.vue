<script lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'
import Swal from 'sweetalert2'
export default {
  name: 'JobListing',
  setup() {
    const jobs = ref<any[]>([])
    const jobsOffer = ref<any[]>([])
    const interships = ref<any[]>([])
    const formatDate = (date: Date) => {
      // Méthode pour formater la date
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
    const fetchjobList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetAllOffre')
        jobs.value = response.data
        return response
      } catch (error) {
        return error
      }
    }
    const fetchjobOfferList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetOffreEmplois')
        jobsOffer.value = response.data
        return response
      } catch (error) {
        return error
      }
    }
    const fetchIntershipList = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/Offre/GetOffreStage')
        interships.value = response.data
        return response
      } catch (error) {
        return error
      }
    }
    const handleDelete = async (id: string) => {
      Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
      }).then(async (result) => {
        if (result.isConfirmed) {
          try {
            const response = await axios.delete(`http://localhost:5094/api/Offre/DeleteOffre/${id}`)
            Swal.fire({
              icon: 'success',
              title: 'Offre supprimée avec success!',
              showConfirmButton: false,
              timer: 1500
            })
            fetchjobList()
            return response
          } catch (error) {
            Swal.fire({
              icon: 'error',
              title: 'An Error Occured!',
              showConfirmButton: false,
              timer: 1500
            })
            return error
          }
        }
      })
    }
    onMounted(async () => {
      await fetchjobList()
      await fetchjobOfferList()
      await fetchIntershipList()
    })

    return {
      jobs,
      jobsOffer,
      interships,
      formatDate,
      fetchjobList,
      handleDelete,
      fetchjobOfferList,
      fetchIntershipList
    }
  }
}
</script>
<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h1
          class="text-center mb-5 wow fadeInUp"
          data-wow-delay="0.1s"
          style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
        >
          Job Listing
        </h1><hr>
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
                <h6 class="mt-n1 mb-0">Featured</h6>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="d-flex align-items-center text-start mx-3 pb-3"
                data-bs-toggle="pill"
                href="#tab-2"
              >
                <h6 class="mt-n1 mb-0">Jobs</h6>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="d-flex align-items-center text-start mx-3 me-0 pb-3"
                data-bs-toggle="pill"
                href="#tab-3"
              >
                <h6 class="mt-n1 mb-0">Interships</h6>
              </a>
            </li>
          </ul>
          <div class="tab-content">
            <div id="tab-1" class="tab-pane fade show p-0 active">
              <div class="job-item p-4 mb-4" v-for="job in jobs.slice(0, 3)" :key="job.id">
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ job.titre }}</h5>
                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i>{{ job.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i>{{ job.contractType }}</span
                      >
                   
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                   
                      <router-link class="btn btn-primary" :to="`/jobDetail/${job.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Started On :
                      {{ formatDate(job.dateDebut) }}</small
                    >
                  </div>
                </div>
              </div>
              <router-link class="btn btn-primary py-3 px-5" to="/jobs"
                >Browse More Jobs</router-link
              >
            </div>
            <div id="tab-2" class="tab-pane fade show p-0">
              <div class="job-item p-4 mb-4" v-for="jobOffer in jobsOffer.slice(0, 3)" :key="jobOffer.id">
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ jobOffer.titre }}</h5>
                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i>{{ jobOffer.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i>{{ jobOffer.contractType }}</span
                      >
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                    
                      <router-link class="btn btn-primary" :to="`/jobDetail/${jobOffer.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                      {{ formatDate(jobOffer.dateDebut) }}</small
                    >
                  </div>
                </div>
              </div>
            </div>
            <div id="tab-3" class="tab-pane fade show p-0">
              <div class="job-item p-4 mb-4" v-for="intership in interships.slice(0, 3)" :key="intership.id">
                <div class="row g-4">
                  <div class="col-sm-12 col-md-8 d-flex align-items-center">
                    <div class="text-start ps-4">
                      <h5 class="mb-3">{{ intership.titre }}</h5>
                      <span class="text-truncate me-3"
                        ><i class="fa fa-map-marker text-primary me-2"></i>{{ intership.lieu }}</span
                      >
                      <span class="text-truncate me-3"
                        ><i class="far fa-clock text-primary me-2"></i>{{ intership.contractType }}</span
                      >
                    </div>
                  </div>
                  <div
                    class="col-sm-12 col-md-4 d-flex flex-column align-items-start align-items-md-end justify-content-center"
                  >
                    <div class="d-flex mb-3">
                    
                      <router-link class="btn btn-primary" :to="`/jobDetail/${intership.id}`"
                        >Apply Now</router-link
                      >
                    </div>
                    <small class="text-truncate"
                      ><i class="far fa-calendar-alt text-primary me-2"></i>Date Line:
                      {{ formatDate(intership.dateDebut) }}</small
                    >
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
<style></style>
