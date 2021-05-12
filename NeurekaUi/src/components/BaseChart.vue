<template>
  <highcharts class="hc" ref="lineCharts" :options="chartOptions"></highcharts>
</template>

<script>
import { Chart } from "highcharts-vue";
import Highcharts from "highcharts";
import exportingInit from "highcharts/modules/exporting";
import stockInit from "highcharts/modules/stock";
import offlineExporting from "highcharts/modules/offline-exporting";
import exportdata from "highcharts/modules/export-data";

stockInit(Highcharts);
exportingInit(Highcharts);
offlineExporting(Highcharts);
exportdata(Highcharts);

export default {
  props: ["chartOptions"],
  components: {
    highcharts: Chart
  },
  data() {
    return {};
  }
};
</script>

<style>
.hc {
  height: 400px;
}
</style>
