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
          <v-spacer></v-spacer>
          <v-toolbar-items>
            <v-btn dark text @click.prevent=""> Save </v-btn>
          </v-toolbar-items>
        </v-toolbar>
        <v-flex>
          <draggable
            class="row mx-2 reporting-container"
            tag="div"
            v-model="charts"
            :options="dragOptions"
          >
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="chart in charts"
              :cols="chart.col"
              :key="uuid(chart)"
            >
              <chart :chart="chart" />
              <v-icon light>{{ chart.icon }}</v-icon> {{ chart.name }}
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
              <v-btn bottom color="primary" dark fab small fixed right v-on="on">
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
                        <v-list-item-title>{{ element.name }}</v-list-item-title>
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
import Chart from "@/components/dashboards/Chart.vue";

export default {
  mixins: [uuidMixin, cloneMixin],
  props: ["dialog", "close"],
  components: {
    draggable,
    Chart,
  },
  data() {
    return {
      localdialog: false,
      chartTypes: [
        {
          id: null,
          name: "Line chart",
          icon: "show_chart",
          col: "6",
        },
      ],
      charts: [],
      menu: false,
      dragOptions: {
        animation: 0,
        group: "charts",
      },
      availableItemOptions: {
        group: {
          name: "charts",
          pull: "clone",
          put: false,
        },
        sort: false,
      },
    };
  },
  watch: {
    dialog(val) {
      this.localdialog = val;
    },
  },
};
</script>

<style scoped>
.reporting-container {
  height: 100vh;
  margin-top: 10px;
}
</style>
