import axios from "axios";
import router from "@/router";
export const config = {
  baseURL: process.env.VUE_APP_API_URL_ROOT,
  headers: { "Content-Type": "application/json" }
};

export const apiClient = axios.create(config);
apiClient.defaults.headers.common["Access-Control-Allow-Origin"] = "*";

export const setJWT = () => {
  const user = JSON.parse(localStorage.user);
  const accessToken = user.accessToken;
  apiClient.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
};

apiClient.interceptors.request.use(
  config => {
    let token = null;
    if (localStorage.user) {
      const user = JSON.parse(localStorage.user);
      token = user.accessToken;
    }
    if (token) {
      config.headers.common["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);
apiClient.interceptors.response.use(
  response => {
    if (response.status === 200 || response.status === 201) {
      return Promise.resolve(response);
    } else {
      return Promise.reject(response);
    }
  },
  error => {
    if (error.response.status) {
      switch (error.response.status) {
        case 400:
          //do something
          break;
        case 401:
          {
            const authenticated = localStorage.user
              ? JSON.parse(localStorage.user)
              : null;
            if (authenticated) logout();
            router.replace("/401");
          }
          break;
        case 403:
          router.replace("/403");
          break;
        case 404:
          router.replace("/404");
          break;
        case 502:
          router.replace("/500");
      }
      return Promise.reject(error.response);
    }
  }
);

export const loging = user =>
  new Promise((resolve, reject) => {
    apiClient
      .post("auth/authenticate", user)
      .then(response => {
        localStorage.setItem("user", JSON.stringify(response.data));
        setJWT();
        resolve(response.data);
      })
      .catch(error => reject(error));
  });

export const resetPassword = user =>
  new Promise((resolve, reject) => {
    apiClient
      .post("auth/reset-password", user)
      .then(response => {
        resolve(response.data);
      })
      .catch(error => reject(error));
  });

export const changePassword = user =>
  new Promise((resolve, reject) => {
    apiClient
      .post("auth/change-password", user)
      .then(response => {
        localStorage.setItem("user", JSON.stringify(response.data));
        setJWT();
        resolve(response.data);
      })
      .catch(error => reject(error));
  });

export const logout = () =>
  new Promise((resolve, reject) => {
    localStorage.removeItem("user");
    delete apiClient.defaults.headers.common.Authorization;
    router.replace("/login");
    resolve();
    console.log(reject);
  });

export const createPatient = patient =>
  new Promise((resolve, reject) => {
    apiClient
      .post("Patient", patient)
      .then(response => resolve(response.data.value))
      .catch(error => reject(error));
  });

export const editPatient = patient =>
  new Promise((resolve, reject) => {
    apiClient
      .put(`Patient/id:length(24)?id=${patient.id}`, patient)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const getPatients = () =>
  new Promise((resolve, reject) => {
    apiClient
      .get("Patient")
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const createVisit = visit =>
  new Promise((resolve, reject) => {
    apiClient
      .post("Visit", visit)
      .then(response => resolve(response.data.value))
      .catch(error => reject(error));
  });

export const getVisits = id =>
  new Promise((resolve, reject) => {
    apiClient
      .get(`Visit/patients/${id}`)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const removeVisit = id =>
  new Promise((resolve, reject) => {
    apiClient
      .delete(`Visit/id:length(24)?id=${id}`)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const editVisit = visit =>
  new Promise((resolve, reject) => {
    apiClient
      .put(`Visit/id:length(24)?id=${visit.id}`, visit)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const createtemplate = template =>
  new Promise((resolve, reject) => {
    apiClient
      .post("Template", template)
      .then(response => resolve(response.data.value))
      .catch(error => reject(error));
  });

export const getTemplates = () =>
  new Promise((resolve, reject) => {
    apiClient
      .get("Template")
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const editTemplate = template =>
  new Promise((resolve, reject) => {
    apiClient
      .put(`Template/id:length(24)?id=${template.id}`, template)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const removeTemplate = id =>
  new Promise((resolve, reject) => {
    apiClient
      .delete(`Template/id:length(24)?id=${id}`)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const getTemplate = id =>
  new Promise((resolve, reject) => {
    apiClient
      .get(`Template/${id}`)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const getVisit = id =>
  new Promise((resolve, reject) => {
    apiClient
      .get(`Visit/${id}`)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const deleteFile = fileName =>
  new Promise((resolve, reject) => {
    apiClient
      .post("Visit/DeleteFile", { fileName: fileName })
      .then(response => resolve(response.data.value))
      .catch(error => reject(error));
  });

export const uploadFile = f => {
  const fileconfig = {
    baseURL: process.env.VUE_APP_API_URL_ROOT,
    headers: { "Content-Type": "multipart/form-data" }
  };

  const fileapiClient = axios.create(fileconfig);
  fileapiClient.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
  const user = JSON.parse(localStorage.user);
  const accessToken = user.accessToken;
  fileapiClient.defaults.headers.common.Authorization = `Bearer ${accessToken}`;

  return new Promise((resolve, reject) => {
    fileapiClient
      .post(`Visit/upload`, f)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });
};

export const getEvents = dto =>
  new Promise((resolve, reject) => {
    apiClient
      .post("Event/GetEventsByDate", dto)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const createUser = user =>
  new Promise((resolve, reject) => {
    apiClient
      .post("User", user)
      .then(response => resolve(response.data.value))
      .catch(error => reject(error));
  });

export const editUser = user =>
  new Promise((resolve, reject) => {
    apiClient
      .put(`User/id:length(24)?id=${user.id}`, user)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const getUsers = () =>
  new Promise((resolve, reject) => {
    apiClient
      .get("User")
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export const removeUser = id =>
  new Promise((resolve, reject) => {
    apiClient
      .delete(`User/id:length(24)?id=${id}`)
      .then(response => resolve(response))
      .catch(error => reject(error));
  });

export const getUsersByRole = role =>
  new Promise((resolve, reject) => {
    apiClient
      .get("User/" + role)
      .then(response => resolve(response.data))
      .catch(error => reject(error));
  });

export function exportVisits(patient) {
  return new Promise((resolve, reject) => {
    apiClient
      .get(`Visit/export/excel?patientId=${patient.id}`, {
        responseType: "blob"
      })
      .then(response => {
        const blob = new Blob([response.data], {
          type:
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        });
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute(
          "download",
          `${patient.firstName} ${patient.lastName}_visits.xlsx`
        );
        document.body.appendChild(link);
        link.click();
      })
      .catch(error => reject(error));
  });
}
