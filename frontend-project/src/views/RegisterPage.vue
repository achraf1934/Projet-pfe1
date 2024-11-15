<script lang="ts">
import Swal from 'sweetalert2'
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'RegisterPage',
  components: {},
  setup() {
    const data = reactive({
      username: '',
      email: '',
      password: '',
      phoneNumber: ''
    })
    const router = useRouter()
    const showPassword = ref(false)
    const passwordFieldType = ref('password')
    const toggleShowPassword = () => {
      showPassword.value = !showPassword.value
      passwordFieldType.value = showPassword.value ? 'text' : 'password'
    }
    const submit = async () => {
      try {
        const response = await fetch('http://localhost:5094/api/Account/register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(data)
        })

        if (!response.ok) {
          const errorMessage = await response.text()
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: errorMessage,
            showConfirmButton: false,
            timer: 3000
          })
          throw new Error(errorMessage)
        }

        Swal.fire({
          icon: 'success',
          title: 'Welcome!',
          showConfirmButton: false,
          timer: 1500
        })
        await router.push('/login')
      } catch (error: any) {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: error.message
        })
      }
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

<template>
  <div class="container-xxl bg-white p-0">
    <div class="container-xxl py-5">
      <div class="container">
        <h1 class="text-center mb-5 wow fadeInUp" data-wow-delay="0.1s">Sign up for free</h1>
        <div class="row justify-content-center align-items-center">
          <div class="col-md-3 wow fadeInUp" data-wow-delay="0.1s">
            <div class="wow fadeInUp" data-wow-delay="0.5s">
              <form @submit.prevent="submit">
                <div class="row g-3">
                  <div class="col-md-6">
                    <div class="form-floating">
                      <input
                        type="text"
                        class="form-control"
                        v-model="data.username"
                        placeholder="username"
                      />
                      <label for="name">Username</label>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="form-floating">
                      <input
                        type="tel"
                        id="phone"
                        name="phone"
                        class="form-control"
                        v-model="data.phoneNumber"
                        required
                        placeholder="phone number"
                      />
                      <label for="phone">phone</label>
                    </div>
                  </div>
                  <div class="col-md-12">
                    <div class="form-floating">
                      <input
                        type="email"
                        class="form-control"
                        v-model="data.email"
                        placeholder="Your Email"
                      />
                      <label for="email">Your Email</label>
                    </div>
                  </div>

                  <div class="col-md-12">
                    <div class="form-floating">
                      <input
                        :type="passwordFieldType"
                        required
                        class="form-control"
                        v-model="data.password"
                        placeholder="Your password"
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
                        >&nbsp;</i
                      >
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
                      <label for="password">Your password</label>
                    </div>
                  </div>

                  <div class="col-12">
                    <button class="btn btn-primary w-100 py-3" type="submit">Sign up</button>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
    <router-link to="/login" class="btn btn-primary mb-3">Back to login</router-link>
  </div>
</template>
<style scoped></style>
