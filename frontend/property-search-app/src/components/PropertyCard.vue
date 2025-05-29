<template>
  <div class="property-card">
    <div class="property-image">
      <img 
        :src="property.imageUrl || '/placeholder-property.jpg'" 
        :alt="property.address"
        @error="onImageError"
      />
      <div class="property-type-badge">{{ property.propertyType }}</div>
    </div>
    
    <div class="property-content">
      <div class="property-header">
        <h3 class="property-address">{{ property.address }}</h3>
        <div class="property-price">${{ formatPrice(property.salePrice) }}</div>
      </div>
      
      <div class="property-location">
        <span class="region">{{ property.region }}</span>
        <span class="separator">•</span>
        <span class="district">{{ property.district }}</span>
      </div>
      
      <div class="property-details">
        <div class="detail-item">
          <span class="detail-icon">🛏️</span>
          <span>{{ property.bedrooms }} bed{{ property.bedrooms !== 1 ? 's' : '' }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-icon">🚿</span>
          <span>{{ property.bathrooms }} bath{{ property.bathrooms !== 1 ? 's' : '' }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-icon">📐</span>
          <span>{{ property.area }} m²</span>
        </div>
      </div>
      
      <div class="property-description">
        {{ truncateDescription(property.description) }}
      </div>
      
      <div class="property-footer">
        <div class="listed-date">
          Listed: {{ formatDate(property.listedDate) }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Property } from '@/types/property'

interface Props {
  property: Property
}

defineProps<Props>()

function formatPrice(price: number): string {
  return price.toLocaleString()
}

function formatDate(dateString: string): string {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { 
    year: 'numeric', 
    month: 'short', 
    day: 'numeric' 
  })
}

function truncateDescription(description: string, maxLength: number = 120): string {
  if (description.length <= maxLength) return description
  return description.substring(0, maxLength) + '...'
}

function onImageError(event: Event) {
  const img = event.target as HTMLImageElement
  img.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KICA8cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZGRkIi8+CiAgPHRleHQgeD0iNTAlIiB5PSI1MCUiIGZvbnQtZmFtaWx5PSJBcmlhbCwgc2Fucy1zZXJpZiIgZm9udC1zaXplPSIxOCIgZmlsbD0iIzk5OSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPk5vIEltYWdlPC90ZXh0Pgo8L3N2Zz4K'
}
</script>

<style scoped>
.property-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.property-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
}

.property-image {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.property-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.property-type-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  background: rgba(0, 123, 255, 0.9);
  color: white;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 600;
}

.property-content {
  padding: 16px;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.property-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.property-address {
  font-size: 16px;
  font-weight: 600;
  margin: 0;
  color: #333;
  flex: 1;
  margin-right: 10px;
}

.property-price {
  font-size: 18px;
  font-weight: 700;
  color: #007bff;
  white-space: nowrap;
}

.property-location {
  color: #666;
  font-size: 14px;
  margin-bottom: 12px;
}

.separator {
  margin: 0 8px;
}

.property-details {
  display: flex;
  gap: 16px;
  margin-bottom: 12px;
  flex-wrap: wrap;
}

.detail-item {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 14px;
  color: #666;
}

.detail-icon {
  font-size: 16px;
}

.property-description {
  color: #666;
  font-size: 14px;
  line-height: 1.4;
  margin-bottom: 12px;
  flex: 1;
}

.property-footer {
  margin-top: auto;
  padding-top: 12px;
  border-top: 1px solid #eee;
}

.listed-date {
  font-size: 12px;
  color: #999;
}

@media (max-width: 768px) {
  .property-header {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .property-price {
    margin-top: 4px;
  }
  
  .property-details {
    gap: 12px;
  }
}
</style>