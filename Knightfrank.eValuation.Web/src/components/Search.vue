<template>
  <div class="search-container">
    <h1>{{ $t('text.search.title') }}</h1>

    <!-- Token Status -->
    <div v-if="!propertyStore.isTokenValid" class="token-status">
      <el-alert title="Initializing..."
                type="info"
                :closable="false"
                show-icon>
        <template #default>
          Setting up anonymous access token...
        </template>
      </el-alert>
    </div>

    <!-- Error Display -->
    <div v-if="propertyStore.error" class="error-display">
      <el-alert :title="propertyStore.error"
                type="error"
                :closable="true"
                @close="propertyStore.clearError"
                show-icon>
      </el-alert>
    </div>

    <el-form :model="propertyStore.searchForm" class="form-inline" :disabled="propertyStore.loading">
      <el-form-item :label="$t('text.search.zone')" required>
        <el-select v-model="propertyStore.searchForm.zone"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   @change="propertyStore.onZoneChange"
                   :loading="propertyStore.loading">
          <el-option v-for="zone in propertyStore.zones"
                     :key="zone"
                     :label="zone"
                     :value="zone" />
        </el-select>
      </el-form-item>

      <el-form-item :label="$t('text.search.district')" required>
        <el-select v-model="propertyStore.searchForm.district"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   @change="propertyStore.onDistrictChange"
                   :disabled="!propertyStore.searchForm.zone"
                   :loading="propertyStore.loading">
          <el-option v-for="district in propertyStore.districts"
                     :key="district"
                     :label="district"
                     :value="district" />
        </el-select>
      </el-form-item>

      <el-form-item :label="$t('text.search.estateName')" required>
        <el-select v-model="propertyStore.searchForm.estateName"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   @change="propertyStore.onEstateNameChange"
                   :disabled="!propertyStore.searchForm.district"
                   :loading="propertyStore.loading">
          <el-option v-for="estate in propertyStore.estateNames"
                     :key="estate"
                     :label="estate"
                     :value="estate" />
        </el-select>
      </el-form-item>

      <el-form-item :label="$t('text.search.block')">
        <el-select v-model="propertyStore.searchForm.block"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   @change="propertyStore.onBlockChange"
                   :disabled="!propertyStore.searchForm.estateName"
                   :loading="propertyStore.loading">
          <el-option v-for="block in propertyStore.blocks"
                     :key="block"
                     :label="block"
                     :value="block" />
        </el-select>
      </el-form-item>

      <el-form-item :label="$t('text.search.floor')">
        <el-select v-model="propertyStore.searchForm.floor"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   @change="propertyStore.onFloorChange"
                   :disabled="!propertyStore.searchForm.block"
                   :loading="propertyStore.loading">
          <el-option v-for="floor in propertyStore.floors"
                     :key="floor"
                     :label="floor"
                     :value="floor" />
        </el-select>
      </el-form-item>

      <el-form-item :label="$t('text.search.unit')">
        <el-select v-model="propertyStore.searchForm.unit"
                   :placeholder="$t('text.placeholder')"
                   clearable
                   :disabled="!propertyStore.searchForm.floor"
                   :loading="propertyStore.loading">
          <el-option v-for="unit in propertyStore.units"
                     :key="unit"
                     :label="unit"
                     :value="unit" />
        </el-select>
      </el-form-item>

      <el-form-item>
        <el-button type="primary"
                   @click="onEvaluate"
                   :loading="propertyStore.loading"
                   :disabled="!propertyStore.isFormValid || !propertyStore.isTokenValid">
          {{ $t('button.evaluate') }}
        </el-button>
        <el-button type="info"
                   @click="onReset"
                   :disabled="propertyStore.loading">
          {{ $t('button.reset') }}
        </el-button>
      </el-form-item>
    </el-form>

    <!-- Token Info (for debugging) -->
    <!--<div v-if="propertyStore.tokenInfo && propertyStore.isTokenValid" class="token-info">
      <el-collapse>
        <el-collapse-item title="Token Information" name="1">
          <p><strong>Session ID:</strong> {{ propertyStore.tokenInfo.sessionId }}</p>
          <p><strong>Requests Made:</strong> {{ propertyStore.tokenInfo.requestCount }}</p>
          <p><strong>Expires At:</strong> {{ new Date(propertyStore.tokenInfo.expiresAt).toLocaleString() }}</p>
        </el-collapse-item>
      </el-collapse>
    </div>-->
  </div>
</template>

<script setup lang="ts">
  import { onMounted } from 'vue'
  import {
    ElButton,
    ElForm,
    ElFormItem,
    ElSelect,
    ElOption,
    ElAlert,
    ElCollapse,
    ElCollapseItem
  } from 'element-plus'
  import { usePropertyStore } from '@/stores/propertyStore'

  const propertyStore = usePropertyStore()

  onMounted(async () => {
    // Initialize token and load initial data
    await propertyStore.initializeToken()
    if (propertyStore.isTokenValid) {
      await propertyStore.loadZones()
    }
  })

  const onEvaluate = async () => {
    if (propertyStore.isFormValid && propertyStore.isTokenValid) {
      await propertyStore.getValuation()
    }
  }

  const onReset = () => {
    propertyStore.resetForm()
  }
</script>

<style scoped>
  .form-inline {
    margin-top: 20px;
  }

  .token-status, .error-display {
    margin-bottom: 20px;
  }

  .token-info {
    margin-top: 20px;
    font-size: 12px;
    color: #666;
  }

    .token-info p {
      margin: 5px 0;
    }
</style>
