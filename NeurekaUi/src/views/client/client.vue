<template>
    <div>client</div>
</template>

<script>
import { mapGetters } from "vuex";
export default {
    computed: {
    ...mapGetters(["clientVisits", "clientVisit"])
  },
  created() {
    //this.$store.dispatch("getPatientVisit", this.$route.params.id);
    this.$store.dispatch("getPatientVisits", this.$route.params.id);
  }
}
</script>