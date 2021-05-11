<template>
  <div class="d-flex page">
    <div class="title-page">
      <h1>Dashboard</h1>
    </div>
    <div
      class="content-page"
      style="background:none;border:0;padding-left:0;padding-right:0"
    >
      <div class="row">
        <div class="col-sm-4">
          <v-card class="mx-auto" outlined>
            <v-list-item class="pl-0">
              <v-list-item-avatar class="my-0" tile size="80" color="primary">
                <v-icon x-large dark>people</v-icon>
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title class=" mb-1">Users</v-list-item-title>
                <v-list-item-subtitle>{{ users.length }}</v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
          </v-card>
        </div>
        <div class="col-sm-4">
          <v-card class="mx-auto" outlined>
            <v-list-item class="pl-0">
              <v-list-item-avatar class="my-0" tile size="80" color="info">
                <v-icon x-large dark>assignment_ind</v-icon>
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title class=" mb-1">Forms</v-list-item-title>
                <v-list-item-subtitle
                  >{{ templates.length }} Templates</v-list-item-subtitle
                >
              </v-list-item-content>
            </v-list-item>
          </v-card>
        </div>
        <div class="col-sm-4">
          <v-card class="mx-auto" outlined>
            <v-list-item class="pl-0">
              <v-list-item-avatar class="my-0" tile size="80" color="secondary">
                <v-icon x-large dark>accessibility_new</v-icon>
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title class=" mb-1">Patients</v-list-item-title>
                <v-list-item-subtitle
                  >{{ patients.length }} patients</v-list-item-subtitle
                >
              </v-list-item-content>
            </v-list-item>
          </v-card>
        </div>
        <div class="col">
          <calendar />
          <!--<v-card outlined>
            <v-card-text class="pb-0 title-visit">today's visits</v-card-text>
            <v-card-text class="pa-0">
              <v-data-table
                :headers="headers"
                :items="events"
                :items-per-page="5"
                class="elevation-0"
              >
                <template v-slot:[`item.start`]="{ item }">
                  <span>{{
                    item.start.substring(
                      item.start.length - 5,
                      item.start.length
                    )
                  }}</span>
                </template>
                <template v-slot:[`item.end`]="{ item }">
                  <span>{{
                    item.start.substring(
                      item.start.length - 5,
                      item.start.length
                    )
                  }}</span>
                </template>
              </v-data-table>
            </v-card-text>
          </v-card>-->
        </div>
        <div class="col">
          <v-card outlined class="pa-2">
            <BarChart />
          </v-card>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BarChart from "@/components/dashboards/BarChart.vue";
import Calendar from "@/components/Calendar.vue";
import { mapGetters } from "vuex";
export default {
  name: "DashboardDashboard",
  components: {
    BarChart,
    Calendar
  },
  computed: {
    ...mapGetters(["patients", "users", "templates", "events"])
  },
  mounted() {
    let today = new Date();
    const dd = String(today.getDate()).padStart(2, "0");
    const mm = String(today.getMonth() + 1).padStart(2, "0");
    const yyyy = today.getFullYear();
    today = yyyy + "-" + mm + "-" + dd;
    const focus = today;
    this.$store.dispatch("getEvents", {
      date: focus,
      type: "day",
      doctorId: null
    });
  },
  data() {
    return {
      headers: [
        {
          text: "Title",
          align: "start",
          sortable: true,
          value: "name"
        },
        {
          text: "Patient",
          value: "patient"
        },
        {
          text: "Start",
          value: "start"
        },
        {
          text: "End",
          value: "end"
        },
        {
          text: "Doctor",
          value: "doctor"
        }
      ]
    };
  }
};
</script>

<style lang="scss" scoped>
.content-page {
  .v-list-item__title {
    font-weight: 700;
    font-size: 1.3rem;
    text-transform: uppercase;
  }

  .title-visit {
    text-transform: uppercase;
    color: #000;
    font-size: 1rem;
    font-weight: 600;
    border-bottom: thin solid rgba(0, 0, 0, 0.12);
  }
}
</style>
