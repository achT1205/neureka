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
  })
};
</script>
