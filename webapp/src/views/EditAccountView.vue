<!-- Generative AI - CoPilot was used to assist in the creation of this file.
  This file was adapted from the RegisterView.vue file which was created with CoPilot.
  Comments in that file explain how is was created. -->
<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Card from "primevue/card";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import WipMessage from "@/components/WipMessage.vue";

const accountDetails = {
  username: "johndoe",
  email: "john@example.com",
};
</script>

<template>
  <div class="edit-account-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <WipMessage
        message="The Edit Account page is currently a work in progress"
        description="The 'Save Changes' button is NOT indended to trigger an API call yet"
      />
      <Card>
        <template #title>
          <h2>Edit Account</h2>
        </template>
        <template #content>
          <form class="form-container form-container-common">
            <div class="field-common">
              <label for="username">Username</label>
              <InputText id="username" v-model="accountDetails.username" />
            </div>
            <div class="field-common">
              <label for="email">Email</label>
              <InputText id="email" v-model="accountDetails.email" type="email" />
            </div>
            <Button type="submit" label="Save Changes" class="mt-2" />
          </form>
        </template>
      </Card>
    </div>
  </div>
</template>

<style scoped>
.edit-account-view {
  min-height: 100vh;
}
</style>
