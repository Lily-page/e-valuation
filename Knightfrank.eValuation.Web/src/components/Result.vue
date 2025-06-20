<template>
  <div class="valuation-container">
    <h1>{{ $t('text.result.title') }}</h1>

    <!-- Loading State -->
    <div v-if="propertyStore.loading" class="loading-state">
      <el-skeleton :rows="7" animated />
    </div>

    <!-- No Results State -->
    <div v-else-if="!propertyStore.hasValuation" class="no-results">
      <el-empty description="No valuation data available">
        <template #image>
          <el-icon size="60"><Document /></el-icon>
        </template>
        <template #description>
          <p>{{ $t('text.result.noData') }}</p>
        </template>
      </el-empty>
    </div>

    <!-- Valuation Results -->
    <div v-else class="valuation-results">
      <el-card class="property-card" shadow="hover">
        <template #header>
          <div class="card-header">
            <span class="property-title">{{ propertyStore.valuation?.propertyDetails }}</span>
            <el-tag type="success">{{ $t('text.result.latest') }}</el-tag>
          </div>
        </template>

        <el-descriptions :column="2" class="valuation-content" border>
          <el-descriptions-item :label="$t('text.result.valuation')"
                                :span="2"
                                :label-style="labelStyle">
            <span class="valuation-amount">
              HK$ {{ formatCurrency(propertyStore.valuation?.estimatedValue) }}
            </span>
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.gfa')"
                                :label-style="labelStyle">
            {{ formatArea(propertyStore.valuation?.grossFloorArea) }}
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.sfa')"
                                :label-style="labelStyle">
            {{ formatArea(propertyStore.valuation?.saleableFloorArea) }}
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.gfaUnitRate')"
                                :label-style="labelStyle">
            {{ formatUnitRate(propertyStore.valuation?.gfaUnitRate) }}
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.sfaUnitRate')"
                                :label-style="labelStyle">
            {{ formatUnitRate(propertyStore.valuation?.sfaUnitRate) }}
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.year')"
                                :label-style="labelStyle">
            {{ formatYear(propertyStore.valuation?.builtYear) }}
          </el-descriptions-item>

          <el-descriptions-item :label="$t('text.result.date')"
                                :label-style="labelStyle">
            {{ formatDate(propertyStore.valuation?.valuationDate) }}
          </el-descriptions-item>
        </el-descriptions>

        <!-- Disclaimer -->
        <div class="disclaimer">
          <el-alert :title="$t('text.result.disclaimer')"
                    type="warning"
                    :closable="false"
                    show-icon>
          </el-alert>
        </div>
      </el-card>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { computed } from 'vue'
  import {
    ElDescriptions,
    ElDescriptionsItem,
    ElCard,
    ElTag,
    ElEmpty,
    ElSkeleton,
    ElAlert,
    ElIcon
  } from 'element-plus'
  import { Document } from '@element-plus/icons-vue'
  import { usePropertyStore } from '@/stores/propertyStore'

  const propertyStore = usePropertyStore()

  const labelStyle = computed(() => ({
    fontWeight: 'bold',
    color: '#085054',
    fontSize: '14px'
  }))

  const formatCurrency = (value?: number): string => {
    if (!value) return 'N/A'
    return new Intl.NumberFormat('en-HK', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(value)
  }

  const formatArea = (value?: number): string => {
    if (!value) return 'N/A'
    return `${value.toFixed(2)} sq ft`
  }

  const formatUnitRate = (value?: number): string => {
    if (!value) return 'N/A'
    return `HK$ ${formatCurrency(value)} / sq ft`
  }

  const formatYear = (value?: string): string => {
    if (!value) return 'N/A'
    return new Date(value).getFullYear().toString()
  }

  const formatDate = (value?: string): string => {
    if (!value) return 'N/A'
    return new Date(value).toLocaleDateString('en-HK', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    })
  }
</script>

<style scoped>
  .valuation-content {
    margin-top: 20px;
  }

  .property-card {
    margin-top: 20px;
  }

  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .property-title {
    font-size: 16px;
    font-weight: bold;
    color: #085054;
  }

  .valuation-amount {
    font-size: 24px;
    font-weight: bold;
    color: #085054;
  }

  .disclaimer {
    margin-top: 20px;
  }

  .loading-state {
    margin-top: 20px;
  }

  .no-results {
    margin-top: 40px;
    text-align: center;
  }

  .custom-label .el-descriptions__label {
    font-weight: bold;
    color: black;
  }
</style>
