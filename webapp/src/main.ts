import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import PrimeVue from "primevue/config";
import "primeicons/primeicons.css";
import Aura from "@primeuix/themes/aura";
import "./assets/styles/variables.css";
import { ToastService } from "primevue";

const app = createApp(App);

app.use(router);
app.use(ToastService);

//setup primevue
app.use(PrimeVue, {
  theme: {
    preset: Aura,
  },
});

app.mount("#app");
