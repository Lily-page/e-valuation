<template>
  <header>
    <img class="header-logo" src="@/assets/header_20221220.png" />

    <div class="wrapper">
      <!--<HelloWorld msg="You did it!" />-->
      <!--<h1>{{ $t('welcome') }}</h1>

      <button @click="toggleLang">
        {{ $t('button.toggleLang') }}
      </button>-->
      <!--<nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>-->
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
}</script>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.header-logo {
  display: block;
  max-width: 82vw;
  margin: 0 auto;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
/*    padding-right: calc(var(--section-gap) / 2);*/
  }

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
  }
}
</style>
