<script lang="ts">
import FooterShow from '../components/footer/FooterShow.vue'

export default {
  name: 'AccessDenied',
  components: {
    FooterShow
  }
}
</script>
<template>
     <div class="container-xxl bg-white p-0">


     <!-- 404 Start -->
     <div class="container-xxl py-5 wow fadeInUp" data-wow-delay="0.1s">
            <div class="container text-center">
                <div class="row justify-content-center">
                    <div class="col-lg-6">
                        <i class="bi bi-exclamation-triangle display-1 text-primary"></i>
                        <h1 class="display-1">403</h1>
                        <h1 class="mb-4">Unauthorized</h1>
                        <p class="mb-4">We’re sorry, the page you have looked for is authorized only for admin</p>
                        <router-link  class="btn btn-primary py-3 px-5" to="/">Go Back To Home</router-link>
                    </div>
                </div>
            </div>
        </div>
        <!-- 404 End -->
</div>
  <FooterShow />
</template>
<style></style>
