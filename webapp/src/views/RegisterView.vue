<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Card from "primevue/card";
import Button from "primevue/button";
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { Form, Field } from "vee-validate";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Message from "primevue/message";

import "@/assets/styles/forms.css";

type RegisterFormValues = {
  email: string;
  password: string;
  confirmPassword: string;
};

const initialValues = {
  email: "",
  password: "",
  confirmPassword: "",
};

const resolver = toTypedSchema(
  z
    .object({
      email: z
        .string()
        .min(1, { message: "Email is required" })
        .email({ message: "Invalid email" }),
      password: z.string().min(6, { message: "Password must be at least 6 characters" }),
      confirmPassword: z.string().min(1, { message: "Please confirm your password" }),
    })
    .refine((data) => data.password === data.confirmPassword, {
      message: "Passwords do not match",
      path: ["confirmPassword"],
    }),
);

const onFormSubmit = (values: unknown) => {
  const inputtedValues = values as RegisterFormValues;
  console.log("Register submit", inputtedValues);
  // TODO: call API to register user
  // after success: router.push('/login') or auto-login
};
</script>

<template>
  <div class="register-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <Card>
        <template #title>
          <h2>Register</h2>
        </template>
        <template #content>
          <Form
            :initialValues="initialValues"
            :resolver="resolver"
            v-slot="{ errors }"
            @submit="onFormSubmit"
          >
            <div class="field field-common">
              <label for="email">Email</label>
              <Field name="email" v-slot="{ field }">
                <InputText v-bind="field" placeholder="you@example.com" />
              </Field>
              <Message v-if="errors.email" severity="error" size="small" variant="simple">{{
                errors.email
              }}</Message>
            </div>

            <div class="field field-common">
              <label for="password">Password</label>
              <Field name="password" v-slot="{ field }">
                <Password v-bind="field" :feedback="false" toggleMask />
              </Field>
              <Message v-if="errors.password" severity="error" size="small" variant="simple">{{
                errors.password
              }}</Message>
            </div>

            <div class="field field-common">
              <label for="confirmPassword">Confirm Password</label>
              <Field name="confirmPassword" v-slot="{ field }">
                <Password v-bind="field" :feedback="false" toggleMask />
              </Field>
              <Message
                v-if="errors.confirmPassword"
                severity="error"
                size="small"
                variant="simple"
                >{{ errors.confirmPassword }}</Message
              >
            </div>

            <Button type="submit" label="Register" class="mt-2" />
          </Form>
        </template>
      </Card>
    </div>
  </div>
</template>
