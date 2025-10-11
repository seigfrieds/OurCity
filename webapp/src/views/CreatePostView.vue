<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import Button from "primevue/button";
import Card from "primevue/card";
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { Form, Field } from "vee-validate";
import FileUpload from "primevue/fileupload";
import InputText from "primevue/inputtext";
import Editor from "primevue/editor";
import Message from "primevue/message";

import "@/assets/styles/forms.css";

const initialValues = {
  title: "",
  location: "",
  description: "",
  image: undefined,
};

const resolver = toTypedSchema(
  z.object({
    title: z.string().min(1, { message: "Title is required" }),
    location: z.string().min(1, { message: "Location is required" }),
    description: z.string().min(1, { message: "Description is required" }),
    image: z
      .instanceof(File, { message: "Image is required" })
      .refine((file: File) => file.size < 1000000, { message: "Image must be less than 1MB" }),
  }),
);

const onFormSubmit = (values: any) => {
  console.log("Submitting post:", values);
  // Here you would typically make an API call to create the post
};

const onFileSelect = (event: any, setFieldValue: (name: string, value: any) => void) => {
  const file = event?.files && event.files[0];
  if (file) setFieldValue("image", file);
};
</script>

<template>
  <div class="create-post-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <Card>
        <template #title>
          <h2>Create a Post</h2>
        </template>
        <template #content>
          <Form
            v-slot="{ setFieldValue, values, errors }"
            :initialValues="initialValues"
            :resolver="resolver"
            @submit="onFormSubmit"
            class="form-container form-container-common"
          >
            <div class="field-common">
              <label for="title">Title</label>
              <Field name="title" v-slot="{ field }">
                <InputText v-bind="field" placeholder="Title" />
              </Field>
              <Message v-if="errors.title" severity="error" size="small" variant="simple">{{
                errors.title
              }}</Message>
            </div>
            <div class="field-common">
              <label for="location">Location</label>
              <Field name="location" v-slot="{ field }">
                <InputText v-bind="field" placeholder="Location" />
              </Field>
              <Message v-if="errors.location" severity="error" size="small" variant="simple">{{
                errors.location
              }}</Message>
            </div>
            <div class="field-common">
              <label for="image">Image</label>
              <FileUpload
                name="image"
                @select="onFileSelect($event, setFieldValue)"
                :multiple="false"
                accept="image/*"
                :maxFileSize="1000000"
                customUpload
              >
                <template #empty>
                  <span>Drag and drop files to here to upload.</span>
                </template>
              </FileUpload>
              <Message v-if="errors.image" severity="error" size="small" variant="simple">{{
                errors.image
              }}</Message>
            </div>
            <div class="field-common">
              <label for="description">Description</label>
              <Field name="description" v-slot="{ field }">
                <Editor
                  :modelValue="field.value"
                  @update:modelValue="field.onInput"
                  editorStyle="height: 320px"
                />
              </Field>
              <Message v-if="errors.description" severity="error" size="small" variant="simple">{{
                errors.description
              }}</Message>
            </div>
            <Button type="submit" label="Create Post" class="mt-2"></Button>
          </Form>
        </template>
      </Card>
    </div>
  </div>
</template>
