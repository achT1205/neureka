<template>
  <v-app>
    <vue-confirm-dialog></vue-confirm-dialog>
    <router-view></router-view>
  </v-app>
</template>
<script>
import { mapGetters } from "vuex";
export default {
  computed: {
    ...mapGetters(["editingInprogress"])
  },
  methods: {
    preventNav(event) {
      if (!this.editingInprogress) {
        return;
      }
      event.preventDefault();
      event.returnValue = "";
    }
  },
  beforeMount() {
    window.addEventListener("beforeunload", this.preventNav);
  },

  beforeDestroy() {
    window.removeEventListener("beforeunload", this.preventNav);
  },
  mounted() {
    if (localStorage.user) {
      const user = JSON.parse(localStorage.user);
      if (user.user.role === "secretary") {
        this.$store.dispatch("getUsersDovtors", "doctor");
      }
      this.$store.commit("AUTH_SUCCESS", user);
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/template.scss";
</style>
