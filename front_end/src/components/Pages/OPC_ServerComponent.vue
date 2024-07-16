<template>
  <div class="main-page">
    <Fieldset legend="OPC Server" :toggleable="true">
      <div class="container">
        <div class="row">
          <div class="col-sm-2">
            <b>Status:</b>
          </div>
          <div class="col-sm-2">
            <div v-if="status">{{ status.status ?? 'Non disponibile' }}</div>
          </div>
          <div class="col-sm-2">
            <b>Address:</b>
          </div>
          <div class="col-sm">
            <div v-if="status">Address: {{ status.address ?? 'Non disponibile' }}</div>
          </div>
        </div>
      </div>
    </Fieldset>
  </div>
</template>

<script>

import Fieldset from 'primevue/fieldset';
import TreeTable from 'primevue/treetable';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';

export default {
  name: 'OPC_ServerComponent',
  components: {
    Fieldset,
    TreeTable,
    Column,
    DataTable,
    Button
  },
  data() {
    return {
      status: null
    };
  },
  mounted() {


    this.intervalId1 = setInterval(() => {
      this.loadStatus();
    }, 10000);

    this.intervalId2 = setInterval(() => {


    }, 2000);

    this.loadStatus();


  },
  beforeUnmount() {
    clearInterval(this.intervalId1);
    clearInterval(this.intervalId2);
  }, methods: {

    loadStatus() {
      fetch('/OPC_Server/status')
        .then(response => response.json())
        .then((data) => {
          this.status = data;
        })
        .catch(
          error => {
            console.error('Errore durante il caricamento del JSON:', error)
          }
        );

    }
  }
}
</script>



<style scoped>
.main-page {
  flex-grow: 1;
  padding: 20px;
}

.treegrid {
  --p-treetable-body-cell-padding: 0px;
}

.tablegrid {
  --p-datatable-body-cell-padding: 0px;
}

.fieldset {
  --p-fieldset-legend-padding: 0px;
}
</style>