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
              @duplicateForm="duplicateForm"
            />
          </v-card-title>

          <draggable
            class="row mx-2 content-row"
            tag="div"
            v-model="field.fields"
            v-bind="dragOptions"
            :move="onMove"
            @start="isDragging = true"
            @end="onEnd"
            group="people"
          >
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="(subfield, subfieldindex) in field.fields"
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
                :readonly="subfield.readonly"
                :disabled="subfield.disabled"
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
                v-else-if="subfield.type === 'currency / decimal / percentage'"
                :type="subfield.type"
                v-money="subfield.option"
                :readonly="subfield.readonly"
                :disabled="subfield.disabled"
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
                :readonly="subfield.readonly"
                :disabled="subfield.disabled"
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
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
                  />
                </template>
              </v-textarea>
              <date-picker-field
                v-else-if="subfield.type === 'date'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="index"
                @edit="edit"
                @remove="remove"
                @editVisibility="editVisibility"
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
                @updateDate="updateTime"
                @duplicateField="duplicateField"
                @saveAsFieldTemplate="saveAsFieldTemplate"
                :actions="fieldActions"
              />

              <v-checkbox
                v-else-if="subfield.type === 'checkbox'"
                v-model="subfield.model"
                :readonly="subfield.readonly"
                :disabled="subfield.disabled"
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
                      @duplicateField="duplicateField"
                      @saveAsFieldTemplate="saveAsFieldTemplate"
                    />
                  </div>
                </template>
              </v-checkbox>

              <v-switch
                v-else-if="subfield.type === 'switch'"
                v-model="subfield.model"
                :readonly="subfield.readonly"
                :disabled="subfield.disabled"
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
                  :readonly="subfield.readonly"
                  :disabled="subfield.disabled"
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
                    @duplicateField="duplicateField"
                    @saveAsFieldTemplate="saveAsFieldTemplate"
                  />
                  <v-radio-group
                    v-model="subfield.model"
                    :readonly="subfield.readonly"
                    :disabled="subfield.disabled"
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
                                <v-icon>mdi-dots-vertical</v-icon>
                              </v-btn>
                            </template>

                            <v-list>
                              <v-list-item
                                @click="
                                  editRadio(
                                    index,
                                    subfieldindex,
                                    subfield,
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
                  :readonly="subfield.readonly"
                  :disabled="subfield.disabled"
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
                      close-icon="mdi-delete"
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
              <v-col class="align-center justify-space-between" cols="6">
                <v-checkbox
                  v-model="field.readonly"
                  :label="`Readonly ? : ${
                    field.readonly ? field.readonly.toString() : ''
                  }`"
                ></v-checkbox>
              </v-col>
              <v-col class="align-center justify-space-between" cols="6">
                <v-checkbox
                  v-model="field.disabled"
                  :label="`Disabled ? : ${
                    field.disabled ? field.disabled.toString() : ''
                  }`"
                ></v-checkbox>
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

              <v-row
                class="mx-2"
                v-if="['text', 'email', 'textarea'].includes(field.type)"
              >
                <v-col cols="12">
                  <v-text-field
                    outlined
                    v-model.number="field.maxlength"
                    label="Maxlength"
                    clearable
                    placeholder="Maxlength"
                  ></v-text-field>
                </v-col>
              </v-row>

              <v-row class="mx-2" v-if="field.type === 'radiogroup'">
                <v-col class="align-center justify-space-between" cols="12">
                  <v-select
                    outlined
                    v-model="field.radioDirection"
                    :items="radioDirections"
                    item-text="title"
                    item-value="id"
                    label="Dierction"
                    placeholder="Dierction"
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
            <v-row
              class="mx-2"
              v-if="field.type === 'currency / decimal / percentage'"
            >
              <v-col class="align-center justify-space-between" cols="6">
                <v-text-field
                  outlined
                  v-model="field.option.min"
                  label="Minimum Value"
                  clearable
                  placeholder="Minimum Value"
                ></v-text-field>
              </v-col>
              <v-col class="align-center justify-space-between" cols="6">
                <v-text-field
                  outlined
                  v-model="field.option.max"
                  label="Maximum Value"
                  clearable
                  placeholder="Maximum Value"
                ></v-text-field>
              </v-col>
              <v-col class="align-center justify-space-between" cols="6">
                <v-text-field
                  outlined
                  v-model="field.option.precision"
                  label="Precision"
                  clearable
                  placeholder="Precision"
                ></v-text-field>
              </v-col>
              <v-row>
                <v-col clos="12">
                  <v-radio-group
                    v-model="field.option.type"
                    @change="currencyOrDecimalChange(field.option.type)"
                    row
                  >
                    <v-radio label="Currenry" value="currenry"></v-radio>
                    <v-radio label="Decimal" value="decimal"></v-radio>
                    <v-radio label="Percentage" value="percentage"></v-radio>
                  </v-radio-group>
                </v-col>
              </v-row>
              <v-col
                v-if="field.option.type === 'currenry'"
                class="align-center justify-space-between"
                cols="12"
              >
                <v-select
                  outlined
                  v-model="field.option.prefix"
                  :items="currencies"
                  item-text="name"
                  item-value="id"
                  label="Shose a currency"
                  placeholder="Shose a currency"
                ></v-select>
              </v-col>
            </v-row>
          </v-flex>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="closeDialog">Close</v-btn>
          <v-btn text @click="save">Save</v-btn>
        </v-card-actions>
      </v-card>
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
      <v-btn class="ma-2" color="primary" @click="saveVisitData">Save</v-btn>
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
        class="full-flex-width"
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
                  @end="onEnd"
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
              <v-divider></v-divider>
              <v-list>
                <v-subheader>Custom inputs</v-subheader>
                <draggable
                  v-model="fieldtemplates"
                  v-bind="dragOptions"
                  :move="onMove"
                  @start="isDragging = true"
                  @end="onEnd"
                  :group="{ name: 'people', pull: 'clone', put: false }"
                  :clone="cloneDog"
                >
                  <transition-group type="transition" :name="'flip-list'">
                    <v-list-item
                      class="list-group-item"
                      v-for="element in fieldtemplates"
                      :key="element.id"
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
  },
  directives: { money: VMoney },
  data() {
    return {
      formActions: [
        {
          title: "Save as Template",
          icon: "favorite",
        },
        {
          title: "Edit",
          icon: "edit",
        },
        {
          title: "Remove",
          icon: "delete",
        },
        {
          title: "Duplicate",
          icon: "control_point_duplicate",
        },
        {
          titles: ["Allow patient to see", "Hide to the patient"],
          icons: ["visibility", "visibility_off"],
        },
      ],
      fieldActions: [
        {
          title: "Edit",
          icon: "edit",
        },
        {
          title: "Remove",
          icon: "delete",
        },
        {
          title: "Duplicate",
          icon: "control_point_duplicate",
        },
        {
          titles: ["Allow patient to see", "Hide to the patient"],
          icons: ["visibility", "visibility_off"],
        },
        {
          title: "Save as Template",
          icon: "favorite",
        },
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
      editable: true,
      isDragging: false,
      delayedDragging: false,
      adding: false,
      sessionIndex: null,
      dragedElement: null,
      menu: false,
      subfieldindex: null,
      currentRadio: null,
      radioTitle: null,
      selectTitle: null,
      radioDialog: false,
      selectionDialog: false,
      fromDateMenu: false,
      fromTimeMenu: false,
      template: {},
      fieldTemplate: {},
      field: {},
      multiple: null,
      editingSession: false,
      currentSession: null,
      removingDialog: false,
      col: null,
      cols: [
        {
          id: "12",
          title: "full width",
        },
        {
          id: "6",
          title: "1/2",
        },
        {
          id: "4",
          title: "1/3",
        },
        {
          id: "3",
          title: "1/4",
        },
        {
          id: "2",
          title: "1/6",
        },
        {
          id: "1",
          title: "1/12",
        },
      ],
      inputType: null,
      radioDirections: [
        {
          id: false,
          title: "column",
        },
        {
          id: true,
          title: "row",
        },
      ],
      defaultForm: {
        id: null,
        inputType: "session",
        type: "form",
        title: null,
        icon: "list_alt",
        fields: [],
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
          maxlength: 1000,
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
          maxlength: 15,
        },
        {
          id: null,
          title: "",
          icon: "money",
          col: "12",
          inputType: "field",
          type: "currency / decimal / percentage",
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
            type: "currency",
          },
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
          maxlength: 300,
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
          disabled: false,
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
          disabled: false,
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
          disabled: false,
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
          disabled: false,
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
          maxlength: 2000,
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
          disabled: false,
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
          disabled: false,
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
          disabled: false,
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
          disabled: false,
        },
      ],
      selectedSession: null,
      sessionName: "",
      visitType: "session",
      dialog: false,
      templateDialog: false,
      fieldTemplateDialog: false,
      drag: false,

      list2: [
        {
          name: "1",
          id: 1,
        },
      ],
      currencies: [
        { name: "USD", id: "$ " },
        { name: "EUR", id: "€ " },
        { name: "GBP", id: "£ " },
      ],
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
    isDragging(newValue) {
      if (newValue) {
        this.delayedDragging = true;
        return;
      }
      this.$nextTick(() => {
        this.delayedDragging = false;
      });
    },
    editingVisit(val) {
      this.$emit("isEditingVisit", val);
    },
  },
  computed: {
    ...mapGetters(["currentVisit", "templates", "fieldtemplates"]),
    sessions() {
      return this.currentVisit.fields
        ? this.currentVisit.fields.filter((s) => s.type === "session")
        : [];
    },
    dragOptions() {
      return {
        animation: 0,
        group: "description",
        disabled: !this.editable,
        ghostClass: "ghost",
      };
    },
  },
  methods: {
    onEnd() {
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
      this.isDragging = false;
    },
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
            id: this.generateNewId(),
            formdata: formdata,
          });
        }
      }
    },
    delefile(sessionIndex, fieldIndex, fileIndex) {
      const visit = {
        ...this.currentVisit,
      };
      const fileInfo =
        visit.fields[sessionIndex].fields[fieldIndex].fileInfos[fileIndex];
      this.$store.dispatch("deleteFile", {
        sessionIndex,
        fieldIndex,
        fileIndex,
        visit,
        fileInfo,
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
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    updateTime(index, subfieldindex, val) {
      const visit = this.currentVisit;
      visit.fields[index].fields[subfieldindex].model = val;
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    cloneDog(item) {
      const f = {
        ...item,
      };
      f.id = this.generateNewId();
      this.editingVisit = true;
      return f;
    },
    onMove({ relatedContext, draggedContext }) {
      const relatedElement = relatedContext.element;
      const draggedElement = draggedContext.element;
      return (
        (!relatedElement || !relatedElement.fixed) && !draggedElement.fixed
      );
    },
    editVisibility(index, subfieldindex) {
      const visit = this.currentVisit;
      if (!subfieldindex) {
        visit.fields[index].isVisible = !visit.fields[index].isVisible;
      } else {
        visit.fields[index].fields[subfieldindex].isVisible = !visit.fields[
          index
        ].fields[subfieldindex].isVisible;
      }
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    addForm() {
      let form = {
        ...this.defaultForm,
      };
      if (this.selectedTemplate) {
        form = {
          ...this.templates.find((t) => t.id === this.selectedTemplate),
        };
        this.selectedTemplate = null;
      }
      const id = this.generateNewId();
      form.id = id;
      const visit = this.currentVisit;
      if (!visit.fields) visit.fields = [];
      visit.fields.push(form);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
      this.active = null;
      this.fromDialog = false;

      /* const options = {
                                                                                container: "#visit-container",
                                                                                easing: "ease-in",
                                                                                offset: 0,
                                                                                force: true,
                                                                                Closeable: true,
                                                                                x: false,
                                                                                y: true
                                                                              }; */
      /* const opt = {
                                                                                selector: "#visit-container",
                                                                                duration: 300,
                                                                                offset: 0,
                                                                                easing: "easeInOutCubic",
                                                                              };
                                                                              this.$vuetify.goTo(this.$refs.id, opt); */
      //this.$scrollTo("#form_id_" + id, 200, options);
    },
    removeRadio(index, subfieldindex, radioIndex) {
      const visit = this.currentVisit;
      visit.fields[index].fields[subfieldindex].radios.splice(radioIndex, 1);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },

    openRadioDialog(index, subfieldindex, subfield) {
      this.sessionIndex = index;
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.radioDialog = true;
    },

    openSelectionDialog(index, subfieldindex, subfield) {
      this.sessionIndex = index;
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.selectionDialog = true;
    },

    removeSelectItem(selectindex) {
      const visit = {
        ...this.currentVisit,
      };
      this.field.selects.splice(selectindex, 1);
      visit.fields[this.sessionIndex].fields[this.subfieldindex].selects.splice(
        selectindex,
        1
      );
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    editRadio(index, subfieldindex, subfield, radio) {
      this.sessionIndex = index;
      this.subfieldindex = subfieldindex;
      this.field = subfield;
      this.currentRadio = radio;
      this.radioTitle = radio.title;
      this.radioDialog = true;
    },
    saveRadio() {
      if (this.currentRadio && this.currentRadio.id) {
        const visit = this.currentVisit;
        this.currentRadio.title = this.radioTitle;
        const radioIndex = this.field.radios.findIndex(
          (r) => r.id === this.currentRadio.id
        );
        this.field.radios[radioIndex] = this.currentRadio;
        visit.fields[this.sessionIndex].fields[this.subfieldindex] = this.field;
        this.$store.commit("SET_VISIT", visit);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true,
        });
        this.radioTitle = null;
        this.sessionIndex = null;
        this.subfieldindex = null;
        this.currentRadio = null;
        this.field = {};
        this.radioDialog = false;
      } else {
        const radio = {
          title: this.radioTitle,
          id: this.generateNewId(),
        };
        if (!this.field.radios) {
          this.field.radios = [radio];
        } else {
          this.field.radios.push(radio);
        }
        const visit = this.currentVisit;
        visit.fields[this.sessionIndex].fields[this.subfieldindex] = this.field;
        this.$store.commit("SET_VISIT", visit);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true,
        });
        this.radioTitle = null;
        this.radioDialog = false;
      }
    },

    saveSelectionItem() {
      const select = {
        title: this.selectTitle,
        id: this.generateNewId(),
      };
      if (!this.field.selects) {
        this.field.selects = [select];
      } else {
        this.field.selects.push(select);
      }

      const visit = this.currentVisit;
      visit.fields[this.sessionIndex].fields[this.subfieldindex] = this.field;
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });

      this.selectTitle = null;
    },
    remove(sessionIndex, fieldindex) {
      const visit = this.currentVisit;
      visit.fields[sessionIndex].fields.splice(fieldindex, 1);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    duplicateForm(index, form) {
      const visit = this.currentVisit;
      const duble = {
        ...form,
      };
      duble.id = this.generateNewId();
      visit.fields.splice(index + 1, 0, duble);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    duplicateField(index, subfieldindex, subfield) {
      const visit = this.currentVisit;
      const duble = {
        ...subfield,
      };
      duble.id = this.generateNewId();

      visit.fields[index].fields.splice(subfieldindex + 1, 0, duble);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    saveAsFieldTemplate(template) {
      this.fieldTemplate = {
        ...template,
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
        this.$store.commit("SET_VISIT", visit);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true,
        });
      } else {
        this.sessionIndex = index;
        this.removingDialog = !this.removingDialog;
      }
    },
    confirRemovingVisit() {
      const visit = this.currentVisit;
      visit.fields.splice(this.sessionIndex, 1);
      this.$store.commit("SET_VISIT", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
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
        ...this.currentVisit,
      };
      this.subfieldindex = visit.fields[this.sessionIndex].fields.findIndex(
        (f) => f.id === field.id
      );
      debugger;
      this.field = {
        ...field,
      };
      this.dialog = !this.dialog;
    },
    closeDialog() {
      this.dialog = false;
      setTimeout(function () {
        this.subfieldindex = null;
        this.field = null;
      }, 3000);
    },
    save() {
      if (this.editingSession) {
        this.currentSession.title = this.sessionName;
        const visit = this.currentVisit;
        const index = visit.fields.findIndex(
          (f) => f.id === this.currentSession.id
        );
        visit.fields[index].title = this.sessionName;
        this.$store.commit("SET_VISIT", visit);
        this.$store.commit("SET_EDITING_INPROGRESS", true, {
          root: true,
        });
        this.sessionName = null;
        this.currentSession = null;
        this.editingSession = false;
      } else {
        if (this.field && this.field.id) {
          const visit = this.currentVisit;
          const fieldIndex = visit.fields[this.sessionIndex].fields.findIndex(
            (f) => f.id === this.field.id
          );
          visit.fields[this.sessionIndex].fields[fieldIndex] = this.field;
          this.$store.commit("SET_VISIT", visit);
          this.$store.commit("SET_EDITING_INPROGRESS", true, {
            root: true,
          });
          this.dialog = false;
        } else {
          const field = {
            ...this.field,
          };
          field.id = this.generateNewId();
          const visit = this.currentVisit;
          visit.fields[this.sessionIndex].fields.push(field);
          this.$store.commit("SET_VISIT", visit);
          this.$store.commit("SET_EDITING_INPROGRESS", true, {
            root: true,
          });
          this.dialog = false;
        }
      }
    },
    saveVisitData() {
      this.$store.dispatch("editVisit", {
        visit: this.currentVisit,
        filesData: this.filesData,
      });
      this.snackbar = true;
      this.snackText = "The data has been saved successfully !";
      this.editingVisit = true;
    },
    saveAsTemplate(template) {
      this.template = {
        ...template,
      };
      this.template.fields.forEach((f) => {
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
      this.template.id = this.generateNewId();
      this.$store.dispatch("addTemplate", this.template);
      this.templateDialog = false;
    },
    saveFieldTemplate() {
      if (!this.fieldTemplate.title || !this.fieldTemplate.title) {
        return;
      }
      this.fieldTemplate.id = this.generateNewId();
      this.fieldTemplate.modeld = 0;
      this.$store.dispatch("addFieldTemplate", this.fieldTemplate);
      this.fieldTemplateDialog = false;
    },
    generateNewId() {
      const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);
      const id =
        timestamp +
        "xxxxxxxxxxxxxxxx"
          .replace(/[x]/g, function () {
            return ((Math.random() * 16) | 0).toString(16);
          })
          .toLowerCase();
      return id;
    },
    currencyOrDecimalChange(type) {
      if (type === "currency") {
        this.field.icon = "money";
        this.field.option.prefix = "";
        this.field.option.suffix = "";
      }
      if (type === "decimal") {
        this.field.icon = "10k";
        this.field.option.prefix = "";
        this.field.option.suffix = "";
      }
      if (type === "percentage") {
        this.field.icon = "%";
        this.field.option.prefix = "";
        this.field.option.suffix = " %";
      }
    },
  },
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
