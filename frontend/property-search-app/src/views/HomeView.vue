<script setup lang="ts">
import { onMounted } from 'vue'
import { usePropertyStore } from '@/stores/counter'
import PropertySearchFilters from '@/components/PropertySearchFilters.vue'
import PropertyGrid from '@/components/PropertyGrid.vue'

const propertyStore = usePropertyStore()

onMounted(async () => {
  await propertyStore.loadFilterOptions()
  await propertyStore.searchProperties()
})
</script>

<template>
  <main class="property-search-page">
    <div class="page-header">
      <h1>Property Search</h1>
      <p>Find your perfect property with our advanced search filters</p>
    </div>
    
    <PropertySearchFilters />
    <PropertyGrid />
  </main>
</template>

<style scoped>
.property-search-page {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.page-header {
  text-align: center;
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
  margin: 0;
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
}
</style>
