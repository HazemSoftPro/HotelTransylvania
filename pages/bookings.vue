<template>
  <div v-if="!isStepper">
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>Rezerwacje</h3>
      </div>

      <!-- <div class="col-auto ms-auto text-end mt-n1">
        <a href="#" class="btn btn-primary d-flex align-items-center gap-2">
          Odswieź <span class="material-icons-sharp">refresh</span>
        </a>
      </div> -->
    </div>
    <div class="row align-items-center">
      <div class="col d-flex align-items-center me-2">
        <button
          class="btn btn-primary me-2"
          aria-label="Edit"
          @click="stepper('open')"
        >
          <span class="material-icons-sharp">add</span>
        </button>
        <input v-model="searchQuery" placeholder="Szukaj" type="input" />
        <div class="m-2">
          <label>Zakres dat:</label>
          <input id="start" v-model="startDate" type="date" name="trip-start" />
          <label> - </label>
          <input id="end" v-model="endDate" type="date" name="trip-end" />
        </div>
      </div>

      <div class="col text-end">
        {{ $t('show_entries') }}
        <select v-model="entriesPerPage" class="selectpicker">
          <option value="10">10</option>
          <option value="25">25</option>
          <option value="50">50</option>
          <option value="100">100</option>
        </select>
      </div>
    </div>

    <div class="table-responsive mt-2">
      <table class="table table-striped">
        <thead>
          <tr class="align-middle">
            <th scope="col">Numer</th>
            <th scope="col">OTA</th>
            <th scope="col">{{ $t('booking_person') }}</th>
            <th scope="col">Pokój</th>
            <th scope="col">Data przyjazdu</th>
            <th scope="col">Data wyjazdu</th>
            <th scope="col">Status</th>
            <th scope="col">Źródło</th>
            <th scope="col">Kwota</th>
            <th scope="col">Dodatki</th>
            <th scope="col">Akcje</th>
          </tr>
        </thead>
        <tbody class="align-middle">
          <tr v-for="booking in bookings" :key="booking.id">
            <td>{{ booking.id }}</td>
            <td v-if="booking.ota">{{ booking.ota }}</td>
            <td v-else>-</td>
            <td>
              {{ booking.guest.name + " " + booking.guest.surname }}
            </td>
            <td>{{ booking.room.number }}</td>
            <td>
              {{ new Date(formatDate(booking.startDate)).toLocaleDateString() }}
            </td>
            <td>
              {{ new Date(formatDate(booking.endDate)).toLocaleDateString() }}
            </td>
            <td>{{ translateStatus(booking.status) }}</td>
            <td>{{ translateSource(booking.source) }}</td>
            <td>{{ booking.price + " PLN" }}</td>
            <td>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isParkingAvailable(booking.additions),
                  'text-muted': !isParkingAvailable(booking.additions),
                }"
              >
                local_parking
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isMealAvailable(booking.additions),
                  'text-muted': !isMealAvailable(booking.additions),
                }"
              >
                restaurant
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isPoolAvailable(booking.additions),
                  'text-muted': !isPoolAvailable(booking.additions),
                }"
              >
                pool
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isGymAvailable(booking.additions),
                  'text-muted': !isGymAvailable(booking.additions),
                }"
              >
                fitness_center
              </span>
            </td>
            <td>
              <div class="d-flex justify-content-between align-items-center">
                <button
                  class="btn btn-success p-1"
                  aria-label="Edit"
                  :disabled="booking.status === 'CANCELED'"
                  @click="openModal(booking.id, 'checkin')"
                >
                  <span class="material-icons-sharp">logout</span>
                </button>
                <button
                  class="btn btn-danger p-1"
                  aria-label="Edit"
                  :disabled="booking.status === 'CANCELED'"
                  @click="openModal(booking.id, 'cancel')"
                >
                  <span class="material-icons-sharp">event_busy</span>
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <nav aria-label="Page navigation">
      <ul class="pagination justify-content-center">
        <li
          class="page-item"
          :class="{ disabled: currentPage === 1 || totalPages === 0 }"
        >
          <a class="page-link" href="#" @click.prevent="currentPage--">
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
          <a class="page-link" href="#" @click.prevent="currentPage++">
            {{ $t('next') }}
          </a>
        </li>
      </ul>
    </nav>
  </div>
  <StepperBookings v-else />
