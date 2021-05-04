import {
  createUser,
  getUsers,
  editUser,
  removeUser,
  loging,
  logout,
  changePassword,
  getUsersByRole,
  resetPassword
} from "../api";
const UserModule = {
  state: () => ({
    users: [],
    doctors: [],
    currentUser: {},
    token: null,
    authenticatedUser: null,
    loginError: null,
    temporaryPassword: null
  }),
  mutations: {
    SET_USERS: (state, payload) => (state.users = payload),
    SET_DOCTORS: (state, payload) => (state.doctors = payload),
    SET_USER: (state, payload) => state.users.push(payload),
    AUTH_SUCCESS: (state, payload) => (state.authenticatedUser = payload),
    AUTH_LOGOUT: state => (state.authenticatedUser = null),
    SET_LOGINERROR: (state, payload) => (state.loginError = payload),
    SET_TEMPORARY_PASSWORD: (state, payload) =>
      (state.temporaryPassword = payload)
  },
  actions: {
    createUser({ dispatch }, payload) {
      createUser(payload).then(() => dispatch("getUsers"));
    },
    editUser({ dispatch }, payload) {
      editUser(payload).then(() => dispatch("getUsers"));
    },
    editProfile({ commit }, payload) {
      editUser(payload.user).then(() => {
        localStorage.setItem("user", JSON.stringify(payload));
        commit("AUTH_SUCCESS", payload);
      });
    },
    getUsers({ commit }) {
      getUsers().then(data => commit("SET_USERS", data));
    },
    getUsersDovtors({ commit }, payload) {
      getUsersByRole(payload).then(data => commit("SET_DOCTORS", data));
    },

    removeUser({ dispatch }, payload) {
      removeUser(payload).then(() => dispatch("getUsers"));
    },
    filterUsers({ commit, state }, payload) {
      const { users } = state;
      commit(
        "SET_USERS",
        users.filter(
          p =>
            p.firstName.toLowerCase().indexOf(payload) > -1 ||
            p.lastName.toLowerCase().indexOf(payload) > -1
        )
      );
    },
    login({ commit, dispatch }, payload) {
      commit("SET_LOGINERROR", null);
      const { user, router } = payload;
      commit("AUTH_SUCCESS", null);
      loging(user)
        .then(data => {
          commit("SET_LOGINERROR", null);
          commit("AUTH_SUCCESS", data);
          if (data.user.role === "secretary") {
            dispatch("getUsersDovtors", "doctor");
          }
          if (data.user.role === "patient") {
            router.push(`/clients/${data.user.id}`);
          }
          else {
            router.push("/");
          }
        })
        .catch(error => {
          const err = {
            status: error.status,
            message: "The email or password is incorrect"
          };
          commit("SET_LOGINERROR", err);
          console.log(error);
        });
    },
    resetPassword({ commit }, payload) {
      commit("SET_LOGINERROR", null);
      commit("SET_TEMPORARY_PASSWORD", null);
      const { user } = payload;
      resetPassword(user)
        .then(data => {
          const success = {
            status: 200,
            message: `Your temporary password was sent to ${payload.email}, Pass: ${data}`
          };
          commit("SET_LOGINERROR", success);
          commit("SET_TEMPORARY_PASSWORD", data);
        })
        .catch(error => {
          const err = {
            status: error.status,
            message: error.data
          };
          commit("SET_LOGINERROR", err);
          console.log(error);
        });
    },
    changePassword({ commit, dispatch }, payload) {
      commit("SET_LOGINERROR", null);
      commit("SET_TEMPORARY_PASSWORD", null);
      const { user, router } = payload;
      commit("AUTH_SUCCESS", null);
      changePassword(user)
        .then(data => {
          commit("AUTH_SUCCESS", data);
          if (data.user.role === "secretary") {
            dispatch("getUsersDovtors", "doctor");
          }
          router.push("/");
        })
        .catch(error => {
          const err = {
            status: error.status,
            message: "The email or password is incorrect"
          };
          commit("SET_LOGINERROR", err);
          console.log(error);
        });
    },
    logout({ commit }) {
      commit("AUTH_LOGOUT");
      logout();
    }
  },
  getters: {
    users: state => state.users,
    currentUser: state => state.currentUser,
    token: state => state.token,
    authenticatedUser: state => state.authenticatedUser,
    doctors: state => state.doctors,
    loginError: state => state.loginError,
    temporaryPassword: state => state.temporaryPassword
  }
};

export default UserModule;
