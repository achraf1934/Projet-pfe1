<template>
  <h4 class="card-title">Job applications</h4>

               <LineChart :chartData="data" :options="options" />


</template>

<script>
import { LineChart } from 'vue-chart-3'
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

export default {
  name: 'LinearChart',
  components: { LineChart },
  setup() {
    const data = {
      labels: ['2020', '2021', '2022', '2023', '2024', '2025'],
      datasets: [
        {
          data: [12, 19, 3, 5, 2, 3]
        }
      ]
    }

    const options = {
      scales: {
        x: {},
        y: {}
      }
    }

    return { data, options }
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
