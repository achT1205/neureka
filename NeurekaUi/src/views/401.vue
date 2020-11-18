<template>
  <v-app id="inspire">
    <v-container class="fill-height" fluid>
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="2">
          <img
            :src="errGif"
            width="313"
            height="428"
            alt="Girl has dropped her ice cream."
          />
        </v-col>
        <v-col cols="12" sm="8" md="4">
          <v-alert border="bottom" colored-border type="warning">
            <h1>OOPS!</h1>
            You are not authenticated, click here to go to the login page
            <v-btn
              @click="$router.push('/login')"
              class="ma-2"
              outlined
              color="blue"
              >Login</v-btn
            >
          </v-alert>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>
<script>
import errGif from "@/assets/images/401.gif";
export default {
  data() {
    return {
      errGif: errGif + "?" + +new Date()
    };
  }
};
</script>
