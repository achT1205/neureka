import { getEvents } from "../api";
const EventModule = {
  state: () => ({
    events: [],
    currentEvent: {}
  }),
  mutations: {
    SET_EVENTS: (state, payload) => (state.events = payload)
  },
  actions: {
    getEvents({ commit }, payload) {
      getEvents(payload).then(data => commit("SET_EVENTS", data));
    }
  },
  getters: {
    events: state => state.events
  }
};

export default EventModule;
