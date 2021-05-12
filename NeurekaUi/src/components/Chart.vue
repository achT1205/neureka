<template>
  <div>
    <div>
      <h5 v-for="(select, i) in selected" :key="i">
        {{ select }}
        <v-menu
          bottom
          left
          :close-on-content-click="false"
          min-width="500"
          transition="slide-x-transition"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              icon
              v-bind="attrs"
              v-on="on"
              color="primary"
              dark
              fab
              x-small
              @click="selectItem(select)"
            >
              <v-icon>settings</v-icon>
            </v-btn>
          </template>

          <v-card class="pa-3">
            <v-list-item one-line>
              <v-list-item-content>
                <v-list-item-title> Settings </v-list-item-title>
              </v-list-item-content>
            </v-list-item>

            <v-divider></v-divider>
            <v-row>
              <v-col cols="6">
                <v-select
                  outlined
                  v-model="chartType"
                  :items="chartTypes"
                  label="Shose type of chart"
                  placeholder="Shose a type of chart"
                  @change="onChartTypeChange"
                ></v-select>
              </v-col>
              <v-col cols="6">
                <color-picker
                  :value="select.color"
                  :label="'Color'"
                  @updateColor="updateItemColor"
                  placeholder="Color"
                  append-inner-icon="palette"
                />
              </v-col>
            </v-row>
          </v-card>
        </v-menu>
      </h5>
    </div>
    <v-menu
      bottom
      left
      :close-on-content-click="false"
      min-width="400"
      height="400"
      transition="slide-x-transition"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-btn icon v-bind="attrs" v-on="on" color="primary" dark fab small>
          <v-icon>add</v-icon>
        </v-btn>
      </template>

      <v-card class="pa-3">
        <v-container>
          <v-flex transition="slide-x-transition">
            <v-row class="mx-2">
              <v-select
                outlined
                v-model="label"
                :items="reportingLabels"
                label="Shose a data a vizualise"
                placeholder="Shose a data a vizualise"
                @change="handleOnchange"
              ></v-select>
            </v-row>
          </v-flex>
        </v-container>
      </v-card>
    </v-menu>
    <base-chart :chart-options="options" />
  </div>
</template>
<script>
import ColorPicker from "@/components/ColorPicker.vue";
import { mapGetters } from "vuex";
import BaseChart from "./BaseChart";
import { getReportingdata } from "@/store/api";

export default {
  components: {
    ColorPicker,
    BaseChart
  },
  computed: {
    ...mapGetters(["reportingLabels"])
  },
  data() {
    return {
      chartType: "",
      chartTypes: [
        "column",
        "spline",
        "line",
        "area",
        "areaspline",
        "bar",
        "pie",
        "scatter"
      ],
      label: "",
      selected: [],
      options: {
        title: {
          text: ""
        },
        subtitle: {
          text: ""
        },
        legend: {
          align: "right",
          verticalAlign: "middle",
          layout: "vertical"
        },
        yAxis: {
          title: {
            text: "Amount"
          }
        },
        xAxis: {
          categories: []
        },
        tooltip: {
          crosshairs: true,
          shared: true
        },
        credits: {
          enabled: false
        },
        plotOptions: {
          spline: {
            marker: {
              radius: 4,
              lineColor: "#666666",
              lineWidth: 1
            }
          },
          line: {
            dataLabels: {
              enabled: true
            },
            enableMouseTracking: true
          }
        },
        series: [],
        responsive: {
          rules: [
            {
              condition: {
                maxWidth: 500
              },
              chartOptions: {
                legend: {
                  align: "center",
                  verticalAlign: "bottom",
                  layout: "horizontal"
                }
              }
            }
          ]
        }
      }
    };
  },
  methods: {
    onChartTypeChange(type) {
      const index = this.options.series.findIndex(
        s => s.name === this.selectedItem
      );
      this.options.series[index].type = type;
      const color = this.options.series[index].color;
      this.options.series[index].color = "#fff";
      this.options.series[index].color = color;
      this.chartType = "";
    },

    selectItem(item) {
      this.selectedItem = item;
    },
    updateItemColor(color) {
      const index = this.options.series.findIndex(
        s => s.name === this.selectedItem
      );
      this.options.series[index].color = color;
    },
    handleOnchange(e) {
      const index = this.selected.findIndex(s => s.name === e);
      if (index === -1) {
        this.selected.push(e);

        const dto = {
          patientId: this.$route.params.id,
          label: e
        };
        getReportingdata(dto).then(data => {
          const sery = data;
          if (
            data.categories &&
            sery.categories.length > this.options.xAxis.categories.length
          )
            this.options.xAxis.categories = sery.categories;
          delete sery.categories;
          sery.color = "#26aa5a";
          this.options.series.push(sery);
        });
      }
    }
  },
  mounted() {
    this.handleOnchange(this.reportingLabels[1]);
  }
};
</script>
