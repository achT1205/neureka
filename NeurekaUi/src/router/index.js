import Vue from "vue";
import VueRouter from "vue-router";
import DocLayout from "@/layouts/DoctorLayout.vue";
// import AdminLayout from "@/layouts/AdminLayout.vue";
import store from "@/store";

Vue.use(VueRouter);

const routes = [
  {
    path: "/login",
    component: () => import("@/views/Login.vue"),
    name: "login"
  },
  {
    path: "/change-password",
    component: () => import("@/views/ChangePassword.vue"),
    name: "change-password"
  },
  {
    path: "/401",
    component: () => import("@/views/401.vue"),
    name: "401"
  },
  {
    path: "/403",
    component: () => import("@/views/403.vue"),
    name: "403"
  },
  {
    path: "/404",
    component: () => import("@/views/404.vue"),
    name: "404"
  },
  {
    path: "/500",
    component: () => import("@/views/500.vue"),
    name: "500"
  },
  {
    path: "/clients",
    component: () => import("@/layouts/ClientLayout.vue"),
    name: "client",
    children: [
      {
        path: "/clients/:id",
        component: () => import("@/views/client/client.vue"),
        name: "client"
      },
    ]
  },
  {
    path: "/",
    component: DocLayout,
    redirect: "/patients",
    name: "Patient",
    children: [
      {
        path: "patients",
        component: () => import("@/views/patients/Patients.vue"),
        name: "patients"
      },
      {
        path: "patients/:id",
        component: () => import("@/views/patients/PatientSchedules.vue"),
        name: "Schedules"
      },
      {
        path: "home",
        component: () => import("@/views/Dashboard.vue"),
        name: "Dashboard"
      }
    ]
  },
  {
    path: "/admin",
    name: "Admin",
    component: DocLayout,
    children: [
      {
        path: "forms",
        component: () => import("../views/admin/Forms.vue"),
        name: "Forms"
      },
      {
        path: "fields",
        component: () => import("../views/admin/Fields.vue"),
        name: "Fields"
      },
      /*{
        path: "fields/:id",
        component: () => import("../views/admin/Field.vue"),
        name: "Fields"
      },*/
      {
        path: "forms/:id",
        component: () => import("../views/admin/FormFields.vue"),
        name: "Fields"
      },
      {
        path: "users",
        component: () => import("../views/admin/ManageUsers.vue"),
        name: "Users"
      },
      {
        path: "settings",
        component: () => import("../views/admin/Settings.vue"),
        name: "Settings"
      },
      {
        path: "profile",
        component: () => import("../views/admin/Profile.vue"),
        name: "Profile"
      }
    ]
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

router.beforeEach((to, from, next) => {

  const authenticated = localStorage.user
    ? JSON.parse(localStorage.user)
    : null;

  if (to.name === "login" && !store.getters.authenticatedUser) {
    store.commit("SET_EDITING_INPROGRESS", false);
    next();
    return;
  }

  else if (authenticated && authenticated.user.role === "patient" && to.name !== "client") {
    next({ name: "client", params: { id: authenticated.user.id } })
  }


  if (to.name === "login" && authenticated) next({ name: "patients" });
  else if (
    to.name !== "change-password" &&
    authenticated &&
    !authenticated.user.changePassword
  )
    next({ name: "change-password" });
  else if (to.name !== "login" && !authenticated) next({ name: "login" });
  else {
    if (!store.getters.editingInprogress) {
      next();
    } else {
      Vue.$confirm({
        message: `Changes that you made may not be saved. Are your sure you want to continue ?`,
        button: {
          no: "No",
          yes: "Yes"
        },
        callback: confirm => {
          if (confirm) {
            store.commit("SET_EDITING_INPROGRESS", false);
            next();
          } else {
            next(false);
          }
        }
      });
    }
  }


});

export default router;
