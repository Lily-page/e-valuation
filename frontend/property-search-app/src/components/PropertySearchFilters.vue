<template>
  <div class="search-filters">
    <div class="filters-container">
      <div class="filter-group">
        <label for="region">Region:</label>
        <select 
          id="region" 
          v-model="propertyStore.selectedRegion"
          @change="onFilterChange"
        >
          <option value="">All Regions</option>
          <option 
            v-for="region in propertyStore.filterOptions?.regions" 
            :key="region" 
            :value="region"
          >
            {{ region }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="district">District:</label>
        <select 
          id="district" 
          v-model="propertyStore.selectedDistrict"
          @change="onFilterChange"
        >
          <option value="">All Districts</option>
          <option 
            v-for="district in propertyStore.filterOptions?.districts" 
            :key="district" 
            :value="district"
          >
            {{ district }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="propertyType">Property Type:</label>
        <select 
          id="propertyType" 
          v-model="propertyStore.selectedPropertyType"
          @change="onFilterChange"
        >
          <option value="">All Types</option>
          <option 
            v-for="type in propertyStore.filterOptions?.propertyTypes" 
            :key="type" 
            :value="type"
          >
            {{ type }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label for="priceRange">Sale Price:</label>
        <select 
          id="priceRange" 
          @change="onPriceRangeChange"
        >
          <option value="">All Prices</option>
          <option 
            v-for="range in propertyStore.filterOptions?.priceRanges" 
            :key="range.label" 
            :value="JSON.stringify({ min: range.min, max: range.max })"
          >
            {{ range.label }}
          </option>
        </select>
      </div>

      <div class="filter-actions">
        <button 
          @click="onSearch" 
          class="search-btn"
          :disabled="propertyStore.loading"
        >
          {{ propertyStore.loading ? 'Searching...' : 'Search' }}
        </button>
        <button 
          @click="onReset" 
          class="reset-btn"
          :disabled="propertyStore.loading"
        >
          Reset
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { usePropertyStore } from '@/stores/counter'

const propertyStore = usePropertyStore()

function onFilterChange() {
  propertyStore.currentPage = 1
}

function onPriceRangeChange(event: Event) {
  const target = event.target as HTMLSelectElement
  if (target.value) {
    const priceRange = JSON.parse(target.value)
    propertyStore.setPriceRange(priceRange)
  } else {
    propertyStore.setPriceRange({})
  }
}

function onSearch() {
  propertyStore.searchProperties()
}

function onReset() {
  propertyStore.resetFilters()
  propertyStore.searchProperties()
}
</script>

<style scoped>
.search-filters {
  background: #f8f9fa;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.filters-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 15px;
  align-items: end;
}

.filter-group {
  display: flex;
  flex-direction: column;
}

.filter-group label {
  font-weight: 600;
  margin-bottom: 5px;
  color: #333;
}

.filter-group select {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  background: white;
}

  .filter-group select:focus {
    outline: none;
    border-color: #ed1944;
    box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
  }

.filter-actions {
  display: flex;
  gap: 10px;
}

  .search-btn, .reset-btn {
    flex: 1;
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.2s;
  }

  .search-btn {
    background: #ed1944;
    color: white;
  }

    .search-btn:hover:not(:disabled) {
      background: #d0103a;
    }

.search-btn:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

.reset-btn {
  background: #6c757d;
  color: white;
}

.reset-btn:hover:not(:disabled) {
  background: #545b62;
}

.reset-btn:disabled {
  background: #adb5bd;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .filters-container {
    grid-template-columns: 1fr;
  }

  .filter-actions {
    display: flex;
    justify-content: stretch;
    gap:10px;
  }
  
  .search-btn, .reset-btn {
    flex: 1;
  }
}
</style>
