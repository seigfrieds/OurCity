<!-- Where the JS/TS goes -->
<script setup lang="ts">
import { ref } from "vue";
import config from "./config.ts";
import { useRouter } from "vue-router";
import Button from "primevue/button";

interface TypeScriptType {
  prop: string;
}

const router = useRouter();

const hello = ref<TypeScriptType>({ prop: "Hello world!" });

const goToHome = () => {
  router.push("/");
};

const goToAbout = () => {
  router.push("/about");
};

const goToAccounts = () => {
  router.push("/accounts");
};
</script>

<!-- Where the HTML goes -->
<template>
  <h1>{{ hello.prop }}</h1>
  <h1>{{ config.example }}</h1>
  <nav>
    <Button label="Home" @click="goToHome" />
    <Button label="About" @click="goToAbout" />
    <Button label="Accounts" @click="goToAccounts" />
  </nav>
  <main>
    <router-view />
  </main>
</template>

<!-- Where styling goes -->
<style scoped>
/* note: scoped means the styles only affect THIS component */
</style>
