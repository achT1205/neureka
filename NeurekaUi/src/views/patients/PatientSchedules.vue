<template>
  <div class="d-flex page" v-if="patient && patient.id">
    <div class="title-page">
      <h1>{{ patient.firstName }} {{ patient.lastName }}</h1>
      <div>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn icon>
              <v-icon v-on="on" @click.prevent="addNewEvent">add</v-icon>
            </v-btn>
          </template>
          Add a new visit
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn icon>
              <v-icon v-on="on" @click.prevent="exportVisits"
                >import_export</v-icon
              >
            </v-btn>
          </template>
          Export visits to excel
        </v-tooltip>
      </div>
    </div>
    <div class="content-page">
      <v-overlay :value="currentVisit === null || forceOverlay">
        <v-progress-circular indeterminate size="64"></v-progress-circular>
      </v-overlay>
      <div class="d-flex flex-wrap full-flex-width">
        <v-card
          class="elevation-0"
          width="100%"
          v-if="visits && visits.length > 0"
        >
          <v-card-text>
            <v-tabs
              v-model="tab"
              background-color="primary"
              color="white"
              centered
              slider-color="black"
              dark
              grow
              show-arrows
            >
              <v-tab v-for="(visit, index) in visits" :key="visit.id">
                {{ visit.title }}
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <v-icon
                      class="ml-3"
                      x-small
                      color="black"
                      v-on="on"
                      @click="editVisit(index)"
                      >edit</v-icon
                    >
                  </template>
                  Edit Visit
                </v-tooltip>
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <v-icon
                      class="ml-3"
                      x-small
                      color="black"
                      v-on="on"
                      @click="OpenOrcloseVisit(index)"
                      >{{!visit.closed ? "lock" :"lock_open"}} </v-icon
                    >
                  </template>
                 {{!visit.closed ? "Close this visit" :"Open this vist"}}
                </v-tooltip>
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <v-icon
                      color="black"
                      class="ml-3"
                      x-small
                      v-on="on"
                      @click="removeVisit()"
                      >delete</v-icon
                    >
                  </template>
                  Remove Visit
                </v-tooltip>
              </v-tab>
            </v-tabs>
          </v-card-text>
        </v-card>
        <div class="full-flex-width">
          <template v-if="visits && visits.length > 0">
            <v-tabs-items v-model="tab">
              <v-tab-item v-for="visit in visits" :key="visit.id">
                <v-card flat>
                  <v-card-text>
                    <schedule-content
                      :id="visit.id"
                      @isEditingVisit="isEditingVisit"
                    />
                  </v-card-text>
                </v-card>
              </v-tab-item>
            </v-tabs-items>
          </template>
          <template v-else>
            <v-alert color="primary" class="mb-0" style="color:white;">
              There is currently no visit for
              <strong v-if="patient"
                >{{ patient.firstName }} {{ patient.lastName }}
              </strong>
              please click on the <strong class="ml-2 mr-2">+</strong> button to
              create one
            </v-alert>
          </template>
          <v-dialog v-model="removingDialog" persistent max-width="290">
            <v-card>
              <v-card-title class="headline">Removing Alert !</v-card-title>
              <v-card-text
                >Are you sure you want to remove this appointment completly
                ?</v-card-text
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                  color="green darken-1"
                  text
                  @click="removingDialog = false"
                  >Disagree</v-btn
                >
                <v-btn color="red darken-1" text @click="confirRemovingVisit()"
                  >Agree</v-btn
                >
              </v-card-actions>
            </v-card>
          </v-dialog>

          <v-dialog v-model="scheduleTitledialog" persistent width="1200px">
            <v-card>
              <v-card-title>
                <v-spacer></v-spacer>
                <v-icon @click="scheduleTitledialog = false" x-small
                  >close</v-icon
                >
              </v-card-title>
              <v-container>
                <v-row class="mx-2">
                  <v-col cols="7">
                    <calendar />
                  </v-col>
                  <v-col cols="5">
                    <v-flex class="ml-3">
                      <v-row>
                        <v-col cols="12" v-if="doctors && doctors.length">
                          <v-select
                            v-model="selectedDoctor"
                            :items="doctors"
                            menu-props="auto"
                            label="Filter by a Dorctor"
                            hide-details
                            single-line
                            item-text="fullName"
                            item-value="id"
                          ></v-select>
                        </v-col>
                        <v-col cols="12">
                          <v-text-field
                            v-model="event.title"
                            label="Title"
                            clearable
                            placeholder="Title"
                            outlined
                          ></v-text-field>
                        </v-col>
                        <v-col cols="6">
                          <date-picker
                            :value="event.startDate"
                            :min="today"
                            :label="'Start Date'"
                            @updateDate="updateStartDate"
                          />
                        </v-col>

                        <v-col cols="6">
                          <time-picker
                            :value="event.startTime"
                            :label="'Start Time'"
                            :min="timeMin"
                            :max="timeMax"
                            @updateTime="updateStartTime"
                          />
                        </v-col>
                        <v-col cols="12">
                          <color-picker
                            :value="event.color"
                            :label="'Color'"
                            @updateColor="updateColor"
                            placeholder="Color"
                            append-inner-icon="palette"
                          />
                        </v-col>
                        <v-col cols="6">
                          <date-picker
                            :value="event.endDate"
                            :label="'End Date'"
                            :min="event.startDate"
                            @updateDate="updateEndDate"
                          />
                        </v-col>

                        <v-col cols="6">
                          <time-picker
                            :value="event.endTime"
                            :min="event.startTime"
                            :max="timeMax"
                            :label="'End Time'"
                            @updateTime="updateEndTime"
                          />
                        </v-col>
                        <v-alert
                          v-if="!valid"
                          border="bottom"
                          colored-border
                          type="warning"
                          elevation="2"
                          >You have to feel all this fields before you
                          continue</v-alert
                        >
                      </v-row>
                    </v-flex>
                  </v-col>
                </v-row>
              </v-container>
              <v-card-actions>
                <v-btn text color="primary" @click="scheduleTitledialog = false"
                  >Close</v-btn
                >
                <v-spacer></v-spacer>
                <v-btn right text @click.prevent="addVisit">Add</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Calendar from "@/components/Calendar.vue";
