<template>
  <div class="col-3" v-if="patient">
    <div class="card" @click.stop="$router.push(`/patients/${patient.id}`)">
      <!-- Header -->
      <div class="card-header">
        <div class="card-header__avatar">
          <v-avatar height="128" width="128">
            <img
              src="@/assets/avatar/male.png"
              v-if="patient.sexe === 'male'"
            />
            <img
              src="@/assets/avatar/female.png"
              v-if="patient.sexe === 'female'"
            />
            <img
              src="@/assets/avatar/none.png"
              v-if="patient.sexe === 'none'"
            />
          </v-avatar>
        </div>
        <div class="show-detail-patient">
          <v-btn small :to="`/patients/${patient.id}`">show-detail</v-btn>
        </div>
        <a @click.stop="dialog = !dialog" class="card-header__follow">edit</a>
      </div>
      <!-- Content-->
      <div class="card-content">
        <div class="card-content__username">
          {{ patient.firstName }} {{ patient.lastName }}
        </div>
        <h6>{{ patient.email }} <br /></h6>
      </div>
      <!-- Footer-->
      <div class="card-footer">
        <div class="card-footer__pens">
          <span>{{ patient.jobTitle }}</span>
          <div class="label">Job title</div>
        </div>
        <div class="card-footer__pens">
          <span>{{ patient.phone }}</span>
          <div class="label">Phone</div>
        </div>

        <div class="card-footer__pens">
          <span>{{ patient.age }}</span>
          <div class="label">Age(years old)</div>
        </div>
      </div>
    </div>
    <edit-patient
      v-if="patient"
      :dialog="dialog"
      :current-patient="patient"
      @close="close"
    />
  </div>
</template>

<script>
import EditPatient from "@/components/Patients/EditePatient";
export default {
  components: {
    EditPatient
  },
  props: {
    patient: Object
  },
  data() {
    return {
      dialog: false
    };
  },
  methods: {
    close() {
      this.dialog = false;
    }
  }
};
</script>

<style lang="scss" scoped>
.card {
  background: #ffffff;
  border-radius: 4px;
  border: 1px solid #eee;
  margin: 0 auto;
  overflow: hidden;
}

.card-header {
  position: relative;
  background: #303841;
  height: 200px;
  text-align: center;
  overflow: hidden;
}

.card-header__avatar {
  background: #303841;
  background-position: center 30%;
  background-size: 100%;
  height: 100%;
  width: 100%;
}

.card-header__follow {
  position: absolute;
  top: 20px;
  right: 20px;
  background: #ffffff;
  border-radius: 2px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  padding: 6px 10px;
  color: #333333;
  font-size: 10px;
  font-weight: 600;
  line-height: normal;
  text-decoration: none;
  text-transform: uppercase;
}

.card-content {
  text-align: center;
  padding: 10px 20px;
}

.card-content__username {
  margin: 0 0 10px;
  color: #333333;
  font-size: 14px;
  font-weight: 600;
  text-transform: uppercase;
}

.card-content__username .badge {
  display: inline-block;
  background: #fcd000;
  border-radius: 2px;
  margin: 0 10px 0;
  padding: 4px;
  color: #333333;
  font-size: 10px;
  font-weight: 600;
  vertical-align: middle;
}

.card-content__bio {
  color: #666666;
  font-size: 12px;
}

.card-footer {
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -webkit-flex-direction: row;
  -ms-flex-direction: row;
  flex-direction: row;
  -webkit-flex-wrap: nowrap;
  -ms-flex-wrap: nowrap;
  flex-wrap: nowrap;
  -webkit-box-pack: justify;
  -webkit-justify-content: space-between;
  -ms-flex-pack: justify;
  justify-content: space-between;
  background: #f3f3f3;
  padding: 15px 10px;
  color: #333333;
  font-size: 14px;
  font-weight: 600;
  text-align: center;
}

.card-footer .label {
  display: block;
  margin: 4px 0 0;
  color: #666666;
  font-size: 10px;
  font-weight: 400;
}

.code {
  background: rgba(0, 0, 0, 0.1);
  max-width: 600px;
  border-radius: 2px;
  margin: 40px auto 100px;
  font-family: monospace;
  overflow: hidden;
  overflow-x: auto;
}

.code:before {
  content: "HTML Code";
  display: block;
  padding: 20px 20px 0;
  color: #333333;
  font-weight: 600;
}
</style>
