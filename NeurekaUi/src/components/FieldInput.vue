<template>
  <v-flex>
    <v-text-field
      outlined
      clearable
      :label="localField.title"
      :placeholder="localField.title"
      v-if="
        !localField.inputType ||
          localField.inputType === 'text' ||
          localField.inputType === 'number' ||
          localField.inputType === 'email'
      "
      :type="localField.inputType"
      v-model="localField.model"
    >
      <template v-slot:append-outer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon x-small v-on="on" @click="edit()">edit</v-icon>
          </template>
          Edit
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
          </template>
          Remove
        </v-tooltip>

        <v-tooltip bottom v-if="!localField.isVisible">
          <template v-slot:activator="{ on }">
            <v-icon class="ml-1" x-small v-on="on" @click="edit()"
              >visibility</v-icon
            >
          </template>
          Allow patient to see
        </v-tooltip>
        <v-tooltip bottom v-if="localField.isVisible">
          <template v-slot:activator="{ on }">
            <v-icon class="ml-1" x-small v-on="on" @click="edit()"
              >visibility_off</v-icon
            >
          </template>
          Hide to the patient
        </v-tooltip>
      </template>
    </v-text-field>
    <v-textarea
      outlined
      :placeholder="localField.title"
      v-else-if="localField.inputType === 'textarea'"
      clearable
      clear-icon="close"
      :label="localField.title"
      v-model="localField.model"
    >
      <template v-slot:append-outer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
          </template>
          Remove
        </v-tooltip>
      </template>
    </v-textarea>

    <v-menu
      v-else-if="field.inputType === 'date'"
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
          :label="localField.title"
          :placeholder="localField.title"
          prepend-inner-icon="event"
          readonly
          v-model="localField.model"
          v-on="on"
        >
          <template v-slot:append-outer>
            <v-tooltip bottom>
              <template v-slot:activator="{ on }">
                <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
              </template>
              Remove
            </v-tooltip>
          </template>
        </v-text-field>
      </template>
      <v-date-picker
        locale="en-in"
        v-model="localField.model"
        @input="fromDateMenu = false"
      ></v-date-picker>
    </v-menu>
    <v-menu
      v-else-if="field.inputType === 'time'"
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
          :label="localField.title"
          :placeholder="localField.title"
          prepend-inner-icon="schedule"
          readonly
          v-model="localField.model"
          v-on="on"
        >
          <template v-slot:append-outer>
            <v-tooltip bottom>
              <template v-slot:activator="{ on }">
                <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
              </template>
              Remove
            </v-tooltip>
          </template>
        </v-text-field>
      </template>
      <v-time-picker v-model="localField.model" format="24hr"></v-time-picker>
    </v-menu>
    <v-checkbox
      v-else-if="field.inputType === 'checkbox'"
      v-model="localField.model"
    >
      <template v-slot:label>
        <div>
          {{ localField.title }}
          <v-tooltip bottom>
            <template v-slot:activator="{ on }">
              <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
            </template>
            Remove
          </v-tooltip>
        </div>
      </template>
    </v-checkbox>
    <v-switch
      v-else-if="field.inputType === 'switch'"
      v-model="localField.model"
    >
      <template v-slot:label>
        <div>
          {{ localField.title }}
          <v-tooltip bottom>
            <template v-slot:activator="{ on }">
              <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
            </template>
            Remove
          </v-tooltip>
        </div>
      </template>
    </v-switch>

    <v-flex v-else-if="field.inputType === 'radiogroup'">
      <v-col cols="12">
        Sexe
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
          </template>
          Remove
        </v-tooltip>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon small v-on="on" @click="radioDialog = !radioDialog"
              >add_circle_outline</v-icon
            >
          </template>
          Add an item
        </v-tooltip>
        <span class="mr-2 mf-2">:</span>
        <v-radio-group
          v-model="localField.model"
          :row="field.radioDirection === 'row' ? true : false"
        >
          <v-radio
            v-for="(radio, index) in localField.radios"
            :key="index"
            :value="radio.title"
          >
            <template v-slot:label>
              <div>
                {{ radio.title }}
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <v-icon x-small v-on="on" @click="editRadio(radio)"
                      >edit</v-icon
                    >
                  </template>
                  Edit
                </v-tooltip>
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <v-icon x-small v-on="on" @click="removeRadio(index)"
                      >delete</v-icon
                    >
                  </template>
                  Remove
                </v-tooltip>
              </div>
            </template>
          </v-radio>
        </v-radio-group>
      </v-col>
    </v-flex>

    <v-file-input
      outlined
      v-else-if="field.inputType === 'files'"
      v-model="field.model"
      :placeholder="
        field.multiple ? 'Upload your documents' : 'Upload your document'
      "
      :label="field.title"
      :multiple="field.multiple"
      counter
      prepend-inner-icon="mdi-paperclip"
    >
      <template v-slot:selection="{ index, text }">
        <v-chip v-if="index < 2" color="blue accent-4" dark label small>{{
          text
        }}</v-chip>

        <span
          v-else-if="index === 2"
          class="overline grey--text text--darken-3 mx-2"
          >+{{ field.model.length - 2 }} File(s)</span
        >
      </template>
      <template v-slot:append-outer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-icon x-small v-on="on" @click="remove()">delete</v-icon>
          </template>
          Remove
        </v-tooltip>
      </template>
    </v-file-input>
    <v-dialog v-model="radioDialog" width="800px">
      <v-card>
        <v-container>
          <v-row class="mx-2">
            <v-col cols="12">
              <v-text-field
                outlined
                v-model="radioTitle"
                clearable
                placeholder="Radio Title"
                label="Radio Title"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="radioDialog = false">Close</v-btn>
          <v-btn text @click="save">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialog" width="800px">
      <v-card>
        <v-container>
          <v-row class="mx-2">
            <v-col cols="6">
              <v-btn
                class="ma-2"
                block
                outlined
                :color="itemType === 'session' ? 'blue' : 'gray'"
                @click="selectItemType('session')"
                >Add a new session</v-btn
              >
            </v-col>
            <v-col cols="6">
              <v-btn
                class="ma-2"
                block
                outlined
                :color="itemType === 'field' ? 'blue' : 'gray'"
                @click="selectItemType('field')"
                >Add a new field</v-btn
              >
            </v-col>
          </v-row>
          <v-flex v-if="itemType === 'session'" transition="slide-x-transition">
            <v-row class="mx-2">
              <v-col class="align-center justify-space-between" cols="12">
                <v-row align="center" class="mr-0">
                  <v-text-field
                    outlined
                    v-model="sessionName"
                    clearable
                    placeholder="Session Name"
                    label="Session Name"
                  ></v-text-field>
                </v-row>
              </v-col>
            </v-row>
          </v-flex>
          <v-flex v-if="itemType === 'field'" transition="slide-x-transition">
            <v-row class="mx-2" v-if="sessions.length">
              <v-col class="align-center justify-space-between" cols="12">
                <v-select
                  outlined
                  :readonly="field && field.id ? true : false"
                  v-model="selectedSession"
                  :items="sessions"
                  item-text="title"
                  item-value="id"
                  label="Shose a session"
                  placeholder="Shose a session"
                ></v-select>
              </v-col>
              <v-col class="align-center justify-space-between" cols="6">
                <v-text-field
                  outlined
                  v-model="field.title"
                  label="Field Name"
                  clearable
                  placeholder="Field Name"
                ></v-text-field>
              </v-col>
              <v-col class="align-center justify-space-between" cols="6">
                <v-select
                  outlined
                  v-model="field.col"
                  :items="cols"
                  item-text="title"
                  item-value="id"
                  label="Shose a width"
                  placeholder="Shose a width"
                ></v-select>
              </v-col>
              <v-col class="align-center justify-space-between" cols="12">
                <v-select
                  outlined
                  v-model="field.inputType"
                  :items="inputTypes"
                  item-text="title"
                  item-value="id"
                  label="Type of input"
                  placeholder="Type of input"
                ></v-select>
              </v-col>
              <v-col
                v-if="field.inputType === 'radiogroup'"
                class="align-center justify-space-between"
                cols="12"
              >
                <v-select
                  outlined
                  v-model="field.radioDirection"
                  :items="radioDirections"
                  item-text="title"
                  item-value="id"
                  label="Type of input"
                  placeholder="Type of input"
                ></v-select>
              </v-col>
              <v-col
                v-if="field.inputType === 'files'"
                class="align-center justify-space-between"
                cols="12"
              >
                <v-checkbox
                  v-model="field.multiple"
                  label="Multiple files ?"
                ></v-checkbox>
              </v-col>
            </v-row>
            <v-alert
              v-else
              border="bottom"
              colored-border
              type="warning"
              elevation="2"
            >
              You have to create at least a session first, before you add a
              field !
            </v-alert>
          </v-flex>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="dialog = false">Close</v-btn>
          <v-btn text @click="save">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-btn
      bottom
      color="pink"
      dark
      fab
      small
      fixed
      right
      @click="dialog = !dialog"
    >
      <v-icon>mdi-plus</v-icon>
    </v-btn>
  </v-flex>
