<template>
  <v-app id="inspire" v-if="authenticatedUser">
    <v-navigation-drawer v-model="drawer" clipped app>
      <!--<div class="d-flex justify-center">
        <img
          @click.prevent="redirectTo('/')"
          src="@/assets/logo/neureka.png"
          class="app-logo"
        />
      </div>-->
      <v-list dense>
        <template v-for="item in items">
          <v-row v-if="item.heading" :key="item.heading" align="center">
            <v-col cols="6">
              <v-subheader v-if="item.heading">{{ item.heading }}</v-subheader>
            </v-col>
            <v-col cols="6" class="text-center">
              <a href="#!" class="body-2 black--text">EDIT</a>
            </v-col>
          </v-row>
          <v-list-group
            v-else-if="item.children"
            :key="item.text"
            v-model="item.model"
            :prepend-inner-icon="item.model ? item.icon : item['icon-alt']"
            append-icon
          >
            <template v-slot:activator>
              <v-list-item-content>
                <v-list-item-title>{{ item.text }}</v-list-item-title>
              </v-list-item-content>
            </template>
            <v-list-item
              v-for="(child, i) in item.children"
              :to="child.to"
              :key="i"
              link
            >
              <v-list-item-action v-if="child.icon">
                <v-icon>{{ child.icon }}</v-icon>
              </v-list-item-action>
              <v-list-item-content>
                <v-list-item-title>{{ child.text }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-group>
          <v-list-item v-else :key="item.text" link :to="item.to">
            <v-list-item-action>
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ item.text }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list>
    </v-navigation-drawer>
    <app-bar></app-bar>
    <v-main>
      <v-container fluid>
        <v-row>
          <router-view></router-view>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapGetters } from "vuex";
import AppBar from "@/components/AppBar";
export default {
  components: {
    AppBar
  },
  props: {
    source: String
  },
  data: () => ({
    dialog: false,
    drawer: null,
    items: [
      {
        icon: "mdi-contacts",
        text: "Manage users",
        to: "/admin/users"
      },
      {
        icon: "list_alt",
        text: "Form templates",
        to: "/admin/forms"
      }
      /*{
                      icon: "mdi-cog",
                      text: "Settings",
                      to: "/admin/settings"
                  }*/
    ]
  }),
  computed: {
    ...mapGetters(["authenticatedUser"])
  },
  created() {
    this.$store.dispatch("getTemplates");
    this.$store.dispatch("getFieldTemplates");
    
  }
};
</script>

<style lang="scss" scoped>
.app-logo {
  height: 70px;
  margin-top: 8px;
  cursor: pointer;
}
</style>
