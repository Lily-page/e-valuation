<template>
  <div class="valuation-container">
    <h1>{{ $t('text.result.title') }}</h1>
    <el-descriptions size="100px" :column="4" class="valuation-content">
      <el-descriptions-item :label="$t('text.result.valuation')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.gfa')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.sfa')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.gfaUnitRate')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.sfaUnitRate')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.year')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
      <el-descriptions-item :label="$t('text.result.date')" :span="8" :label-style="{ fontWeight: 'bold', color: 'black' }"></el-descriptions-item>
    </el-descriptions>
  </div>
</template>

<script setup lang="ts">
  import { ElDescriptions, ElDescriptionsItem } from 'element-plus'

</script>

<style scoped>
  .valuation-content {
    margin-top: 20px;
  }

  .custom-label .el-descriptions__label {
    font-weight: bold;
    color: black;
  }
</style>
