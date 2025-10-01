<template>
  <div>
    <!-- Header section -->
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>Nowy rezerwacja</h3>
      </div>

      <div class="col-auto ms-auto text-end mt-n1">
        <button
          class="btn btn-primary d-flex align-items-center gap-2"
          @click="stepper('close')"
        >
          Anuluj <span class="material-icons-sharp">cancel</span>
        </button>
      </div>
    </div>

    <!-- Stepper content -->
    <div class="row">
      <div class="col">
        <!-- Stepper navigation -->
        <div class="steps-container">
          <div v-for="step in steps" :key="step.id" class="step-wrapper">
            <div
              class="step"
              :class="{
                active: step.id === currentStep,
                disabled: !step.completed,
              }"
              @click="setCurrentStep(step.id)"
            >
              <div class="step-container">
                <div class="circle">
                  <span class="material-icons-sharp">{{ step.icon }}</span>
                </div>
                <div class="title d-none d-md-block">{{ step.title }}</div>
              </div>
            </div>
          </div>
        </div>

        <!-- Step details -->
        <div class="step-details mt-4 relative">
          <div
            v-if="activeStep"
            class="d-flex flex-column flex-md-row justify-content-evenly"
          >
            <p class="title d-block d-md-none">{{ activeStep.title }}</p>
            <div v-if="activeStep.id === 1" class="m-2">
              <VDatePicker
                ref="calendar"
                v-model.range="date"
                :attributes="attrs"
                :columns="columns"
                :expanded="expanded"
                :min-date="new Date()"
              >
                <template #footer>
                  <div class="w-full px-4 pb-3">
                    <button class="btn btn-primary" @click="moveToday">
                      Dzisiaj
                    </button>
                  </div>
                </template>
              </VDatePicker>
            </div>
            <div v-if="activeStep.id === 1" class="m-2">
              <p>
                <strong>Data przyjazdu:</strong>
                {{ new Date(formatDate(dateStart)).toLocaleDateString() }}
              </p>
              <p>
                <strong>Data wyjazdu:</strong>
                {{ new Date(formatDate(dateEnd)).toLocaleDateString() }}
              </p>
              <p>
                <strong>{{ $t('stay_length') }}</strong>
                {{ calculateNights(date.start, date.end) }}
              </p>
              <p>
                <strong>{{ $t('room_availability') }}</strong>
                {{ vacant }}
              </p>
            </div>
            <div v-if="activeStep.id === 2" class="m-2 w-100">
              <p class="d-none d-sm-block">
                <strong>{{ $t('select_room') }}</strong>
              </p>
              <div
                class="d-flex flex-wrap"
                style="overflow-y: auto; max-height: 500px"
              >
                <div
                  v-for="room in rooms"
                  :key="room.id"
                  :class="{ 'selected-border': room.id === sroom }"
                  class="card m-2"
                >
                  <div class="card-body">
                    <h5 class="card-title">{{ room.number }}</h5>
                    <p class="card-text">
                      <strong>Standard:</strong>
                      {{ translateStandard(room.type) }}
                    </p>
                    <p class="card-text">
                      <strong>{{ $t('guest_count') }}</strong>
                      {{ room.maxGuests }}
                    </p>
                    <p class="card-text">
                      <strong>Cena za noc:</strong> {{ room.pricePerNight }} PLN
                    </p>
                    <button
                      class="btn btn-primary me-1"
                      @click="store.selectRoom(room.id)"
                    >
                      Wybierz
                    </button>
                    <button
                      class="btn btn-info"
                      @click="roomStore.openModal('info', room.id)"
                    >
                      <i class="bi bi-info-circle"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div v-if="activeStep.id === 3" class="m-2 w-100">
              <p class="d-none d-sm-block">
                <strong>{{ $t('select_guest') }}</strong>
              </p>
              <div class="row align-items-center">
                <div class="col mb-2">
                  <button
                    class="btn btn-primary me-2 mb-2"
                    aria-label="Edit"
                    @click="openPanel('add')"
                  >
                    {{ $t('new_guest') }}
                  </button>
                  <input
                    v-model="searchQuery"
                    placeholder="{{ $t('search_guest') }}"
                    type="input"
                  />
                </div>

                <div class="col text-end">
                  {{ $t('displayed_quantity') }}
                  <select v-model="entriesPerPage" class="selectpicker">
                    <option value="10">10</option>
                    <option value="25">25</option>
                    <option value="50">50</option>
                    <option value="100">100</option>
                  </select>
                </div>
              </div>

              <div class="d-flex flex-wrap">
                <div
                  v-for="guest in guests"
                  :key="guest.id"
                  :class="{ 'selected-border': guest.id === sguest }"
                  class="card m-2"
                >
                  <div class="card-body">
                    <h5 class="card-title">
                      {{ guest.name + " " + guest.surname }}
                    </h5>
                    <p class="card-text">
                      <strong>Telefon:</strong>
                      {{ guest.phone }}
                    </p>
                    <button
                      class="btn btn-primary me-1"
                      @click="store.selectGuest(guest.id)"
                    >
                      Wybierz
                    </button>
                    <button
                      class="btn btn-info"
                      @click="openPanel('edit', guest.id)"
                    >
                      <i class="bi bi-pen"></i>
                    </button>
                  </div>
                </div>
              </div>

              <nav aria-label="Page navigation">
                <ul class="pagination justify-content-center">
                  <li
                    class="page-item"
                    :class="{ disabled: currentPage === 1 || totalPages === 0 }"
                  >
                    <a
                      class="page-link"
                      href="#"
                      @click.prevent="currentPage--"
                    >
                      Poprzednia
                    </a>
                  </li>
                  <li class="page-item align-self-center">
                    <div class="input-group input-group-sm">
                      <input
                        v-model.number="currentPage"
                        type="number"
                        class="form-control text-center"
                        min="1"
                        :max="totalPages"
                      />
                      <span class="input-group-text">/ {{ totalPages }}</span>
                    </div>
                  </li>
                  <li
                    class="page-item"
                    :class="{
                      disabled: currentPage === totalPages || totalPages === 0,
                    }"
                  >
                    <a
                      class="page-link"
                      href="#"
                      @click.prevent="currentPage++"
                    >
                      {{ $t('next') }}
                    </a>
                  </li>
                </ul>
              </nav>
            </div>
            <div v-if="activeStep.id === 4" class="m-2 w-100">
              <p class="d-none d-sm-block">
                <strong>Dodatki</strong>
              </p>
              <div class="d-flex flex-wrap">
                <div
                  v-for="addition in additions"
                  :key="addition.id"
                  class="card m-2"
                >
                  <div class="card-body">
                    <h5 class="card-title">{{ addition.name }}</h5>
                    <p class="card-text">
                      <strong>Cena:</strong>
                      {{ addition.price }}
                    </p>
                    <div class="form-floating">
                      <input
                        id="quantity"
                        v-model.number="addition.quantity"
                        class="form-control"
                        placeholder="Ilość"
                        type="number"
                        min="0"
                        default="0"
                        @input="updateAddition(addition.id, addition.quantity)"
                      />
                      <label for="quantity">Ilość</label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div v-if="activeStep.id === 5" class="m-4 w-100">
              <p class="fs-4 fw-bold mb-3 d-none d-sm-block">Podsumowanie</p>

              <div class="mb-4">
                <p class="fs-4 mb-2">
                  <i class="bi bi-person-fill"></i>
                  Gość: {{ selectedGuest?.name }} {{ selectedGuest?.surname }}
                  <br />
                  <i class="bi bi-telephone-fill"></i>
                  Telefon: {{ selectedGuest?.phone }} <br />
                  <i class="bi bi-geo-alt-fill"></i>
                  Adres: {{ selectedGuest?.address }} {{ selectedGuest?.city }}
                  {{ selectedGuest?.zip_code }} {{ selectedGuest?.country }}
                  <br />
                  <i class="bi bi-envelope-fill"></i>
                  Email: {{ selectedGuest?.email }}
                </p>

                <p class="fs-4 mb-2">
                  <i class="bi bi-house-door-fill"></i>
                  Pokój: {{ selectedRoom?.number }}
                  <i class="text-muted">
                    ({{ $t('price_per_night') }} {{ selectedRoom?.pricePerNight }} zł)</i
                  >
                </p>

                <p class="fs-4 mb-2">
                  <i class="bi bi-basket-fill"></i>
                  Dodatki:
                  {{
                    additions.reduce(
                      (acc, curr) => acc + curr.quantity * curr.price,
                      0,
                    ) + " zł"
                  }}
                  <i class="text-muted"> ({{ $t('price_per_day_stay') }})</i>
                </p>

                <ul class="list-group mb-2">
                  <li
                    v-for="addition in additions.filter((a) => a.quantity > 0)"
                    :key="addition.id"
                    class="list-group-item fs-5"
                  >
                    {{ addition.name }}: {{ addition.quantity }} x
                    {{ addition.price }} zł =
                    {{ addition.quantity * addition.price }} zł
                  </li>
                </ul>
                <p class="fs-4 mb-2">
                  <i class="bi bi-calendar-check"></i>
                  Data przyjazdu:
                  {{ new Date(formatDate(dateStart)).toLocaleDateString() }}
                </p>
                <p class="fs-4 mb-2">
                  <i class="bi bi-calendar-minus"></i>
                  Data wyjazdu:
                  {{ new Date(formatDate(dateEnd)).toLocaleDateString() }}
                </p>
                <p class="fs-4 mb-2">
                  <i class="bi bi-calendar3"></i>
                  Ilość nocy:
                  {{ calculateNights(date.start, date.end) }}
                </p>
                <p class="fs-2 mb-2">
                  <strong>{{ $t('total_stay_price') }}</strong>
                  {{ totalPrice + " zł" }}
                </p>
              </div>
            </div>
          </div>
        </div>

        <!-- Navigation buttons -->
        <div class="navigation-buttons mt-4">
          <button
            class="btn btn-primary"
            :disabled="!activeStep?.completed"
            @click="
              currentStep === 5 ? store.addBooking(totalPrice) : nextStep()
            "
          >
            {{ currentStep === 5 ? $t('add') : $t('next') }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { format } from "date-fns";
const formatDate = (date: Date) => {
  return format(date, "yyyy-MM-dd");
};

const calculateNights = (start: Date, end: Date) => {
  const diffTime = Math.abs(end.getTime() - start.getTime());
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  return diffDays;
};

const store = useMyBookingsStore();
const roomStore = useMyRoomStore();
const guestStore = useMyGuestStore();
const modalStore = useMyModalStore();
const slideoutStore = useMySlideoutStore();

const { openPanel } = useMySlideoutStore();
const totalPages = computed(() => guestStore.totalPages);

const currentStep = computed(() => store.currentStep);
const steps = computed(() => store.steps);
const activeStep = computed(() => {
  return steps.value.find((step) => step.id === currentStep.value);
});

const setCurrentStep = (step: number) => {
  store.currentStep = step;
};

const nextStep = () => store.nextStep();
const stepper = (action: string) => store.stepper(action);

import { useScreens } from "vue-screen-utils";

const dateStart = computed(() => store.dateStart);
const dateEnd = computed(() => store.dateEnd);
const vacant = computed(() => roomStore.availableVacant);
const rooms = computed(() => roomStore.availableRoom);

const guests = computed(() => guestStore.guests);

const additions = computed(() => {
  return store.additions.map((addition) => ({
    ...addition,
    quantity: addition.quantity ?? 0,
  }));
});
const updateAddition = (id: number, quantity: number) => {
  store.updateAddition(id, quantity);
};

const totalPrice = computed(() => {
  let roomPrice = 0;
  const nights = calculateNights(date.value.start, date.value.end);

  if (selectedRoom.value) {
    roomPrice = selectedRoom.value.pricePerNight * nights;
  }

  return (
    roomPrice +
    additions.value.reduce(
      (acc, curr) => acc + curr.quantity * curr.price * nights,
      0,
    )
  );
});

const sroom = computed(() => store.room);
const sguest = computed(() => store.guest);

const selectedGuest = computed(() =>
  guests.value.find((guest) => guest.id === sguest.value),
);
const selectedRoom = computed(() =>
  rooms.value.find((room) => room.id === sroom.value),
);

const date = ref({
  start: new Date(),
  end: (() => {
    const date = new Date();
    date.setDate(date.getDate() + 1);
    return date;
  })(),
});

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

const entriesPerPage = ref(10);
const currentPage = ref(1);
const searchQuery = ref("");

onMounted(async () => {
  await guestStore.fetchGuests(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
  );
});

onMounted(async () => {
  await store.getAddition();
});

onMounted(async () => {
  await store.setDate(date.value.start, date.value.end);
});

watch(date, async () => {
  await store.setDate(date.value.start, date.value.end);
});

onMounted(async () => {
  await roomStore.fetchAvailableRooms(
    formatDate(date.value.start),
    formatDate(date.value.end),
  );
});

watch(dateStart, async () => {
  await roomStore.fetchAvailableRooms(
    formatDate(date.value.start),
    formatDate(date.value.end),
  );
});

watch(
  () => modalStore.isModalOpen,
  async (newVal, oldVal) => {
    if (!newVal && oldVal) {
      await guestStore.fetchGuests(
        entriesPerPage.value,
        currentPage.value,
        searchQuery.value,
      );
    }
  },
);
watch(
  () => slideoutStore.isPanelOpen,
  async (newVal, oldVal) => {
    if (!newVal && oldVal) {
      await guestStore.fetchGuests(
        entriesPerPage.value,
        currentPage.value,
        searchQuery.value,
      );
    }
  },
);

watch([entriesPerPage, currentPage, searchQuery], async () => {
  await guestStore.fetchGuests(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
  );
});

const attrs = ref([
  {
    key: "today",
    highlight: {
      color: "green",
      fillMode: "solid",
    },
    dates: new Date(),
  },
]);

const calendar = ref();

function moveToday() {
  calendar.value.move(new Date());
}

const { mapCurrent } = useScreens({
  xs: "0px",
  sm: "640px",
  md: "768px",
  lg: "1024px",
});

const columns = mapCurrent({ lg: 2 }, 1);
const expanded = mapCurrent({ lg: false }, true);

onMounted(async () => {
  await store.checkStepCompleted(currentStep.value);
});

watch(dateStart, async () => {
  await store.checkStepCompleted(currentStep.value);
});
</script>

<style lang="scss" scoped>
.steps-container {
  display: flex;
  align-items: center;
}

.step-container {
  display: flex;
  align-items: center;
}

.step-wrapper {
  display: flex;
  align-items: center;
  margin-right: 20px;
}

.step {
  display: flex;
  cursor: pointer;
}

.circle {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #ccc;
  color: black;
}

.active .circle {
  background-color: #007bff;
  color: white;
}

.title {
  padding-left: 5px;
  padding-right: 20px;
  font-size: 16px;
}

.step-details {
  border: 1px solid #ccc;
  padding: 20px;
  border-radius: 5px;
}
.disabled {
  pointer-events: none;
  opacity: 0.6;
}

.card {
  width: 18rem;
}
@media (max-width: 767px) {
  .material-icons-sharp {
    font-size: 18px;
  }
}
.selected-border {
  border: 2px solid green;
  animation: border-pulse 2s infinite;
}

@keyframes border-pulse {
  0% {
    border-color: green;
  }
  50% {
    border-color: #00ff00;
  }
  100% {
    border-color: green;
  }
}
</style>
