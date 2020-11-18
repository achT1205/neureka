<template>
  <v-menu
    v-model="fromDateMenu"
    :close-on-content-click="false"
    :nudge-right="40"
    transition="scale-transition"
    offset-y
    max-width="290px"
    min-width="290px"
  >
    <template v-slot:activator="{ on }">
      <v-text-field
        outlined
        :label="label"
        :placeholder="label"
        prepend-inner-icon="event"
        readonly
        :value="value"
        v-on="on"
      ></v-text-field>
    </template>
    <v-date-picker
      locale="en-in"
      v-model="fromDateVal"
      @input="fromDateMenu = false"
      :min="min"
    ></v-date-picker>
  </v-menu>
</template>
<script>
export default {
  props: ["updateDate", "value", "label", "min"],
  data() {
    return {
      fromDateMenu: false,
      fromDateVal: null
    };
  },
  watch: {
    fromDateVal(val) {
      this.$emit("updateDate", val);
    }
  }
};
</script>
