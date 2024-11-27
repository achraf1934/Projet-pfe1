<template>
  <h4 class="card-title">Job applications</h4>

  <LineChart :chartData="data" :options="options" />
  {{ console.log('data', data) }}
</template>

<script>
import { LineChart } from 'vue-chart-3'
import { Chart, registerables } from 'chart.js'
import axios from 'axios'
import { computed, onMounted, reactive, ref } from 'vue'

Chart.register(...registerables)

export default {
  name: 'LinearChart',
  components: { LineChart },
  setup() {
    const data = ref({
      labels: [],
      datasets: [
        {
          label: 'Total job applications',

          data: []
        }
      ]
    })

    const options = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top'
        },
        title: {
          display: true,
          text: 'Candidatures over Time'
        }
      },
      scales: {
        x: {
          title: {
            display: true,
            text: 'Time (Months/Years)'
          },

          offset: true,
          grid: {
            drawOnChartArea: false
          }
        },
        y: {
          title: {
            display: true,
            text: 'Applications number'
          },
          beginAtZero: true,
          ticks: {
        callback: function(value) {
          return Number.isInteger(value) ? value : ''; // Afficher uniquement les entiers
        }
      }
        }
      }
    }

    const fetchCandidaturesOverTime = async () => {
      try {
        const response = await axios.get(
          'http://localhost:5094/api/Candidature/GetCandidaturesByMonth'
        )
        const stats = response.data

        data.value.labels = stats.map(
          (stat) => `${stat.year}-${String(stat.month).padStart(2, '0')}`
        )
        data.value.datasets[0].data = stats.map((stat) => stat.candidatureCount)

        console.log('Linechart', data.value)
      } catch (error) {
        console.error('Erreur lors de la récupération des données:', error)
      }
    }

    fetchCandidaturesOverTime()
    return {
      fetchCandidaturesOverTime,
      data,
      options
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
