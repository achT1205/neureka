<template>
  <v-flex class="pa-2">
    <v-sheet height="64">
      <v-toolbar flat color="white">
        <v-btn outlined class="mr-4" color="grey darken-2" @click="setToday"
          >Today</v-btn
        >
        <v-btn fab text small color="grey darken-2" @click="prev">
          <v-icon small>mdi-chevron-left</v-icon>
        </v-btn>
        <v-btn fab text small color="grey darken-2" @click="next">
          <v-icon small>mdi-chevron-right</v-icon>
        </v-btn>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-row class="mx-2">
          <v-col cols="12">
            <v-select
              v-if="doctors && doctors.length"
              v-model="selectedDoctor"
              :items="doctors"
              menu-props="auto"
              label="Filter by a Dorctor"
              hide-details
              single-line
              item-text="fullName"
              item-value="id"
              @change="handleDoctorChange"
              clearable
            ></v-select>
          </v-col>
        </v-row>
        <v-spacer></v-spacer>
        <v-menu bottom right>
          <template v-slot:activator="{ on }">
            <v-btn outlined color="grey darken-2" v-on="on">
              <span>{{ typeToLabel[type] }}</span>
              <v-icon right>mdi-menu-down</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-item @click="type = 'day'">
              <v-list-item-title>Day</v-list-item-title>
            </v-list-item>
            <v-list-item @click="type = 'week'">
              <v-list-item-title>Week</v-list-item-title>
            </v-list-item>
            <v-list-item @click="type = 'month'">
              <v-list-item-title>Month</v-list-item-title>
            </v-list-item>
            <v-list-item @click="type = '4day'">
              <v-list-item-title>4 days</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-toolbar>
    </v-sheet>
    <v-sheet height="400">
      <v-calendar
        ref="calendar"
        v-model="focus"
        color="primary"
        :events="events"
        :event-color="getEventColor"
        :now="today"
        :type="type"
        @click:event="showEvent"
        @click:more="viewDay"
        @click:date="viewDay"
      ></v-calendar>
      <v-menu
        v-model="selectedOpen"
        :close-on-content-click="false"
        :activator="selectedElement"
        offset-x
      >
        <v-card color="grey lighten-4" min-width="350px" flat>
          <v-toolbar :color="selectedEvent.color" dark>
            <v-btn icon>
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-toolbar-title v-html="selectedEvent.name"></v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon>
              <v-icon>mdi-heart</v-icon>
            </v-btn>
            <v-btn icon>
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </v-toolbar>
          <v-card-text>
            <span v-html="selectedEvent.details"></span>
          </v-card-text>
          <v-card-actions>
            <v-btn text color="secondary" @click="selectedOpen = false"
              >Close</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-menu>
    </v-sheet>
  </v-flex>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "Calendar",
  components: {},
  data: () => ({
    queryDate: null,
    selectedDoctor: null,
    today: null,
    focus: "",
    type: "day",
    typeToLabel: {
      month: "Month",
      week: "Week",
      day: "Day",
      "4day": "4 Days"
    },
    selectedEvent: {},
    selectedElement: null,
    selectedOpen: false
  }),
  computed: {
    title() {
      return this.type === "day"
        ? "Today's Events"
        : this.type === "month"
        ? "This Month Events"
        : this.type === "week"
        ? "This Week's Events"
        : this.type === "4days"
        ? "Next 4 Days Events"
        : "";
    },
    monthFormatter() {
      return this.$refs.calendar.getFormatter({
        timeZone: "UTC",
        month: "long"
      });
    },
    ...mapGetters(["events", "doctors"])
  },
  mounted() {
    this.$refs.calendar.checkChange();
    this.setToday();
  },
  watch: {
    type(val, old) {
      if (val && val !== old) {
        this.queryType = val;
        this.$store.dispatch("getEvents", {
          date: this.focus,
          type: val,
          doctorId: this.selectedDoctor
        });
      }
    },
    focus(val, old) {
      if (val && val !== old) {
        this.queryDate = val;
        this.$store.dispatch("getEvents", {
          date: val,
          type: this.type,
          doctorId: this.selectedDoctor
        });
      }
    }
  },
  methods: {
    handleDoctorChange() {
      this.$store.dispatch("getEvents", {
        data: this.focus,
        type: this.type,
        doctorId: this.selectedDoctor
      });
    },
    viewDay({ date }) {
      this.focus = date;
      this.type = "day";
    },
    getEventColor(event) {
      return event.color;
    },
    setToday() {
      var today = new Date();
      var dd = String(today.getDate()).padStart(2, "0");
      var mm = String(today.getMonth() + 1).padStart(2, "0");
      var yyyy = today.getFullYear();
      today = yyyy + "-" + mm + "-" + dd;
      this.focus = today;
    },
    prev() {
      this.$refs.calendar.prev();
    },
    next() {
      this.$refs.calendar.next();
    },
    showEvent({ nativeEvent, event }) {
      const open = () => {
        this.selectedEvent = event;
        this.selectedElement = nativeEvent.target;
        setTimeout(() => (this.selectedOpen = true), 10);
      };

      if (this.selectedOpen) {
        this.selectedOpen = false;
        setTimeout(open, 10);
      } else {
        open();
      }

      nativeEvent.stopPropagation();
    }
  }
};
</script>
