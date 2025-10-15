<!-- Generative AI - CoPilot was used to assist in the creation of this file.
  CoPilot was asked to create a page for users to log in with their email and password. -->
<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Button from "primevue/button";
import Card from "primevue/card";
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { Form, Field } from "vee-validate";
import InputText from "primevue/inputtext";

import "@/assets/styles/forms.css";
import WipMessage from "@/components/WipMessage.vue";

type LoginFormValues = {
  username: "";
};

const initialValues = {
  username: "",
};

const resolver = toTypedSchema(
  z.object({
    username: z.string().min(1, { message: "Username is required" }).nonempty(),
  }),
);

const onFormSubmit = (values: unknown) => {
  //API connection is not currently implemented.
  const inputtedValues = values as LoginFormValues;
  console.log("Login submit", inputtedValues);
};
</script>
<template>
  <div class="login-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <WipMessage
        message="The Login Page is currently a work in progress"
        description="The 'Log in' button is intended to NOT trigger a login API call yet"
      />

      <Card>
        <template #title>
          <h2 id="login-header">Login</h2>
        </template>
        <template #content>
          <Form :initialValues="initialValues" :resolver="resolver" @submit="onFormSubmit">
            <div class="field-common">
              <label id="username-label" for="username">username</label>
              <Field id="username-field" name="username" v-slot="{ field }">
                <InputText id="uesrname-inputText" v-bind="field" placeholder="John Doe" />
              </Field>
            </div>
            <Button id="login-btn" type="submit" label="Log in" class="mt-2" />
          </Form>
          <div class="card-footer">
            <span>New here? </span><router-link to="/register">Create an account</router-link>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>

<style>
#login-header {
  margin-top: 1rem;
  margin-bottom: 2rem;
}

#login-btn {
  margin-top: 2rem;
}
</style>
