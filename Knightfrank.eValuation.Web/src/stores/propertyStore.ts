import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import propertyService, { type PropertySearchRequest, type ValuationResponse } from '@/services/propertyService'
import tokenService from '@/services/tokenService'

export const usePropertyStore = defineStore('property', () => {
  // State
  const searchForm = ref<PropertySearchRequest>({
    zone: '',
    district: '',
    estateName: '',
    block: '',
    floor: '',
    unit: ''
  })

  const valuation = ref<ValuationResponse | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Dropdown options
  const zones = ref<string[]>([])
  const districts = ref<string[]>([])
  const estateNames = ref<string[]>([])
  const blocks = ref<string[]>([])
  const floors = ref<string[]>([])
  const units = ref<string[]>([])

  // Token info
  const tokenInfo = ref<any>(null)
  const isTokenValid = ref(false)

  // Computed
  const hasValuation = computed(() => valuation.value !== null)
  const isFormValid = computed(() => {
    return !!(searchForm.value.zone &&
      searchForm.value.district &&
      searchForm.value.estateName)
  })

  // Actions
  async function initializeToken() {
    try {
      loading.value = true
      error.value = null

      await tokenService.ensureValidToken()
      isTokenValid.value = true

      const info = await tokenService.getTokenInfo()
      tokenInfo.value = info
    } catch (err: any) {
      error.value = err.message || 'Failed to initialize token'
      isTokenValid.value = false
    } finally {
      loading.value = false
    }
  }

  async function loadZones() {
    try {
      zones.value = await propertyService.getZones()
    } catch (err: any) {
      error.value = err.message || 'Failed to load zones'
    }
  }

  async function loadDistricts() {
    try {
      districts.value = await propertyService.getDistricts(searchForm.value.zone)
    } catch (err: any) {
      error.value = err.message || 'Failed to load districts'
    }
  }

  async function loadEstateNames() {
    try {
      estateNames.value = await propertyService.getEstateNames(
        searchForm.value.zone,
        searchForm.value.district
      )
    } catch (err: any) {
      error.value = err.message || 'Failed to load estate names'
    }
  }

  async function loadBlocks() {
    try {
      blocks.value = await propertyService.getBlocks(
        searchForm.value.zone,
        searchForm.value.district,
        searchForm.value.estateName
      )
    } catch (err: any) {
      error.value = err.message || 'Failed to load blocks'
    }
  }

  async function loadFloors() {
    try {
      floors.value = await propertyService.getFloors(
        searchForm.value.zone,
        searchForm.value.district,
        searchForm.value.estateName,
        searchForm.value.block
      )
    } catch (err: any) {
      error.value = err.message || 'Failed to load floors'
    }
  }

  async function loadUnits() {
    try {
      units.value = await propertyService.getUnits(
        searchForm.value.zone,
        searchForm.value.district,
        searchForm.value.estateName,
        searchForm.value.block,
        searchForm.value.floor
      )
    } catch (err: any) {
      error.value = err.message || 'Failed to load units'
    }
  }

  async function getValuation() {
    try {
      loading.value = true
      error.value = null

      const result = await propertyService.getValuation(searchForm.value)
      valuation.value = result
    } catch (err: any) {
      error.value = err.message || 'Failed to get valuation'
      valuation.value = null
    } finally {
      loading.value = false
    }
  }

  function resetForm() {
    searchForm.value = {
      zone: '',
      district: '',
      estateName: '',
      block: '',
      floor: '',
      unit: ''
    }
    valuation.value = null
    error.value = null

    // Clear dependent dropdowns
    districts.value = []
    estateNames.value = []
    blocks.value = []
    floors.value = []
    units.value = []
  }

  function clearError() {
    error.value = null
  }

  // Watchers for cascading dropdowns
  function onZoneChange() {
    searchForm.value.district = ''
    searchForm.value.estateName = ''
    searchForm.value.block = ''
    searchForm.value.floor = ''
    searchForm.value.unit = ''

    estateNames.value = []
    blocks.value = []
    floors.value = []
    units.value = []

    if (searchForm.value.zone) {
      loadDistricts()
    } else {
      districts.value = []
    }
  }

  function onDistrictChange() {
    searchForm.value.estateName = ''
    searchForm.value.block = ''
    searchForm.value.floor = ''
    searchForm.value.unit = ''

    blocks.value = []
    floors.value = []
    units.value = []

    if (searchForm.value.district) {
      loadEstateNames()
    } else {
      estateNames.value = []
    }
  }

  function onEstateNameChange() {
    searchForm.value.block = ''
    searchForm.value.floor = ''
    searchForm.value.unit = ''

    floors.value = []
    units.value = []

    if (searchForm.value.estateName) {
      loadBlocks()
    } else {
      blocks.value = []
    }
  }

  function onBlockChange() {
    searchForm.value.floor = ''
    searchForm.value.unit = ''

    units.value = []

    if (searchForm.value.block) {
      loadFloors()
    } else {
      floors.value = []
    }
  }

  function onFloorChange() {
    searchForm.value.unit = ''

    if (searchForm.value.floor) {
      loadUnits()
    } else {
      units.value = []
    }
  }

  return {
    // State
    searchForm,
    valuation,
    loading,
    error,
    zones,
    districts,
    estateNames,
    blocks,
    floors,
    units,
    tokenInfo,
    isTokenValid,

    // Computed
    hasValuation,
    isFormValid,

    // Actions
    initializeToken,
    loadZones,
    loadDistricts,
    loadEstateNames,
    loadBlocks,
    loadFloors,
    loadUnits,
    getValuation,
    resetForm,
    clearError,
    onZoneChange,
    onDistrictChange,
    onEstateNameChange,
    onBlockChange,
    onFloorChange
  }
})
