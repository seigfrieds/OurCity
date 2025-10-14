<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Card from "primevue/card";
import Button from "primevue/button";
import InputText from "primevue/inputtext";

const accountDetails = {
  username: "johndoe",
  email: "john@example.com",
  // Add more fields as needed
};
</script>

<template>
  <div class="edit-account-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
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