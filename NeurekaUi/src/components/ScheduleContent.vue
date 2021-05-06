<template>
  <v-flex>
    <v-flex
      id="visit-container"
      v-if="currentVisit && currentVisit.fields && currentVisit.fields.length"
    >
      <v-flex
        v-for="(field, index) in currentVisit.fields"
        :key="field.id"
        :class="index !== 0 ? 'mt-3' : ''"
        :id="'form_id_' + field.id"
      >
        <v-card :loading="sessionIndex === index && adding">
          <v-card-title class="darken-2">
            {{ field.title ? field.title : "Form_" + index }}
            <v-spacer></v-spacer>
            <form-actions
              :actions="formActions"
              :field="field"
              :index="index"
              @saveAsTemplate="saveAsTemplate"
              @editSessionvisit="editSessionvisit"
              @removeSessionvisit="removeSessionvisit"
              @editVisibility="editVisibility"
              @canOnlyRead="canOnlyRead"
              @duplicateForm="duplicateForm"
            />
          </v-card-title>

          <draggable
            class="row mx-2 content-row"
            tag="div"
            v-model="field.fields"
            :options="dragOptions"
          >
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="(subfield, subfieldindex) in field.fields"
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
                    :index="index"
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
                    :index="index"
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
                    :index="index"
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
                      :index="index"
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
                :index="index"
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
                :index="index"
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
                v-model="subfield.model"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="index"
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
                v-model="subfield.model"
              >
                <template v-slot:label>
                  <div>
                    {{
                      subfield.title ? subfield.title : "Field_" + subfieldindex
                    }}
                    <field-actions
                      :actions="fieldActions"
                      :field="field"
                      :index="index"
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
                      :index="index"
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
                      :index="index"
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
                    :index="index"
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
                  @change="
                    handleFileChange(index, subfieldindex, subfield.model)
                  "
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
                      :index="index"
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
                <div v-if="subfield.fileInfos && subfield.fileInfos.length">
                  <span
                    v-for="(fileInfo, fileInfosIndex) in subfield.fileInfos"
                    :key="fileInfosIndex"
                  >
                    <v-chip
                      v-if="fileInfo && fileInfo.id"
                      class="ma-2"
                      close
                      color="teal"
                      text-color="white"
                      close-icon="delete"
                      @click:close="
                        delefile(index, subfieldindex, fileInfosIndex)
                      "
                    >
                      <v-avatar left>
                        <v-icon>
                          {{
                            fileInfo.type === "application/pdf"
                              ? "picture_as_pdf"
                              : fileInfo.type.indexOf("image/") > -1
                              ? "photo"
                              : "attachment"
                          }}
                        </v-icon>
                      </v-avatar>
                      {{ fileInfo.title }}
                    </v-chip>
                  </span>
                </div>
              </v-flex>
            </v-col>
          </draggable>
        </v-card>
      </v-flex>
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
          <v-btn text color="primary" @click="radioDialog = false">Close</v-btn>
          <v-btn text @click="saveRadio">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="removingDialog" persistent max-width="290">
      <v-card class="pa-3">
        <v-card-title class="headline">Removing Alert !</v-card-title>
        <v-card-text
          >Are you sure you want to remove this Session ?</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1" text @click="removingDialog = false"
            >Disagree</v-btn
          >
          <v-btn color="red darken-1" text @click="confirRemovingVisit()"
            >Agree</v-btn
          >
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
          <v-btn text color="primary" @click="editingSession = false"
            >Close</v-btn
          >
          <v-btn text @click="save">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="templateDialog" width="800px">
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
                    v-model="template.title"
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
                    v-model="template.description"
                    placeholder="Template Description"
                  ></v-textarea>
                </v-row>
              </v-col>
            </v-row>
          </v-flex>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="templateDialog = false"
            >Close</v-btn
          >
          <v-btn text @click="saveTemplate">Save</v-btn>
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
          <v-btn text color="primary" @click="fieldTemplateDialog = false"
            >Close</v-btn
          >
          <v-btn text @click="saveFieldTemplate">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialog" persistent width="800px">
      <edit-field-options
        :field="field"
        @closeDialog="closeDialog"
        @save="save"
        @close="closeDialog"
      />
    </v-dialog>

    <v-dialog v-model="fromDialog" width="800px">
      <v-card class="pa-3">
        <v-container>
          <v-flex transition="slide-x-transition">
            <v-row class="mx-2">
              <v-col cols="6">
                <v-btn
                  block
                  outlined
                  :color="active === 1 ? 'blue' : 'black'"
                  dark
                  @click="ShoseTemplate(1)"
                  >Create a blank form</v-btn
                >
              </v-col>
              <v-col cols="6">
                <v-btn
                  block
                  outlined
                  :color="active === 2 ? 'blue' : 'black'"
                  dark
                  @click="ShoseTemplate(2)"
                  >Shose a template</v-btn
                >
              </v-col>
            </v-row>
            <v-row class="mx-2">
              <v-col active-class="primary" cols="12">
                <v-expand-transition>
                  <v-autocomplete
                    v-model="selectedTemplate"
                    v-if="active === 2"
                    :items="templates"
                    :filter="customFilter"
                    color="white"
                    item-text="title"
                    item-value="id"
                    label="Select a template"
                    placeholder="Select a template"
                  ></v-autocomplete>
                </v-expand-transition>
              </v-col>
            </v-row>
          </v-flex>
        </v-container>
        <v-card-actions v-if="active === 2">
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="fromDialog = false">Close</v-btn>
          <v-btn text @click="addForm">Add</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <div
      class="mt-5 d-flex justify-end"
      v-if="currentVisit && currentVisit.fields && currentVisit.fields.length"
    >
      <v-btn class="ma-2" color="primary" @click="saveVisitData">Publish</v-btn>
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
    <v-flex v-if="currentVisit" class="full-flex-width">
      <v-alert
        v-if="currentVisit.fields && currentVisit.fields.length === 0"
        class="full-flex-width text-center"
        color="warning"
        style="color: white"
      >
        There is currently no Form for the current visit, please click on the
        <strong class="ml-2 mr-2">blue</strong> button bellow to add one
      </v-alert>
      <v-tooltip left>
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            class="mb-42"
            bottom
            color="blue"
            dark
            fab
            small
            fixed
            right
            v-bind="attrs"
            v-on="on"
            @click="fromDialog = !fromDialog"
          >
            <v-icon>list_alt</v-icon>
          </v-btn>
        </template>
        <span>Add a new form</span>
      </v-tooltip>

      <v-flex>
        <v-menu
          v-model="menu"
          :close-on-content-click="false"
          :nudge-width="200"
          :max-height="700"
          offset-x
        >
          <template v-slot:activator="{ on }">
            <v-btn
              bottom
              color="primary"
              dark
              fab
              small
              fixed
              right
              v-on="on"
              v-show="currentVisit.fields && currentVisit.fields.length > 0"
            >
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
//import { uploadFile } from "@/store/api";
import { mapGetters } from "vuex";
export default {
  order: 3,
  props: ["id", "isEditingVisit"],
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
          title: "Save as Template",
          icon: "favorite"
        },
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
          titles: ["Patient can see", "Patient can not see"],
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
          titles: ["Patient can see", "Patient can not see"],
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
          model: null,
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
          model: null,
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
      sessionName: "",
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
  created() {
    this.$store.dispatch("getVisit", this.id);
  },
  watch: {
    dialog(val) {
      if (!val) {
        this.fieldTemplate.option = this.field.icon;
        this.fieldTemplate.option = this.field.option;
        this.field = {};
      }
    },
    editingVisit(val) {
      this.$emit("isEditingVisit", val);
    }
  },
  computed: {
    ...mapGetters(["currentVisit", "templates", "fieldtemplates"]),
    sessions() {
      return this.currentVisit.fields
        ? this.currentVisit.fields.filter(s => s.type === "session")
        : [];
    }
  },
  methods: {
    customFilter(item, queryText) {
      const textOne = item.title.toLowerCase();
      const textTwo = item.description.toLowerCase();
      const searchText = queryText.toLowerCase();

      return (
        textOne.indexOf(searchText) > -1 || textTwo.indexOf(searchText) > -1
      );
    },
    handleFileChange(index, subfieldindex, model) {
      if (model) {
        for (let i = 0; i < model.length; i++) {
          const formdata = new FormData();
          formdata.append("file", model[i]);
          this.filesData.push({
            type: model[i].type,
            index: index,
            subfieldindex: subfieldindex,
            id: this.uuid({}),
            formdata: formdata
          });
        }
      }
    },
    delefile(sessionIndex, fieldIndex, fileIndex) {
      const visit = {
        ...this.currentVisit
      };
      const fileInfo =
        visit.fields[sessionIndex].fields[fieldIndex].fileInfos[fileIndex];
      this.$store.dispatch("deleteFile", {
        sessionIndex,
        fieldIndex,
        fileIndex,
        visit,
        fileInfo
      });
    },
    ShoseTemplate(target) {
      this.active = target;
      if (target === 1) {
        this.addForm();
      }
    },
    updateDate(index, subfieldindex, val) {
      const visit = this.currentVisit;
      visit.fields[index].fields[subfieldindex].model = val;
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    updateTime(index, subfieldindex, val) {
      const visit = this.currentVisit;
      visit.fields[index].fields[subfieldindex].model = val;
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    handleClone(item) {
      // Create a fresh copy of item
      let cloneMe = JSON.parse(JSON.stringify(item));

      this.$delete(cloneMe, "uid");

      return cloneMe;
    },

    editVisibility(index, subfieldindex) {
      const visit = this.currentVisit;
      if (!subfieldindex && subfieldindex !== 0) {
        visit.fields[index].isVisible = !visit.fields[index].isVisible;
      } else if (subfieldindex >= 0) {
        visit.fields[index].fields[subfieldindex].isVisible = !visit.fields[
          index
        ].fields[subfieldindex].isVisible;
      }
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: this.filesData
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },
    canOnlyRead(index, subfieldindex) {
      const visit = this.currentVisit;
      if (!subfieldindex && subfieldindex !== 0) {
        visit.fields[index].readonly = !visit.fields[index].readonly;
      } else {
        visit.fields[index].fields[subfieldindex].readonly = !visit.fields[
          index
        ].fields[subfieldindex].readonly;
      }
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: this.filesData
      });

      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: false
      });
    },
    addForm() {
      let form = {
        ...this.defaultForm
      };
      if (this.selectedTemplate) {
        form = {
          ...this.templates.find(t => t.id === this.selectedTemplate)
        };
        this.selectedTemplate = null;
      }
      form.id = this.uuid(form);
      const visit = this.currentVisit;
      if (!visit.fields) visit.fields = [];
      visit.fields.push(form);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
      this.active = null;
      this.fromDialog = false;
    },
    removeRadio(index, subfieldindex, radioIndex) {
      const visit = { ...this.currentVisit };
      visit.fields[index].fields[subfieldindex].radios.splice(radioIndex, 1);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },

    editRadio(index, subfieldindex, radioIndex, radio) {
      this.sessionIndex = index;
      this.subfieldindex = subfieldindex;
      this.radioIndex = radioIndex;
      this.radioTitle = radio;
      this.radioDialog = true;
    },
    saveRadio() {
      const visit = this.currentVisit;
      visit.fields[this.sessionIndex].fields[this.subfieldindex].radios[
        this.radioIndex
      ] = this.radioTitle;
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.radioTitle = null;
      this.sessionIndex = null;
      this.subfieldindex = null;
      this.radioDialog = false;
      this.radioIndex = null;
    },

    remove(sessionIndex, fieldindex) {
      const visit = this.currentVisit;
      visit.fields[sessionIndex].fields.splice(fieldindex, 1);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: this.filesData
      });

      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: false
      });
    },

    duplicateForm(index, form) {
      const visit = this.currentVisit;
      const doble = {
        ...form
      };
      delete doble.id;
      doble.id = this.uuid(doble);
      visit.fields.splice(index + 1, 0, doble);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },

    duplicateField(index, subfieldindex, subfield) {
      const visit = this.currentVisit;
      const doble = {
        ...subfield
      };
      delete doble.id;
      doble.id = this.uuid(doble);

      visit.fields[index].fields.splice(subfieldindex + 1, 0, doble);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: null
      });
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true
      });
    },

    saveAsFieldTemplate(template) {
      this.fieldTemplate = {
        ...template
      };
      this.fieldTemplate.modeld = null;
      this.fieldTemplateDialog = true;
    },

    removeSessionvisit(index) {
      const visit = this.currentVisit;
      if (
        !visit.fields[index].fields ||
        visit.fields[index].fields.length === 0
      ) {
        visit.fields.splice(index, 1);
        visit.published = false;
        this.$store.dispatch("editVisit", {
          visit: visit,
          filesData: this.filesData
        });

        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: false
        });
      } else {
        this.sessionIndex = index;
        this.removingDialog = !this.removingDialog;
      }
    },

    confirRemovingVisit() {
      const visit = this.currentVisit;
      visit.fields.splice(this.sessionIndex, 1);
      visit.published = false;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: this.filesData
      });

      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: false
      });
      this.sessionIndex = null;
      this.removingDialog = false;
    },

    editSessionvisit(session) {
      this.sessionName = session.title;
      this.currentSession = session;
      this.editingSession = true;
    },
    edit(sessionIndex, field) {
      this.sessionIndex = sessionIndex;
      const visit = {
        ...this.currentVisit
      };
      this.subfieldindex = visit.fields[this.sessionIndex].fields.findIndex(
        f => f.id === field.id
      );
      this.field = {
        ...field
      };
      this.dialog = !this.dialog;
    },
    closeDialog() {
      this.dialog = false;
      setTimeout(function() {
        this.subfieldindex = null;
        this.field = null;
      }, 3000);
    },
    save(field) {
      if (this.editingSession) {
        this.currentSession.title = this.sessionName;
        const visit = this.currentVisit;
        const index = visit.fields.findIndex(
          f => f.id === this.currentSession.id
        );
        visit.fields[index].title = this.sessionName;
        visit.published = false;
        this.$store.dispatch("editVisit", {
          visit: visit,
          filesData: null
        });
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true
        });
        this.sessionName = null;
        this.currentSession = null;
        this.editingSession = false;
        this.field = {};
      } else {
        if (field && field.id) {
          const visit = this.currentVisit;
          const fieldIndex = visit.fields[this.sessionIndex].fields.findIndex(
            f => f.id === this.field.id
          );
          visit.fields[this.sessionIndex].fields[fieldIndex] = field;
          visit.published = false;
          this.$store.dispatch("editVisit", {
            visit: visit,
            filesData: null
          });
          this.$store.commit("SET_EDITING_INPROGRESS", true, {
            root: true
          });
          this.dialog = false;
          this.field = {};
        } else {
          const field = {
            ...this.field
          };
          delete field.id;
          field.id = this.uuid(field);
          const visit = this.currentVisit;
          visit.fields[this.sessionIndex].fields.push(field);
          visit.published = false;
          this.$store.dispatch("editVisit", {
            visit: visit,
            filesData: null
          });
          this.$store.commit("SET_EDITING_INPROGRESS", true, {
            root: true
          });
          this.dialog = false;
          this.field = {};
        }
      }
    },
    saveVisitData() {
      const visit = { ...this.currentVisit };
      visit.published = true;
      this.$store.dispatch("editVisit", {
        visit: visit,
        filesData: this.filesData
      });
      this.snackbar = true;
      this.snackText = "The data has been saved successfully !";
      this.editingVisit = true;
    },
    saveAsTemplate(template) {
      this.template = {
        ...template
      };
      this.template.fields.forEach(f => {
        f.model = null;
        f.fileInfos = null;
        f.models = null;
      });
      this.templateDialog = true;
    },
    saveTemplate() {
      if (!this.template.title || !this.template.title) {
        return;
      }
      delete this.template.patientId;
      this.template.id = this.uuid(this.template);
      this.$store.dispatch("addTemplate", this.template);
      this.templateDialog = false;
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
