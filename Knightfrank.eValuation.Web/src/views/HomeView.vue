<script setup lang="ts">
  import Search from '@/components/Search.vue'
  import Result from '@/components/Result.vue'

</script>

<template>
  <main class="property-search-page">
    <div class="page-header">
      <h1 style="color: #085054;">{{ $t('text.title') }}</h1>
      <p>{{ $t('text.description') }}</p>
      <hr />
    </div>
    <div class="page-container">
      <Search class="page-search" />
      <Result class="page-result" />
    </div>
  </main>
</template>

<style scoped>

  .page-header {
    margin-bottom: 30px;
  }

    .page-header h1 {
      font-size: 2.5rem;
      color: #333;
      margin-bottom: 8px;
    }

    .page-header p {
      font-size: 1.1rem;
      color: #666;
    }

  .page-container {
    width: 100%;
    display: flex;
    justify-content: space-between;
  }

  .page-search {
    width: 48%;
  }

  .page-result {
    width: 48%;
  }

  @media (max-width: 768px) {
    .property-search-page {
      padding: 16px;
    }

    .page-header h1 {
      font-size: 2rem;
    }

    .page-header p {
      font-size: 1rem;
    }

    .page-container {
      width: 100%;
      display: flex;
      flex-direction: column;
    }

    .page-search {
      width: 100%;
    }

    .page-result {
      width: 100%;
    }
  }
</style>
