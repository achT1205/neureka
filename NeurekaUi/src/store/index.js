import Vue from "vue";
import Vuex from "vuex";
import PatientModule from "./modules/patients";
import VisitModule from "./modules/visits";
import TemplateModule from "./modules/templates";
import EventModule from "./modules/events";
import UserModule from "./modules/users";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    editingInprogress: false
  },
  mutations: {
    SET_EDITING_INPROGRESS: (state, payload) =>
      (state.editingInprogress = payload)
  },
  actions: {},
  getters: {
    editingInprogress: state => state.editingInprogress
  },
  modules: {
    PatientModule,
    VisitModule,
    TemplateModule,
    EventModule,
    UserModule
  }
});
