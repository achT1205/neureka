<template>
  <v-flex>
    <v-flex>
      <v-flex class="mt-3">
        <v-card>
          <v-card-title class="darken-2">
            {{ currentTemplate.title }}
            <v-spacer></v-spacer>
            <template-actions
              :actions="templateActions"
              :field="currentTemplate"
              @editSessionTemplate="editSessionTemplate"
              @editVisibility="editVisibility"
            />
          </v-card-title>
          <draggable
            class="row mx-2 content-row"
            tag="div"
            v-model="currentTemplate.fields"
            v-bind="dragOptions"
            :move="onMove"
            @start="isDragging = true"
            @end="isDragging = false"
            group="people"
          >
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="(subfield, subfieldindex) in currentTemplate.fields"
              :cols="subfield.col"
              :key="subfield.id ? subfield.id : subfieldindex"
            >
              <v-text-field
                outlined
                clearable
                :label="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                :placeholder="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                v-if="['text', 'number', 'email'].includes(subfield.type)"
                :type="subfield.type"
                v-model="subfield.model"
                :prepend-inner-icon="subfield.icon"
              >
                <template v-slot:append-outer>
                  <template-field-actions
                    :actions="fieldActions"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @duplicateField="duplicateField"
                  />
                </template>
              </v-text-field>
              <v-textarea
                outlined
                :placeholder="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                v-else-if="subfield.type === 'textarea'"
                clearable
                clear-icon="close"
                :label="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                v-model="subfield.model"
                :prepend-inner-icon="subfield.icon"
              >
                <template v-slot:append-outer>
                  <template-field-actions
                    :actions="fieldActions"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @duplicateField="duplicateField"
                  />
                </template>
              </v-textarea>
              <date-picker-field
                v-else-if="subfield.type === 'date'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="null"
                @edit="edit"
                @remove="remove"
                @editVisibility="editVisibility"
                @updateDate="updateDate"
              />

              <time-picker-field
                v-else-if="subfield.type === 'time'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="null"
                @edit="edit"
                @remove="remove"
                @editVisibility="editVisibility"
                @updateDate="updateTime"
              />

              <v-checkbox
                v-else-if="subfield.type === 'checkbox'"
                v-model="subfield.model"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <template-field-actions
                      :actions="fieldActions"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @duplicateField="duplicateField"
                    />
                  </div>
                </template>
              </v-checkbox>

              <v-switch
                v-else-if="subfield.type === 'switch'"
                v-model="subfield.model"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <template-field-actions
                      :actions="fieldActions"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @duplicateField="duplicateField"
                    />
                  </div>
                </template>
              </v-switch>

              <v-flex v-else-if="subfield.type === 'selection'">
                {{ subfield.title ? subfield.title : "Field_" + subfieldindex }}
                <v-select
                  outlined
                  v-model="subfield.model"
                  :items="subfield.selects"
                  :label="subfield.title"
                  :placeholder="subfield.title"
                  :prepend-inner-icon="subfield.icon"
                  menu-props="auto"
                  single-line
                  item-text="title"
                  item-value="id"
                  :multiple="subfield.multiple"
                  no-data-text="No item available, click on '+' to add a new item "
                >
                  <template v-slot:append-outer>
                    <template-field-actions
                      :actions="fieldActions"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @duplicateField="duplicateField"
                    />
                  </template>
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'selections'">
                {{ subfield.title ? subfield.title : "Field_" + subfieldindex }}
                <v-select
                  outlined
                  v-model="subfield.models"
                  :items="subfield.selects"
                  :label="subfield.title"
                  :placeholder="subfield.title"
                  :prepend-inner-icon="subfield.icon"
                  menu-props="auto"
                  single-line
                  item-text="title"
                  item-value="id"
                  :multiple="subfield.multiple"
                  no-data-text="No item available, click on '+' to add a new item "
                  chips
                >
                  <template v-slot:append-outer>
                    <template-field-actions
                      :actions="fieldActions"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @duplicateField="duplicateField"
                    />
                  </template>
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'radiogroup'">
                <v-col cols="12">
                  {{
                    subfield.title ? subfield.title : "Field_" + subfieldindex
                  }}
                  <template-field-actions
                    :actions="fieldActions"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @duplicateField="duplicateField"
                  />
                  <v-radio-group
                    v-model="subfield.model"
                    :row="subfield.radioDirection"
                  >
                    <v-radio
                      v-for="(radio, radioIndex) in subfield.radios"
                      :key="radioIndex"
                      :value="radio.title"
                    >
                      <template v-slot:label>
                        <div>
                          {{ radio.title }}
                          <v-tooltip bottom>
                            <template v-slot:activator="{ on }">
                              <v-icon
                                small
                                v-on="on"
                                @click="
                                  editRadio(subfieldindex, subfield, radio)
                                "
                                >edit</v-icon
                              >
                            </template>
                            Edit
                          </v-tooltip>
                          <v-tooltip bottom>
                            <template v-slot:activator="{ on }">
                              <v-icon
                                small
                                v-on="on"
                                @click="removeRadio(subfieldindex, radioIndex)"
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
                v-else-if="subfield.type === 'files'"
                v-model="subfield.model"
                :placeholder="
                  subfield.multiple
                    ? 'Upload your documents'
                    : 'Upload your document'
                "
                :label="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                :multiple="subfield.multiple"
                counter
                prepend-icon=""
                prepend-inner-icon="mdi-paperclip"
              >
                <template v-slot:selection="{ index, text }">
                  <v-chip
                    v-if="index < 2"
                    color="blue accent-4"
                    dark
                    label
                    small
                    >{{ text }}</v-chip
                  >

                  <span
                    v-else-if="index === 2"
                    class="overline grey--text text--darken-3 mx-2"
                    >+{{ subfield.model.length - 2 }} File(s)</span
                  >
                </template>
                <template v-slot:append-outer>
                  <template-field-actions
                    :actions="fieldActions"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @duplicateField="duplicateField"
                  />
                </template>
              </v-file-input>
            </v-col>
          </draggable>
        </v-card>
      </v-flex>
      <v-dialog v-model="radioDialog" width="800px">
        <v-card class="pa-3">
          <v-container>
            <v-row class="mx-2">
              <v-col cols="12">
                <v-text-field
                  outlined
                  v-model="radioTitle"
                  clearable
                  placeholder="Radio Title"
                  label="Radio Title"
                  :prepend-inner-icon="'title'"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="radioDialog = false"
              >Close</v-btn
            >
            <v-btn text @click="saveRadio">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="selectionDialog" width="800px">
        <v-card class="pa-3">
          <v-container>
            <v-row class="mx-2">
              <v-col cols="12">
                <v-text-field
                  outlined
                  v-model="selectTitle"
                  clearable
                  placeholder="Item title"
                  label="Item title"
                  :prepend-inner-icon="'title'"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="selectionDialog = false"
              >Close</v-btn
            >
            <v-btn text @click="saveSelectionItem">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <div
        class="mt-5"
        v-if="
          currentTemplate &&
            currentTemplate.fields &&
            currentTemplate.fields.length
        "
      >
        <v-btn class="ma-2" outlined color="blue" @click="saveTemplate"
          >Save</v-btn
        >
      </div>
      <v-snackbar
        v-model="snackbar"
        :timeout="timeout"
        absolute
        bottom
        color="success"
        outlined
        >{{ snackText }}</v-snackbar
      >

      <v-dialog v-model="editingSession" width="800px">
        <v-card class="pa-3">
          <v-container>
            <v-flex transition="slide-x-transition">
              <v-row class="mx-2">
                <v-col class="align-center justify-space-between" cols="12">
                  <v-row align="center" class="mr-0">
                    <v-text-field
                      outlined
                      v-model="sessionName"
                      clearable
                      placeholder="Form name"
                      label="Give a name to this form"
                      :prepend-inner-icon="'title'"
                    ></v-text-field>
                  </v-row>
                </v-col>
              </v-row>
            </v-flex>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="editingSession = false"
              >Close</v-btn
            >
            <v-btn text @click="saveSession">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="dialog" persistent width="800px">
        <v-card class="pa-3">
          <v-container>
            <v-flex transition="slide-x-transition">
              <v-row class="mx-2">
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
                <v-col
                  v-if="field.type === 'files'"
                  class="align-center justify-space-between"
                  cols="12"
                >
                  <v-checkbox
                    v-model="field.multiple"
                    label="Multiple files ?"
                  ></v-checkbox>
                </v-col>

                <v-row class="mx-2" v-if="field.type === 'radiogroup'">
                  <v-col class="align-center justify-space-between" cols="12">
                    <v-select
                      outlined
                      v-model="field.radioDirection"
                      :items="radioDirections"
                      item-text="title"
                      item-value="id"
                      label="Dierction"
                      placeholder="Direction"
                    ></v-select>
                  </v-col>

                  <v-col cols="12">
                    <v-text-field
                      outlined
                      v-model="radioTitle"
                      clearable
                      placeholder="Type a new item and enter 'Enter' Key to add"
                      label="Add a new Radio item"
                      @keyup.enter="saveRadio"
                      :prepend-inner-icon="'title'"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-row>
              <v-flex
                v-if="field.type === 'selections' || field.type === 'selection'"
              >
                <v-row class="mx-2">
                  <v-col cols="12">
                    <v-text-field
                      outlined
                      v-model="selectTitle"
                      clearable
                      placeholder="Type a new item and enter 'Enter' Key to add"
                      label="Add a new selection item"
                      @keyup.enter="saveSelectionItem"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row class="mx-2" v-if="field && field.selects">
                  <h5>Selectin items</h5>
                  <v-col cols="12">
                    <v-chip
                      v-for="(select, selectindex) in field.selects"
                      :key="selectindex"
                      class="ma-2"
                      close
                      color="teal"
                      text-color="white"
                      close-icon="mdi-delete"
                      @click:close="removeSelectItem(selectindex)"
                      >{{ select.title }}</v-chip
                    >
                  </v-col>
                </v-row>
              </v-flex>
            </v-flex>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="closeDialog">Close</v-btn>
            <v-btn text @click="save">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-flex>
    <v-flex>
      <v-menu
        v-model="menu"
        :close-on-content-click="false"
        :nudge-width="200"
        offset-x
      >
        <template v-slot:activator="{ on }">
          <v-btn bottom color="pink" dark fab small fixed right v-on="on">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </template>
        <v-card class="pa-3">
          <v-flex>
            <v-list>
              <draggable
                v-model="inputTypes"
                v-bind="dragOptions"
                :move="onMove"
                @start="isDragging = true"
                @end="isDragging = false"
                :group="{ name: 'people', pull: 'clone', put: false }"
                :clone="cloneDog"
              >
                <transition-group type="transition" :name="'flip-list'">
                  <v-list-item
                    class="list-group-item"
                    v-for="element in inputTypes"
                    :key="element.type"
                  >
                    <v-list-item-action>
                      <v-icon light>{{ element.icon }}</v-icon>
                    </v-list-item-action>
                    <v-list-item-title>{{ element.type }}</v-list-item-title>
                  </v-list-item>
                </transition-group>
              </draggable>
            </v-list>
          </v-flex>
        </v-card>
      </v-menu>
    </v-flex>
  </v-flex>
