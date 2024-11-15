
<script lang="ts">
import { onMounted, ref } from 'vue';
import axios from 'axios';

export default {
  name: 'CategoryList',
  setup() {
    const categories = ref<string[]>([])

    // Fonction pour récupérer les catégories (départements)
    const fetchCategories = async () => {
      try {
        const response = await axios.get('http://localhost:5094/api/offre/departements') // Modifier l'URL selon votre API
        categories.value = response.data
      } catch (error) {
        console.error('Erreur lors de la récupération des catégories:', error)
      }
    }

    // Récupérer les catégories lors du montage du composant
    onMounted(() => {
      fetchCategories()
    })
    return {categories}
  }
}
</script>
<template>
  <div class="container-xxl py-5">
    <div class="container">
      <h1
        class="text-center mb-5 wow fadeInUp"
        data-wow-delay="0.1s"
        style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
      >
        Explore By Category
      </h1><hr>
      <div class="row g-4">
        <div
         v-for="(category, index) in categories"
          :key="index"
          class="col-lg-3 col-sm-6 wow fadeInUp"
          data-wow-delay="0.1s"
          style="visibility: visible; animation-delay: 0.1s; animation-name: fadeInUp"
        >
        <a class="cat-item rounded p-4" :href="`/offers/${category}`">
            <h6 class="mb-3">{{ category }}</h6>
            <!-- Vous pouvez également afficher le nombre d'offres si nécessaire -->
            <p class="mb-0">{{ category }} offers</p>
          </a>
        </div>
   
      </div>
    </div>
  </div>
</template>
<style></style>
