<template>
  <div class="menu">



    <div class="button-list">
      <button type="button" class="btn btn-primary btn-sm" v-on:click="handleButtonClick('/')">
        Home
      </button>
      <button type="button" class="btn btn-primary btn-sm" v-on:click="handleButtonClick('/opc_client')">
        OPC Client
      </button>
      <button type="button" class="btn btn-primary btn-sm" @click="$router.push('/opc_server')">OPC Server</button>
      <button type="button" class="btn btn-primary btn-sm" disabled>Bodbus Client</button>
      <button type="button" class="btn btn-primary btn-sm" disabled>Modbus Server</button>
      <button type="button" class="btn btn-primary btn-sm" disabled>TAG</button>
    </div>


  </div>
</template>

<script>


export default {

  methods: {
    handleButtonClick(path) {
      this.$router.push(path)
    },
    computed: {
      currentRouteName() {
        return this.$route.name;
      }
    }
  }
}
</script>

<style scoped>
.menu {
  width: 100%;
  height: 100%;
  background-color: #a3c0cf;
  padding: 10px;
}

.button-list {
  display: flex;
  flex-direction: column;
  align-items: center;
  /* Centra i pulsanti orizzontalmente (opzionale) */
}

.button-list button {
  margin: 1px 0;
  width: 90%;

}
</style>