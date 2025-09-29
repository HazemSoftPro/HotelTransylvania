<template>
  <Notivue v-slot="item">
    <Notification :item="item" />
  </Notivue>

  <NuxtLoadingIndicator />
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script>
export default {
  setup() {
    const { locale } = useI18n();
    
    // Watch for locale changes to update document direction
    watch(locale, (newLocale) => {
      if (newLocale === 'ar') {
        document.documentElement.setAttribute('dir', 'rtl');
        document.documentElement.setAttribute('lang', 'ar');
      } else {
        document.documentElement.setAttribute('dir', 'ltr');
        document.documentElement.setAttribute('lang', 'en');
      }
    });
    
    return {};
  }
}
</script>

<style>
/* RTL specific styles */
[dir="rtl"] .text-right {
  text-align: left !important;
}

[dir="rtl"] .me-2 {
  margin-left: 0.5rem !important;
  margin-right: 0 !important;
}

[dir="rtl"] .me-1 {
  margin-left: 0.25rem !important;
  margin-right: 0 !important;
}

[dir="rtl"] .navbar-align {
  margin-right: auto !important;
  margin-left: 0 !important;
}
</style>