import DatePicker from "@/components/DatePicker.vue";
import TimePicker from "@/components/TimePicker.vue";
import ColorPicker from "@/components/ColorPicker.vue";
import ScheduleContent from "@/components/ScheduleContent.vue";
import { mapGetters } from "vuex";

export default {
  components: {
    Calendar,
    DatePicker,
    TimePicker,
    ColorPicker,
    ScheduleContent
  },
  data() {
    return {
      forceOverlay: false,
      filteredBy: null,
      selectedDoctor: null,
      timeMin: "7:00",
      timeMax: "20:00",
      eitingVisit: false,
      removingDialog: false,
      currentVisitIndex: null,
      editingVisit: false,
      valid: true,
      tab: 0,
      events: [],
      event: {
        id: null,
        title: null,
        startDate: null,
        startTime: null,
        endDate: null,
        endTime: null,
        color: null,
        fields: []
      },
      defaultEvent: {
        id: null,
        title: null,
        startDate: null,
        startTime: null,
        endDate: null,
        endTime: null,
        color: null,
        fields: []
      },
      scheduleName: "",
      scheduleTitledialog: false
    };
  },
  computed: {
    ...mapGetters([
      "patients",
      "visits",
      "currentVisit",
      "authenticatedUser",
      "doctors",
      "editingInprogress"
    ]),
    patient() {
      return this.patients.find(x => x.id == this.$route.params.id);
    },
    today() {
      let today = new Date();
      const dd = String(today.getDate()).padStart(2, "0");
      const mm = String(today.getMonth() + 1).padStart(2, "0");
      const yyyy = today.getFullYear();
      today = yyyy + "-" + mm + "-" + dd;
      return today;
    }
  },
  watch: {
    patient(val, old) {
      if (val && val !== old) {
        this.$store.dispatch("getVisits", val.id);
      }
    },
    visits(val, old) {
      if (val && old !== val) {
        this.tab = val.length - 1;
      }
    },
    scheduleTitledialog(val) {
      if (!val) {
        this.event = this.defaultEvent;
      }
    },
    tab(val, old) {
      if (val !== old) {
        this.$store.commit("SET_VISIT", this.visits[val]);
      }
    }
  },
  methods: {
    addNewEvent() {
      if (this.editingInprogress) {
        this.$confirm({
          message: `Changes that you made may not be saved. Are your sure you want to continue ?`,
          button: {
            no: "No",
            yes: "Yes"
          },
          callback: confirm => {
            if (confirm) {
              this.scheduleTitledialog = !this.scheduleTitledialog;
              this.valid = true;
            } else {
              return;
            }
          }
        });
      } else {
        this.scheduleTitledialog = !this.scheduleTitledialog;
        this.valid = true;
      }
    },
    updateStartDate(date) {
      this.event.startDate = date;
      this.event.endDate = date;
    },
    updateStartTime(time) {
      this.event.startTime = time;
      const v = time.split(":");
      let h = parseInt(v[0]);
      let m = parseInt(v[1]);
      let hours = h;
      let minuts = m;
      const duration = 15;
      minuts = m + duration;
      if (minuts >= 60) {
        hours = hours + 1;
        minuts = minuts - 60;
      }

      if (hours >= 24) {
        hours = 0;
      }

      const hstr = hours < 10 ? "0" + hours.toString() : hours.toString();

      const mstr = minuts < 10 ? "0" + minuts.toString() : minuts.toString();

      this.event.endTime = hstr + ":" + mstr;
    },
    updateEndDate(date) {
      this.event.endDate = date;
    },
    updateEndTime(time) {
      this.event.endTime = time;
    },
    updateColor(color) {
      this.event.color = color;
    },
    editVisit(index) {
      if (this.editingInprogress) {
        this.$confirm({
          message: `Changes that you made may not be saved. Are your sure you want to continue ?`,
          button: {
            no: "No",
            yes: "Yes"
          },
          callback: confirm => {
            if (confirm) {
              this.edit(index);
            } else {
              return;
            }
          }
        });
      } else {
        this.edit(index);
      }
    },
    OpenOrcloseVisit(index){
      const visit = this.visits[index];
      visit.closed = !visit.closed  ? true : false;;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null,
      });
    },

    edit(index) {
      this.forceOverlay = true;
      this.$store.dispatch("getVisit", this.visits[index].id);
      this.editingVisit = true;
      const that = this;
      setTimeout(() => {
        that.event = {
          ...that.currentVisit
        };
        that.valid = true;
        that.scheduleTitledialog = !that.scheduleTitledialog;
        this.forceOverlay = false;
      }, 1000);
    },

    removeVisit() {
      this.currentVisitId = this.currentVisit.id;
      this.removingDialog = !this.removingDialog;
    },
    confirRemovingVisit() {
      this.$store.dispatch("removeVisit", {
        visitId: this.currentVisit.id,
        patientId: this.$route.params.id
      });
      this.removingDialog = false;
    },
    addVisit() {
      this.valid = true;
      if (
        !this.event.title ||
        !this.event.startDate ||
        !this.event.startTime ||
        !this.event.endDate ||
        !this.event.endTime
      ) {
        this.valid = false;
        return;
      }
      if (this.editingVisit) {
        this.$store.dispatch("editVisit", {
          visit: this.event,
          filesData: this.filesData,
          all: true,
          patientId: this.$route.params.id
        });
        this.scheduleTitledialog = false;
        this.event = this.defaultEvent;
      } else {
        this.event.id = this.generateNewId();
        this.event.doctorId = this.selectedDoctor
          ? this.selectedDoctor
          : this.authenticatedUser.user.id;
        this.event.patientId = this.$route.params.id;
        this.$store.dispatch("createVisit", {
          visit: this.event,
          patientId: this.$route.params.id
        });
        this.forceOverlay = true;
        this.scheduleTitledialog = false;
        const that = this;
        setTimeout(() => {
          that.tab = that.visits.length - 1;
          that.event = that.defaultEvent;
          that.forceOverlay = false;
        }, 1000);
      }
    },
    generateNewId() {
      const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);
      const id =
        timestamp +
        "xxxxxxxxxxxxxxxx"
          .replace(/[x]/g, function() {
            return ((Math.random() * 16) | 0).toString(16);
          })
          .toLowerCase();
      return id;
    },
    isEditingVisit(val) {
      this.eitingVisit = val;
    },
    exportVisits() {
      if (this.editingInprogress) {
        this.$confirm({
          message: `Changes that you made may not be saved. Are your sure you want to continue ?`,
          button: {
            no: "No",
            yes: "Yes"
          },
          callback: confirm => {
            if (confirm) {
              this.$store.dispatch("exportVisits", this.patient);
            } else {
              return;
            }
          }
        });
      } else {
        this.$store.dispatch("exportVisits", this.patient);
      }
    }
  },
  mounted() {
    this.$store.dispatch("getVisits", this.$route.params.id);
  }
};
</script>
