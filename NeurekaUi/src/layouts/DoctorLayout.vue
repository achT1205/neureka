<template>
  <v-main>
    <app-bar></app-bar>
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
import AppBar from "@/components/AppBar";
import { mapGetters } from "vuex";
export default {
  components: {
    AppBar
  },
  computed: {
    ...mapGetters(["patients", "authenticatedUser"])
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
  methods: {
    redirectTo(to) {
      this.$router.push(to);
    },
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
      this.connection.on("ReceiveAddedVisit", visit => {
        console.log("added visit", visit);
        this.$store.commit("ADD_NEW_TO_LIST", visit);
      });

      this.connection.on("ReceiveNewUpdatedVisit", visit => {
        console.log("updated visit", visit);
        this.$store.commit("UPDATE_IN_LIST", visit);
      });
    }
  },
  created() {
    this.$store.dispatch("getPatients");
    this.$store.dispatch("getTemplates");
    this.$store.dispatch("getFieldTemplates");
    this.$store.dispatch("getUsers");
    this.hubConnection();
  },
  mounted() {
    this.getRealTimeData();
  }
};
</script>