</template>

<script>
import DatePickerField from "@/components/DatePickerField.vue";
import TimePickerField from "@/components/TimePickerField.vue";
import TemplateActions from "@/components/TemplateActions.vue";
import TemplateFieldActions from "@/components/TemplateFieldActions.vue";
import draggable from "vuedraggable";
import { mapGetters } from "vuex";
export default {
  components: {
    DatePickerField,
    TimePickerField,
    TemplateActions,
    draggable,
    TemplateFieldActions
  },
  data() {
    return {
      templateActions: [
        {
          title: "Edit Session",
          icon: "edit"
        },
        {
          titles: ["Allow patient to see", "Hide to the patient"],
          icons: ["visibility", "visibility_off"]
        }
      ],
      fieldActions: [
        {
          title: "Edit",
          icon: "edit"
        },
        {
          title: "Remove",
          icon: "delete"
        },
        {
          title: "Duplicate",
          icon: "control_point_duplicate"
        },
        {
          titles: ["Allow patient to see", "Hide to the patient"],
          icons: ["visibility", "visibility_off"]
        }
      ],
      snackbar: false,
      snackText: "The template has been saved successfully !",
      timeout: 2000,
      fab: false,
      editable: true,
      isDragging: false,
      delayedDragging: false,
      menu: false,
      subfieldindex: null,
      currentRadio: null,
      currentSelect: null,
      radioTitle: null,
      selectTitle: null,
      radioDialog: false,
      selectionDialog: false,
      fromDateMenu: false,
      fromTimeMenu: false,
      validTemplate: true,
      template: {},
      field: {},
      multiple: null,
      editingSession: false,
      currentSession: null,
      removingDialog: false,
      col: null,
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
      inputType: null,
      radioDirections: [
        {
          id: false,
          title: "column"
        },
        {
          id: true,
          title: "row"
        }
      ],
      defaultForm: {
        id: null,
        inputType: "session",
        type: "form",
        title: null,
        icon: "list_alt",
        fields: []
      },
      inputTypes: [
        {
          id: null,
          title: "",
          icon: "title",
          col: "12",
          inputType: "field",
          type: "text",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "plus_one",
          col: "12",
          inputType: "field",
          type: "number",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "alternate_email",
          col: "12",
          inputType: "field",
          type: "email",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "event",
          col: "2",
          inputType: "field",
          type: "date",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "access_time",
          col: "2",
          inputType: "field",
          type: "time",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "done",
          col: "2",
          inputType: "field",
          type: "checkbox",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "toggle_off",
          col: "2",
          inputType: "field",
          type: "switch",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "text_fields",
          col: "12",
          inputType: "field",
          type: "textarea",
          model: null,
          isVisible: false
        },
        {
          id: null,
          title: "",
          icon: "radio_button_checked",
          col: "12",
          inputType: "field",
          type: "radiogroup",
          model: null,
          radioDirection: true,
          isVisible: false,
          radios: []
        },
        {
          id: null,
          title: "",
          icon: "indeterminate_check_box",
          col: "12",
          inputType: "field",
          type: "selection",
          model: null,
          isVisible: false,
          multiple: false,
          selects: []
        },
        {
          id: null,
          title: "",
          icon: "done_all",
          col: "12",
          inputType: "field",
          type: "selections",
          models: null,
          isVisible: false,
          multiple: true,
          selects: []
        },

        {
          id: null,
          title: "",
          icon: "attach_file",
          col: "12",
          inputType: "field",
          type: "files",
          model: null,
          isVisible: false,
          multiple: false
        },
        {
          id: null,
          title: "",
          icon: "photo",
          col: "12",
          inputType: "field",
          type: "image",
          model: null,
          isVisible: false
        }
      ],
      selectedSession: null,
      sessionName: "",
      dialog: false,
      drag: false
    };
  },
  watch: {
    dialog(val) {
      if (!val) {
        this.field = {};
      }
    },
    isDragging(newValue) {
      if (newValue) {
        this.delayedDragging = true;
        return;
      }
      this.$nextTick(() => {
        this.delayedDragging = false;
      });
    }
  },
  computed: {
    ...mapGetters(["currentTemplate"]),
    dragOptions() {
      return {
        animation: 0,
        group: "description",
        disabled: !this.editable,
        ghostClass: "ghost"
      };
    }
  },
  methods: {
    saveSession() {
      const template = this.currentTemplate;
      template.title = this.sessionName;
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.sessionName = null;
      this.editingSession = false;
    },
    editSessionTemplate() {
      this.sessionName = this.currentTemplate.title;
      this.editingSession = true;
    },
    updateDate(index, subfieldindex, val) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].model = val;
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    updateTime(index, subfieldindex, val) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].model = val;
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    cloneDog(item) {
      const f = {
        ...item
      };
      f.id = this.generateNewId();
      return f;
    },
    onMove({ relatedContext, draggedContext }) {
      const relatedElement = relatedContext.element;
      const draggedElement = draggedContext.element;
      return (
        (!relatedElement || !relatedElement.fixed) && !draggedElement.fixed
      );
    },
    duplicateField(subfieldindex, subfield) {
      const template = this.currentTemplate;
      const duble = {
        ...subfield
      };
      duble.id = this.generateNewId();

      template.fields.splice(subfieldindex + 1, 0, duble);
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    editVisibility(subfieldindex) {
      const template = this.currentTemplate;
      if (!subfieldindex) {
        template.isVisible = template.isVisible ? false : true;
      } else {
        template.fields[subfieldindex].isVisible = !template.fields[
          subfieldindex
        ].isVisible;
      }
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    addForm() {
      const form = {
        ...this.defaultForm
      };
      form.id = this.generateNewId();
      const template = this.currentTemplate;
      if (!template.fields) template.fields = [];
      template.fields.push(form);
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    removeRadio(subfieldindex, radioIndex) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].radios[radioIndex];
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },

    removeSelection(subfieldindex, selectIndex) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].radios[selectIndex];
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.$store.commit("SET_TEMPLATE", template);
    },

    openRadioDialog(subfieldindex, subfield) {
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.radioDialog = true;
    },

    openSelectionDialog(subfieldindex, subfield) {
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.selectionDialog = true;
    },

    editRadio(subfieldindex, subfield, radio) {
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.currentRadio = radio;
      this.radioTitle = radio.title;
      this.radioDialog = true;
    },
    saveRadio() {
      if (this.currentRadio && this.currentRadio.id) {
        const template = this.currentTemplate;
        this.currentRadio.title = this.radioTitle;
        const radioIndex = this.field.radios.findIndex(
          r => r.id === this.currentRadio.id
        );
        this.field.radios[radioIndex] = this.currentRadio;
        template.fields[this.subfieldindex] = this.field;
        this.$store.commit("SET_TEMPLATE", template);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true
        });
        this.radioTitle = null;
        this.subfieldindex = null;
        this.currentRadio = null;
        this.field = {};
        this.radioDialog = false;
      } else {
        const radio = {
          title: this.radioTitle,
          id: this.generateNewId()
        };
        if (!this.field.radios) {
          this.field.radios = [radio];
        } else {
          this.field.radios.push(radio);
        }
        const template = this.currentTemplate;
        template.fields[this.subfieldindex] = this.field;
        this.$store.commit("SET_TEMPLATE", template);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true
        });
        this.radioTitle = null;
      }
    },

    saveSelectionItem() {
      const select = {
        title: this.selectTitle,
        id: this.generateNewId()
      };
      if (!this.field.selects) {
        this.field.selects = [select];
      } else {
        this.field.selects.push(select);
      }
      const template = this.currentTemplate;
      template.fields[this.subfieldindex] = this.field;
      this.$store.commit("SET_TEMPLATE", template);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.selectTitle = null;
    },

    closeDialog() {
      this.dialog = false;
      this.subfieldindex = null;
      this.field = null;
      this.dialog = false;
    },
    removeSelectItem(selectindex) {
      const template = {
        ...this.currentTemplate
      };
      this.field.selects.splice(selectindex, 1);
      template.fields[this.subfieldindex].selects.splice(selectindex, 1);
      this.$store.commit("SET_TEMPLATE", template);
    },

    remove(fieldindex) {
      const template = this.currentTemplate;
      template.fields.splice(fieldindex, 1);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.$store.commit("SET_TEMPLATE", template);
    },
    edit(subfieldindex, field) {
      this.subfieldindex = subfieldindex;
      this.field = {
        ...field
      };
      this.dialog = !this.dialog;
    },
    save() {
      if (this.field && this.field.id) {
        const template = this.currentTemplate;
        template.fields[this.subfieldindex] = this.field;
        this.$store.commit("SET_TEMPLATE", template);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true
        });
        this.subfieldindex = null;
        this.dialog = false;
      } else {
        const field = {
          ...this.field
        };
        field.id = this.generateNewId();
        const template = this.currentTemplate;
        template.fields.push(field);
        this.$store.commit("SET_TEMPLATE", template);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true
        });
        this.dialog = false;
      }
    },
    saveTemplate() {
      const template = this.currentTemplate;
      template.fields.forEach(f => {
        delete f.model;
        delete f.models;
      });
      this.$store.dispatch("editTemplate", template);
      this.snackbar = true;
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
  }
};
</script>

<style>
.flip-list-move {
  transition: transform 0.5s;
}

.no-move {
  transition: transform 0s;
}

.ghost {
  opacity: 0.5;
  margin-bottom: 30px;
  background: lightgray;
}

.list-group-item {
  cursor: move;
}

.list-group-item i {
  cursor: pointer;
}

.mb-42 {
  margin-bottom: 42px;
}

.content-row {
  min-height: 150px;
}
</style>
