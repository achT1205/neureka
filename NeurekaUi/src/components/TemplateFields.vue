<template>
  <v-flex>
    <v-flex>
      <v-flex class="mt-3">
        <v-card>
          <v-card-title class="darken-2">
            {{ currentTemplate.title }}
            <v-spacer></v-spacer>
            <form-actions
              :templating="true"
              :actions="formActions"
              :field="currentTemplate"
              @editSessionvisit="editSessionvisit"
              @editVisibility="editVisibility"
            />
          </v-card-title>

          <draggable
            class="row mx-2 content-row"
            tag="div"
            v-model="currentTemplate.fields"
            :options="dragOptions"
          >
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="(subfield, subfieldindex) in currentTemplate.fields"
              :cols="subfield.col"
              :key="uuid(subfield)"
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
                :maxlength="subfield.maxlength"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
                <template v-slot:append-outer>
                  <field-actions
                    :actions="fieldActions"
                    :field="field"
                    :index="null"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @canOnlyRead="canOnlyRead"
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
                  />
                </template>
              </v-text-field>

              <v-text-field
                outlined
                clearable
                :label="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                :placeholder="
                  subfield.title ? subfield.title : 'Field' + subfieldindex
                "
                v-else-if="subfield.type === 'decimal'"
                :type="subfield.type"
                v-money="subfield.option"
                v-model.lazy="subfield.model"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
                <template v-slot:append-outer>
                  <field-actions
                    :actions="fieldActions"
                    :field="field"
                    :index="null"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @canOnlyRead="canOnlyRead"
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
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
                :maxlength="subfield.maxlength"
                :prepend-inner-icon="subfield.icon"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
                <template v-slot:append-outer>
                  <field-actions
                    :actions="fieldActions"
                    :field="field"
                    :index="null"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @canOnlyRead="canOnlyRead"
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
                  />
                </template>
              </v-textarea>
              <v-row v-else-if="subfield.type === 'editor'">
                <v-col cols="12">
                  <label class="d-flex justify-start">
                    {{
                      subfield.title ? subfield.title : "Field" + subfieldindex
                    }}
                  </label>
                  <label class="d-flex justify-end">
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </label>
                </v-col>
                <v-col cols="12">
                  <vue-editor v-model="subfield.model"></vue-editor>
                </v-col>
              </v-row>
              <date-picker-field
                v-else-if="subfield.type === 'date'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="null"
                @edit="edit"
                @remove="remove"
                @editVisibility="editVisibility"
                @canOnlyRead="canOnlyRead"
                @updateDate="updateDate"
                @duplicateField="duplicateField"
                @saveAsFieldTemplate="saveAsFieldTemplate"
                :actions="fieldActions"
              />

              <time-picker-field
                v-else-if="subfield.type === 'time'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="null"
                @edit="edit"
                @remove="remove"
                @editVisibility="editVisibility"
                @canOnlyRead="canOnlyRead"
                @updateDate="updateTime"
                @duplicateField="duplicateField"
                @saveAsFieldTemplate="saveAsFieldTemplate"
                :actions="fieldActions"
              />

              <v-checkbox
                v-else-if="subfield.type === 'checkbox'"
                v-model="subfield.boolModel"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </div>
                </template>
              </v-checkbox>

              <v-switch
                v-else-if="subfield.type === 'switch'"
                v-model="subfield.boolModel"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </div>
                </template>
              </v-switch>

              <v-flex v-else-if="subfield.type === 'selection'">
                <label>{{
                  subfield.title ? subfield.title : "Field_" + subfieldindex
                }}</label>
                <v-select
                  outlined
                  v-model="subfield.model"
                  :items="subfield.selects"
                  :label="subfield.title"
                  :placeholder="
                    subfield.title ? subfield.title : 'Field_' + subfieldindex
                  "
                  :prepend-inner-icon="subfield.icon"
                  menu-props="auto"
                  single-line
                  item-text="title"
                  item-value="id"
                  :multiple="subfield.multiple"
                  no-data-text="No item available, click on '+' to add a new item "
                >
                  <template v-slot:append-outer>
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </template>
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'selections'">
                <label>{{
                  subfield.title ? subfield.title : "Field_" + subfieldindex
                }}</label>
                <v-select
                  outlined
                  v-model="subfield.models"
                  :items="subfield.selects"
                  :label="subfield.title"
                  :placeholder="
                    subfield.title ? subfield.title : 'Field_' + subfieldindex
                  "
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
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </template>
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'radiogroup'">
                <v-col cols="12">
                  {{
                    subfield.title ? subfield.title : "Field_" + subfieldindex
                  }}
                  <field-actions
                    :actions="fieldActions"
                    :field="field"
                    :index="null"
                    :subfield="subfield"
                    :subfieldindex="subfieldindex"
                    @edit="edit"
                    @remove="remove"
                    @editVisibility="editVisibility"
                    @canOnlyRead="canOnlyRead"
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
                  />
                  <v-radio-group
                    v-model="subfield.model"
                    :row="subfield.radioDirection"
                  >
                    <v-radio
                      v-for="(radio, radioIndex) in subfield.radios"
                      :key="radioIndex"
                      :value="radio"
                    >
                      <template v-slot:label>
                        <div>
                          {{ radio }}
                          <v-menu
                            bottom
                            left
                            :close-on-content-click="false"
                            transition="slide-x-transition"
                          >
                            <template v-slot:activator="{ on, attrs }">
                              <v-btn
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
                                @click="
                                  editRadio(
                                    index,
                                    subfieldindex,
                                    radioIndex,
                                    radio
                                  )
                                "
                              >
                                <v-list-item-action>
                                  <v-icon>edit</v-icon>
                                </v-list-item-action>
                                <v-list-item-title>Edit</v-list-item-title>
                              </v-list-item>
                              <v-list-item
                                @click="
                                  removeRadio(index, subfieldindex, radioIndex)
                                "
                              >
                                <v-list-item-action>
                                  <v-icon>delete</v-icon>
                                </v-list-item-action>
                                <v-list-item-title>Remove</v-list-item-title>
                              </v-list-item>
                            </v-list>
                          </v-menu>
                        </div>
                      </template>
                    </v-radio>
                  </v-radio-group>
                </v-col>
              </v-flex>
              <v-flex v-else-if="subfield.type === 'files'">
                <v-file-input
                  outlined
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
                  show-size
                  counter
                  prepend-icon
                  prepend-inner-icon="mdi-paperclip"
                  accept="application/*, image/*"
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
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="null"
                      :subfield="subfield"
                      :subfieldindex="subfieldindex"
                      @edit="edit"
                      @remove="remove"
                      @editVisibility="editVisibility"
                      @canOnlyRead="canOnlyRead"
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </template>
                </v-file-input>
              </v-flex>
            </v-col>
          </draggable> </v-card
      ></v-flex>
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
            <v-btn text color="secondary" @click="radioDialog = false"
              >Close</v-btn
            >
            <v-btn text color="primary" @click="saveRadio">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

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
            <v-btn text color="secondary" @click="editingSession = false"
              >Close</v-btn
            >
            <v-btn text color="primary" @click="save">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <v-dialog v-model="fieldTemplateDialog" width="800px">
        <v-card class="pa-3">
          <v-card-title class="headline"
            >Give a name and a description to this template</v-card-title
          >
          <v-container>
            <v-flex transition="slide-x-transition">
              <v-row class="mx-2">
                <v-col class="align-center justify-space-between" cols="12">
                  <v-row align="center" class="mr-0">
                    <v-text-field
                      outlined
                      label="Template Title"
                      clearable
                      v-model="fieldTemplate.title"
                      placeholder="Template Title"
                      :prepend-inner-icon="'title'"
                    ></v-text-field>
                  </v-row>
                </v-col>
                <v-col class="align-center justify-space-between" cols="12">
                  <v-row align="center" class="mr-0">
                    <v-textarea
                      outlined
                      label="Template Description"
                      clearable
                      v-model="fieldTemplate.description"
                      placeholder="Template Description"
                    ></v-textarea>
                  </v-row>
                </v-col>
              </v-row>
            </v-flex>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="secondary" @click="fieldTemplateDialog = false"
              >Close</v-btn
            >
            <v-btn text color="primary" @click="saveFieldTemplate">Save</v-btn>
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

      <v-dialog v-model="dialog" persistent width="800px">
        <edit-field-options
          :field="field"
          @closeDialog="closeDialog"
          @save="save"
          @close="closeDialog"
          :fieldNameExistAlreadyExist="fieldNameExistAlreadyExist"
        />
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
            <v-icon>add</v-icon>
          </v-btn>
        </template>
        <v-card class="pa-3">
          <v-flex>
            <v-list>
              <draggable
                v-model="inputTypes"
                :options="availableItemOptions"
                :clone="handleClone"
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
            <v-divider></v-divider>
            <v-list>
              <v-subheader>Custom inputs</v-subheader>
              <draggable
                v-model="fieldtemplates"
                :options="availableItemOptions"
                :clone="handleClone"
              >
                <transition-group type="transition" :name="'flip-list'">
                  <v-list-item
                    class="list-group-item"
                    v-for="element in fieldtemplates"
                    :key="uuid(element)"
                  >
                    <v-list-item-action>
                      <v-icon light>{{ element.icon }}</v-icon>
                    </v-list-item-action>
                    <v-list-item-title>{{ element.title }}</v-list-item-title>
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
import draggable from "vuedraggable";
import FormActions from "@/components/FormActions.vue";
import FieldActions from "@/components/FieldActions.vue";
import EditFieldOptions from "@/components/EditFieldOptions.vue";
import { VueEditor } from "vue2-editor";
import { VMoney } from "v-money";
import { mapGetters } from "vuex";
export default {
  components: {
    FormActions,
    FieldActions,
    DatePickerField,
    TimePickerField,
    draggable,
    VueEditor,
    EditFieldOptions
  },
  directives: { money: VMoney },
  data() {
    return {
      formActions: [
        {
          title: "Edit",
          icon: "edit"
        },
        {
          titles: ["Show to patient", "Hide to patient"],
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
          titles: ["Show to patient", "Hide to patient"],
          icons: ["visibility", "visibility_off"]
        },
        {
          titles: ["Patient can edit ", "Patient can only read"],
          icons: ["lock_open", "lock"]
        },
        {
          title: "Save as Template",
          icon: "favorite"
        }
      ],
      sessionName: "",
      snackbar: false,
      snackText: "",
      timeout: 2000,
      filesData: [],
      selectedTemplate: null,
      active: null,
      fromDialog: false,
      editingVisit: false,
      fab: false,
      adding: false,
      sessionIndex: null,
      menu: false,
      subfieldindex: null,
      radioTitle: null,
      radioDialog: false,
      template: {},
      fieldTemplate: {},
      field: {},
      multiple: null,
      editingSession: false,
      currentSession: null,
      removingDialog: false,
      col: null,
      inputType: null,
      fieldNameExistAlreadyExist: false,
      defaultForm: {
        id: null,
        inputType: "session",
        type: "form",
        title: null,
        icon: "list_alt",
        isVisible: false,
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
          isVisible: false,
          readonly: false,
          disabled: false,
          maxlength: 1000
        },
        {
          id: null,
          title: "",
          icon: "plus_one",
          col: "12",
          inputType: "field",
          type: "number",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false,
          maxlength: 15
        },
        {
          id: null,
          title: "",
          icon: "money",
          col: "12",
          inputType: "field",
          type: "decimal",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false,
          option: {
            decimal: ",",
            thousands: ".",
            prefix: "",
            suffix: "",
            precision: 2,
            masked: false /* doesn't work with directive */,
            min: -999999999,
            max: 999999999,
            type: "currency"
          }
        },
        {
          id: null,
          title: "",
          icon: "alternate_email",
          col: "12",
          inputType: "field",
          type: "email",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false,
          maxlength: 300
        },
        {
          id: null,
          title: "",
          icon: "event",
          col: "2",
          inputType: "field",
          type: "date",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false
        },
        {
          id: null,
          title: "",
          icon: "access_time",
          col: "2",
          inputType: "field",
          type: "time",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false
        },
        {
          id: null,
          title: "",
          icon: "done",
          col: "2",
          inputType: "field",
          type: "checkbox",
          boolModel: null,
          isVisible: false,
          readonly: false,
          disabled: false
        },
        {
          id: null,
          title: "",
          icon: "toggle_off",
          col: "2",
          inputType: "field",
          type: "switch",
          boolModel: null,
          isVisible: false,
          readonly: false,
          disabled: false
        },
        {
          id: null,
          title: "",
          icon: "text_fields",
          col: "12",
          inputType: "field",
          type: "textarea",
          model: null,
          isVisible: false,
          readonly: false,
          disabled: false,
          maxlength: 2000
        },
        {
          id: null,
          title: "",
          icon: "edit_note",
          col: "12",
          inputType: "field",
          type: "editor",
          model: "<h1>Some initial content</h1>",
          isVisible: false,
          readonly: false,
          disabled: false,
          maxlength: 2000
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
          radios: [],
          readonly: false,
          disabled: false
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
          selects: [],
          readonly: false,
          disabled: false
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
          selects: [],
          readonly: false,
          disabled: false
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
          multiple: false,
          readonly: false,
          disabled: false
        }
      ],
      dialog: false,
      templateDialog: false,
      fieldTemplateDialog: false,
      dragOptions: {
        animation: 0,
        group: "inputs"
      },
      availableItemOptions: {
        group: {
          name: "inputs",
          pull: "clone",
          put: false
        },
        sort: false
      }
    };
  },
  watch: {
    dialog(val) {
      if (!val) {
        this.field = {};
      }
    }
  },
  computed: {
    ...mapGetters(["currentTemplate", "fieldtemplates"])
  },
  methods: {
    updateDate(index, subfieldindex, val) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].model = val;
      this.$store.dispatch("editTemplate", template);
    },
    updateTime(index, subfieldindex, val) {
      const template = this.currentTemplate;
      template.fields[subfieldindex].model = val;
      this.$store.dispatch("editTemplate", template);
    },
    handleClone(item) {
      let cloneMe = JSON.parse(JSON.stringify(item));
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.$delete(cloneMe, "uid");

      return cloneMe;
    },
    editVisibility(index, subfieldindex) {
      const template = this.currentTemplate;
      if (!subfieldindex && subfieldindex !== 0) {
        template.isVisible = !template.isVisible;
      } else if (subfieldindex >= 0) {
        template.fields[subfieldindex].isVisible = !template.fields[
          subfieldindex
        ].isVisible;
      }
      this.$store.dispatch("editTemplate", template);
    },
    canOnlyRead(index, subfieldindex) {
      const template = this.currentTemplate;
      if (!subfieldindex && subfieldindex !== 0) {
        template.readonly = !template.readonly;
      } else {
        template.fields[subfieldindex].readonly = !template.fields[
          subfieldindex
        ].readonly;
      }
      this.$store.dispatch("editTemplate", template);
    },
    removeRadio(index, subfieldindex, radioIndex) {
      const template = { ...this.currentTemplate };
      template.fields[subfieldindex].radios.splice(radioIndex, 1);
      this.$store.dispatch("editTemplate", template);
    },
    editRadio(index, subfieldindex, radioIndex, radio) {
      this.sessionIndex = index;
      this.subfieldindex = subfieldindex;
      this.radioIndex = radioIndex;
      this.radioTitle = radio;
      this.radioDialog = true;
    },
    saveRadio() {
      const template = this.currentTemplate;
      template.fields[this.subfieldindex].radios[
        this.radioIndex
      ] = this.radioTitle;
      this.$store.dispatch("editTemplate", template);
      this.radioTitle = null;
      this.sessionIndex = null;
      this.subfieldindex = null;
      this.radioDialog = false;
      this.radioIndex = null;
    },
    remove(sessionIndex, fieldindex) {
      const template = this.currentTemplate;
      template.fields.splice(fieldindex, 1);
      this.$store.dispatch("editTemplate", template);
    },
    duplicateField(index, subfieldindex, subfield) {
      const template = this.currentTemplate;
      const doble = {
        ...subfield
      };
      doble.id = this.uuid({});

      template.fields.splice(subfieldindex + 1, 0, doble);
      this.$store.dispatch("editTemplate", template);
    },
    edit(sessionIndex, field) {
      const template = {
        ...this.currentTemplate
      };
      template.fields.findIndex(f => f.id === field.id);
      this.field = {
        ...field
      };
      this.dialog = !this.dialog;
    },
    closeDialog() {
      this.fieldNameExistAlreadyExist = false;
      this.dialog = false;
      setTimeout(function() {
        this.subfieldindex = null;
        this.field = null;
      }, 3000);
    },

    fieldNameExist(field) {
      const sessions = this.currentVisit.fields;
      let result = false;
      sessions.forEach(s => {
        const fields = s.fields;
        const exist = fields.filter(
          f =>
            f.title.toLowerCase() === field.title.toLowerCase() &&
            f.id !== field.id
        );
        if (exist && exist.length > 0) {
          result = true;
        }
      });
      return result;
    },

    save(field) {
      this.fieldNameExistAlreadyExist = false;
      if (this.editingSession) {
        this.currentSession.title = this.sessionName;
        const template = this.currentTemplate;
        template.title = this.sessionName;
        this.$store.dispatch("editTemplate", template);
        this.sessionName = null;
        this.currentSession = null;
        this.editingSession = false;
        this.field = {};
      } else {
        if (field && field.id) {
          if (this.fieldNameExist(field)) {
            this.fieldNameExistAlreadyExist = true;
            return;
          }
          const template = this.currentTemplate;
          const fieldIndex = template.fields.findIndex(
            f => f.id === this.field.id
          );
          template.fields[fieldIndex] = field;
          this.$store.dispatch("editTemplate", template);
          this.dialog = false;
          this.field = {};
        } else {
          const field = {
            ...this.field
          };
          delete field.id;
          field.id = this.uuid(field);
          const template = this.currentTemplate;
          template.fields.push(field);
          this.$store.dispatch("editTemplate", template);
          this.dialog = false;
          this.field = {};
        }
      }
    },
    saveTemplate() {
      const template = this.currentTemplate;
      template.fields.forEach(f => {
        delete f.model;
        delete f.boolModel;
        delete f.models;
      });
      this.$store.dispatch("editTemplate", template);
      this.snackbar = true;
    },
    editSessionvisit(session) {
      this.sessionName = session.title;
      this.currentSession = session;
      this.editingSession = true;
    },

    saveFieldTemplate() {
      if (!this.fieldTemplate.title || !this.fieldTemplate.title) {
        return;
      }
      this.fieldTemplate.id = this.uuid(this.fieldTemplate);
      this.fieldTemplate.modeld = 0;
      this.$store.dispatch("addFieldTemplate", this.fieldTemplate);
      this.fieldTemplateDialog = false;
    },

    saveAsFieldTemplate(template) {
      this.fieldTemplate = {
        ...template
      };
      this.fieldTemplate.modeld = null;
      this.fieldTemplateDialog = true;
    },
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
