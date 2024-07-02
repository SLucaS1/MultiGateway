<template>
    <div class="title">   
      <h1>GatoWay</h1>
    </div>
    <div class="image">     
      <img src="/GattoWayMini.png" alt="Logo">
    </div>
  
  </template>
  
  <script>
  export default {
    name: 'TitleComponent'
  }
  </script>
  
  <style scoped>
  .title {
    text-align: center;
    background-color: #2896D1;
    padding: 10px;
    height: 100%;
  }

  .image {
    position: absolute;  
    top: 0; 
    left: 0; 
  }
  </style>
  