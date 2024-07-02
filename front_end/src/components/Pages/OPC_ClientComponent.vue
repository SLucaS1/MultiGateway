<template>
  <div class="main-page">
    <p>OPC Client</p>
    <Fieldset legend="OPC Client" :toggleable="true">
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

    <Fieldset legend="Tags" :toggleable="true" class="fieldset">

      <DataTable :value="tags" tableStyle="min-width: 50rem" class="tablegrid">
        <Column field="name" header="Tag" style="width: 25%"></Column>
        <Column field="value" header="Value" style="width: 10%"></Column>
        <Column field="quality" header="Quality" style="width: 10%"></Column>
        <Column field="counterRead" header="CounterRead"></Column>
        <Column field="counterWrite" header="CounterWrite"></Column>
        <Column field="updateRate" header="UpdateRate"></Column>
        <Column field="lastUpdatedTime" header="LastUpdatedTime"></Column>


        <Column style="width: 10rem">
        <template #body="{ data, frozenRow, index }">
          <div class="flex flex-wrap gap-2">
            <Button type="button" icon="pi pi-trash" severity="danger" size="small"  text raised @click="deleteTag(data, frozenRow, index)"/>
          </div>
        </template>
    </Column>
      </DataTable>
    </Fieldset>

    <Fieldset legend="Tree" :toggleable="true" class="treegrid">
      <TreeTable :value="tree" tableStyle="min-width: 50rem">
        <Column field="displayName" header="DisplayName" expander style="width: 30%"></Column>
        <Column field="browseName" header="BrowseName" ></Column>
        <Column field="nodeClass" header="NodeClass" ></Column>
        <Column field="nodeId" header="NodeId" ></Column>
        <Column field="identifier" header="Identifier" ></Column>
        <Column field="namespaceIndex" header="NamespaceIndex" ></Column>
        <Column field="serverIndex" header="ServerIndex" ></Column>
      </TreeTable>


    </Fieldset>
  </div>


</template>

<script>

import Fieldset from 'primevue/fieldset';
import TreeTable from 'primevue/treetable';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';

import { PrimeIcons } from '@primevue/core/api';


export default {
  name: 'OPC_ClientComponent',
  components: {
    Fieldset,
    TreeTable,
    Column,
    DataTable,
    Button
  },
  data() {
    return {
      status: null,
      tags: null,
      tree: null
    };
  },
  mounted() {


    this.intervalId1 = setInterval(() => {
      this.loadStatus();
    }, 10000); 

    this.intervalId2 = setInterval(() => {
      this.loadTags();
    }, 2000); 

    this.loadStatus();
    this.loadTags();
    this.loadTree()


  },
  beforeUnmount() {
    clearInterval(this.intervalId1); 
    clearInterval(this.intervalId2); 
  },
  methods: {

    loadStatus() {
      fetch('/OPC_Client/status')
        .then(response => response.json())
        .then((data) => {
          this.status = data;
        })
        .catch(
          error => {
            console.error('Errore durante il caricamento del JSON:', error)
          }
        );
    },
    loadTags() {
      fetch('/OPC_Client/tags')
        .then(response => response.json())
        .then((data) => {
          this.tags = data;
        })
        .catch(
          error => {
            console.error('Errore durante il caricamento del JSON:', error)
          }
        );
    },
    loadTree() {
      fetch('/OPC_Client/tree')
        .then(response => response.json())
        .then((data) => {
          this.tree = data;
        })
        .catch(
          error => {
            console.error('Errore durante il caricamento del JSON:', error)
          }
        );
    },
    deleteTag(data, frozenRow, index)
    {

      console.log(data)
    }
  }
}
</script>

<style scoped>
.main-page {
  flex-grow: 1;
  padding: 20px;
}

.treegrid{
  --p-treetable-body-cell-padding:0px;
}

.tablegrid{
  --p-datatable-body-cell-padding:0px;
}

.fieldset{
  --p-fieldset-legend-padding: 0px;
}

</style>