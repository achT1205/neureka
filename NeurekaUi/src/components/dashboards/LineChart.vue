<script>
import { Line } from "vue-chartjs";

export default {
  extends: Line,
  mounted() {
    this.renderChart(this.dts, this.options);
  },
  data() {
    return {
      dts: {
        labels: [
          "January",
          "February",
          "Mars",
          "April",
          "Mai",
          "June",
          "July",
          "August",
          "September",
          "October"
        ],
        datasets: [
          {
            data: [10, 26, 15, 8, 35, 13, 18, 45, 10, 13],
            label: "Visit per month",
            borderColor: "#3e95cd",
            fill: true
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        height: 100
      }
    };
  }
};
</script>
