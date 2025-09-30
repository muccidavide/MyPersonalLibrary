import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import 'primeicons/primeicons.css'                         // Icone 
import Aura from '@primeuix/themes/aura';
import 'bootstrap/dist/css/bootstrap.min.css'


const app = createApp(App)
app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});

app.mount('#app')