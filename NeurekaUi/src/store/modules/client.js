import { getClientVisit} from "../api";

const ClientModule = {
  state: () => ({
    clientVisit: {}
  }),
  mutations: {
    SET_PATIENT_VISIT: (state, payload) => (state.clientVisit = payload)
  },
  actions: {
    getClientVisit({ commit }, payload) {
      getClientVisit(payload)
        .then(data => commit("SET_PATIENT_VISIT", data))
        .catch(() => commit("SET_PATIENT_VISIT", {}));
    }
  },
  getters: {
    clientVisit: state => state.clientVisit
  }
};

export default ClientModule;
