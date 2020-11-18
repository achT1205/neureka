import { createPatient, getPatients, editPatient } from "../api";
const PatientModule = {
  state: () => ({
    patients: [],
    currentPatient: {}
  }),
  mutations: {
    SET_PATIENTS: (state, payload) => (state.patients = payload),
    SET_PATIENT: (state, payload) => state.patients.push(payload),
    SET_PATIENT_FROM_TEMPLATE: (state, payload) => {
      delete payload.id;
      state.currentPatient = payload;
    }
  },
  actions: {
    createPatient({ dispatch }, payload) {
      createPatient(payload).then(() => dispatch("getPatients"));
    },
    editPatient({ dispatch }, payload) {
      editPatient(payload).then(() => dispatch("getPatients"));
    },
    getPatients({ commit }) {
      getPatients().then(data => commit("SET_PATIENTS", data));
    },
    filterPatients({ commit, state }, payload) {
      const { patients } = state;
      commit(
        "SET_PATIENTS",
        patients.filter(
          p =>
            p.firstName.toLowerCase().indexOf(payload) > -1 ||
            p.lastName.toLowerCase().indexOf(payload) > -1
        )
      );
    }
  },
  getters: {
    patients: state => state.patients,
    currentPatient: state => state.currentPatient
  }
};

export default PatientModule;
