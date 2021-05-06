<template>
  <v-app-bar app color="white" dark clipped-left>
    <img
      @click.prevent="redirectTo('/')"
      src="@/assets/logo/neureka.png"
      class="app-logo"
    />
    <v-spacer></v-spacer>
    <v-toolbar-items>
      <v-btn color="secondary" active-class="actived" small text to="/home"
        >Dashboards</v-btn
      >
      <v-btn color="secondary" active-class="actived" small text to="/patients"
        >Patients</v-btn
      >
      <v-btn
        color="secondary"
        active-class="actived"
        small
        text
        to="/admin/users"
        >Administration</v-btn
      >
      <v-btn
        color="secondary"
        active-class="actived"
        small
        text
        to="/admin/forms"
        >Custom Forms</v-btn
      >
      <v-btn
        color="secondary"
        active-class="actived"
        small
        text
        to="/admin/fields"
        >Custom Fields</v-btn
      >
    </v-toolbar-items>
    <v-spacer></v-spacer>

    <div class="d-flex align-center">
      <notifications />
      <div class="avatar-info">
        <div class="avatar-name">
          <span>
            {{ authenticatedUser.user.firstName }}
            {{ authenticatedUser.user.lastName }}
          </span>
        </div>
        <div class="avatar-role">{{ authenticatedUser.user.role }}</div>
      </div>
      <v-menu
        v-model="menu"
        :close-on-content-click="false"
        :nudge-width="160"
        offset-y
        v-if="authenticatedUser"
        class="menu-drop"
      >
        <template v-slot:activator="{ on }">
          <v-list-item-avatar v-on="on">
            <img
              src="@/assets/avatar/male.png"
              v-if="authenticatedUser.user.sexe === 'male'"
              :alt="
                `${authenticatedUser.user.firstName} ${authenticatedUser.user.lastName}`
              "
              class="avatar-all"
            />
            <img
              src="@/assets/avatar/female.png"
              v-if="authenticatedUser.user.sexe === 'female'"
              :alt="
                `${authenticatedUser.user.firstName} ${authenticatedUser.user.lastName}`
              "
              class="avatar-all"
            />
            <img
              src="@/assets/avatar/none.png"
              v-if="authenticatedUser.user.sexe === 'none'"
              :alt="
                `${authenticatedUser.user.firstName} ${authenticatedUser.user.lastName}`
              "
              class="avatar-all"
            />
          </v-list-item-avatar>
        </template>
        <v-list dense>
          <v-list-item to="/admin/profile">
            <v-list-item-icon class="ml-0 mr-2">
              <v-icon>person</v-icon>
            </v-list-item-icon>
            <v-list-item-title> Profile </v-list-item-title>
          </v-list-item>
          <v-divider></v-divider>
          <v-list-item @click="logout">
            <v-list-item-icon class="ml-0 mr-2">
              <v-icon>power_settings_new</v-icon>
            </v-list-item-icon>
            <v-list-item-title>Logout</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </div>
  </v-app-bar>
</template>

<script>
import Notifications from "@/components/Notifications.vue";
import { mapGetters } from "vuex";
export default {
  components: {
    Notifications
  },
  data() {
    return {
      menu: false,
      notificationMenu: false
    };
  },
  computed: {
    ...mapGetters(["authenticatedUser", "editingInprogress", "notifications"])
  },
  methods: {
    redirectTo(to) {
      this.$router.push(to);
    },
    logout() {
      if (!this.editingInprogress) {
        this.$store.dispatch("logout");
      } else {
        this.$confirm({
          message: `Changes that you made may not be saved. Are your sure you want to continue ?`,
          button: {
            no: "No",
            yes: "Yes"
          },
          callback: confirm => {
            if (confirm) {
              this.$store.dispatch("logout");
            }
          }
        });
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.avatar-info {
  margin-left: 15px;
  color: #80bbbd;

  .avatar-name {
    font-size: 0.8em;
    font-weight: 600;
  }

  .avatar-role {
    color: #364749;
    font-size: 0.7em;
    text-align: right;
  }
}

.actived.secondary--text {
  background: #364749 !important;
  color: white !important;
}

.app-logo {
  height: 70px;
  cursor: pointer;
}
.menu-drop {
  box-shadow: 0 0 4px 0px rgba(0, 0, 0, 0.09) !important;
  border: 1px solid #eee !important;
}
</style>
