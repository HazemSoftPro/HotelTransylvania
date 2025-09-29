<template>
  <main class="d-flex w-100">
    <div class="container d-flex flex-column">
      <div class="row vh-100">
        <LoginForm />
        <!-- <footer class="footer d-flex justify-content-center align-items-center">
					<a href="https://github.com/your-github-username" target="_blank" class="ml-3">
						<i class="bi bi-github"> github</i>
					</a>
				</footer> -->
      </div>
    </div>
  </main>
</template>

<script lang="ts" setup>
definePageMeta({
  layout: "auth",
  middleware: ["not-auth"],
});
</script>

<style>
.footer {
  margin-top: auto;
  background: transparent !important;
  color: #000;
}

.footer a {
  color: inherit; /* Ustaw kolor linku na taki sam jak kolor tekstu */
  text-decoration: none; /* {{ $t('remove_link_underline') }} */
}
</style>
