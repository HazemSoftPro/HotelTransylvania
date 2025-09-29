<template>
  <div class="login--card p-4 border rounded shadow bg-light my-auto mx-auto">
    <div class="text-center">
      <img class="logo img-fluid my-3" src="/assets/img/logo.png" alt="" />
      <h1 class="title">HMSuite</h1>
    </div>

    <form class="my-3" @submit.prevent="login">
      <div class="mb-3 position-relative">
        <label
          for="username"
          class="form-label position-absolute top-50 start-0 translate-middle-y ps-2 bg-white"
          >Username</label
        >
        <div class="input-group">
          <span class="input-group-text">
            <span class="material-icons-sharp">person</span>
          </span>
          <input
            id="username"
            v-model="LoginForm.username"
            type="text"
            class="form-control"
            name="username"
            placeholder="Login"
          />
        </div>
      </div>
      <div class="mb-3 position-relative">
        <label
          for="password"
          class="form-label position-absolute top-50 start-0 translate-middle-y ps-2 bg-white"
          >Password</label
        >
        <div class="input-group">
          <span class="input-group-text">
            <span class="material-icons-sharp">vpn_key</span>
          </span>
          <input
            id="password"
            v-model="LoginForm.password"
            type="password"
            class="form-control"
            name="password"
            placeholder="{{ $t('password') }}"
          />
        </div>
      </div>
      <button
        type="submit"
        class="btn btn-primary w-100 transform-hover"
        :disabled="!isFormValid"
      >
        {{ $t('login') }}
      </button>
    </form>
    <p
      v-if="login_message"
      class="d-flex align-items-center justify-content-center text-danger"
    >
      <span class="material-icons-sharp me-2">error</span> {{ login_message }}
    </p>
  </div>
</template>

<script lang="ts" setup>
interface LoginForm {
  username: string;
  password: string;
}

const LoginForm = reactive({
  username: "",
  password: "",
});

const login_message = ref("");
const isFormValid = computed(() => LoginForm.username && LoginForm.password);
const authStore = useAuthStore();

const login = async () => {
  const { username, password } = LoginForm;

  try {
    await authStore.login(username, password);
    if (!authStore.data.authed) {
      login_message.value = "{{ $t('invalid_login_data') }}";
    }
  } catch (error) {
    console.log(error);
    login_message.value = "{{ $t('error_occurred') }}";
  }
};
</script>

<style>
.login--card {
  width: 100%;
  max-width: 400px;
}

.logo {
  max-width: clamp(100px, 50%, 300px);
}

.title {
  font-size: clamp(1.2rem, 5vw, 2rem);
}

.transform-hover {
  transition: transform 0.3s ease-in-out;
}

.transform-hover:hover {
  transform: scale(1.05);
}
</style>
