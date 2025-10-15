<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import PageHeader from "@/components/PageHeader.vue";
import ImageModal from "@/components/ImageModal.vue";
import VoteBox from "@/components/VoteBox.vue";
import CommentInput from "@/components/CommentInput.vue";
import CommentList from "@/components/CommentList.vue";
import type { PostProps } from "@/types/interfaces";
import posts from "@/data/mockPosts";

const route = useRoute();
const router = useRouter();
const postId = route.params.id;
const post = ref<PostProps | undefined>(undefined);
const currentImageIndex = ref(0);
const showImageModal = ref(false);
const commentText = ref("");

onMounted(() => {
  post.value = posts.find((p) => p.id == Number(postId));
});

function openImageModal() {
  showImageModal.value = true;
}

function closeImageModal() {
  showImageModal.value = false;
}

// added edit/delete handlers
function editPost() {
  // navigate to the edit page, passing the post id as a query parameter
  const id = post?.value?.id;
  if (id == null) return;
  router.push({ name: "edit", query: { id: String(id) } });
}

function deletePost() {
  if (confirm("Are you sure you want to delete this post?")) {
    console.log("Delete post", post?.value?.id);
    // placeholder: perform delete and navigate away
  }
}
</script>

<!-- Layout for post details page is based off of Reddit -->
<template>
  <PageHeader />
  <div class="post-detail-view">
    <h1 class="post-title">{{ post?.title }}</h1>

    <!-- replaced single author line with meta row that contains author on the left
         and vertically stacked edit/delete buttons right-aligned -->
    <div class="post-meta">
      <div class="post-author">By @{{ post?.author }}</div>

      <div class="post-actions" v-if="post">
        <button class="post-action-btn edit-btn" @click="editPost" aria-label="Edit post">
          Edit
        </button>
        <button class="post-action-btn delete-btn" @click="deletePost" aria-label="Delete post">
          Delete
        </button>
      </div>
    </div>

    <div v-if="post?.location" class="post-location">
      <div class="location-icon">
        <i class="pi pi-map-marker"></i>
      </div>
      <span>{{ post.location }}</span>
    </div>

    <div v-if="post?.imageUrls && post.imageUrls.length" class="post-image-wrapper">
      <div class="image-hover-wrapper">
        <img
          :src="post.imageUrls[currentImageIndex]"
          :alt="post.title"
          class="post-image"
          @click="openImageModal()"
        />
        <div class="image-zoom-icon">
          <i class="pi pi-arrow-up-right-and-arrow-down-left-from-center"></i>
        </div>
        <button
          v-if="currentImageIndex > 0"
          class="image-btn image-prev"
          @click="currentImageIndex--"
          aria-label="Previous image"
        >
          <i class="pi pi-chevron-left"></i>
        </button>
        <button
          v-if="post.imageUrls && currentImageIndex < post.imageUrls.length - 1"
          class="image-btn image-next"
          @click="currentImageIndex++"
          aria-label="Next image"
        >
          <i class="pi pi-chevron-right"></i>
        </button>
      </div>
    </div>

    <div class="post-description">
      <p>{{ post?.description }}</p>
    </div>

    <VoteBox class="post-votes" :votes="post?.votes ?? 0" />

    <div>
      <CommentInput @submit="(text: string) => (commentText = text)" />
      <div v-if="post?.comments && post.comments.length > 0" class="content">
        <CommentList :comments="post.comments" />
      </div>
      <div v-else class="no-comments">Start a discussion!</div>
    </div>

    <ImageModal
      :show="showImageModal"
      :imageUrl="post?.imageUrls && post.imageUrls[currentImageIndex]"
      :title="post?.title"
      @close="closeImageModal"
    />
  </div>
</template>

<style scoped>
.post-detail-view {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
  max-width: 50rem;
  margin: 2rem auto;
}
.post-title {
  font-size: 2.5rem;
  font-weight: bold;
  text-align: left;
  color: var(--surface-color);
}

/* new: layout for author + actions row */
.post-meta {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
  width: 100%;
}
.post-author {
  font-size: 1rem;
  color: var(--text-color);
}

/* vertical action buttons, right aligned */
.post-actions {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  align-items: flex-end;
}
.post-action-btn {
  padding: 0.35rem 0.75rem;
  border-radius: 0.5rem;
  font-size: 0.9rem;
  border: none;
  cursor: pointer;
}
.post-action-btn.edit-btn {
  background: var(--surface-variant);
  color: var(--text-color);
  transition:
    transform 0.12s ease,
    box-shadow 0.12s ease,
    filter 0.12s ease;
}
.post-action-btn.edit-btn:hover,
.post-action-btn.edit-btn:focus,
.post-action-btn.edit-btn:focus-visible {
  transform: translateY(-2px);
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.12);
  filter: brightness(0.98);
  outline: none;
}
.post-action-btn.delete-btn {
  background: var(--danger-color);
  color: white;
  transition:
    transform 0.12s ease,
    box-shadow 0.12s ease,
    filter 0.12s ease;
}
.post-action-btn.delete-btn:hover,
.post-action-btn.delete-btn:focus,
.post-action-btn.delete-btn:focus-visible {
  transform: translateY(-2px);
  box-shadow: 0 6px 18px rgba(255, 77, 79, 0.18);
  filter: brightness(0.95);
  outline: none;
}

/* ...existing code... */
.post-location {
  display: flex;
  align-items: center;
  font-size: 1rem;
  color: var(--text-color);
}
.location-icon {
  margin-right: 0.25rem;
}
.post-description {
  font-size: 1.25rem;
  text-align: left;
  color: var(--text-color);
}
.post-image-wrapper {
  display: flex;
  margin-bottom: 1rem;
  justify-content: center;
  width: 100%;
  height: 30rem;
  max-width: 100%;
  position: relative;
}
.image-hover-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  cursor: pointer;
}
.image-hover-wrapper:hover .image-zoom-icon {
  opacity: 1;
}
.post-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 2rem;
  display: block;
}
.image-zoom-icon {
  position: absolute;
  right: 1rem;
  bottom: 1rem;
  font-size: 1.5rem;
  color: var(--text-color);
  background: rgba(0, 0, 0, 0.4);
  border-radius: 50%;
  width: 2.5rem;
  height: 2.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.2s;
}

.image-btn {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background: rgba(0, 0, 0, 0.4);
  border: none;
  border-radius: 50%;
  width: 2.5rem;
  height: 2.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--text-color);
  cursor: pointer;
  z-index: 2;
  opacity: 0;
  pointer-events: none;
  transition: opacity 0.2s;
}
.image-hover-wrapper:hover .image-btn {
  opacity: 1;
  pointer-events: auto;
}
.image-prev {
  left: 1rem;
}
.image-next {
  right: 1rem;
}
.no-comments {
  margin: 1.5rem 0;
  padding: 1rem;
  text-align: center;
  color: var(--text-color);
  border-radius: 1rem;
  font-size: 1.5rem;
  font-weight: 500;
}
</style>
