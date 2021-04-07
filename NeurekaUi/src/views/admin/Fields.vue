<template>
  <v-flex class="page">
    <div class="title-page">
      <h1>Field templates</h1>
    </div>
    <div class="content-page">
      <v-data-table
        class="table-full-width"
        :headers="headers"
        :items="fieldtemplates"
        sort-by="title"
        :search="search"
      >
        <template v-slot:top>
          <div class="row">
            <div class="col">
              <v-text-field
                v-model="search"
                prepend-inner-icon="mdi-magnify"
                label="Search"
                single-line
                hide-details
                placeholder="Search"
                dense
                outlined
              ></v-text-field>
            </div>
            <div class="col">
              <div class="d-flex justify-end align-center">
                <v-dialog v-model="dialog" width="800px">
                  <!--
                    <template v-slot:activator="{ on }">
                    <v-btn color="success" dark v-on="on">
                      <v-icon>add</v-icon>
                    </v-btn>
                  </template>
                  -->
                  <v-card>
                    <v-card-title>
                      <span class="headline">{{ formTitle }}</span>
                    </v-card-title>
                    <v-card-text>
                      <div class="d-flex flex-wrap full-flex-width">
                        <v-col cols="12">
                          <v-text-field
                            clearable
                            dense
                            outlined
                            label="Template Name"
                            v-model="editedItem.title"
                            placeholder="Template Name"
                          ></v-text-field>
                        </v-col>
                        <v-col
                          class="align-center justify-space-between"
                          cols="12"
                        >
                          <v-textarea
                            label="Description"
                            placeholder="Description"
                            clearable
                            clear-icon="close"
                            v-model="editedItem.description"
                            dense
                            outlined
                          />
                        </v-col>
                        <v-col
                          cols="12"
                          v-if="!valid"
                          class="align-center justify-space-between"
                        >
                          <v-alert
                            border="bottom"
                            colored-border
                            type="warning"
                            elevation="2"
                          >
                            You have to feel all this fields before you continue
                          </v-alert>
                        </v-col>
                      </div>
                    </v-card-text>
                    <v-card-actions>
                      <div
                        class="d-flex justify-end full-flex-width"
                        style="padding: 0 22px;"
                      >
                        <v-btn color="secondary" text @click="close"
                          >Close</v-btn
                        >
                        <v-btn color="primary" @click="save">Save</v-btn>
                      </div>
                    </v-card-actions>
                  </v-card>
                </v-dialog>
                <v-dialog v-model="removingDialog" persistent max-width="290">
                  <v-card>
                    <v-card-title class="headline"
                      >Removing Alert !</v-card-title
                    >
                    <v-card-text
                      >Are you sure you want to remove this Template
                      ?</v-card-text
                    >
                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn
                        color="green darken-1"
                        text
                        @click="removingDialog = false"
                        >Disagree</v-btn
                      >
                      <v-btn
                        color="red darken-1"
                        text
                        @click="confirRemovingTemplate()"
                        >Agree</v-btn
                      >
                    </v-card-actions>
                  </v-card>
                </v-dialog>
              </div>
            </div>
          </div>
        </template>
        <template v-slot:[`item.actions`]="{ item }">
          <!--<v-btn class="mr-1" small color="warning">
            <v-icon small @click="editItemFields(item.id)">list</v-icon>
          </v-btn>-->
          <v-btn class="mr-1" small color="primary">
            <v-icon small @click="editItem(item)">mdi-pencil</v-icon>
          </v-btn>
          <v-btn small color="error">
            <v-icon small @click="deleteItem(item)">mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </div>
  </v-flex>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  data: () => ({
    removingDialog: false,
    valid: true,
    dialog: false,
    search: "",
    headers: [
      {
        text: "Title",
        align: "start",
        value: "title",
        sortable: true,
        filterable: true
      },
      {
        text: "Description",
        value: "description",
        sortable: true,
        filterable: true
      },
      { text: "Actions", value: "actions", sortable: false, align: "end" }
    ],
    forms: [],
    editedIndex: -1,
    editedItem: {
      title: "",
      description: "",
      fields: []
    },
    defaultItem: {
      title: "",
      description: "",
      fields: []
    }
  }),
  computed: {
    ...mapGetters(["fieldtemplates"]),
    formTitle() {
      return this.editedIndex === -1 ? "New Template" : "Edit Template";
    }
  },
  watch: {
    dialog(val) {
      val || this.close();
    }
  },
  methods: {
    editItemFields(id) {
      this.$router.push("/admin/fields/" + id);
    },
    editItem(item) {
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      this.currentItem = item;
      this.removingDialog = !this.removingDialog;
    },

    confirRemovingTemplate() {
      this.$store.dispatch("removeFieldTemplate", this.currentItem.id);
      this.close();
    },
    close() {
      this.dialog = false;
      this.editedItem = this.defaultItem;
      this.removingDialog = false;
      this.currentItem = null;
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
    },
    save() {
      if (!this.editedItem.title || !this.editedItem.description) {
        this.valid = false;
        return;
      } else {
        if (this.editedItem.id) {
          this.$store.dispatch("editFieldTemplate", this.editedItem);
          this.close();
        } else {
          this.editedItem.id = this.generateNewId();
          this.fields = [];
          this.$store.dispatch("addFieldTemplate", this.editedItem);
          this.close();
        }
      }
    }
  }
};
</script>
