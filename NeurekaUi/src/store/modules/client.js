import { getClientVisit, updateClientVisit } from "../api";

const ClientModule = {
  state: () => ({
    clientVisit: {}
  }),
  mutations: {
    SET_CLIENT_VISIT: (state, payload) => {
      console.log(payload);
      state.clientVisit = payload;
    }
  },
  actions: {
    getClientVisit({ commit }, payload) {
      getClientVisit(payload)
        .then(data => commit("SET_CLIENT_VISIT", data))
        .catch(() => commit("SET_CLIENT_VISIT", {}));
    },
    updateClientVisit({ commit }, payload) {
      updateClientVisit(payload)
        .then(data => commit("SET_CLIENT_VISIT", data))
        .catch(() => commit("SET_CLIENT_VISIT", {}));
    }
  },
  getters: {
    clientVisit: state => state.clientVisit
  }
};

export default ClientModule;
