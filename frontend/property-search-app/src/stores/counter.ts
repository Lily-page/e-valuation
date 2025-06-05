import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { Property, PropertySearchRequest, PagedResult, FilterOptions } from '@/types/property'
import { propertyService } from '@/services/propertyService'

export const usePropertyStore = defineStore('property', () => {
  const properties = ref<Property[]>([])
  const filterOptions = ref<FilterOptions | null>(null)
  const currentPage = ref(1)
  const pageSize = ref(9)
  const totalCount = ref(0)
  const totalPages = ref(0)
  const hasNextPage = ref(false)
  const hasPreviousPage = ref(false)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Search filters
  const selectedRegion = ref<string>('')
  const selectedDistrict = ref<string>('')
  const selectedPropertyType = ref<string>('')
  const selectedPriceRange = ref<{ min?: number; max?: number }>({})

  const searchRequest = computed((): PropertySearchRequest => ({
    region: selectedRegion.value || undefined,
    district: selectedDistrict.value || undefined,
    propertyType: selectedPropertyType.value || undefined,
    minPrice: selectedPriceRange.value.min,
    maxPrice: selectedPriceRange.value.max,
    page: currentPage.value,
    pageSize: pageSize.value
  }))

  async function loadFilterOptions() {
    try {
      loading.value = true
      error.value = null
      filterOptions.value = await propertyService.getFilterOptions()
    } catch (err) {
      error.value = 'Failed to load filter options'
      console.error('Error loading filter options:', err)
    } finally {
      loading.value = false
    }
  }

  async function searchProperties() {
    try {
      loading.value = true
      error.value = null
      const result: PagedResult<Property> = await propertyService.searchProperties(searchRequest.value)
      
      properties.value = result.data
      totalCount.value = result.totalCount
      totalPages.value = result.totalPages
      hasNextPage.value = result.hasNextPage
      hasPreviousPage.value = result.hasPreviousPage
    } catch (err) {
      error.value = 'Failed to search properties'
      console.error('Error searching properties:', err)
    } finally {
      loading.value = false
    }
  }

  function setPage(page: number) {
    currentPage.value = page
    searchProperties()
  }

  function nextPage() {
    if (hasNextPage.value) {
      setPage(currentPage.value + 1)
    }
  }

  function previousPage() {
    if (hasPreviousPage.value) {
      setPage(currentPage.value - 1)
    }
  }

  function resetFilters() {
    selectedRegion.value = ''
    selectedDistrict.value = ''
    selectedPropertyType.value = ''
    selectedPriceRange.value = {}
    currentPage.value = 1
  }

  function setPriceRange(priceRange: { min?: number; max?: number }) {
    selectedPriceRange.value = priceRange
    currentPage.value = 1
  }

  return {
    // State
    properties,
    filterOptions,
    currentPage,
    pageSize,
    totalCount,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    loading,
    error,
    selectedRegion,
    selectedDistrict,
    selectedPropertyType,
    selectedPriceRange,
    
    // Getters
    searchRequest,
    
    // Actions
    loadFilterOptions,
    searchProperties,
    setPage,
    nextPage,
    previousPage,
    resetFilters,
    setPriceRange
  }
})
