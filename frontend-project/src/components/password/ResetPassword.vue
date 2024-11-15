<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h1 class="text-center mb-5 wow fadeInUp" data-wow-delay="0.1s">Reset Password</h1>
        <div class="row justify-content-center align-items-center">
          <div class="col-md-6 wow fadeInUp" data-wow-delay="0.1s">
            <div class="wow fadeInUp" data-wow-delay="0.5s">
              <form @submit.prevent="submitResetPassword">
                <div class="row g-3">
                  <div class="col-12 position-relative">
                    <input
                      :type="passwordFieldType"
                      v-model="newPassword"
                      placeholder="New password"
                      required
                      class="form-control"
                    />
                    <i
                      v-if="!showPassword"
                      class="fa fa-eye icon-eye"
                      style="
                        color: green;
                        cursor: pointer;
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        transform: translateY(-50%);
                      "
                      @click="toggleShowPassword"
                    >&nbsp;</i>
                    <i
                      v-else
                      class="fa fa-eye-slash icon-eye-off"
                      style="
                        color: green;
                        cursor: pointer;
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        transform: translateY(-50%);
                      "
                      @click="toggleShowPassword"
                    ></i>
                  </div>
                  <div class="col-12 position-relative">
                    <input
                      :type="passwordFieldType"
                      placeholder="Confirm password"
                      required
                      class="form-control"
                    />
                    <i
                      v-if="!showPassword"
                      class="fa fa-eye icon-eye"
                      style="
                        color: green;
                        cursor: pointer;
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        transform: translateY(-50%);
                      "
                      @click="toggleShowPassword"
                    >&nbsp;</i>
                    <i
                      v-else
                      class="fa fa-eye-slash icon-eye-off"
                      style="
                        color: green;
                        cursor: pointer;
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        transform: translateY(-50%);
                      "
                      @click="toggleShowPassword"
                    ></i> 
                  </div>
                </div>
                <hr />
                <div class="col-12">
                  <button class="btn btn-primary w-100 py-3" type="submit">Reset Password</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { onMounted, ref } from 'vue'
import axios from 'axios'
import { useRoute } from 'vue-router'

export default {
  name: 'ResetPassword',
  setup() {
    const route = useRoute()
    const email1 = ref('')
    const token1 = ref(route.query.token || '')
    const newPassword = ref('')
    const token = route.query.token as string
    const email = route.query.email as string

    const showPassword = ref(false)
    const passwordFieldType = ref('password')
    const toggleShowPassword = () => {
      showPassword.value = !showPassword.value
      passwordFieldType.value = showPassword.value ? 'text' : 'password'
    }
    const submitResetPassword = async () => {
      try {
        await axios.post('http://localhost:5094/api/account/reset-password', {
          email: email,
          token: token,
          newPassword: newPassword.value
        })
        alert('Password reset successfully!')
      } catch (error) {
        console.error(error)
        alert('Error resetting password.')
      }
    }
    onMounted(() => {
      console.log('Token:', token)
      console.log('Email:', email)
    })
    return {
      email,
      token,
      newPassword,
      submitResetPassword,
      showPassword,
      toggleShowPassword,
      passwordFieldType
    }
  }
}
</script>
