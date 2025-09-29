<template>
  <div class="overlay" :class="{ active: isOpen }">
    <div
      class="modal fade"
      :class="{ show: isOpen }"
      :style="{ display: isOpen ? 'block' : 'none' }"
    >
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ $t('delete_guest') }}</h5>
          </div>
          <div class="modal-body">
            <p>
              {{ $t('confirm_delete_guest') }}
              <strong
                >{{ modalStore.guestdata.name }}
                {{ modalStore.guestdata.surname }}</strong
              >?
            </p>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              @click="modalStore.closeModal"
            >
              Nie
            </button>
            <button
              type="button"
              class="btn btn-danger"
              @click="modalStore.deleteGuest(modalStore.guestdata.id)"
            >
              Tak
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const modalStore = useMyModalStore();

const isOpen = computed(() => modalStore.$state.isModalOpen);
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
