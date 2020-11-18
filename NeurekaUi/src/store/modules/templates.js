import {
  createtemplate,
  getTemplates,
  editTemplate,
  removeTemplate,
  getTemplate
} from "../api";
const TemplateModule = {
  state: () => ({
    templates: [],
    currentTemplate: {}
  }),
  mutations: {
    SET_TEMPLATES: (state, payload) => (state.templates = payload),
    SET_TEMPLATE: (state, payload) => (state.currentTemplate = payload)
  },
  actions: {
    addTemplate({ dispatch }, payload) {
      createtemplate(payload).then(() => dispatch("getTemplates"));
    },
    getTemplates({ commit }) {
      getTemplates().then(data => commit("SET_TEMPLATES", data));
    },
    editTemplate({ commit }, payload) {
      editTemplate(payload).then(() => {
        commit("SET_EDITING_INPROGRESS", false, { root: true });
        commit("SET_TEMPLATE", payload);
      });
    },
    removeTemplate({ dispatch }, payload) {
      removeTemplate(payload).then(() => dispatch("getTemplates"));
    },
    getTemplate({ commit }, payload) {
      getTemplate(payload).then(data => {
        commit("SET_TEMPLATE", data);
      });
    }
  },
  getters: {
    templates: state => state.templates,
    currentTemplate: state => state.currentTemplate
  }
};

export default TemplateModule;
