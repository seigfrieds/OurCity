<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Button from "primevue/button";
import Card from "primevue/card";
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { Form, Field } from "vee-validate";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Message from "primevue/message";

import "@/assets/styles/forms.css";

type LoginFormValues = {
  email: string;
  password: string;
};

const initialValues = {
  email: "",
  password: "",
};

const resolver = toTypedSchema(
  z.object({
    email: z.string().min(1, { message: "Email is required" }).email({ message: "Invalid email" }),
    password: z.string().min(6, { message: "Password must be at least 6 characters" }),
  }),
);

const onFormSubmit = (values: unknown) => {
  const inputtedValues = values as LoginFormValues;
  console.log("Login submit", inputtedValues);
  // TODO: call API client to authenticate
};
</script>

<template>
  <div class="login-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <Card>
        <template #title>
          <h2>Login</h2>
        </template>
        <template #content>
          <Form
            :initialValues="initialValues"
            :resolver="resolver"
            v-slot="{ errors }"
            @submit="onFormSubmit"
          >
            <div class="field-common">
              <label for="email">Email</label>
              <Field name="email" v-slot="{ field }">
                <InputText v-bind="field" placeholder="you@example.com" />
              </Field>
              <Message v-if="errors.email" severity="error" size="small" variant="simple">{{
                errors.email
              }}</Message>
            </div>

            <div class="field-common">
              <label for="password">Password</label>
              <Field name="password" v-slot="{ field }">
                <Password v-bind="field" :feedback="false" toggleMask />
              </Field>
              <Message v-if="errors.password" severity="error" size="small" variant="simple">{{
                errors.password
              }}</Message>
            </div>

            <Button type="submit" label="Log in" class="mt-2" />
          </Form>
          <div class="card-footer">
            <span>New here? </span><router-link to="/register">Create an account</router-link>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>
