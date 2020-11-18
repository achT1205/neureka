<template>
  <v-app id="inspire">
    <v-container class="fill-height" fluid>
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="4">
          <v-card class="elevation-12">
            <ValidationObserver ref="observer">
              <v-toolbar color="primary" dark flat>
                <v-toolbar-title>
                  It your first connection, please chnage your password to
                  continue
                </v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>
              <v-card-text>
                <v-form>
                  <ValidationProvider
                    v-slot="{ errors }"
                    name="Email"
                    rules="required|email"
                  >
                    <v-text-field
                      label="Email"
                      name="login"
                      prepend-inner-icon="mdi-account"
                      type="email"
                      v-model="user.email"
                      placeholder="Email"
                      :error-messages="errors"
                      required
                    ></v-text-field>
                  </ValidationProvider>
                  <ValidationProvider
                    v-slot="{ errors }"
                    name="Oldpassword"
                    rules="required|required|min:8"
                  >
                    <v-text-field
                      :append-icon="showoldpassword ? 'mdi-eye' : 'mdi-eye-off'"
                      @click:append="showoldpassword = !showoldpassword"
                      id="oldpassword"
                      label="Old Password"
                      name="oldpassword"
                      placeholder="Old Password"
                      prepend-inner-icon="mdi-lock"
                      :type="showoldpassword ? 'text' : 'password'"
                      :error-messages="errors"
                      v-model="user.oldPassword"
                      required
                    ></v-text-field>
                  </ValidationProvider>
                  <ValidationProvider
                    v-slot="{ errors }"
                    name="Password"
                    rules="required|required|min:8"
                  >
                    <v-text-field
                      :append-icon="showpassword ? 'mdi-eye' : 'mdi-eye-off'"
                      @click:append="showpassword = !showpassword"
                      id="password"
                      label="Password"
                      name="password"
                      placeholder="Password"
                      prepend-inner-icon="mdi-lock"
                      :type="showpassword ? 'text' : 'password'"
                      :error-messages="errors"
                      v-model="user.password"
                      required
                    ></v-text-field>
                  </ValidationProvider>
                </v-form>
                <v-row align="center" justify="center">
                  <v-alert
                    v-if="loginError"
                    outlined
                    :type="loginError.status === 200 ? 'success' : 'warning'"
                    text
                    :prominent="loginError.status !== 200 ? true : false"
                    :border="loginError.status !== 200 ? 'left' : ''"
                    >{{ loginError.message }}</v-alert
                  >
                </v-row>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="update" color="primary">Update</v-btn>
              </v-card-actions>
            </ValidationObserver>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>

<script>
import { required, email, max, min } from "vee-validate/dist/rules";
import {
  extend,
  ValidationObserver,
  ValidationProvider,
  setInteractionMode
} from "vee-validate";
import { mapGetters } from "vuex";
setInteractionMode("eager");

extend("required", {
  ...required,
  message: "{_field_} can not be empty"
});

extend("max", {
  ...max,
  message: "{_field_} may not be greater than {length} characters"
});

extend("min", {
  ...min,
  message: "{_field_} may not be smaller than {length} characters"
});

extend("email", {
  ...email,
  message: "Email must be valid"
});
export default {
  data() {
    return {
      showoldpassword: false,
      showpassword: false,
      user: {}
    };
  },
  mounted() {
    this.$store.commit("SET_LOGINERROR", null);
  },
  computed: {
    ...mapGetters(["loginError"])
  },
  components: {
    ValidationObserver,
    ValidationProvider
  },
  methods: {
    update() {
      const errors = this.$refs.observer.errors;
      if (
        !errors.Email.length &&
        !errors.Oldpassword.length &&
        !errors.Password.length
      )
        this.$store.dispatch("changePassword", {
          user: this.user,
          router: this.$router
        });
    }
  }
};
</script>
