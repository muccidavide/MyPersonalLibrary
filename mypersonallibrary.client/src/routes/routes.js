import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/HomeView.vue'
import { jwtDecode } from 'jwt-decode';
import rolesConfig from './roles.js';

const { ROLES } = rolesConfig;
const routes = [
  { path: '/', name: 'Home', component: Home },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: () => import('../views/DashboardView.vue'),
    meta: {
      requiresAuth: true,
      allowedRoles: [ROLES.ADMIN]
    }
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('accessToken');

  if (!to.meta.requiresAuth) {
    return next();
  }

  if (!token || token.split('.').length !== 3) {
    return next('/');
  }

  let decoded;
  try {
    decoded = jwtDecode(token);
  } catch {
    console.error('Invalid token');
    return next('/');
  }
  const allowedRoles = to.meta.allowedRoles || [];

  if (allowedRoles.includes(decoded.role)) {
    return next();
  }

  return next('/');
});

export default router