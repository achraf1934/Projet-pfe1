<script lang="ts">
import axios from 'axios'
import FooterShow from '../footer/FooterShow.vue'
import NavbarShow from '../navbar/NavbarShow.vue'
import { onMounted, reactive } from 'vue'
import router from '../../router'
import Swal from 'sweetalert2'
import { useAuthStore } from '../../stores/auth'

export default {
  name: 'ProfileUser',
  components: {
    NavbarShow,
    FooterShow
  },
  setup() {
    const jwtToken = sessionStorage.getItem('jwtToken')
    const authStore = useAuthStore()

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
    const logout = () => {
      authStore.state.isLoggedIn = false
      router.push('/login')

      sessionStorage.removeItem('jwtToken')
      sessionStorage.removeItem('isAdmin')
    }
    const formatDate = (date: Date) => {
      return new Date(date).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }
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
    const deleteUser = async (id: string) => {
      try {
        const response = await axios.delete(`http://localhost:5094/api/Account/DeleteUser/${id}`)
        logout()
        Swal.fire({
          icon: 'success',
          title: ' Account Removed !',
          showConfirmButton: false,
          timer: 1500
        })
        return response
      } catch (error) {
        Swal.fire({
          icon: 'error',
          title: 'Error removing  !',
          showConfirmButton: false,
          timer: 1500
        })
        return error
      }
    }
    const editProfile = (appUserId: string) => {
      router.push(`/edit-profile/${appUserId}`)
    }
    const interview = (appUserId: string) => {
      router.push(`/interview-user/${appUserId}`)
    }
    onMounted(() => {
      fetchUser()
    })
    console.error("la récupération de l'utilisateur:", state.currentUser)

    return {
      state,
      fetchUser,
      formatDate,
      editProfile,
      deleteUser,
      interview
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
          Profile
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
                <h4 class="mb-3 text-left">Email</h4>
                <p class="text-left">
                  <i class="fa fa-angle-right text-primary me-2"></i>{{ state.currentUser.email }}
                </p>
                <h4 class="mb-4 text-left">Phone</h4>
                <p class="text-left">
                  <i class="fa fa-angle-right text-primary me-2"></i
                  >{{ state.currentUser.phoneNumber }}
                </p>
                <h4 class="mb-4 text-left">Joined</h4>
                <p class="m-0 text-left">
                  <i class="fa fa-angle-right text-primary me-2"></i
                  >{{ formatDate(state.currentUser.dateCreated) }}
                </p>
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
                <button class="btn btn-link" @click="interview(state.currentUser.Id)">Interviews</button>
              </p>
            </div>
            <div class="col-lg-12"><hr /></div>
            <button class="btn btn-link" @click="editProfile(state.currentUser.Id)">
              <i class="fa fa-edit"></i> Edit profile
            </button>
            <div class="col-lg-12">
              <button
                class="btn btn-link"
                @click="deleteUser(state.currentUser.Id)"
                style="color: red"
              >
                <i class="fa fa-trash"></i> Delete account
              </button>
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
.profile {
  max-width: 600px;
  margin: 50px auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
  background-color: #f9f9f9;
}
.profile-header {
  text-align: center;
}
.profile-picture {
  width: 150px;
  height: 150px;
  border-radius: 50%;
  margin-bottom: 20px;
}
.profile-name {
  margin: 0;
  color: #333;
}
.profile-info,
.profile-skills {
  margin-top: 20px;
}
.info-heading,
.skills-heading {
  color: #333;
}
.skills-list {
  list-style-type: none;
  padding: 0;
}
.skills-list li {
  margin-bottom: 5px;
  color: #555;
}
</style>
