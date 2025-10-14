<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import PostList from "@/components/PostList.vue";

const posts = [
  {
    id: 1,
    title: "A beautiful day in the park",
    location: "St. Vital South",
    description:
      "I spent the afternoon walking through the park, enjoying the beautiful weather. The leaves are just starting to change color, and the air is crisp and cool.",
    imageUrl:
      "https://images.unsplash.com/photo-1533105079780-52b9be462077?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
  },
  {
    id: 2,
    title: "Exploring the city streets",
    location: "Tuxedo",
    description:
      "I love the energy of the city. There is always something new to see and do. I could spend hours just walking around and exploring.",
    imageUrl:
      "https://images.unsplash.com/photo-1503376780353-7e6692767b70?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
  },
];
</script>

<template>
  <div class="home-view">
    <PageHeader />
    <div class="content">
      <PostList :posts="posts" />
    </div>
  </div>
</template>

<style scoped></style>
