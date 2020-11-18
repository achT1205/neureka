<template>
  <span>
    <v-menu
      v-model="fromColorMenu"
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
          :color="value"
          :label="label"
          :placeholder="color"
          readonly
          :value="fromColorDisp ? fromColorDisp : value"
          prepend-inner-icon="palette"
          v-on="on"
        ></v-text-field>
      </template>
      <v-color-picker :mode="mode" v-model="fromColorVal" />
    </v-menu>
  </span>
</template>
<script>
export default {
  props: ["updateColor", "value", "label"],
  data() {
    return {
      mode: "hexa",
      color: "",
      fromColorMenu: false,
      fromColorVal: null
    };
  },
  watch: {
    fromColorVal(val) {
      this.color = val.hexa;
      this.$emit("updateColor", this.color);
    }
  },
  computed: {
    fromColorDisp() {
      if (this.fromColorVal && this.fromColorVal.hexa) {
        return this.fromColorVal.hexa;
      }
      return null;
    }
  }
};
</script>
