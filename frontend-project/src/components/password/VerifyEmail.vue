<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h2 class="text-center mb-5 wow fadeInUp" data-wow-delay="0.1s">Email Verification</h2>
        <div class="row justify-content-center align-items-center">
          <div class="col-md-6 wow fadeInUp" data-wow-delay="0.1s">
            <div class="wow fadeInUp" data-wow-delay="0.5s">
              <div>
                <h1></h1>
                <p v-if="verificationStatus">{{ verificationStatus }}</p>
              </div>
              <div class="col-12">
                  <router-link to="/login"  type="submit">Login</router-link>
                </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import axios from 'axios'

export default defineComponent({
  name: 'VerifyEmail',
  setup() {
    const route = useRoute() // Accéder aux paramètres de l'URL
    const verificationStatus = ref<string | null>(null)

    onMounted(async () => {
      const token = route.query.token as string
      const email = route.query.email as string

      try {
        const response = await axios.post('http://localhost:5094/api/account/verify-email', {
          token,
          email
        })
        verificationStatus.value = response.data.message
      } catch (error) {
        verificationStatus.value = 'Verification failed. The token may be invalid or expired.'
      }
    })

    return { verificationStatus }
  }
})
</script>
