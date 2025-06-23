<template>
  <header>
    <img class="header-logo" src="@/assets/logo.png" />
    <div class="wrapper">
      <a @click="toggleLang()">{{ $t('button.toggleLang') }}</a>
    </div>
  </header>

  <RouterView />
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import HelloWorld from './components/HelloWorld.vue'

import { useI18n } from 'vue-i18n'

const { locale } = useI18n()

function toggleLang() {
  locale.value = locale.value === 'en' ? 'zh' : 'en'
}
</script>

<style scoped>
  header {
    line-height: 1.5;
    max-height: 100vh;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .header-logo {
    display: block;
    max-width: 15vw;
    margin-top: 5vh;
  }

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

@media (min-width: 1024px) {
  /*header {
    display: flex;
    place-items: center;*/
/*    padding-right: calc(var(--section-gap) / 2);*/
  /*}

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }*/
}
</style>
