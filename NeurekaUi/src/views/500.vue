<template>
  <v-app id="inspire">
    <v-container class="fill-height" fluid>
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="4">
          <img
            :src="errGif"
            width="700"
            height="500"
            alt="Girl has dropped her ice cream."
          />
        </v-col>
        <v-col cols="12" sm="8" md="4">
          <v-alert border="bottom" colored-border type="warning">
            <h1>OOPS!</h1>
            Error while requesting the server, contact the support !
            <v-btn @click="$router.go(-1)" class="ma-2" outlined color="blue"
              >Back</v-btn
            >
          </v-alert>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>
<script>
import errGif from "@/assets/images/500.gif";
export default {
  data() {
    return {
      errGif: errGif + "?" + +new Date()
    };
  }
};
</script>
