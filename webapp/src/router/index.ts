import { createRouter, createWebHistory } from "vue-router";

import HomeView from "@/views/HomeView.vue";
import CreatePostView from "@/views/CreatePostView.vue";
import LoginView from "@/views/LoginView.vue";
import RegisterView from "@/views/RegisterView.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", name: 'home', component: HomeView },
    { path: "/create", name: 'create', component: CreatePostView },
    { path: "/login", name: 'login', component: LoginView },
    { path: "/register", name: 'register', component: RegisterView },
  ],
});

export default router;
