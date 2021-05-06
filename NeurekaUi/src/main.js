import Vue from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import VueConfirmDialog from "vue-confirm-dialog";
import "./plugins/chartist";
import "./plugins/vee-validate";
import money from "v-money";

Vue.use(VueConfirmDialog);
Vue.component("vue-confirm-dialog", VueConfirmDialog.default);
const VueScrollTo = require("vue-scrollto");

Vue.config.productionTip = false;
Vue.use(VueScrollTo);
Vue.use(money, { precision: 4 });
new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount("#app");
