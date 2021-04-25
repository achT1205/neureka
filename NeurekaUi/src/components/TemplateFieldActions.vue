<template>
  <v-menu
    bottom
    left
    :close-on-content-click="false"
    transition="slide-x-transition"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-btn dark icon v-bind="attrs" v-on="on" color="grey">
        <v-icon>more_vert</v-icon>
      </v-btn>
    </template>

    <v-list>
      <v-list-item
        v-for="(item, i) in actions"
        :key="i"
        @click="
          i === 0
            ? $emit('edit', subfieldindex, subfield)
            : i === 1
            ? $emit('remove', subfieldindex)
            : i === 2
            ? $emit('duplicateField', subfieldindex, subfield)
            : $emit('editVisibility', subfieldindex)
        "
      >
        <v-list-item-action>
          <v-icon light v-if="i < 3">{{ item.icon }}</v-icon>
          <v-icon light v-if="i === 3">{{
            !subfield.isVisible ? item.icons[0] : item.icons[1]
          }}</v-icon>
        </v-list-item-action>
        <v-list-item-title v-if="i < 3">{{ item.title }}</v-list-item-title>
        <v-list-item-title v-if="i === 3">{{
          !subfield.isVisible ? item.titles[0] : item.titles[1]
        }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>
<script>
export default {
  props: [
    "actions",
    "subfield",
    "subfieldindex",
    "edit",
    "remove",
    "editVisibility",
    "duplicateField"
  ]
};
</script>
