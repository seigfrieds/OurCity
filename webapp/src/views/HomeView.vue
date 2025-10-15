<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import PostList from "@/components/PostList.vue";
import posts from "@/data/mockPosts";
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
