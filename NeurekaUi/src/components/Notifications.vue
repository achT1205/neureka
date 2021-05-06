<template>
  <v-menu
    v-model="notificationMenu"
    :close-on-content-click="false"
    :nudge-width="200"
    :max-height="400"
    :max-width="400"
    left
    offset-x
  >
    <template v-slot:activator="{ on, attrs }">
      <v-btn
        icon
        :color="
          notifications && notifications.length > 0 ? 'primary' : 'secondary'
        "
        v-bind="attrs"
        v-on="on"
      >
        <v-icon>notifications_active</v-icon>
        <v-badge
          class="ml-1"
          :color="notifications && notifications.length > 0 ? 'primary' : ''"
          :content="notifications.length"
        >
        </v-badge>
      </v-btn>
    </template>
    <v-card>
      <v-list v-if="notifications && notifications.length > 0">
        <v-subheader>
          <v-tooltip bottom>
            <template v-slot:activator="{ on, attrs }">
              <v-spacer />
              <v-icon
                @click="clearNotifications"
                color="primary"
                dark
                v-bind="attrs"
                v-on="on"
              >
                close
              </v-icon>
            </template>
            <span>Clear notifications</span>
          </v-tooltip>
        </v-subheader>
        <v-list-item
          v-for="notification in notifications"
          :key="notification.visitId"
          :to="
            notification.fromRole === 'patient'
              ? `/clients/${notification.patientId}`
              : `/patients/${notification.patientId}`
          "
        >
          <v-list-item-title
            v-if="notification.fromId !== authenticatedUser.id"
          >
            {{
              notification.fromRole === "patient"
                ? `Somme update with Dc ${notification.fromFirstName} ${notification.fromLastName}`
                : `Somme update with ${notification.fromFirstName} ${notification.fromLastName}`
            }}
          </v-list-item-title>
        </v-list-item>
      </v-list>
      <v-alert
        v-else
        color="primary text-center"
        class="mb-0"
        style="color: white"
      >
        No notification yet !
      </v-alert>
    </v-card>
  </v-menu>
</template>

<script>
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      notificationMenu: false
    };
  },
  computed: {
    ...mapGetters(["authenticatedUser", "notifications"])
  },
  methods: {
    clearNotifications() {
      this.notificationMenu = false;
      localStorage.removeItem("notifications");
      this.$store.commit("SET_NOTIFICATIONS", []);
    }
  }
};
</script>
