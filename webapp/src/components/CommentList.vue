<script setup lang="ts">
import CommentItem from "./CommentItem.vue";
import type { CommentProps } from "@/types/interfaces";

defineProps<{ comments: CommentProps[] }>();
</script>

<template>
  <div class="comments-list">
    <CommentItem
      v-for="comment in comments"
      :key="comment.id"
      :id="comment.id"
      :author="comment.author"
      :content="comment.content"
      :votes="comment.votes"
      :replies="comment.replies"
    />
  </div>
</template>

<style scoped>
.comments-list {
  margin-top: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
</style>
