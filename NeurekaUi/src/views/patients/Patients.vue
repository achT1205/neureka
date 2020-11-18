<template>
  <div class="d-flex page">
    <div class="title-page">
      <h1>Patients <template v-if="calendar">- Calendar</template></h1>
      <div class="d-flex align-center">
        <div class="mx-3 pt-1">{{ `${calendar ? "Calendar" : "List"} ` }}</div>
        <v-switch
          :hide-details="'auto'"
          class="mt-0"
          v-model="calendar"
          inset
        ></v-switch>
      </div>
    </div>
    <div class="content-page">
      <template v-if="patients">
        <div class="flex" v-if="calendar">
          <calendar />
        </div>
        <div class="d-flex flex-wrap" style="width:100%" v-else>
          <div class="col-12">
            <v-text-field
              clearable
              hide-details
              prepend-inner-icon="mdi-magnify"
              v-model="search"
              label="Search by Name"
              class="hidden-sm-and-down"
              placeholder="Search by Name"
              outlined
            ></v-text-field>
          </div>
          <patients-list
            v-for="patient in patients"
            :key="patient.id"
            :patient="patient"
          />
          <edit-patient
            :dialog="dialog"
            :current-patient="patient"
            @close="dialog = false"
          />
          <v-btn
            bottom
            color="pink"
            dark
            fab
            small
            fixed
            right
            @click="dialog = !dialog"
          >
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import Calendar from "@/components/Calendar.vue";
import PatientsList from "@/components/Patients/PatientsList.vue";
import EditPatient from "@/components/Patients/EditePatient";
import { mapGetters } from "vuex";
export default {
  components: {
    Calendar,
    PatientsList,
    EditPatient
  },
  data() {
    return {
      search: null,
      patient: null,
      dialog: false,
      calendar: false,
      items: [
        {
          text: "Dashboard",
          disabled: false,
          href: "breadcrumbs_dashboard"
        },
        {
          text: "Link 1",
          disabled: false,
          href: "breadcrumbs_link_1"
        },
        {
          text: "Link 2",
          disabled: true,
          href: "breadcrumbs_link_2"
        }
      ]
    };
  },
  methods: {
    showCalendar() {
      this.calendar = !this.calendar;
    },
    toggle() {
      this.dialog = !this.dialog;
      if (this.dialog) {
        this.patient = null;
      }
    }
  },
  watch: {
    search(val, old) {
      if (val && val !== old) {
        this.$store.dispatch("filterPatients", val.toLowerCase());
      } else if (!val) {
        this.$store.dispatch("getPatients");
      }
    }
  },
  computed: {
    ...mapGetters(["patients"])
  }
};
</script>
