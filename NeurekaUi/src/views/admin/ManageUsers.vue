<template>
  <v-flex class="page">
    <div class="title-page">
      <h1>Users</h1>
    </div>
    <div class="content-page">
      <v-data-table
        :headers="headers"
        :items="users"
        sort-by="title"
        :search="search"
        style="width:100%"
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
                dense
                outlined
                placeholder="Search"
              ></v-text-field>
            </div>
            <div class="col">
              <div class="d-flex justify-end align-center">
                <v-dialog v-model="dialog" width="800px" persistent>
                  <template v-slot:activator="{ on }">
                    <v-btn color="success" dark v-on="on">
                      <v-icon>add</v-icon>
                    </v-btn>
                  </template>
                  <ValidationObserver ref="observer" v-slot="{ invalid }">
                    <v-card>
                      <v-card-title>
                        <span class="headline">{{ formTitle }}</span>
                      </v-card-title>
                      <v-card-text>
                        <v-form style="margin: 0 -12px;">
                          <div class="d-flex flex-wrap">
                            <v-col cols="6">
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="First Name"
                                rules="required|max:20"
                              >
                                <v-text-field
                                  clearable
                                  :counter="20"
                                  dense
                                  :error-messages="errors"
                                  label="First Name"
                                  v-model="editedItem.firstName"
                                  placeholder="First Name"
                                  outlined
                                  required
                                ></v-text-field>
                              </ValidationProvider>
                            </v-col>
                            <v-col cols="6">
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="Last Name"
                                rules="required|max:20"
                              >
                                <v-text-field
                                  clearable
                                  label="Last Name"
                                  :counter="20"
                                  :error-messages="errors"
                                  v-model="editedItem.lastName"
                                  placeholder="Last Name"
                                  dense
                                  outlined
                                  required
                                ></v-text-field>
                              </ValidationProvider>
                            </v-col>

                            <v-col
                              class="align-center justify-space-between"
                              cols="6"
                            >
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="Job Title"
                                rules="required|max:50"
                              >
                                <v-text-field
                                  label="Job title"
                                  placeholder="Job title"
                                  clearable
                                  dense
                                  :counter="50"
                                  :error-messages="errors"
                                  clear-icon="close"
                                  outlined
                                  v-model="editedItem.jobTitle"
                                  required
                                ></v-text-field>
                              </ValidationProvider>
                            </v-col>

                            <v-col
                              class="align-center justify-space-between"
                              cols="6"
                            >
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="Email"
                                rules="required|email"
                              >
                                <v-text-field
                                  label="Email"
                                  placeholder="Email"
                                  clearable
                                  clear-icon="close"
                                  outlined
                                  dense
                                  v-model="editedItem.email"
                                  :error-messages="errors"
                                  required
                                ></v-text-field>
                              </ValidationProvider>
                            </v-col>

                            <v-col
                              class="align-center justify-space-between"
                              cols="6"
                            >
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="Phone"
                                rules="required|max:10"
                              >
                                <v-text-field
                                  label="Phone"
                                  placeholder="Phone"
                                  :counter="10"
                                  :error-messages="errors"
                                  clearable
                                  hide-details="auto"
                                  dense
                                  clear-icon="close"
                                  outlined
                                  v-model="editedItem.phone"
                                ></v-text-field>
                              </ValidationProvider>
                            </v-col>
                            <v-col cols="6">
                              <ValidationProvider
                                v-slot="{ errors }"
                                name="Role"
                                rules="required"
                              >
                                <v-select
                                  :items="roles"
                                  v-model="editedItem.role"
                                  label="Role"
                                  item-text="title"
                                  item-value="id"
                                  hide-details="auto"
                                  :error-messages="errors"
                                  data-vv-name="Role"
                                  outlined
                                  dense
                                  placeholder="Role"
                                  required
                                ></v-select>
                              </ValidationProvider>
                            </v-col>
                            <v-col cols="12">
                              Sexe
                              <ValidationProvider
                                v-slot="{ errors }"
                                rules="required"
                                name="Sexe"
                              >
                                <v-radio-group
                                  :error-messages="errors"
                                  v-model="editedItem.sexe"
                                  row
                                  justify="space-around"
                                >
                                  <v-radio label="Male" value="male"></v-radio>
                                  <v-radio
                                    label="Female"
                                    value="female"
                                  ></v-radio>
                                  <v-radio
                                    label="None Binary"
                                    value="none"
                                  ></v-radio>
                                </v-radio-group>
                              </ValidationProvider>
                            </v-col>
                          </div>
                        </v-form>
                      </v-card-text>
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="secondary" text @click="close"
                          >Close</v-btn
                        >
                        <v-btn color="primary" :disabled="invalid" @click="save"
                          >Save</v-btn
                        >
                      </v-card-actions>
                    </v-card>
                  </ValidationObserver>
                </v-dialog>
                <v-dialog v-model="removingDialog" persistent max-width="290">
                  <v-card>
                    <v-card-title class="headline"
                      >Removing Alert !</v-card-title
                    >
                    <v-card-text v-if="currentItem">
                      Are you sure you want to remove the User
                      {{ currentItem.firstName }}
                      {{ currentItem.lastName }} ?
                    </v-card-text>
                    <v-card-actions>
                      <v-btn
                        color="green darken-1"
                        text
                        @click="removingDialog = false"
                        >Disagree</v-btn
                      >
                      <v-spacer></v-spacer>
                      <v-btn
                        color="red darken-1"
                        text
                        @click="confirRemovingUser()"
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
          <v-btn class="mr-1" small color="primary">
            <v-icon @click="editItem(item)">
              edit
            </v-icon>
          </v-btn>
          <v-btn small color="error">
            <v-icon @click="deleteItem(item)">
              delete
            </v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </div>
  </v-flex>
