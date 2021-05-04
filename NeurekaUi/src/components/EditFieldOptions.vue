<template>
  <v-card class="pa-3">
    <v-container>
      <v-flex transition="slide-x-transition">
        <v-row class="mx-2">
          <v-col class="align-center justify-space-between" cols="6">
            <v-text-field
              outlined
              v-model="localfield.title"
              label="Field Name"
              clearable
              placeholder="Field Name"
            ></v-text-field>
          </v-col>
          <v-col class="align-center justify-space-between" cols="6">
            <v-select
              outlined
              v-model="localfield.col"
              :items="cols"
              item-text="title"
              item-value="id"
              label="Shose a width"
              placeholder="Shose a width"
            ></v-select>
          </v-col>
          <v-col
            v-if="localfield.type === 'files'"
            class="align-center justify-space-between"
            cols="12"
          >
            <v-checkbox
              v-model="localfield.multiple"
              label="Multiple files ?"
            ></v-checkbox>
          </v-col>

          <v-row
            class="mx-2"
            v-if="['text', 'email', 'textarea'].includes(localfield.type)"
          >
            <v-col cols="12">
              <v-text-field
                outlined
                v-model.number="localfield.maxlength"
                label="Maxlength"
                clearable
                placeholder="Maxlength"
              ></v-text-field>
            </v-col>
          </v-row>

          <v-row class="mx-2" v-if="localfield.type === 'radiogroup'">
            <v-col class="align-center justify-space-between" cols="12">
              <v-select
                outlined
                v-model="localfield.radioDirection"
                :items="radioDirections"
                item-text="title"
                item-value="id"
                label="Dierction"
                placeholder="Dierction"
              ></v-select>
            </v-col>

            <v-col cols="12">
              <v-combobox
                outlined
                v-model="localfield.radios"
                chips
                clearable
                label="Type a new item and enter 'Enter' Key to add"
                multiple
                solo
              >
                <template v-slot:selection="{ attrs, item, select, selected }">
                  <v-chip
                    v-bind="attrs"
                    :input-value="selected"
                    close
                    @click="select"
                    @click:close="removeRadioItem(item)"
                    class="ma-2"
                    color="teal"
                    text-color="white"
                    close-icon="delete"
                  >
                    <strong>{{ item }}</strong>
                  </v-chip>
                </template>
              </v-combobox>
            </v-col>
          </v-row>
        </v-row>
        <v-flex
          v-if="
            localfield.type === 'selections' || localfield.type === 'selection'
          "
        >
          <v-row class="mx-2">
            <v-col cols="12">
              <v-combobox
                outlined
                v-model="localfield.selects"
                chips
                clearable
                label="Type a new item and enter 'Enter' Key to add"
                multiple
                solo
              >
                <template v-slot:selection="{ attrs, item, select, selected }">
                  <v-chip
                    v-bind="attrs"
                    :input-value="selected"
                    close
                    @click="select"
                    @click:close="removeSelectItem(item)"
                    class="ma-2"
                    color="teal"
                    text-color="white"
                    close-icon="delete"
                  >
                    <strong>{{ item }}</strong>
                  </v-chip>
                </template>
              </v-combobox>
            </v-col>
          </v-row>
        </v-flex>
        <v-row class="mx-2" v-if="localfield.type === 'decimal'">
          <v-col class="align-center justify-space-between" cols="6">
            <v-text-field
              outlined
              v-model="localfield.option.min"
              label="Minimum Value"
              clearable
              placeholder="Minimum Value"
            ></v-text-field>
          </v-col>
          <v-col class="align-center justify-space-between" cols="6">
            <v-text-field
              outlined
              v-model="localfield.option.max"
              label="Maximum Value"
              clearable
              placeholder="Maximum Value"
            ></v-text-field>
          </v-col>
          <v-col class="align-center justify-space-between" cols="4">
            <v-text-field
              type="number"
              outlined
              v-model="localfield.option.precision"
              label="Precision"
              clearable
              placeholder="Precision"
            ></v-text-field>
          </v-col>
          <v-col class="align-center justify-space-between" cols="4">
            <v-select
              outlined
              v-model="localfield.option.prefix"
              :items="symboles"
              item-text="name"
              item-value="id"
              label="Prefix symbole"
              placeholder="Shose a symbole"
            ></v-select>
          </v-col>
          <v-col class="align-center justify-space-between" cols="4">
            <v-select
              outlined
              v-model="localfield.option.suffix"
              :items="symboles"
              item-text="name"
              item-value="id"
              label="Suffix symbole"
              placeholder="Shose a symbole"
            ></v-select>
          </v-col>
        </v-row>
      </v-flex>
    </v-container>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn text color="primary" @click="close">Close</v-btn>
      <v-btn text @click="saveChanges">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: ["field", "save", "closeDialog"],
  data() {
    return {
      selectTitle: "",
      symboles: [
        { name: "", id: "" },
        { name: "USD", id: " $ " },
        { name: "EUR", id: " € " },
        { name: "GBP", id: " £ " },
        { name: "Kg", id: " Kg " }
      ],
      cols: [
        {
          id: "12",
          title: "full width"
        },
        {
          id: "6",
          title: "1/2"
        },
        {
          id: "4",
          title: "1/3"
        },
        {
          id: "3",
          title: "1/4"
        },
        {
          id: "2",
          title: "1/6"
        },
        {
          id: "1",
          title: "1/12"
        }
      ],
      localfield: {},
      radioDirections: [
        {
          id: false,
          title: "column"
        },
        {
          id: true,
          title: "row"
        }
      ]
    };
  },
  watch: {
    field(val, old) {
      if (val && val != old) this.localfield = this.field;
    }
  },
  mounted() {
    this.localfield = JSON.parse(JSON.stringify(this.field));
  },
  methods: {
    saveChanges() {
      this.$emit("save", this.localfield);
    },
    close() {
      this.$emit("closeDialog", this.localfield);
    },
    saveRadio() {},

    uuid(e) {
      if (e.id) return e.id;
      const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);
      const key =
        timestamp +
        "xxxxxxxxxxxxxxxx"
          .replace(/[x]/g, function() {
            return ((Math.random() * 16) | 0).toString(16);
          })
          .toLowerCase();
      this.$set(e, "id", key);
      return e.id;
    },

    removeSelectItem(item) {
      this.localfield.selects.splice(this.localfield.selects.indexOf(item), 1);
    },
    removeRadioItem(item) {
      this.localfield.selects.splice(this.localfield.radios.indexOf(item), 1);
    }
  }
};
</script>
