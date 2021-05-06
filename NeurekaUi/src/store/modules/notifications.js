const NotificationModule = {
  state: () => ({
    notifications: []
  }),
  mutations: {
    SET_NOTIFICATIONS: (state, payload) => {
      state.notifications = payload;
      localStorage.notifications = JSON.stringify(state.notifications);
    },
    SET_NOTIFICATION: (state, payload) => {
      const notifications = state.notifications;
      const ns = notifications.filter(x => x.patientId === payload.patientId);
      if (!ns || ns.length === 0) {
        state.notifications.push(payload);
        localStorage.notifications = JSON.stringify(state.notifications);
      }
    }
  },
  actions: {},
  getters: {
    notifications: state => state.notifications
  }
};

export default NotificationModule;
