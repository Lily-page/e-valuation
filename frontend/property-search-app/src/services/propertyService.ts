import axios from 'axios'
import type { Property, PropertySearchRequest, PagedResult, FilterOptions } from '@/types/property'

const API_BASE_URL = 'http://localhost:12001/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const propertyService = {
  async searchProperties(searchRequest: PropertySearchRequest): Promise<PagedResult<Property>> {
    const response = await api.post<PagedResult<Property>>('/properties/search', searchRequest)
    return response.data
  },

  async getFilterOptions(): Promise<FilterOptions> {
    const response = await api.get<FilterOptions>('/properties/filters')
    return response.data
  },

  async getProperty(id: number): Promise<Property> {
    const response = await api.get<Property>(`/properties/${id}`)
    return response.data
  }
}