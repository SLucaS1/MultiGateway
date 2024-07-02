<template>
  <div id="app">

    <div class="header">
      <TitleComponent />
    </div>
    <div class="Menu">
      <MenuComponent />
    </div>
    <div class="Body">

      <RouterView />
    </div>

    <!-- <TitleComponent />
    <div class="content">
      <MenuComponent />
      <MainPageComponent /> -->

    <!-- <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav> -->
    <!-- </div> -->
  </div>
</template>

<script>
//import { RouterLink, RouterView } from 'vue-router'
import TitleComponent from './components/TitleComponent.vue'
import MenuComponent from './components/MenuComponent.vue'
import MainPageComponent from './components/MainPageComponent.vue'
import { RouterView } from 'vue-router'

export default {
  name: 'App',
  components: {
    TitleComponent,
    MenuComponent,
    MainPageComponent
  }
}
</script>

<style>
:root {
  --header-height: 75px;
  --menu-width: 150px;
}


.header {
  width: 100%;
  height: var(--header-height);
  position: absolute;
  top: 0;
  left: 0;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15), inset 0 -1px 0 rgba(255, 255, 255, 0.15);
}

.Menu {
  width: var(--menu-width);
  height: calc(100vh - var(--header-height));
  position: absolute;
  top: var(--header-height);
  left: 0;
}

.Body {
  width: calc(100% - var(--menu-width));
  height: calc(100vh - var(--header-height));
  position: absolute;
  top: var(--header-height);
  left: var(--menu-width);
}
</style>
