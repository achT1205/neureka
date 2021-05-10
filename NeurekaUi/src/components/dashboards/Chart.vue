<template>
  <div>
    <div>
      <h5 v-for="(select, i) in selected" :key="i">
        {{ select.name }}
        <v-menu
          bottom
          left
          :close-on-content-click="false"
          min-width="400"
          height="400"
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
              @click="selectItem(select, i)"
            >
              <v-icon>settings</v-icon>
            </v-btn>
          </template>

          <v-card class="pa-3">
            <v-container>
              <v-flex transition="slide-x-transition">
                <v-row class="mx-2">
                  <color-picker
                    :value="select.color"
                    :label="'Color'"
                    @updateColor="updateItemColor"
                    placeholder="Color"
                    append-inner-icon="palette"
                  />
                </v-row>
              </v-flex>
            </v-container>
          </v-card>
        </v-menu>
        <v-btn icon color="primary" dark fab x-small @click.prevent="">
          <v-icon>visibility</v-icon>
        </v-btn>
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
            <v-row class="mx-2">
              <color-picker
                :value="color"
                :label="'Color'"
                @updateColor="updateColor"
                placeholder="Color"
                append-inner-icon="palette"
              />
            </v-row>
          </v-flex>
        </v-container>
      </v-card>
    </v-menu>
    <line-chart :options="options" :chart-data="chartdata" />
  </div>
</template>
<script>
import LineChart from "@/components/dashboards/LineChart.vue";
import ColorPicker from "@/components/ColorPicker.vue";
import { mapGetters } from "vuex";
import { getReportingdata } from "@/store/api";

export default {
  props: ["chart"],
  components: {
    LineChart,
    ColorPicker,
  },
  computed: {
    ...mapGetters(["reportingLabels"]),
  },
  data() {
    return {
      selectedItem: null,
      selectedIndex: null,
      selected: [],
      color: "#3e95cd",
      label: null,
      currentData: null,
      chartdata: {
        datasets: [],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        height: 100,
      },
    };
  },
  methods: {
    selectItem(item, index) {
      this.selectedItem = item;
      this.selectedIndex = index;
    },
    updateItemColor(color) {
      const copy = { ...this.chartdata };
      this.chartdata = {
        labels: this.chartdata.labels,
        datasets: [],
      };
      this.chartdata.datasets[this.selectedIndex] = {
        data: copy.datasets[this.selectedIndex].data,
        label: copy.datasets[this.selectedIndex].label,
        borderColor: color,
        fill: true,
      };
    },
    updateColor(color) {
      this.color = color;
      this.chartdata = {
        labels: this.currentData.labels,
        datasets: [
          {
            data: this.currentData.dataSets[0].data,
            label: this.currentData.dataSets[0].label,
            borderColor: this.color,
            fill: true,
          },
        ],
      };
    },
    handleOnchange(e) {
      let index = this.selected.indexOf((s) => s.name.toLowerCase === e.toLowerCase());
      if (index > -1) return;

      if (index === -1) {
        index = this.selected.length;
        this.selected.push({
          name: e,
          color: "#3e95cd",
          fill: true,
        });
      }

      const dto = {
        patientId: this.$route.params.id,
        label: e,
      };
      getReportingdata(dto).then((data) => {
        this.currentData = { ...data };

        this.chartdata = {
          labels: data.labels,
          datasets:
            this.chartdata.datasets && this.chartdata.datasets.length > 0
              ? this.chartdata.datasets
              : [],
        };

        this.chartdata.datasets.push({
          data: data.dataSets[0].data,
          label: data.dataSets[0].label,
          borderColor: "#3e95cd",
          fill: true,
        });
      });
    },
  },

  mounted() {},
};
</script>