</template>
<script>
export default {
  props: [
    "visit",
    "field",
    "fieldId",
    "removeItem",
    "sessionId",
    "editItem",
    "update"
  ],
  data() {
    return {
      dialog: false,
      radioCurrentIndex: null,
      radioTitle: null,
      radioDialog: false,
      fromDateMenu: false,
      fromTimeMenu: false,
      localField: null
    };
  },
  methods: {
    remove() {
      this.$emit("removeItem", this.visit, this.sessionId, this.fieldId);
    },
    edit() {
      this.$emit("editItem", this.visit, this.sessionId, this.field);
    },
    updateDate(date) {
      this.localField.model = date;
    },
    save() {
      if (this.currentRadio && this.currentRadio.id) {
        const v = this.visit;
        const sessionIndex = v.fields.findIndex(f => f.id === this.sessionId);
        const fieldIndex = v.fields[sessionIndex].fields.findIndex(
          f => f.id === this.localField.id
        );
        const radionIndex = v.fields[sessionIndex].fields[
          fieldIndex
        ].radios.findIndex(r => r.id === this.currentRadio.id);
        v.fields[sessionIndex].fields[fieldIndex].radios[
          radionIndex
        ] = this.currentRadio;
        this.$emit("update", v);
        this.currentRadio = null;
        this.radioTitle = null;
        this.radioDialog = false;
      } else {
        const radio = {
          title: this.radioTitle,
          id: this.generateNewId()
        };
        this.localField.radios.push(radio);
        const v = this.visit;
        const sessionIndex = v.fields.findIndex(f => f.id === this.sessionId);
        const fieldIndex = v.fields[sessionIndex].fields.findIndex(
          f => f.id === this.localField.id
        );
        v.fields[sessionIndex].fields[fieldIndex] = this.localField;
        this.$emit("update", v);
        this.radioTitle = null;
      }
    },
    removeRadio(index) {
      this.localField.radios.splice(index, 1);
      const v = this.visit;
      const sessionIndex = v.fields.findIndex(f => f.id === this.sessionId);
      const fieldIndex = v.fields[sessionIndex].fields.findIndex(
        f => f.id === this.localField.id
      );
      v.fields[sessionIndex].fields[fieldIndex] = this.localField;
      this.$emit("update", v);
    },
    editRadio(radio) {
      this.currentRadio = { ...radio };
      this.radioDialog = !this.radioDialog;
    },
    generateNewId() {
      const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);

      const id =
        timestamp +
        "xxxxxxxxxxxxxxxx"
          .replace(/[x]/g, function() {
            return ((Math.random() * 16) | 0).toString(16);
          })
          .toLowerCase();
      return id;
    }
  },
  watch: {
    localField(val, old) {
      if (val && val !== old) {
        const v = this.visit;
        const sessionIndex = v.fields.findIndex(f => f.id === this.sessionId);
        const fieldIndex = v.fields[sessionIndex].fields.findIndex(
          f => f.id === val.id
        );
        v.fields[sessionIndex].fields[fieldIndex] = this.localField;
        //this.$emit("update", v);
      }
    }
  },
  created() {
    this.localField = this.field;
    if (this.localField.inputType === "date" && this.localField.model) {
      this.localField.model = new Date().toISOString().substr(0, 10);
    }
    if (this.localField.inputType === "radiogroup" && !this.localField.radios) {
      this.localField.radios = [];
    }
  }
};
</script>
