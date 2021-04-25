import {
  createtemplate,
  getTemplates,
  editTemplate,
  editFieldTemplate,
  removeTemplate,
  removeFieldTemplate,
  getTemplate,
  createFieldTemplate,
  getFieldTemplates
} from "../api";
const TemplateModule = {
  state: () => ({
    templates: [],
    fieldtemplates: [],
    currentTemplate: {},
    currentFieldTemplate: {}
  }),
  mutations: {
    SET_TEMPLATES: (state, payload) => (state.templates = payload),
    SET_TEMPLATE: (state, payload) => (state.currentTemplate = payload),
    SET_FIELD_TEMPLATES: (state, payload) => (state.fieldtemplates = payload),
    SET_TFIELD_TEMPLATE: (state, payload) =>
      (state.currentFieldTemplate = payload)
  },
  actions: {
    addTemplate({ dispatch }, payload) {
      createtemplate(payload).then(() => dispatch("getTemplates"));
    },
    addFieldTemplate({ dispatch }, payload) {
      createFieldTemplate(payload).then(() => dispatch("getFieldTemplates"));
    },
    getTemplates({ commit }) {
      getTemplates().then(data => commit("SET_TEMPLATES", data));
    },
    getFieldTemplates({ commit }) {
      getFieldTemplates().then(data => commit("SET_FIELD_TEMPLATES", data));
    },
    editTemplate({ commit, dispatch }, payload) {
      editTemplate(payload).then(() => {
        commit("SET_EDITING_INPROGRESS", false, { root: true });
        dispatch("getTemplates", payload);
      });
    },
    editFieldTemplate({ commit, dispatch }, payload) {
      editFieldTemplate(payload).then(() => {
        commit("SET_EDITING_INPROGRESS", false, { root: true });
        dispatch("getFieldTemplates");
      });
    },

    removeTemplate({ dispatch }, payload) {
      removeTemplate(payload).then(() => dispatch("getTemplates"));
    },

    removeFieldTemplate({ dispatch }, payload) {
      removeFieldTemplate(payload).then(() => dispatch("getFieldTemplates"));
    },

    getTemplate({ commit }, payload) {
      getTemplate(payload).then(data => {
        commit("SET_TEMPLATE", data);
      });
    }
  },
  getters: {
    templates: state => state.templates,
    currentTemplate: state => state.currentTemplate,
    fieldtemplates: state => state.fieldtemplates,
    currentFieldTemplate: state => state.currentFieldTemplate
  }
};

export default TemplateModule;
