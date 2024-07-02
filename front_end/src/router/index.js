import { createRouter, createWebHistory } from 'vue-router'
//import HomeView from '../views/HomeView.vue'
import MainPageComponent from '../components/MainPageComponent.vue'
import OPC_ClientComponent from '../components/Pages/OPC_ClientComponent.vue'
import OPC_SereverComponent from '../components/Pages/OPC_ServerComponent.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: MainPageComponent
    },
    {
      path: '/OPC_Client',
      name: 'opc_client',
      component: OPC_ClientComponent
    },
    {
      path: '/OPC_Server',
      name: 'opc_server',
      component: OPC_SereverComponent
    }
  ]
})

export default router