</template>

<script>
import { mapGetters } from "vuex";

import { required, email, max } from "vee-validate/dist/rules";
import {
  extend,
  ValidationObserver,
  ValidationProvider,
  setInteractionMode
} from "vee-validate";

setInteractionMode("eager");

extend("required", {
  ...required,
  message: "{_field_} can not be empty"
});

extend("max", {
  ...max,
  message: "{_field_} may not be greater than {length} characters"
});

extend("email", {
  ...email,
  message: "Email must be valid"
});

export default {
  components: {
    ValidationProvider,
    ValidationObserver
  },
  data: () => ({
    roles: [
      { title: "admin", id: "admin" },
      { title: "secretary", id: "secretary" },
      { title: "superadmin", id: "superadmin" },
      { title: "doctor", id: "doctor" },
      { title: "patient", id: "patient" }
    ],
    removingDialog: false,
    valid: true,
    dialog: false,
    search: "",
    currentItem: null,
    headers: [
      {
        text: "First name",
        align: "start",
        value: "firstName",
        sortable: true,
        filterable: true
      },
      {
        text: "Last name",
        align: "start",
        value: "lastName",
        sortable: true,
        filterable: true
      },
      {
        text: "Job title",
        align: "start",
        value: "jobTitle",
        sortable: true,
        filterable: true
      },
      {
        text: "Email",
        align: "start",
        value: "email",
        sortable: true,
        filterable: true
      },
      {
        text: "Phone",
        align: "start",
        value: "phone",
        sortable: true,
        filterable: true
      },
      {
        text: "Has changed password",
        align: "start",
        value: "changePassword",
        sortable: true,
        filterable: true
      },
      {
        text: "createdAt",
        align: "start",
        value: "createdAt",
        sortable: true,
        filterable: true
      },
      {
        text: "Role",
        align: "start",
        value: "role",
        sortable: true,
        filterable: true
      },
      { text: "Actions", value: "actions", sortable: false }
    ],
    items: [
      {
        text: "Dashboard",
        disabled: false,
        href: "breadcrumbs_dashboard"
      },
      {
        text: "Link 1",
        disabled: false,
        href: "breadcrumbs_link_1"
      },
      {
        text: "Link 2",
        disabled: true,
        href: "breadcrumbs_link_2"
      }
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
    ...mapGetters(["users"]),
    formTitle() {
      return this.editedIndex === -1 ? "New User" : "Edit User";
    }
  },
  mounted() {
    this.$store.dispatch("getUsers");
  },
  watch: {
    dialog(val) {
      if (!val) {
        this.close();
      }
    }
  },
  methods: {
    editItemFields(id) {
      this.$router.push("/admin/forms/" + id);
    },
    editItem(item) {
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      this.currentItem = item;
      this.removingDialog = !this.removingDialog;
    },

    confirRemovingUser() {
      this.$store.dispatch("removeUser", this.currentItem.id);
      this.close();
    },
    close() {
      this.dialog = false;
      this.editedItem = { ...this.defaultItem };
      this.removingDialog = false;
      this.currentItem = null;
      this.$refs.observer.reset();
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
      if (this.editedItem.id) {
        this.$store.dispatch("editUser", this.editedItem);
        this.close();
      } else {
        this.editedItem.id = this.generateNewId();
        this.fields = [];
        this.$store.dispatch("createUser", this.editedItem);
        this.close();
      }
    }
  }
};
</script>
