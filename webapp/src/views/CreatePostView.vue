<!-- Generative AI - CoPilot was used to assist in the creation of this file.
  CoPilot was asked to create a page for creating posts with titles, locations,
  descriptions, and images, using Primevue's form components. -->
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
import WipMessage from "@/components/WipMessage.vue";

type CreatePostFormValues = {
  title: string;
  location: string;
  description: string;
  image: File | undefined;
};

type FileUploadEvent = {
  files: File[];
};

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

const onFormSubmit = (values: unknown) => {
  //API connection is not currently implemented.
  const inputtedValues = values as CreatePostFormValues;
  console.log("Submitting post:", inputtedValues);
};

const onFileSelect = (
  event: FileUploadEvent,
  setFieldValue: (name: string, value: File | undefined) => void,
) => {
  const file = event?.files && event.files[0];
  if (file) setFieldValue("image", file);
};

const editorModules = {
  toolbar: {
    container: ".custom-toolbar",
    toolbarVisible: false,
  },
};
</script>

<template>
  <div class="create-post-view form-layout">
    <PageHeader />
    <div class="form-container form-container-common">
      <WipMessage
        message="The Create Post page is currently a work in progress"
        description="The 'Create Post' and 'Upload' buttons are intended to NOT trigger API calls yet"
      />
      
      <Card>
        <template #title>
          <h2>Create a Post</h2>
        </template>
        <template #content>
          <Form
            v-slot="{ setFieldValue, errors }"
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
                <div class="custom-toolbar ql-toolbar ql-snow">
                  <span class="ql-formats">
                    <select class="ql-header">
                      <option value="1"></option>
                      <option value="2"></option>
                      <option selected></option>
                    </select>
                    <button class="ql-bold"></button>
                    <button class="ql-italic"></button>
                    <button class="ql-underline"></button>
                    <button class="ql-strike"></button>
                  </span>
                  <span class="ql-formats">
                    <button class="ql-list" value="ordered"></button>
                    <button class="ql-list" value="bullet"></button>
                  </span>
                  <span class="ql-formats">
                    <button class="ql-blockquote"></button>
                    <button class="ql-code-block"></button>
                  </span>
                  <span class="ql-formats">
                    <button class="ql-clean"></button>
                  </span>
                </div>
                <Editor
                  :modelValue="field.value"
                  @update:modelValue="field.onInput"
                  :modules="editorModules"
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

<style scoped>
.custom-toolbar {
  margin-bottom: 0.5rem;
  border: 1px solid var(--surface-border);
  border-radius: 6px;
  padding: 0.5rem;
  background: var(--surface-ground);
}

.custom-toolbar .ql-formats {
  margin-right: 15px;
}

.custom-toolbar .ql-formats:last-child {
  margin-right: 0;
}

:deep(.p-editor-toolbar) {
  display: none !important;
}
</style>
