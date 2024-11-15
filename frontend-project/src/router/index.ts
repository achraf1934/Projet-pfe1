import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

import HomePage from '@/views/HomePage.vue'
import LoginPage from '@/views/LoginPage.vue'
import RegisterPage from '@/views/RegisterPage.vue'
import accessDenied from '@/views/access-denied.vue'
import ContactUser from '@/views/ContactUser.vue'
import NotFound from '@/views/NotFound.vue'
import HomePageAdmin from '@/views/HomePageAdmin.vue'
import AboutPage from '@/views/AboutPage.vue'
import AllJobList from '@/components/JobUser/AllJobList.vue'
import JobDetail from '@/components/JobUser/JobDetail.vue'
import CreateJob from '@/components/Admin/Jobs/CreateJob.vue'
import ProfileAdmin from '@/components/Admin/Profile/ProfileAdmin.vue'
import CandidatesList from '@/components/Admin/Candidates/CandidatesList.vue'
import JobsAdmin from '@/components/Admin/Jobs/JobsAdmin.vue'
import JobApplicationList from '@/components/JobApplicationUser/JobApplicationList.vue'
import EditJob from '@/components/Admin/Jobs/EditJob.vue'
import ProfileUser from '@/components/Profile-User/ProfileUser.vue'
import JobsAdminDetails from '@/components/Admin/Jobs/JobsAdminDetails.vue'
import ApplicationsByOffer from '@/components/Admin/JobsApplications/ApplicationsByOffer.vue'
import ApplicationDetail from '@/components/Admin/JobsApplications/ApplicationDetail.vue'
import OffreListByCategory from '@/components/ByCategory/OffreListByCategory.vue'
import CreateResume from '@/components/Resume/CreateResume.vue'
import ForgotPassword from '@/components/password/ForgotPassword.vue'
import ResetPassword from '@/components/password/ResetPassword.vue'
import VerifyEmail from '@/components/password/VerifyEmail.vue'
import JobApplicationDetail from '@/components/JobApplicationUser/JobApplicationDetail.vue'
import InterviewForm from '@/components/Admin/JobsApplications/InterviewForm.vue'
import InterviewList from '@/components/Admin/interviewAdmin/InterviewList.vue'
import EditProfile from '@/components/Profile-User/EditProfile.vue'
import InterviewUserList from '@/components/InterviewUser/InterviewUserList.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomePage
    },
    {
      path: '/dashboard',
      component: HomePageAdmin,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/login',
      component: LoginPage
    },
    {
      path: '/signup',
      component: RegisterPage
    },
    {
      path: '/access-denied',
      component: accessDenied
    },
    {
      path: '/about',
      component: AboutPage
    },
    {
      path: '/contact',
      component: ContactUser
    },
    {
      path: '/profile-admin',
      component: ProfileAdmin,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/candidate-list',
      component: CandidatesList,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/jobs-admin',
      component: JobsAdmin,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/job-applications/:offerId',
      component: ApplicationsByOffer,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/interview/:appUserId/:offreId',
      component: InterviewForm,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/ShowInterviewByOffer/:offreId',
      component: InterviewList,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/createJob',
      component: CreateJob,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/jobAdminDetail/:id',
      component: JobsAdminDetails,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/editJob/:id',
      component: EditJob,
      meta: {
        requiresAdmin: true
      }
    },
    {
      path: '/jobs',
      component: AllJobList
    },
    {
      path: '/jobDetail/:id',
      component: JobDetail
    },
    {
      path: '/job-applicationsC',
      component: JobApplicationList
    },
    {
      path: '/interview-user/:appUserId',
      component: InterviewUserList
    },
    {
      path: '/application-details/:appUserId/:offreId',
      component: ApplicationDetail
    },
    {
      path: '/Jobapplication-details/:appUserId/:offreId',
      component: JobApplicationDetail
    },
    {
      path: '/profile-candidat',
      component: ProfileUser
    },
    {
      path: '/edit-profile/:id',
      component: EditProfile
    },
    {
      path: '/createResume',
      component: CreateResume
    },
    {
      path: '/forgotpassword',
      component: ForgotPassword
    },
    {
      path: '/resetpassword',
      component: ResetPassword
    },
    {
      path: '/verify-email',
      component: VerifyEmail
    },
    {
      path: '/offers/:departement',
      component: OffreListByCategory
    },
    {
      path: '/:catchAll(.*)',
      component: NotFound
    }
  ]
})
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAdmin && !authStore.state.isAdmin) {
    return next('/access-denied')
  }
  next()
})

export default router
