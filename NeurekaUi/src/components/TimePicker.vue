<template>
  <v-menu
    v-model="fromTimeMenu"
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
        prepend-inner-icon="schedule"
        readonly
        :value="value"
        v-on="on"
      ></v-text-field>
    </template>
    <v-time-picker
      :min="min"
      :max="max"
      v-model="fromTimeVal"
      format="24hr"
      @change="handelTimeChage"
    ></v-time-picker>
  </v-menu>
</template>
<script>
export default {
  props: ["updateTime", "value", "label", "min", "max"],
  data() {
    return {
      fromTimeMenu: false,
      fromTimeVal: null
    };
  },
  watch: {
    fromTimeVal(val) {
      this.$emit("updateTime", val);
    }
  },
  methods: {
    handelTimeChage() {
      this.fromTimeMenu = false;
    }
  }
};
</script>
