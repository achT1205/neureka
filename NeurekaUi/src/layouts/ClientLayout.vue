<template>
  <v-main>
    <client-app-bar />
    <v-container fluid>
      <v-row>
        <v-col>
          <router-view></router-view>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
import { HubConnectionBuilder } from "@microsoft/signalr";
import ClientAppBar from "@/components/ClientAppBar";
import { mapGetters } from "vuex";
export default {
  components: {
    ClientAppBar
  },
  computed: {
    ...mapGetters(["authenticatedUser"])
  },
  props: {
    source: String
  },
  data: () => ({
    drawer: null,
    dialog: false,
    currentPatient: null,
    search: null
  }),
  created() {
    this.$store.dispatch("getClientVisit", this.$route.params.id);
    this.hubConnection();
  },
  mounted() {
    this.getRealTimeData();
    const notifications = localStorage.notifications
      ? JSON.parse(localStorage.notifications)
      : [];
    this.$store.commit("SET_NOTIFICATIONS", notifications);
  },
  methods: {
    hubConnection() {
      this.connection = new HubConnectionBuilder()
        .withUrl(`${process.env.VUE_APP_API_URL_ROOT}notification`)
        .withAutomaticReconnect()
        .build();
      this.connection
        .start()
        .then(() => {
          console.log("hub notification", "success connection");
        })
        .catch(error => {
          console.log(error.toString());
        });
    },
    getRealTimeData() {
      this.connection.on("ReceiveNewUpdatedVisit", notification => {
        if (notification.fromId !== this.authenticatedUser.user.id) {
          this.$store.commit("SET_NOTIFICATION", notification);
        }
        if (notification && notification.patientId) {
          this.$store.dispatch("getClientVisit", notification.patientId);
        }
      });
    }
  }
};
</script>
