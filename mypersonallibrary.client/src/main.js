import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import 'bootstrap/dist/css/bootstrap.min.css'

var app = createApp(App)
app.use(PrimeVue).mount('#app');