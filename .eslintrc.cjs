module.exports = {
  root: true,
  extends: [
    "@nuxt/eslint-config",
    "@vue/eslint-config-prettier/skip-formatting",
  ],
  overrides: [
    {
      files: ["*.vue"],
      rules: {
        "vue/multi-word-component-names": 0,
      },
    },
  ],
};
