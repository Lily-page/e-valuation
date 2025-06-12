import { createI18n } from 'vue-i18n'
import en from './locales/en.json'
import zh from './locales/zh.json'

export default createI18n({
  legacy: false, // 使用 Composition API 模式
  locale: 'en',  // default language
  fallbackLocale: 'en',
  messages: {
    en,
    zh
  }
})
