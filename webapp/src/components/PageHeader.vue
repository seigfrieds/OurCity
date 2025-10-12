<script lang="ts">
import { defineComponent } from "vue";
import Menubar from "primevue/menubar";
import InputText from "primevue/inputtext";

type MenuItem = {
  label: string;
  icon: string;
  command: () => void;
};

export default defineComponent({
  name: "PageHeader",
  components: {
    Menubar,
    InputText,
  },
  data() {
    return {
      searchQuery: "",
    };
  },
  computed: {
    items(): MenuItem[] {
      return [
        {
          label: "Create",
          icon: "pi pi-plus",
          command: () => {
            this.$router.push("/create");
          },
        },
        {
          label: "Log In",
          icon: "pi pi-sign-in",
          command: () => {
            this.$router.push("/login");
          },
        },
      ];
    },
  },
  methods: {
    goToHome(): void {
      this.$router.push("/");
    },
  },
});
</script>

<template>
  <header class="app-header">
    <Menubar :model="items" class="app-menubar">
      <template #start>
        <h1 class="app-title" @click="goToHome">
          <i class="pi pi-map-marker title-icon"></i>
          OurCity
        </h1>
        <div class="search-container">
          <span class="p-input-icon-left">
            <i class="pi pi-search" />
            <InputText v-model="searchQuery" placeholder="Search..." class="search-input" />
          </span>
        </div>
      </template>
    </Menubar>
  </header>
</template>

<style scoped>
.app-header {
  background-color: var(--primary-color);
  color: var(--surface-color);
  padding: 0;
}

.app-title {
  margin: 0 1.5rem 0 0;
  font-size: 1.5rem;
  font-weight: bold;
  color: var(--surface-color);
  align-self: center;
  cursor: pointer;
  user-select: none;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.title-icon {
  font-size: 1.2rem;
}

.app-menubar {
  background: transparent;
  border: none;
  color: var(--surface-color);
  display: flex;
  justify-content: space-between;
}

.search-container {
  display: flex;
  justify-content: center;
  flex: 1;
  margin: 0 2rem;
}

.search-input {
  width: 30rem;
  max-width: 100%;
  margin-left: 0.5rem;
}

.app-menubar :deep(.p-inputtext) {
  background-color: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: var(--surface-color);
}

.app-menubar :deep(.p-inputtext:hover),
.app-menubar :deep(.p-inputtext:focus) {
  border-color: rgba(255, 255, 255, 0.4);
  box-shadow: none;
}

.app-menubar :deep(.p-inputtext::placeholder) {
  color: rgba(255, 255, 255, 0.7);
}

.app-menubar :deep(.pi-search) {
  color: rgba(255, 255, 255, 0.7);
}
</style>
