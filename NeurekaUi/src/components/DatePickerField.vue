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
        :disabled="subfield.readonly"
        readonly
        :label="subfield.title ? subfield.title : 'Field' + subfieldindex"
        :placeholder="subfield.title ? subfield.title : 'Field' + subfieldindex"
        prepend-inner-icon="event"
        v-model="subfield.model"
        v-on="on"
      >
        <template v-slot:append-outer>
          <v-menu
            bottom
            left
            :close-on-content-click="false"
            transition="slide-x-transition"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                v-if="!client"
                dark
                icon
                v-bind="attrs"
                v-on="on"
                color="grey"
              >
                <v-icon>more_vert</v-icon>
              </v-btn>
            </template>

            <v-list>
              <v-list-item
                v-for="(item, i) in actions"
                :key="i"
                @click="
                  i === 0
                    ? $emit('edit', index, subfield)
                    : i === 1
                    ? $emit('remove', index, subfieldindex)
                    : i === 2
                    ? $emit('duplicateField', index, subfieldindex, subfield)
                    : i === 3
                    ? $emit('editVisibility', index, subfieldindex)
                    : i === 4
                    ? $emit('canOnlyRead', index, subfieldindex)
                    : $emit('saveAsFieldTemplate', subfield)
                "
              >
                <v-list-item-action>
                  <v-icon light v-if="i < 3">{{ item.icon }}</v-icon>
                  <v-icon light v-if="i === 3">
                    {{ !subfield.isVisible ? item.icons[0] : item.icons[1] }}
                  </v-icon>
                  <v-icon light v-if="i === 4">
                    {{ !subfield.readonly ? item.icons[0] : item.icons[1] }}
                  </v-icon>
                  <v-icon light v-if="i === 5">{{ item.icon }}</v-icon>
                </v-list-item-action>
                <v-list-item-title v-if="i < 3">{{
                  item.title
                }}</v-list-item-title>
                <v-list-item-title v-if="i === 3">
                  {{ !subfield.isVisible ? item.titles[0] : item.titles[1] }}
                </v-list-item-title>
                <v-list-item-title v-if="i === 4">
                  {{ !subfield.readonly ? item.titles[0] : item.titles[1] }}
                </v-list-item-title>
                <v-list-item-title v-if="i === 5">
                  {{ item.title }}
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </template>
      </v-text-field>
    </template>
    <v-date-picker
      locale="en-in"
      v-model="fromDateVal"
      @input="fromDateMenu = false"
    ></v-date-picker>
  </v-menu>
</template>

<script>
export default {
  data() {
    return {
      fromDateMenu: false,
      fromDateVal: this.subfield.model,
    };
  },
  props: [
    "subfield",
    "subfieldindex",
    "index",
    "edit",
    "remove",
    "editVisibility",
    "updateDate",
    "actions",
    "duplicateField",
    "client",
    "saveAsFieldTemplate",
    "canOnlyRead",
  ],
  watch: {
    fromDateVal(val) {
      this.$emit("updateDate", this.index, this.subfieldindex, val);
    },
  },
};
</script>
