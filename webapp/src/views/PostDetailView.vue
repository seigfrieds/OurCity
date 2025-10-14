<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import PageHeader from "@/components/PageHeader.vue";
import ImageModal from "@/components/ImageModal.vue";
import VoteBox from "@/components/VoteBox.vue";
import CommentInput from "@/components/CommentInput.vue";
import CommentList from "@/components/CommentList.vue";
import type { PostProps, CommentProps } from "@/types/interfaces";

const posts = [
  {
    id: 1,
    author: "a_real_prof",
    title: "A beautiful day in the park",
    location: "St. Vital South",
    description:
      "I spent the afternoon walking through the park, enjoying the beautiful weather. The leaves are just starting to change color, and the air is crisp and cool.",
    votes: 7,
    imageUrl:
      "https://images.unsplash.com/photo-1503376780353-7e6692767b70?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
    comments: [],
  },
  {
    id: 2,
    author: "PoStoreGangEyeSayShun",
    title: "Exploring the city streets",
    location: "Tuxedo",
    description:
      "I love the energy of the city. There is always something new to see and do. I could spend hours just walking around and exploring.",
    votes: 21,
    imageUrl:
      "https://images.unsplash.com/photo-1759167317838-4e77e12621cf?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=744",
    comments: [
      {
        id: 1,
        author: "ViewCompOwnItLieBrrrAiry",
        text: "I haven't seen much honestly, where could I go to find some good stuff to do?",
        votes: 12,
        replies: [
          {
            id: 7,
            author: "UnemployedComputerScientist",
            text: "Have you been to the Forks?",
            replies: [],
          },
          {
            id: 8,
            author: "Shopoholic",
            text: "theres always stuff to do at the outlet mall",
            replies: [],
          },
        ],
      },
      {
        id: 2,
        author: "RealMichaelJordanProbably",
        text: "If you're looking for some recommendations on things to do, I'd highly suggest going to a Sea Bears game.",
        votes: 8,
        replies: [],
      },
      {
        id: 3,
        author: "tacobellnachofries",
        text: "check out Pineridge Hollow! It's a little outside the city but it's so much fun",
        votes: 4,
        replies: [],
      },
      {
        id: 4,
        author: "kingoftouchinggrass",
        text: "bruh did anyone else go to nuit blanche? that was wild",
        votes: 3,
        replies: [],
      },
      {
        id: 5,
        author: "ILoveGambling21",
        text: "the casino is pretty great too",
        votes: 1,
        replies: [],
      },
      {
        id: 6,
        author: "YoungSheldonBazinga",
        text: "I have more fun watching young sheldon at home.",
        votes: 1,
        replies: [],
      },
    ],
  },
];

const route = useRoute();
const postId = route.params.id;
const post = ref<PostProps | undefined>(undefined);
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
</script>

<!-- Layout for post details page is based off of Reddit -->
<template>
  <PageHeader />
  <div class="post-detail-view">
    <h1 class="post-title">{{ post?.title }}</h1>
    <div class="post-author">By @{{ post?.author }}</div>

    <div v-if="post?.location" class="post-location">
      <div class="location-icon">
        <i class="pi pi-map-marker"></i>
      </div>
      <span>{{ post.location }}</span>
    </div>

    <div v-if="post?.imageUrl" class="post-image-wrapper">
      <div class="image-hover-wrapper">
        <img :src="post.imageUrl" :alt="post.title" class="post-image" @click="openImageModal()" />
        <div class="image-zoom-icon">
          <i class="pi pi-arrow-up-right-and-arrow-down-left-from-center"></i>
        </div>
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
      :imageUrl="post?.imageUrl"
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
.post-author {
  font-size: 1rem;
  color: var(--text-color);
}
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
}
.image-hover-wrapper {
  position: relative;
  display: inline-block;
  cursor: pointer;
}
.image-hover-wrapper:hover .image-zoom-icon {
  opacity: 1;
}
.post-image {
  width: 100%;
  aspect-ratio: 16 / 9;
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
