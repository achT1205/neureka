<template>
  <v-flex>
    <v-card flat>
      <v-card-text>
        <template-fields />
      </v-card-text>
    </v-card>
  </v-flex>
</template>
<script>
import TemplateFields from "@/components/TemplateFields.vue";
export default {
  data() {
    return {};
  },
  components: {
    TemplateFields
  },
  mounted() {
    this.$store.dispatch("getTemplate", this.$route.params.id);
  }
};
</script>
