<template>
  <div class="d-flex page">
    <div class="title-page">
      <h1>My profile</h1>
    </div>
    <div class="content-page">
      <ValidationObserver ref="profileObserver" v-slot="{ invalid }">
        <v-form>
          <v-row>
            <v-col cols="6">
              <ValidationProvider
                v-slot="{ errors }"
                name="FirstName"
                rules="required|max:20"
              >
                <v-text-field
                  clearable
                  :counter="20"
                  :error-messages="errors"
                  label="First Name"
                  v-model="user.firstName"
                  placeholder="First Name"
                  outlined
                  required
                ></v-text-field>
              </ValidationProvider>
            </v-col>
            <v-col cols="6">
              <ValidationProvider
                v-slot="{ errors }"
                name="LastName"
                rules="required|max:20"
              >
                <v-text-field
                  clearable
                  label="Last Name"
                  :counter="20"
                  :error-messages="errors"
                  v-model="user.lastName"
                  placeholder="Last Name"
                  required
                  outlined
                ></v-text-field>
              </ValidationProvider>
            </v-col>

            <v-col class="align-center justify-space-between" cols="12">
              <ValidationProvider
                v-slot="{ errors }"
                name="JobTitle"
                rules="required|max:50"
              >
                <v-text-field
                  label="Job title"
                  placeholder="Job title"
                  clearable
                  :counter="50"
                  :error-messages="errors"
                  clear-icon="close"
                  v-model="user.jobTitle"
                  required
                  outlined
                ></v-text-field>
              </ValidationProvider>
            </v-col>

            <v-col class="align-center justify-space-between" cols="12">
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
                  v-model="user.email"
                  :error-messages="errors"
                  required
                  disabled
                  outlined
                ></v-text-field>
              </ValidationProvider>
            </v-col>

            <v-col class="align-center justify-space-between" cols="12">
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
                  clear-icon="close"
                  v-model="user.phone"
                  outlined
                ></v-text-field>
              </ValidationProvider>
            </v-col>

            <v-col cols="12">
              Sexe
              <ValidationProvider
                v-slot="{ errors }"
                rules="required"
                name="Sexe"
              >
                <v-radio-group :error-messages="errors" v-model="user.sexe" row>
                  <v-radio label="Male" value="male"></v-radio>
                  <v-radio label="Female" value="female"></v-radio>
                  <v-radio label="None Binary" value="none"></v-radio>
                </v-radio-group>
              </ValidationProvider>
            </v-col>

            <v-col cols="12">
              <ValidationProvider
                v-slot="{ errors }"
                name="Role"
                rules="required"
              >
                <v-select
                  :items="roles"
                  v-model="user.role"
                  label="Role"
                  placeholder="Role"
                  item-text="title"
                  item-value="id"
                  :error-messages="errors"
                  data-vv-name="Role"
                  required
                  disabled
                  outlined
                ></v-select>
              </ValidationProvider>
            </v-col>
          </v-row>
        </v-form>

        <v-row>
          <v-col class="d-flex justify-end">
            <v-btn color="secondary" text @click="back">Go back</v-btn>
            <v-btn color="primary" :disabled="invalid" @click="save"
              >Save</v-btn
            >
          </v-col>
        </v-row>
      </ValidationObserver>

      <v-snackbar
        v-model="snackbar"
        :timeout="timeout"
        absolute
        bottom
        color="success"
        outlined
        >{{ snackText }}</v-snackbar
      >
    </div>
  </div>
</template>

<script>
import { required, email, max } from "vee-validate/dist/rules";
import { mapGetters } from "vuex";
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
  data() {
    return {
      snackbar: false,
      snackText: "",
      timeout: 2000,
      user: {},
      storage: {},
      roles: [
        { title: "admin", id: "admin" },
        { title: "secretary", id: "secretary" },
        { title: "superadmin", id: "superadmin" },
        { title: "doctor", id: "doctor" },
        { title: "patient", id: "patient" }
      ]
    };
  },
  components: {
    ValidationProvider,
    ValidationObserver
  },
  computed: {
    ...mapGetters(["authenticatedUser"])
  },
  mounted() {
    this.user = { ...this.authenticatedUser.user };
  },
  methods: {
    back() {
      this.$router.go(-1);
    },
    save() {
      if (this.user.id) {
        const errors = this.$refs.profileObserver.errors;
        if (
          !errors.FirstName.length &&
          !errors.LastName.length &&
          !errors.JobTitle.length &&
          !errors.Sexe.length
        ) {
          const auth = { ...this.authenticatedUser };
          auth.user = { ...this.user };
          this.$store.dispatch("editProfile", auth);
          this.snackbar = true;
          this.snackText = "Your profile has been saved successfully !";
        }
      }
    }
  }
};
</script>
