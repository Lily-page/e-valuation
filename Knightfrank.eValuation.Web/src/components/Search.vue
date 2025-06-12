<template>
  <div class="search-container">
    <h1>{{ $t('text.search.title') }}</h1>
    <el-form :model="formInline" class="form-inline">
      <el-form-item :label="$t('text.search.zone')">
        <el-select v-model="formInline.zone"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="Hong Kong" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('text.search.district')">
        <el-select v-model="formInline.district"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="shanghai" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('text.search.estateName')">
        <el-select v-model="formInline.estateName"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="shanghai" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('text.search.block')">
        <el-select v-model="formInline.block"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="shanghai" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('text.search.floor')">
        <el-select v-model="formInline.floor"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="shanghai" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item :label="$t('text.search.unit')">
        <el-select v-model="formInline.unit"
                   :placeholder="$t('text.placeholder')"
                   clearable>
          <el-option label="Hong Kong" value="shanghai" />
          <el-option label="Zone two" value="beijing" />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">{{ $t('button.evaluate') }}</el-button>
        <el-button type="info" @click="onSubmit">{{ $t('button.reset') }}</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup lang="ts">
  import { computed, reactive } from 'vue'
  import { ElButton, ElForm, ElFormItem, ElSelect, ElOption } from 'element-plus'

  const formInline = reactive({
    zone: '',
    district: '',
    estateName: '',
    block: '',
    floor: '',
    unit: '',
  })

  const onSubmit = () => {
    console.log('submit!')
  }

</script>

<style scoped>
  .form-inline {
    margin-top: 20px;
  }
</style>
