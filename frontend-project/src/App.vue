<script setup lang="ts">
import { onMounted } from 'vue';
import { RouterView } from 'vue-router'
import { useRouter } from 'vue-router';

const router = useRouter();

onMounted(() => {
  const isAdmin = localStorage.getItem('isAdmin');
  if (isAdmin === 'true') {
    router.push('/dashboard'); // Rediriger vers la page d'administration
  } 
});
</script>

<template>
  <main>
    <RouterView />
  </main>
</template>

<style scoped></style>
