<!-- Generative AI - Claude was used to assist uploading files -->
<!-- Prompt: Help me implement multiple files upload in my Vue.js page, I already have my api set up -->

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
import { ref } from "vue";
import { useToast } from "primevue/usetoast";
import { createPost } from "@/api/posts";
import { uploadImage } from "@/api/images";

import "@/assets/styles/forms.css";
import { getMyInfo } from "@/api/authentication";
import { getUserByUsername } from "@/api/users";
import { useRouter } from "vue-router";

type CreatePostFormValues = {
  title: string;
  location?: string;
  description: string;
  images?: File[];
};

type FileUploadEvent = {
  files: File[];
};

const toast = useToast(); 
const isSubmitting = ref(false);
const router = useRouter();

const initialValues = {
  title: "",
  location: "",
  description: "",
  images: [],
};

const resolver = toTypedSchema(
  z.object({
    title: z.string().min(1, { message: "Title is required" }),
    location: z.string().optional(),
    description: z.string().min(1, { message: "Description is required" }),
    images: z
      .array(z.instanceof(File))
      .optional()
      .refine(
        (files) => !files || files.length === 0 || files.length <= 10,
        { message: "Maximum 10 images allowed" }
      )
      .refine(
        (files) => !files || files.every((file) => file.size < 5000000),
        { message: "Each image must be less than 5MB" }
      ) 
      .refine(
        (files) => !files || files.every((file) => 
          ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'].includes(file.type)
        ),
        { message: "Only image files (JPG, PNG, GIF, WebP) are allowed" }
      ),
  }),
);

const onFormSubmit = async (values: unknown) => {
  const inputtedValues = values as CreatePostFormValues;
  const images = inputtedValues.images; 
  isSubmitting.value = true;

  try {
    console.log("Creating post:", inputtedValues);
    const username = await getMyInfo(); 
    const user = await getUserByUsername(username);

    const createPostDto = {
      authorId: user.id, 
      title: inputtedValues.title,
      description: inputtedValues.description,
      location: inputtedValues.location,
    }
    const post = await createPost(createPostDto);

    if(images && images.length > 0) {
      await uploadImage(post.id, images); 
    }

    toast.add({
      severity: 'success',
      summary: 'Success',
      detail: `Post created successfully`,
      life: 3000,
    });

  } catch(error) {
    console.error("Error creating a post", error);
    
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: 'Failed to create post',
      life: 5000,
    });
    
  } finally {
    isSubmitting.value = false;
    router.push('/');
  }
};

const onFileSelect = (
  event: FileUploadEvent,
  setFieldValue: (name: string, value: File | undefined) => void,
) => {
  const file = event?.files && event.files[0];
  if (file) setFieldValue("image", file);
};

// Editor modules: use custom toolbar and disable the default one
const editorModules = {
  toolbar: {
    container: ".custom-toolbar", // Use our custom toolbar
    toolbarVisible: false, // Hide PrimeVue's default toolbar
  },
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
                <!-- custom toolbar for the editor: explicitly define controls -->
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
/* Custom toolbar styling */
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

/* Hide any default toolbar that might still appear */
:deep(.p-editor-toolbar) {
  display: none !important;
}
</style>
