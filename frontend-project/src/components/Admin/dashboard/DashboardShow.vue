<!-- <template>
   <div class="row">
          <div class="col-md-6">
    <div style="width: 400px">
      <div style="display: flex; justify-content: right">
        <DoughnutChart v-bind="doughnutChartProps" />
      </div>
    </div>
  </div></div>
  </template>
  -->
<template>
  <h4 class="card-title">Candidates per offer</h4>

  <DoughnutChart v-bind="doughnutChartProps" />
</template>
<script lang="ts">
import { computed, defineComponent, ref } from 'vue'
import { DoughnutChart, useDoughnutChart } from 'vue-chart-3'
import { Chart, type ChartData, type ChartOptions, registerables } from 'chart.js'

Chart.register(...registerables)
export default defineComponent({
  name: 'App',
  components: { DoughnutChart },
  setup() {
    const dataValues = ref([30, 40, 60, 70, 5])
    const dataLabels = ref([
      'Web developement',
      'Mobile developement',
      'Integration system',
      'DevOps',
      'Others'
    ])
    const toggleLegend = ref(true)

    const testData = computed<ChartData<'doughnut'>>(() => ({
      labels: dataLabels.value,
      datasets: [
        {
          data: dataValues.value,
          backgroundColor: ['#77CEFF', '#0079AF', '#123E6B', '#97B0C4', '#A5C8ED']
        }
      ]
    }))

    const options = computed<ChartOptions<'doughnut'>>(() => ({
      scales: {
        myScale: {
          type: 'logarithmic',
          position: toggleLegend.value ? 'left' : 'right'
        }
      },
      plugins: {
        legend: {
          position: toggleLegend.value ? 'top' : 'bottom'
        },
        
      }
    }))

    const { doughnutChartProps, doughnutChartRef } = useDoughnutChart({
      chartData: testData,
      options
    })

    let index = ref(20)

    function shuffleData() {
      // dataValues.value = shuffle(dataValues.value);
      dataValues.value.push(index.value)
      dataLabels.value.push('Other' + index.value)
      console.log(dataValues.value)
      index.value++
    }

    function switchLegend() {
      toggleLegend.value = !toggleLegend.value
    }

    return {
      shuffleData,
      switchLegend,
      testData,
      options,
      doughnutChartRef,
      doughnutChartProps
    }
  }
})
</script>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