</template>

<script lang="ts" setup>
definePageMeta({
  middleware: ["auth"],
});

const store = useMyBookingsStore();
const { stepper } = store;
const isStepper = computed(() => store.$state.isStepper);

const bookings = computed(() => store.bookings);

const entriesPerPage = ref(10);
const currentPage = ref(1);
const searchQuery = ref("");
const totalPages = computed(() => store.totalPages);

function formatDate(date: Date | string): string {
  const d = new Date(date);
  let month = "" + (d.getMonth() + 1);
  let day = "" + d.getDate();
  const year = d.getFullYear();

  if (month.length < 2) month = "0" + month;
  if (day.length < 2) day = "0" + day;

  return [year, month, day].join("-");
}

const startDate = ref(formatDate(new Date()));
const endDate = ref(formatDate(new Date(Date.now() + 7 * 24 * 60 * 60 * 1000)));

const openModal = (id: number, type: string) => {
  store.openModal(id, type);
};

interface Addition {
  id: number;
  bookingId: number;
  additionId: number;
  quantity: number;
  addition: {
    id: number;
    name: string;
    price: number;
  };
}

const isParkingAvailable = (additions: Addition[]): boolean => {
  return additions.some(
    (addition) =>
      addition.addition.name === "Miejsce parkingowe" && addition.quantity >= 1,
  );
};

const isMealAvailable = (additions: Addition[]): boolean => {
  return additions.some(
    (addition) =>
      (addition.addition.name === "Śniadanie" ||
        addition.addition.name === "Obiadokolacja") &&
      addition.quantity >= 1,
  );
};

const isPoolAvailable = (additions: Addition[]): boolean => {
  return additions.some(
    (addition) => addition.addition.name === "Basen" && addition.quantity >= 1,
  );
};

const isGymAvailable = (additions: Addition[]): boolean => {
  return additions.some(
    (addition) =>
      addition.addition.name === "Siłownia" && addition.quantity >= 1,
  );
};

function translateStatus(status: string): string {
  switch (status) {
    case "PENDING":
      return "{{ $t('pending') }}";
    case "CONFIRMED":
      return "Potwierdzona";
    case "GUARANTEED":
      return "Gwarantowana";
    case "CANCELED":
      return "Anulowana";
    case "CHECKED_IN":
      return "Zameldowana";
    case "CHECKED_OUT":
      return "Wymeldowana";
    default:
      return status;
  }
}

function translateSource(source: string): string {
  switch (source) {
    case "DIRECT":
      return "{{ $t('direct') }}";
    case "EMAIL":
      return "Email";
    case "PHONE":
      return "Telefon";
    case "BOOKING":
      return "Serwis online";
    case "OTHER":
      return "Inne";
    default:
      return source;
  }
}

onMounted(async () => {
  await store.fetchBookings(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
    ["PENDING,CONFIRMED,GUARANTEED,CANCELED"],
    startDate.value,
    endDate.value,
  );
});

watch(
  [entriesPerPage, currentPage, searchQuery, isStepper, startDate, endDate],
  async () => {
    await store.fetchBookings(
      entriesPerPage.value,
      currentPage.value,
      searchQuery.value,
      ["PENDING,CONFIRMED,GUARANTEED,CANCELED"],
      startDate.value,
      endDate.value,
    );
  },
);

watch(
  () => store.isModalOpen,
  async (newVal, oldVal) => {
    if (!newVal && oldVal) {
      await store.fetchBookings(
        entriesPerPage.value,
        currentPage.value,
        searchQuery.value,
        ["PENDING,CONFIRMED,GUARANTEED,CANCELED"],
        startDate.value,
        endDate.value,
      );
    }
  },
);
</script>

<style lang="scss" scoped>
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
  margin-left: 10px;
  transition:
    background-color 0.3s,
    box-shadow 0.3s;

  .material-icons-sharp {
    font-size: 1.2rem;
    color: white;
  }
}
</style>
