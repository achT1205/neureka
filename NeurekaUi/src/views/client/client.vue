<template>
  <v-flex>
    <v-flex
      id="visit-container"
      v-if="localVisit && localVisit.fields && localVisit.fields.length"
    >
      <v-flex
        v-for="(field, index) in localVisit.fields"
        :key="field.id"
        :class="index !== 0 ? 'mt-3' : ''"
        :id="'form_id_' + field.id"
      >
        <v-card :loading="saving">
          <v-card-title class="darken-2">
            {{ field.title ? field.title : "Form_" + index }}
            <v-spacer></v-spacer>
          </v-card-title>

          <v-row class="mx-2 content-row">
            <v-col
              class="align-center justify-space-between list-group-item"
              v-for="(subfield, subfieldindex) in field.fields"
              :cols="subfield.col"
              :key="subfield.id"
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
                :disabled="subfield.readonly"
                :prepend-inner-icon="subfield.icon"
                :maxlength="subfield.maxlength"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
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
                :readonly="subfield.readonly"
                :disabled="subfield.readonly"
                v-model.lazy="subfield.model"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
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
                :disabled="subfield.readonly"
                :maxlength="subfield.maxlength"
                :prepend-inner-icon="subfield.icon"
                @change="
                  $store.commit('SET_EDITING_INPROGRESS', true, { root: true })
                "
              >
              </v-textarea>
              <v-row v-else-if="subfield.type === 'editor'">
                <v-col cols="12">
                  <label class="d-flex justify-start">
                    {{
                      subfield.title ? subfield.title : "Field" + subfieldindex
                    }}
                  </label>
                </v-col>
                <v-col cols="12">
                  <vue-editor v-if="!subfield.readonly" v-model="subfield.model" ></vue-editor>
                  <div v-else v-html="subfield.model"></div>
                </v-col>
              </v-row>
              <date-picker-field
                v-else-if="subfield.type === 'date'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :client="true"
                :index="index"
                @updateDate="updateDate"
              />

              <time-picker-field
                v-else-if="subfield.type === 'time'"
                :subfield="subfield"
                :subfieldindex="subfieldindex"
                :index="index"
                :client="true"
                @updateDate="updateTime"
              />

              <v-checkbox
                v-else-if="subfield.type === 'checkbox'"
                v-model="subfield.model"
                :readonly="subfield.readonly"
                :disabled="subfield.readonly"
              >
              </v-checkbox>

              <v-switch
                v-else-if="subfield.type === 'switch'"
                v-model="subfield.model"
                :readonly="subfield.readonly"
                :disabled="subfield.readonly"
              >
              </v-switch>

              <v-flex v-else-if="subfield.type === 'selection'">
                <label>{{
                  subfield.title ? subfield.title : "Field_" + subfieldindex
                }}</label>
                <v-select
                  outlined
                  v-model="subfield.model"
                  :readonly="subfield.readonly"
                  :disabled="subfield.readonly"
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
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'selections'">
                <label>{{
                  subfield.title ? subfield.title : "Field_" + subfieldindex
                }}</label>
                <v-select
                  outlined
                  :readonly="subfield.readonly"
                  :disabled="subfield.readonly"
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
                </v-select>
              </v-flex>

              <v-flex v-else-if="subfield.type === 'radiogroup'">
                <v-col cols="12">
                  {{
                    subfield.title ? subfield.title : "Field_" + subfieldindex
                  }}
                  <v-radio-group
                    v-model="subfield.model"
                    :readonly="subfield.readonly"
                    :disabled="subfield.readonly"
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
                  :disabled="subfield.readonly"
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
          </v-row>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="calcel = false">Cancel</v-btn>
            <v-btn text @click="save">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-flex>
  </v-flex>
</template>

<script>
import DatePickerField from "@/components/DatePickerField.vue";
import TimePickerField from "@/components/TimePickerField.vue";
import { VueEditor } from "vue2-editor";
import { VMoney } from "v-money";
import { mapGetters } from "vuex";
export default {
  data() {
    return {
      saving: false,
      localVisit: {},
    };
  },
  watch: {
    clientVisit(val, old) {
      if (val && val !== old) {
        this.localVisit = JSON.parse(JSON.stringify(val));
      }
    },
  },
  directives: { money: VMoney },
  components: {
    DatePickerField,
    TimePickerField,
    VueEditor,
  },
  computed: {
    ...mapGetters(["clientVisit"]),
  },
  methods: {
    updateDate(index, subfieldindex, val) {
      this.localVisit.fields[index].fields[subfieldindex].model = val;
      this.$store.commit("SET_CLIENT_VISIT", this.localVisit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },

    updateTime(index, subfieldindex, val) {
      this.localVisit.fields[index].fields[subfieldindex].model = val;
      this.$store.commit("SET_VISIT", this.localVisit);
      this.$store.commit("SET_EDITING_INPROGRESS", true, {
        root: true,
      });
    },
    save() {
      const visit = this.localVisit;
      this.$store.dispatch("updateClientVisit", visit);
      this.$store.commit("SET_EDITING_INPROGRESS", false, {
        root: true,
      });
    },
    cancel() {
      const visit = this.clientVisit;
      this.localVisit = JSON.parse(JSON.stringify(visit));
    },
  },
};
</script>
