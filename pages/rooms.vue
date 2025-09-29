<template>
  <div>
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>Pokoje</h3>
      </div>
    </div>

    <div class="row">
      <div class="mb-4">
        <button
          class="btn btn-primary me-2"
          aria-label="Edit"
          @click="store.openModal('add')"
        >
          <span>{{ $t('add_room') }}</span>
        </button>
        <input v-model="searchQuery" placeholder="Szukaj" type="input" />
      </div>
      <div style="overflow-y: auto; max-height: 900px">
        <div class="d-flex flex-wrap">
          <div
            v-for="room in rooms"
            :key="room.id"
            class="col-12 col-md-6 col-lg-4 col-xl-3 animate__animated animate__fadeIn p-3"
          >
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">{{ $t('room') }} {{ room.number }}</h5>
                <p class="card-text">
                  <strong>Typ:</strong> {{ translateStandard(room.type) }}
                </p>

                <p class="card-text">
                  <strong>Max os.:</strong> {{ room.maxGuests }}
                </p>
                <p class="card-text">
                  <strong>Cena:</strong> {{ room.pricePerNight }} PLN/noc
                </p>
                <div class="mb-3">
                  <p class="card-text">
                    <strong>Status:</strong> {{ translateStatus(room.status) }}
                  </p>
                </div>
                <div class="card-actions d-flex justify-content-end">
                  <button
                    class="btn btn-primary me-2"
                    @click="store.openModal('edit', room.id)"
                  >
                    <i class="bi bi-pencil-square"></i>
                  </button>
                  <button
                    class="btn btn-danger me-2"
                    @click="store.openModal('delete', room.id)"
                  >
                    <i class="bi bi-trash"></i>
                  </button>
                  <button
                    class="btn btn-info"
                    @click="store.openModal('info', room.id)"
                  >
                    <i class="bi bi-info-circle"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import "animate.css";
definePageMeta({
  middleware: ["auth"],
});

const store = useMyRoomStore();

const rooms = computed(() => store.$state.rooms);

const isModalOpen = computed(() => store.$state.isModalOpen);

const searchQuery = ref("");

function translateStatus(status: string) {
  switch (status) {
    case "CLEAN":
      return "Czysty";
    case "DIRTY":
      return "Brudny";
    case "SERVICE":
      return "Serwis";
    case "OCCUPIED":
      return "{{ $t('room_occupied') }}";
    case "ARRIVAL":
      return "Przyjazd";
    default:
      return status;
  }
}

function translateStandard(standard: string) {
  switch (standard) {
    case "ECONOMY":
      return "Ekonomiczny";
    case "STANDARD":
      return "Standardowy";
    case "LUXURY":
      return "Luksusowy";
    case "APARTMENT":
      return "Apartament";
    default:
      return standard;
  }
}
onMounted(async () => {
  await store.fetchRooms(searchQuery.value);
});

watch(isModalOpen, async (newVal, oldVal) => {
  if (oldVal === true && newVal === false) {
    await store.fetchRooms(searchQuery.value);
  }
});

watch([searchQuery], async () => {
  await store.fetchRooms(searchQuery.value);
});
</script>

<style>
.icon-with-text span {
  vertical-align: middle; /* {{ $t('icons_alignment') }} */
  margin-right: 4px; /* {{ $t('icons_spacing') }} */
}

.icon-with-text {
  display: inline-block;
  margin-right: 10px; /* {{ $t('icons_group_spacing') }} */
}

.card-actions button {
  margin-top: 10px;
}
</style>
