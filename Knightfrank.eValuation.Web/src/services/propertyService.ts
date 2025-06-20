import apiClient from './api'
import tokenService from './tokenService'

export interface PropertySearchRequest {
  zone?: string
  district?: string
  estateName?: string
  block?: string
  floor?: string
  unit?: string
}

export interface Property {
  id: number
  zone: string
  district: string
  estateName: string
  block?: string
  floor?: string
  unit?: string
  grossFloorArea?: number
  saleableFloorArea?: number
  builtYear?: string
  valuations: Valuation[]
}

export interface Valuation {
  id: number
  propertyId: number
  estimatedValue: number
  gfaUnitRate?: number
  sfaUnitRate?: number
  valuationDate: string
  createdAt: string
  notes?: string
}

export interface ValuationResponse {
  estimatedValue: number
  grossFloorArea?: number
  saleableFloorArea?: number
  gfaUnitRate?: number
  sfaUnitRate?: number
  builtYear?: string
  valuationDate: string
  propertyDetails: string
}

class PropertyService {
  /**
   * Get property valuation
   */
  async getValuation(searchRequest: PropertySearchRequest): Promise<ValuationResponse> {
    try {
      // Ensure we have a valid token
      await tokenService.ensureValidToken()

      const response = await apiClient.post('/property/valuation', searchRequest)
      return response.data
    } catch (error: any) {
      console.error('Error getting valuation:', error)

      if (error.response?.status === 404) {
        throw new Error('No property found matching the search criteria')
      } else if (error.response?.status === 401) {
        throw new Error('Authentication failed. Please try again.')
      } else {
        throw new Error('Failed to get property valuation')
      }
    }
  }

  /**
   * Search properties
   */
  async searchProperties(searchRequest: PropertySearchRequest): Promise<Property[]> {
    try {
      // Ensure we have a valid token
      await tokenService.ensureValidToken()

      const response = await apiClient.post('/property/search', searchRequest)
      return response.data
    } catch (error: any) {
      console.error('Error searching properties:', error)

      if (error.response?.status === 401) {
        throw new Error('Authentication failed. Please try again.')
      } else {
        throw new Error('Failed to search properties')
      }
    }
  }

  /**
   * Get available zones
   */
  async getZones(): Promise<string[]> {
    try {
      const response = await apiClient.get('/property/zones')
      return response.data
    } catch (error) {
      console.error('Error getting zones:', error)
      throw new Error('Failed to get zones')
    }
  }

  /**
   * Get available districts
   */
  async getDistricts(zone?: string): Promise<string[]> {
    try {
      const params = zone ? { zone } : {}
      const response = await apiClient.get('/property/districts', { params })
      return response.data
    } catch (error) {
      console.error('Error getting districts:', error)
      throw new Error('Failed to get districts')
    }
  }

  /**
   * Get available estate names
   */
  async getEstateNames(zone?: string, district?: string): Promise<string[]> {
    try {
      const params: any = {}
      if (zone) params.zone = zone
      if (district) params.district = district

      const response = await apiClient.get('/property/estates', { params })
      return response.data
    } catch (error) {
      console.error('Error getting estate names:', error)
      throw new Error('Failed to get estate names')
    }
  }

  /**
   * Get available blocks
   */
  async getBlocks(zone?: string, district?: string, estateName?: string): Promise<string[]> {
    try {
      const params: any = {}
      if (zone) params.zone = zone
      if (district) params.district = district
      if (estateName) params.estateName = estateName

      const response = await apiClient.get('/property/blocks', { params })
      return response.data
    } catch (error) {
      console.error('Error getting blocks:', error)
      throw new Error('Failed to get blocks')
    }
  }

  /**
   * Get available floors
   */
  async getFloors(zone?: string, district?: string, estateName?: string, block?: string): Promise<string[]> {
    try {
      const params: any = {}
      if (zone) params.zone = zone
      if (district) params.district = district
      if (estateName) params.estateName = estateName
      if (block) params.block = block

      const response = await apiClient.get('/property/floors', { params })
      return response.data
    } catch (error) {
      console.error('Error getting floors:', error)
      throw new Error('Failed to get floors')
    }
  }

  /**
   * Get available units
   */
  async getUnits(zone?: string, district?: string, estateName?: string, block?: string, floor?: string): Promise<string[]> {
    try {
      const params: any = {}
      if (zone) params.zone = zone
      if (district) params.district = district
      if (estateName) params.estateName = estateName
      if (block) params.block = block
      if (floor) params.floor = floor

      const response = await apiClient.get('/property/units', { params })
      return response.data
    } catch (error) {
      console.error('Error getting units:', error)
      throw new Error('Failed to get units')
    }
  }
}

export default new PropertyService()
