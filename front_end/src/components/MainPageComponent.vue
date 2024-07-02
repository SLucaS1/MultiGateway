<template>
    <div class="main-page">
      <p>Programma di gataway tra vari protocolli, selezionare il protocollo per configurare</p>
    </div>

    <Fieldset legend="Header" :toggleable="true">
    <p class="m-0">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo
        consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
    </p>
</Fieldset>



  </template>
  
  <script>
import Fieldset from 'primevue/fieldset';
  export default {
    name: 'MainPageComponent',
    components: {
        Fieldset
    }
  }
  </script>
  
  <style scoped>
  .main-page {
    flex-grow: 1;
    padding: 20px;
  }
  </style>
  