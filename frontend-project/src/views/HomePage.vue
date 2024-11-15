<template>
  <div class="container-xxl bg-white p-0">
  <NavbarShow :currentAdmin="{}" />
  <CarrouselShow />
  <CategoryList />
  <JobListing />
  <FooterShow />
</div>
</template>
<script lang="ts">
import CategoryList from '../components/ByCategory/CategoryList.vue'
import JobListing from '../components/JobListing/JobListing.vue'
import CarrouselShow from '../components/carrousel/CarrouselShow.vue'
import FooterShow from '../components/footer/FooterShow.vue'
import NavbarShow from '../components/navbar/NavbarShow.vue'
export default {
  name: 'HomePage',
  components: {
    FooterShow,
    NavbarShow,
    CarrouselShow,
    CategoryList,
    JobListing
  }
}
</script>
<style></style>
