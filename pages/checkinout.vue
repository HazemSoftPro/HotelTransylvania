<template>
  <div v-if="!isStepper">
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>Meldunki</h3>
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
        <!-- <div class="m-2">
          <label>Zakres dat:</label>
          <input id="start" v-model="startDate" type="date" name="trip-start" />
          <label> - </label>
          <input id="end" v-model="endDate" type="date" name="trip-end" />
        </div> -->
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
            <th scope="col">{{ $t('staying_person') }}</th>
            <th scope="col">Pokój</th>
            <th scope="col">Data przyjazdu</th>
            <th scope="col">Data wyjazdu</th>
            <th scope="col">Dodatki</th>
            <th scope="col">Akcje</th>
          </tr>
        </thead>
        <tbody class="align-middle">
          <tr v-for="checkinout in checkinouts" :key="checkinout.id">
            <td>{{ checkinout.id }}</td>
            <td>
              {{ checkinout.guest.name + " " + checkinout.guest.surname }}
            </td>
            <td>{{ checkinout.room.number }}</td>
            <td>
              {{
                new Date(formatDate(checkinout.startDate)).toLocaleDateString()
              }}
            </td>
            <td>
              {{
                new Date(formatDate(checkinout.endDate)).toLocaleDateString()
              }}
            </td>
            <td>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isParkingAvailable(checkinout.additions),
                  'text-muted': !isParkingAvailable(checkinout.additions),
                }"
              >
                local_parking
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isMealAvailable(checkinout.additions),
                  'text-muted': !isMealAvailable(checkinout.additions),
                }"
              >
                restaurant
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isPoolAvailable(checkinout.additions),
                  'text-muted': !isPoolAvailable(checkinout.additions),
                }"
              >
                pool
              </span>
              <span
                class="material-icons-sharp"
                :class="{
                  'text-success': isGymAvailable(checkinout.additions),
                  'text-muted': !isGymAvailable(checkinout.additions),
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
                  @click="openModal(checkinout.id, 'checkout')"
                >
                  <span class="material-icons-sharp">logout</span>
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
  <StepperCheckinout v-else />
</template>

<script lang="ts" setup>
definePageMeta({
  middleware: ["auth"],
});

const store = useMyBookingsStore();
const { stepper } = store;
const isStepper = computed(() => store.$state.isStepper);

const checkinouts = computed(() => store.bookings);

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

onMounted(async () => {
  await store.fetchBookings(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
    ["CHECKED_IN"],
    // startDate.value,
    // endDate.value,
  );
});

watch(
  [entriesPerPage, currentPage, searchQuery, startDate, endDate, isStepper],
  async () => {
    await store.fetchBookings(
      entriesPerPage.value,
      currentPage.value,
      searchQuery.value,
      ["CHECKED_IN"],
      // startDate.value,
      // endDate.value,
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
        ["CHECKED_IN"],
        // startDate.value,
        // endDate.value,
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
