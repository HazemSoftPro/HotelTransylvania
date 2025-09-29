<template>
  <div class="language-switcher">
    <select v-model="locale" class="form-select" @change="switchLanguage">
      <option v-for="lang in availableLocales" :key="lang.code" :value="lang.code">
        {{ lang.name }}
      </option>
    </select>
  </div>
</template>

<script setup lang="ts">
const { locale, availableLocales } = useI18n();

const switchLanguage = () => {
  // Set the language preference in localStorage
  localStorage.setItem('language', locale.value);
  
  // Update the document direction for RTL languages
  if (locale.value === 'ar') {
    document.documentElement.setAttribute('dir', 'rtl');
    document.documentElement.setAttribute('lang', 'ar');
  } else {
    document.documentElement.setAttribute('dir', 'ltr');
    document.documentElement.setAttribute('lang', 'en');
  }
};

// Initialize language from localStorage or browser preference
onMounted(() => {
  const savedLanguage = localStorage.getItem('language');
  if (savedLanguage && availableLocales.value.some(lang => lang.code === savedLanguage)) {
    locale.value = savedLanguage;
  }
  
  // Set document direction based on current language
  if (locale.value === 'ar') {
    document.documentElement.setAttribute('dir', 'rtl');
    document.documentElement.setAttribute('lang', 'ar');
  } else {
    document.documentElement.setAttribute('dir', 'ltr');
    document.documentElement.setAttribute('lang', 'en');
  }
});
</script>

<style scoped lang="scss">
.language-switcher {
  margin: 10px;
}

.form-select {
  width: auto;
  display: inline-block;
}
</style>