<template>
  <div class="property-grid-container">
    <!-- Loading State -->
    <div v-if="propertyStore.loading" class="loading-state">
      <div class="loading-spinner"></div>
      <p>Loading properties...</p>
    </div>

    <!-- Error State -->
    <div v-else-if="propertyStore.error" class="error-state">
      <p>{{ propertyStore.error }}</p>
      <button @click="propertyStore.searchProperties()" class="retry-btn">
        Try Again
      </button>
    </div>

    <!-- Empty State -->
    <div v-else-if="propertyStore.properties.length === 0" class="empty-state">
      <div class="empty-icon">🏠</div>
      <h3>No properties found</h3>
      <p>Try adjusting your search filters to find more properties.</p>
    </div>

    <!-- Properties Grid -->
    <div v-else>
      <!-- Results Summary -->
      <div class="results-summary">
        <p>
          Showing {{ (propertyStore.currentPage - 1) * propertyStore.pageSize + 1 }} - 
          {{ Math.min(propertyStore.currentPage * propertyStore.pageSize, propertyStore.totalCount) }} 
          of {{ propertyStore.totalCount }} properties
        </p>
      </div>

      <!-- Property Grid -->
      <div class="property-grid">
        <PropertyCard 
          v-for="property in propertyStore.properties" 
          :key="property.id"
          :property="property"
        />
      </div>

      <!-- Pagination -->
      <div class="pagination" v-if="propertyStore.totalPages > 1">
        <button 
          @click="propertyStore.previousPage()"
          :disabled="!propertyStore.hasPreviousPage"
          class="pagination-btn"
        >
         < Previous
        </button>

        <div class="page-numbers">
          <button
            v-for="page in visiblePages"
            :key="page"
            @click="propertyStore.setPage(page)"
            :class="['page-btn', { active: page === propertyStore.currentPage }]"
          >
            {{ page }}
          </button>
        </div>

        <button 
          @click="propertyStore.nextPage()"
          :disabled="!propertyStore.hasNextPage"
          class="pagination-btn"
        >
          Next >
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { usePropertyStore } from '@/stores/counter'
import PropertyCard from './PropertyCard.vue'

const propertyStore = usePropertyStore()

const visiblePages = computed(() => {
  const current = propertyStore.currentPage
  const total = propertyStore.totalPages
  const delta = 2 // Number of pages to show on each side of current page
  
  let start = Math.max(1, current - delta)
  let end = Math.min(total, current + delta)
  
  // Adjust if we're near the beginning or end
  if (end - start < delta * 2) {
    if (start === 1) {
      end = Math.min(total, start + delta * 2)
    } else {
      start = Math.max(1, end - delta * 2)
    }
  }
  
  const pages = []
  for (let i = start; i <= end; i++) {
    pages.push(i)
  }
  
  return pages
})
</script>

<style scoped>
.property-grid-container {
  min-height: 400px;
}

.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  color: #666;
}

  .loading-spinner {
    width: 40px;
    height: 40px;
    border: 4px solid #f3f3f3;
    border-top: 4px solid #ed1944;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 16px;
  }

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  color: #dc3545;
}

  .retry-btn {
    margin-top: 16px;
    padding: 8px 16px;
    background: #ed1944;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

.retry-btn:hover {
  background: #0056b3;
}

.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  color: #666;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 16px;
}

.empty-state h3 {
  margin: 0 0 8px 0;
  color: #333;
}

.results-summary {
  margin-bottom: 20px;
  color: #666;
  font-size: 14px;
}

.property-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  margin-top: 40px;
}

.pagination-btn {
  padding: 8px 16px;
  border: 1px solid #ddd;
  background: white;
  color: #333;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s;
}

  .pagination-btn:hover:not(:disabled) {
    background: #f8f9fa;
    border-color: #ed1944;
  }

.pagination-btn:disabled {
  background: #f8f9fa;
  color: #6c757d;
  cursor: not-allowed;
  border-color: #dee2e6;
}

.page-numbers {
  display: flex;
  gap: 4px;
}

.page-btn {
  width: 40px;
  height: 40px;
  border: 1px solid #ddd;
  background: white;
  color: #333;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

  .page-btn:hover {
    background: #f8f9fa;
    border-color: #ed1944;
  }

  .page-btn.active {
    background: #ed1944;
    color: white;
    border-color: #ed1944;
  }

@media (max-width: 768px) {
  .property-grid {
    grid-template-columns: 1fr;
    gap: 16px;
  }
  
  .pagination {
    flex-wrap: wrap;
    gap: 4px;
  }
  
  .pagination-btn {
    padding: 6px 12px;
    font-size: 14px;
  }
  
  .page-btn {
    width: 36px;
    height: 36px;
  }
}

@media (max-width: 480px) {
  .page-numbers {
    order: 3;
    width: 100%;
    justify-content: center;
  }
}
</style>
