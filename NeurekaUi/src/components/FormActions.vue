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
            ? $emit('saveAsTemplate', field)
            : i === 1
            ? $emit('editSessionvisit', field)
            : i === 2
            ? $emit('removeSessionvisit', index)
            : i === 3
            ? $emit('duplicateForm', index, field)
            : $emit('editVisibility', index)
        "
      >
        <v-list-item-action>
          <v-icon light v-if="i < 4">{{ item.icon }}</v-icon>
          <v-icon light v-if="i === 4">
            {{ !field.isVisible ? item.icons[0] : item.icons[1] }}
          </v-icon>
        </v-list-item-action>
        <v-list-item-title v-if="i < 4">{{ item.title }}</v-list-item-title>
        <v-list-item-title v-if="i === 4">
          {{ !field.isVisible ? item.titles[0] : item.titles[1] }}
        </v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>
<script>
export default {
  props: [
    "actions",
    "index",
    "field",
    "saveAsTemplate",
    "editSessionvisit",
    "removeSessionvisit",
    "editVisibility",
    "duplicateForm"
  ]
};
</script>
