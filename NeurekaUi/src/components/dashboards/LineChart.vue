<template>
  <div>
    <div>
      <h5 v-for="(select, i) in selected" :key="i">
        {{ select }}
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
              @click="selectItem(select)"
            >
              <v-icon>settings</v-icon>
            </v-btn>
          </template>

          <v-card class="pa-3">
            <v-row>
              <v-col>
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
          </v-flex>
        </v-container>
      </v-card>
    </v-menu>
    <chart :chart-options="options" />
  </div>
</template>
<script>
import ColorPicker from "@/components/ColorPicker.vue";
import { mapGetters } from "vuex";
import Chart from "./Chart";
import { getReportingdata } from "@/store/api";

export default {
  components: {
    ColorPicker,
    Chart
  },
  computed: {
    ...mapGetters(["reportingLabels"])
  },
  data() {
    return {
      label: "",
      selected: [],
      options: {
        title: {
          text: ""
        },
        subtitle: {
          text: ""
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
          }
        },
        series: []
      }
    };
  },
  methods: {
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
