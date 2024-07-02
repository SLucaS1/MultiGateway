import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import lara from '@primevue/themes/lara';
import nora from '@primevue/themes/nora';

import 'primeicons/primeicons.css';

//import Noir from './presets/Noir.js';

// Import Bootstrap and BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.min.css'



const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Aura,
        options: {
            prefix: 'p',
            darkModeSelector: 'system',
            cssLayer: false
        }
    }
 });


app.use(router)

app.mount('#app')


