<template>
  <v-row justify="center">
    <v-dialog
      v-model="localdialog"
      fullscreen
      hide-overlay
      persistent
      transition="dialog-bottom-transition"
    >
      <v-card>
        <v-toolbar dark color="primary">
          <v-btn icon dark @click="$emit('close')">
            <v-icon>close</v-icon>
          </v-btn>
          <v-toolbar-title>Reporting</v-toolbar-title>
        </v-toolbar>
        <v-flex>
          <draggable
            class="row mx-2 reporting-container"
            tag="div"
            v-model="charts"
            :options="dragOptions"
          >
            <v-col
              class="align-center justify-space-between list-group-item chart-item"
              v-for="chart in charts"
              :cols="chart.col"
              :key="uuid(chart)"
            >
              <line-chart v-if="chart.type === 'line'" />
            </v-col>
          </draggable>
        </v-flex>
        <v-flex>
          <v-menu
            v-model="menu"
            :close-on-content-click="false"
            :nudge-width="200"
            :max-height="700"
            offset-x
          >
            <template v-slot:activator="{ on }">
              <v-btn
                bottom
                color="primary"
                dark
                fab
                small
                fixed
                right
                v-on="on"
              >
                <v-icon>add</v-icon>
              </v-btn>
            </template>
            <v-card class="pa-3">
              <v-flex>
                <v-list>
                  <draggable
                    v-model="chartTypes"
                    :options="availableItemOptions"
                    :clone="handleClone"
                  >
                    <transition-group type="transition" :name="'flip-list'">
                      <v-list-item
                        class="list-group-item"
                        v-for="element in chartTypes"
                        :key="element.name"
                      >
                        <v-list-item-action>
                          <v-icon light>{{ element.icon }}</v-icon>
                        </v-list-item-action>
                        <v-list-item-title>{{
                          element.name
                        }}</v-list-item-title>
                      </v-list-item>
                    </transition-group>
                  </draggable>
                </v-list>
              </v-flex>
            </v-card>
          </v-menu>
        </v-flex>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
import draggable from "vuedraggable";
import uuidMixin from "@/mixins/uuidMixin";
import cloneMixin from "@/mixins/cloneMixin";
import LineChart from "@/components/dashboards/LineChart.vue";

export default {
  mixins: [uuidMixin, cloneMixin],
  props: ["dialog", "close"],
  components: {
    draggable,
    LineChart
  },
  data() {
    return {
      localdialog: false,
      chartTypes: [
        {
          id: null,
          name: "Line chart",
          icon: "show_chart",
          type: "line",
          col: "4"
        }
      ],
      charts: [],
      menu: false,
      dragOptions: {
        animation: 0,
        group: "charts"
      },
      availableItemOptions: {
        group: {
          name: "charts",
          pull: "clone",
          put: false
        },
        sort: false
      }
    };
  },
  watch: {
    dialog(val) {
      this.localdialog = val;
    }
  }
};
</script>

<style scoped>
.reporting-container {
  height: 100vh;
  margin-top: 10px;
}

.chart-item {
  max-height: 400px;
}
</style>
