<template>
  <div
    class="overlay"
    :class="{ active: isCollapsed }"
    @click="slideout.closePanel"
  ></div>
  <div class="sidebar2" :class="{ collapsed: isCollapsed }">
    <button class="close-btn" @click="slideout.closePanel">X</button>
    <!-- <h3 class="text-dark">Active action: {{ action }}</h3> -->

    <div v-if="action === 'edit'">
      <form>
        <div class="row">
          <div class="col-md-6 mb-3">
            <label for="name">Imię</label>
            <input
              id="name"
              v-model="slideout.guestdata.name"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-6 mb-3">
            <label for="surname">Nazwisko</label>
            <input
              id="surname"
              v-model="slideout.guestdata.surname"
              type="text"
              class="form-control"
              required
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="phone">Telefon</label>
          <input
            id="phone"
            v-model="slideout.guestdata.phone"
            type="tel"
            class="form-control"
            required
          />
        </div>

        <div class="mb-3">
          <label for="email">Email</label>
          <input
            id="email"
            v-model="slideout.guestdata.email"
            type="email"
            class="form-control"
            required
          />
        </div>

        <div class="mb-3">
          <label for="address">Adres</label>
          <input
            id="address"
            v-model="slideout.guestdata.address"
            type="text"
            class="form-control"
            required
          />
        </div>

        <div class="row">
          <div class="col-md-5 mb-3">
            <label for="country">Kraj</label>
            <input
              id="country"
              v-model="slideout.guestdata.country"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-4 mb-3">
            <label for="city">Miasto</label>
            <input
              id="city"
              v-model="slideout.guestdata.city"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="zip_code">Kod pocztowy</label>
            <input
              id="zip_code"
              v-model="slideout.guestdata.zip_code"
              type="text"
              class="form-control"
              required
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="description">Opis</label>
          <textarea
            id="description"
            v-model="slideout.guestdata.description"
            class="form-control"
          ></textarea>
        </div>

        <div class="mb-3">
          <label for="nip">NIP</label>
          <input
            id="nip"
            v-model="slideout.guestdata.nip"
            type="text"
            class="form-control"
          />
        </div>

        <button
          class="btn btn-primary"
          type="submit"
          @click.prevent="slideout.editGuest(slideout.guestdata.id)"
        >
          Edytuj
        </button>
      </form>
    </div>

    <div v-if="action === 'add'">
      <form>
        <div class="row">
          <div class="col-md-6 mb-3">
            <label for="name">Imię</label>
            <input
              id="name"
              v-model="slideout.guestdata.name"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-6 mb-3">
            <label for="surname">Nazwisko</label>
            <input
              id="surname"
              v-model="slideout.guestdata.surname"
              type="text"
              class="form-control"
              required
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="phone">Telefon</label>
          <input
            id="phone"
            v-model="slideout.guestdata.phone"
            type="tel"
            class="form-control"
            required
          />
        </div>

        <div class="mb-3">
          <label for="email">Email</label>
          <input
            id="email"
            v-model="slideout.guestdata.email"
            type="email"
            class="form-control"
            required
          />
        </div>

        <div class="mb-3">
          <label for="address">Adres</label>
          <input
            id="address"
            v-model="slideout.guestdata.address"
            type="text"
            class="form-control"
            required
          />
        </div>

        <div class="row">
          <div class="col-md-5 mb-3">
            <label for="country">Kraj</label>
            <input
              id="country"
              v-model="slideout.guestdata.country"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-4 mb-3">
            <label for="city">Miasto</label>
            <input
              id="city"
              v-model="slideout.guestdata.city"
              type="text"
              class="form-control"
              required
            />
          </div>
          <div class="col-md-3 mb-3">
            <label for="zip_code">Kod pocztowy</label>
            <input
              id="zip_code"
              v-model="slideout.guestdata.zip_code"
              type="text"
              class="form-control"
              required
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="description">Opis</label>
          <textarea
            id="description"
            v-model="slideout.guestdata.description"
            class="form-control"
          ></textarea>
        </div>

        <div class="mb-3">
          <label for="nip">NIP</label>
          <input
            id="nip"
            v-model="slideout.guestdata.nip"
            type="text"
            class="form-control"
          />
        </div>

        <button
          class="btn btn-primary"
          type="submit"
          @click.prevent="slideout.addGuest"
        >
          Dodaj
        </button>
      </form>
    </div>
  </div>
</template>

<script lang="ts" setup>
const slideout = useMySlideoutStore();

const isCollapsed = computed(() => slideout.$state.isPanelOpen);

const action = computed(() => slideout.activeAction);
</script>

<style lang="scss">
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: -1;
  opacity: 0;
  transition: opacity 0.35s ease-in-out;
}

.overlay.active {
  opacity: 1;
  z-index: 0;
}
.sidebar2 {
  position: fixed;
  right: 0;
  min-width: 700px;
  max-width: 700px;
  height: 100vh;
  background-color: #f3f5f9;
  border-left: #1d2d40 solid 8px;
  transform: translateX(100%);
  transition: transform 0.35s ease-in-out;
  border-top-left-radius: 4px;
  border-bottom-left-radius: 4px;
  overflow: hidden;
  padding-left: 10px;
  z-index: 2;
  padding: 30px;
  overflow-y: auto;

  @media (max-width: 768px) {
    min-width: 100%;
    max-width: 100%;
    button {
      margin-bottom: 60px;
    }
  }
}

.sidebar2.collapsed {
  transform: translateX(0);
}

.close-btn {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #212529;
}
</style>
