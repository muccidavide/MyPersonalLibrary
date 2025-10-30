import { createRouter, createWebHistory} from 'vue-router';
import Home from '../views/HomeView.vue'

const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/dashboard', name: 'Dashboard', component: () => import('../views/DashboardView.vue') }, 
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router