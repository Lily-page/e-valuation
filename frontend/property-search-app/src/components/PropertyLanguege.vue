<template>
	<div class="page-lan">
		<div class="en">English</div>
		<div class="cn">中文</div>
	</div>
</template>

<script setup lang="ts">
import { usePropertyStore } from '@/stores/counter'

const propertyStore = usePropertyStore()

</script>

<style scoped>
	.page-lan {
		display: flex;
		justify-content: flex-end;
	}
	.en:hover, .cn:hover {
		color: #d0103a;
		cursor: pointer;
	}
	.cn {
		margin-left: 20px;
	}
</style>
