<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h1 class="text-center mb-5 wow fadeInUp" data-wow-delay="0.1s">Forgot Password</h1>
        <div class="row justify-content-center align-items-center">
          <div class="col-md-6 wow fadeInUp" data-wow-delay="0.1s">
            <div class="wow fadeInUp" data-wow-delay="0.5s">
              <div>
                <form @submit.prevent="submitForgotPassword">
                  <div class="row g-3">
                    <div class="col-12">
                      <input
                        v-model="email"
                        class="form-control"
                        type="email"
                        placeholder="Enter your email"
                        required
                      />
                      <hr>
                      <button class="btn btn-primary" type="submit">
                        Send Reset Link
                      </button>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { ref } from 'vue'
import axios from 'axios'

export default {
  name: 'ForgotPassword',
  setup() {
    const email = ref('')

    const submitForgotPassword = async () => {
      try {
        await axios.post('http://localhost:5094/api/account/forgotpassword', { email: email.value })
        alert('Reset link sent. Check your email.')
      } catch (error) {
        console.error(error)
        alert('Error sending reset link.')
      }
    }

    return { email, submitForgotPassword }
  }
}
</script>
