<template>
  <div class="overlay" :class="{ active: isOpen }">
    <div
      class="modal fade"
      :class="{ show: isOpen }"
      :style="{ display: isOpen ? 'block' : 'none' }"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div v-if="modalAction === 'checkout'">
            <div class="modal-header">
              <h5 class="modal-title">Meldunek nr {{ selectBooking?.id }}</h5>
            </div>
            <div class="modal-body">
              <p>{{ $t('confirm_checkout') }}</p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                Nie
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="store.editBooking(store.selectBooking.id, 'checkout')"
              >
                Tak
              </button>
            </div>
          </div>
          <div v-if="modalAction === 'checkin'">
            <div class="modal-header">
              <h5 class="modal-title">Rezerwacja nr {{ selectBooking?.id }}</h5>
            </div>
            <div class="modal-body">
              <p>{{ $t('confirm_checkin') }}</p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                Nie
              </button>
              <button
                type="button"
                class="btn btn-success"
                @click="store.editBooking(store.selectBooking.id, 'checkin')"
              >
                Tak
              </button>
            </div>
          </div>
          <div v-if="modalAction === 'cancel'">
            <div class="modal-header">
              <h5 class="modal-title">Rezerwacja nr {{ selectBooking?.id }}</h5>
            </div>
            <div class="modal-body">
              <p>{{ $t('confirm_cancel_booking') }}</p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                Nie
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="store.editBooking(store.selectBooking.id, 'cancel')"
              >
                Anuluj
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const store = useMyBookingsStore();

const isOpen = computed(() => store.$state.isModalOpen);

const modalAction = computed(() => store.$state.modalAction);

const selectBooking = computed(() => store.$state.selectBooking);
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
</style>
