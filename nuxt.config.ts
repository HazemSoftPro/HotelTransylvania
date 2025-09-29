// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  extends: ["nuxt-modal"],
  devtools: { enabled: true },
  runtimeConfig: {
    access: {
      token: import.meta.env.NUXT_ACCESS_TOKEN,
      exp: import.meta.env.NUXT_ACCESS_TOKEN_EXP,
    },
    refresh: {
      token: import.meta.env.NUXT_REFRESH_TOKEN,
      exp: import.meta.env.NUXT_REFRESH_TOKEN_EXP,
    },
    public: {
      sentry: {
        dsn: import.meta.env.NUXT_PUBLIC_SENTRY_DSN,
        environment: import.meta.env.NUXT_PUBLIC_ENV,
      },
    },
  },
  css: [
    "~/assets/scss/main.scss",
    "notivue/notification.css",
    "notivue/animations.css",
  ],
  vite: {
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: '@use "~/assets/scss/_variables" as *;',
        },
      },
    },
  },
  modules: ["@pinia/nuxt", "notivue/nuxt", "@samk-dev/nuxt-vcalendar", "@nuxtjs/i18n"],
  i18n: {
    locales: ['en', 'ar'],
    defaultLocale: 'en',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_redirected',
      redirectOn: 'root'
    },
    vueI18n: './i18n.config.ts'
  },
  app: {
    head: {
      title: "HMSuite",
      meta: [
        { charset: "utf-8" },
        { name: "viewport", content: "width=device-width, initial-scale=1" },
        {
          hid: "description",
          name: "description",
          content: "Hotel Management System",
        },
      ],
      script: [
        {
          src: "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js",
        },
      ],
      link: [
        {
          rel: "stylesheet",
          href: "https://fonts.googleapis.com/icon?family=Material+Icons+Sharp",
        },
      ],
    },
  },
});
