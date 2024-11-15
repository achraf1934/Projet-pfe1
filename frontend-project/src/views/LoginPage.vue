<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h1 class="text-center mb-5 wow fadeInUp" data-wow-delay="0.1s">Login</h1>
        <div class="row justify-content-center align-items-center">
          <div class="col-md-6 wow fadeInUp" data-wow-delay="0.1s">
            <div class="wow fadeInUp" data-wow-delay="0.5s">
              <form @submit.prevent="submit">
                <div class="row g-3">
                  <div class="col-12">
                    <div class="form-floating">
                      <input
                        type="text"
                        class="form-control"
                        v-model="data.email"
                        placeholder="email"
                      />
                      <label for="email">Email</label>
                    </div>
                  </div>
                  <div class="col-12 position-relative">
                    <div class="form-floating">
                      <input
                        :type="passwordFieldType"
                        class="form-control"
                        v-model="data.password"
                        placeholder="password"
                        style="padding-right: 35px"
                      /><i
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
                      ></i>
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
                      <label for="password">Password</label>
                    </div>
                  </div>
                  <router-link to="/forgotpassword">Forgot password ?</router-link>
                  <p>
                    Do not have an account ?
                    <router-link to="/signup">Join us</router-link>
                  </p>
                  <div class="col-12">
                    <button class="btn btn-primary w-100 py-3" type="submit">Sign in</button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
    <router-link to="/" class="btn btn-primary mb-3">Back to Home</router-link>
  </div>
</template>

<script lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { jwtDecode } from 'jwt-decode'
import { useAuthStore } from '../stores/auth'
import Swal from 'sweetalert2'

export default {
  name: 'LoginPage',
  components: {},
  setup() {
    const data = reactive({
      email: '',
      password: ''
    })
    const router = useRouter()
    const submit = async () => {
      try {
        const response = await fetch('http://localhost:5094/api/Account/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify(data)
        })
        Swal.fire({
          icon: 'success',
          title: 'Welcome  back!',
          showConfirmButton: false,
          timer: 1500
        })
        const authStore = useAuthStore()
        const responseData = await response.text()
        const jwtToken = responseData

        authStore.state.isLoggedIn = true
        //await router.push('/')
        sessionStorage.setItem('jwtToken', jwtToken)
        const decodedToken: any = jwtDecode(jwtToken)

        if (decodedToken.role == 'User') {
          authStore.state.isAdmin = false
          sessionStorage.setItem('isAdmin', 'false')
          router.push('/')
        } else {
          authStore.state.isAdmin = true
          sessionStorage.setItem('isAdmin', 'true')
          router.push('/dashboard')
        }
      } catch (error: any) {
          Swal.fire({
            icon: 'error',
            title: 'login failed !',
            showConfirmButton: false,
            text: error.message,
            timer: 3000
          })
        return error
      }
    }
    const showPassword = ref(false)
    const passwordFieldType = ref('password')

    const toggleShowPassword = () => {
      showPassword.value = !showPassword.value
      passwordFieldType.value = showPassword.value ? 'text' : 'password'
    }

    return {
      data,
      submit,
      showPassword,
      toggleShowPassword,
      passwordFieldType
    }
  }
}
</script>

<style scoped></style>
