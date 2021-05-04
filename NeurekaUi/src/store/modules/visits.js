import {
  createVisit,
  getVisits,
  removeVisit,
  editVisit,
  getVisit,
  deleteFile,
  uploadFile,
  exportVisits
} from "../api";

const VisitModule = {
  state: () => ({
    visits: [],
    currentVisit: {}
  }),
  mutations: {
    SET_VISITS: (state, payload) => (state.visits = payload),
    SET_VISIT: (state, payload) => (state.currentVisit = payload),
    ADD_VISIT: (state, payload) => state.visits.push(payload),
    ADD_FROM_TO_VISIT: (state, payload) =>
      state.currentVisit.fields.push(payload),
    ADD_FIELD_TO_FORM: (state, payload) =>
      state.currentVisit.fields[payload.index].pus(payload.field),
    ADD_NEW_TO_LIST: (state, payload) => {
      if (state.currentVisit.id === payload.id) state.currentVisit = payload;
      else if (state.currentVisit.patientId === payload.patientId)
        state.visits.push(payload);
    },
    UPDATE_IN_LIST(state, payload) {
      if (state.currentVisit.id === payload.id) state.currentVisit = payload;
      else if (state.currentVisit.patientId === payload.patientId) {
        const index = state.visits.findIndex(v => v.id === payload.id);
        if (index > -1) {
          state.visit[index] = payload;
        }
      }
    }
  },
  actions: {
    createVisit({ dispatch }, payload) {
      const { visit, patientId } = payload;
      createVisit(visit).then(() => dispatch("getVisits", patientId));
    },
    getVisits({ commit }, payload) {
      getVisits(payload)
        .then(data => commit("SET_VISITS", data))
        .catch(() => commit("SET_VISITS", []));
    },
    getVisit({ commit }, payload) {
      getVisit(payload)
        .then(data => commit("SET_VISIT", data))
        .catch(() => commit("SET_VISIT", {}));
    },
    removeVisit({ dispatch }, payload) {
      removeVisit(payload.visitId).then(() =>
        dispatch("getVisits", payload.patientId)
      );
    },
    editVisit({ commit, dispatch }, payload) {
      const { visit, filesData, all, patientId } = payload;
      if (!filesData || filesData.length === 0) {
        editVisit(visit).then(() => {
          if (all) {
            dispatch("getVisits", patientId);
          } else {
            commit("SET_EDITING_INPROGRESS", false, { root: true });
            dispatch("getVisit", visit.id);
          }
        });
      } else {
        filesData.forEach(fileData => {
          uploadFile(fileData.formdata).then(data => {
            const fileInfo = {
              id: fileData.id,
              title: data.title,
              fileName: data.fileName,
              type: fileData.type
            };

            if (
              !visit.fields[fileData.index].fields[fileData.subfieldindex]
                .fileInfos
            ) {
              visit.fields[fileData.index].fields[
                fileData.subfieldindex
              ].fileInfos = [fileInfo];
              delete visit.fields[fileData.index].fields[fileData.subfieldindex]
                .model;
            } else {
              visit.fields[fileData.index].fields[
                fileData.subfieldindex
              ].fileInfos.push(fileInfo);
              delete visit.fields[fileData.index].fields[fileData.subfieldindex]
                .model;
            }
            editVisit(visit).then(() => dispatch("getVisit", visit.id));

            //fileInfos.push(fileInfo);

            /*if (
              !visit.fields[fileData.index].fields[fileData.subfieldindex]
                .fileInfos
            ) {
              visit.fields[fileData.index].fields[
                fileData.subfieldindex
              ].fileInfos = [fileInfo];
              delete visit.fields[fileData.index].fields[fileData.subfieldindex]
                .model;
              if (index === filesData.length - 1) {
                editVisit(visit).then(() => dispatch("getVisit", visit.id));
              }
            } else {
              visit.fields[fileData.index].fields[
                fileData.subfieldindex
              ].fileInfos.push(fileInfo);
              delete visit.fields[fileData.index].fields[fileData.subfieldindex]
                .model;

              if (index === filesData.length - 1) {
                editVisit(visit).then(() => dispatch("getVisit", visit.id));
              }
            }*/
          });
        });
      }
    },
    deleteFile({ dispatch }, payload) {
      const { sessionIndex, fieldIndex, fileIndex, visit, fileInfo } = payload;
      deleteFile(fileInfo.fileName)
        .then(() => {
          visit.fields[sessionIndex].fields[fieldIndex].fileInfos.splice(
            fileIndex,
            1
          );
          editVisit(visit).then(() => dispatch("getVisit", visit.id));
        })
        .catch(error => console.error(error));
    },
    exportVisits({ commit }, payload) {
      exportVisits(payload).then(() => commit(""));
    }
  },
  getters: {
    visits: state => state.visits,
    currentVisit: state => state.currentVisit
  }
};

export default VisitModule;
