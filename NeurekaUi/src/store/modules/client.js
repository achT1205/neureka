import {
    getPatientVisits,
    getPatientVisit,
} from "../api";

const ClientModule = {
    state: () => ({
        clientVisits: [],
        clientVisit: {}
    }),
    mutations: {
        SET_PATIENT_VISITS: (state, payload) => (state.clientVisits = payload),
        SET_PATIENT_VISIT: (state, payload) => (state.clientVisit = payload),
    },
    actions: {
        getPatientVisits({ commit }, payload) {
            getPatientVisits(payload)
                .then(data => commit("SET_PATIENT_VISITS", data))
                .catch(() => commit("SET_PATIENT_VISITS", []));
        },
        getPatientVisit({ commit }, payload) {
            getPatientVisit(payload)
                .then(data => commit("SET_PATIENT_VISIT", data))
                .catch(() => commit("SET_PATIENT_VISIT", {}));
        }
    },
    getters: {
        clientVisits: state => state.clientVisits,
        clientVisit: state => state.clientVisit
    }
};

export default ClientModule;
