<template>
  <v-app id="inspire">
    <v-container class="fill-height" fluid v-if="!resetPass">
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="4">
          <div class="d-flex justify-center">
            <img
              class="neureka-logo"
              src="@/assets/logo/neureka.png"
              alt="neureka - logo"
            />
          </div>
          <v-card class="elevation-0 card-neureka">
            <div class="d-flex justify-center">
              <h1>Welcome to Neureka</h1>
            </div>
            <ValidationObserver ref="logingObserver">
              <v-card-text>
                <v-form class="mt-5">
                  <ValidationProvider
                    v-slot="{ errors }"
                    name="Email"
                    rules="required|email"
                  >
                    <v-text-field
                      label="Email *"
                      outlined
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
                    name="Password"
                    rules="required|min:8"
                  >
                    <v-text-field
                      :append-icon="showPass ? 'mdi-eye' : 'mdi-eye-off'"
                      @click:append="showPass = !showPass"
                      id="password"
                      label="Password *"
                      name="password"
                      placeholder="Password"
                      outlined
                      prepend-inner-icon="mdi-lock"
                      :type="showPass ? 'text' : 'password'"
                      :error-messages="errors"
                      v-model="user.password"
                      required
                    ></v-text-field>
                  </ValidationProvider>
                  <p class="font-weight-regular">
                    Forgot password ?
                    <a @click="reset">Reset Password</a>
                  </p>
                </v-form>
              </v-card-text>
              <v-row align="center" justify="center" v-if="loginError">
                <v-alert
                  outlined
                  :type="loginError.status === 200 ? 'success' : 'warning'"
                  text
                  :prominent="loginError.status !== 200 ? true : false"
                  :border="loginError.status !== 200 ? 'left' : ''"
                  >{{ loginError.message }}</v-alert
                >
              </v-row>
              <v-card-actions class="justify-center card-action">
                <v-btn @click="login" depressed large block color="primary"
                  >Login</v-btn
                >
              </v-card-actions>
            </ValidationObserver>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
    <v-container class="fill-height" fluid v-if="resetPass">
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="4">
          <div class="d-flex justify-center">
            <img
              class="neureka-logo"
              src="@/assets/logo/neureka.png"
              alt="neureka - logo"
            />
          </div>
          <v-card class="elevation-0 card-neureka">
            <div class="d-flex justify-center">
              <h1>Reset Your Password</h1>
            </div>
            <ValidationObserver ref="logingObserver">
              <v-card-text>
                <v-form class="mt-5">
                  <ValidationProvider
                    v-slot="{ errors }"
                    name="Email"
                    rules="required|email"
                  >
                    <v-text-field
                      label="Email *"
                      outlined
                      name="login"
                      prepend-inner-icon="mdi-account"
                      type="email"
                      v-model="user.email"
                      placeholder="Email"
                      :error-messages="errors"
                      required
                    ></v-text-field>
                  </ValidationProvider>
                </v-form>
              </v-card-text>
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
              <v-card-actions class="justify-end card-action">
                <v-btn text @click="cancel">{{
                  loginError && loginError.status === 200 ? "Login" : "Cancel"
                }}</v-btn>
                <v-btn depressed large color="primary" @click="login">{{
                  resetPass ? "Reset" : "Login"
                }}</v-btn>
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
      user: {},
      showPass: false,
      resetPass: false
    };
  },
  computed: {
    ...mapGetters(["loginError"])
  },
  mounted() {
    this.$store.commit("SET_LOGINERROR", null);
  },
  components: {
    ValidationObserver,
    ValidationProvider
  },
  methods: {
    login() {
      const errors = this.$refs.logingObserver.errors;
      if (!this.resetPass && !errors.Email.length && !errors.Password.length)
        this.$store.dispatch("login", {
          user: this.user,
          router: this.$router
        });
      if (this.resetPass && !errors.Email.length) {
        this.$store.dispatch("resetPassword", {
          user: this.user,
          router: this.$router
        });
      }
    },
    cancel() {
      this.resetPass = !this.resetPass;
      this.$store.commit("SET_LOGINERROR", null);
    },
    reset() {
      this.resetPass = !this.resetPass;
      this.$store.commit("SET_LOGINERROR", null);
    }
  }
};
</script>
<style lang="scss" scoped>
#inspire {
  background: #edf3f5;
  .neureka-logo {
    width: 100%;
    max-width: 100px;
  }
  .card-neureka {
    border: 1px solid #ccc;
    border-top: 3px solid #364749;
    border-radius: 0;
    margin-top: 1.3em;
    padding-bottom: 1.3em;
    padding-top: 1.3em;
    h1 {
      font-size: 1.3em;
    }
  }
  .card-action {
    padding: 0 16px;
  }
}
</style>
