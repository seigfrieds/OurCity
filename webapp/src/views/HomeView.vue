<script setup lang="ts">
import PageHeader from "@/components/PageHeader.vue";
import PostList from "@/components/PostList.vue";
import WipMessage from "@/components/WipMessage.vue";
import posts from "@/data/mockPosts";
</script>

<template>
  <div class="home-view">
    <PageHeader />
    <div class="content">
      <WipMessage
        message="The Dashboard page is currently a work in progress"
        description="The 'Upvote' and 'Downvote' buttons will not trigger a vote number change yet"
      />
      <PostList :posts="posts" />
    </div>
  </div>
</template>

<style scoped></style>
