<template>
  <v-dialog v-model="localdialog" width="800px">
    <v-card>
      <v-card-title class="darken-2">Create a new patient</v-card-title>
      <v-container>
        <v-row class="mx-2">
          <v-col class="align-center justify-space-between" cols="6">
            <v-row align="center" class="mr-0">
              <v-text-field
                placeholder="First name"
                clearable
                label="First name"
                v-model="patient.firstName"
              ></v-text-field>
            </v-row>
          </v-col>
          <v-col class="align-center justify-space-between" cols="6">
            <v-row align="center" class="mr-0">
              <v-text-field
                placeholder="Last name"
                clearable
                label="Last name"
                v-model="patient.lastName"
              ></v-text-field>
            </v-row>
          </v-col>
          <v-col cols="12">
            <v-text-field
              placeholder="Job title"
              clearable
              label="Job title"
              v-model="patient.jobTitle"
            ></v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              placeholder="Email"
              :rules="emailRules"
              clearable
              label="Email"
              v-model="patient.email"
              type="email"
            ></v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              type="tel"
              clearable
              label="Phone"
              placeholder="Phone"
              v-model="patient.phone"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <v-text-field
              type="number"
              clearable
              label="Age"
              placeholder="Age"
              v-model="patient.age"
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            Sexe
            <v-radio-group v-model="patient.sexe" row>
              <v-radio label="Male" value="male"></v-radio>
              <v-radio label="Female" value="female"></v-radio>
              <v-radio label="None Binary" value="none"></v-radio>
            </v-radio-group>
          </v-col>

          <v-col
            cols="12"
            v-if="!valid"
            class="align-center justify-space-between"
          >
            <v-alert border="bottom" colored-border type="warning" elevation="2"
              >You have to feel all this fields before you continue</v-alert
            >
          </v-col>
        </v-row>
      </v-container>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text color="primary" @click="$emit('close')">Cancel</v-btn>
        <v-btn text @click="createPatient">Save</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ["dialog", "currentPatient", "close"],
  data() {
    return {
      patient: {},
      valid: true,
      localdialog: false,
      emailRules: [
        v => !!v || "E-mail is required",
        v => /.+@.+\..+/.test(v) || "E-mail must be valid"
      ]
    };
  },
  methods: {
    createPatient() {
      this.valid = true;
      if (
        !this.patient.firstName ||
        !this.patient.lastName ||
        !this.patient.email ||
        !this.patient.phone ||
        !this.patient.age ||
        !this.patient.sexe
      ) {
        this.valid = false;
        return;
      }
      if (this.currentPatient && this.currentPatient.id) {
        this.$store.dispatch("editPatient", this.patient);
        this.$emit("close");
      } else {
        const timestamp = ((new Date().getTime() / 1000) | 0).toString(16);

        const id =
          timestamp +
          "xxxxxxxxxxxxxxxx"
            .replace(/[x]/g, function() {
              return ((Math.random() * 16) | 0).toString(16);
            })
            .toLowerCase();
        this.patient.id = id;
        this.patient.age = parseInt(this.patient.age);
        this.patient.active = true;
        this.patient.avatar = "https://cdn.vuetifyjs.com/images/lists/2.jpg";
        delete this.patient.avatarFile;
        this.$store.dispatch("createPatient", this.patient);
        this.$emit("close");
      }
    }
  },
  watch: {
    dialog(val) {
      this.localdialog = val;
    },
    localdialog(val) {
      if (!val) {
        this.$emit("close");
      }
    }
  },
  mounted() {
    this.patient = {
      ...this.currentPatient
    };
    this.localdialog = this.dialog;
  }
};
</script>
