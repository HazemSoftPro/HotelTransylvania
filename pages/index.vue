<template>
  <div class="container-fluid p-4">
    <div class="row mb-3">
      <div class="col">
        <h3>Dashboard</h3>
        <p>Dzisiaj jest {{ currentDay }} {{ currentDate }}</p>
      </div>

      <div class="col-auto ms-auto text-end mt-n1">
        <a href="#" class="btn btn-primary d-flex align-items-center gap-2">
          {{ $t('refresh') }} <span class="material-icons-sharp">refresh</span>
        </a>
      </div>
    </div>

    <div class="row">
      <div class="col-xl-6 col-xxl-5 d-flex">
        <div class="w-100">
          <div class="row info">
            <div v-for="card in cards" :key="card.id" class="col-sm-6">
              <div class="card info-card">
                <div class="card-body">
                  <h5 class="card-title">{{ card.title }}</h5>
                  <h1 class="mt-1 mb-3">{{ card.value }}</h1>
                  <div class="mb-0">
                    <span class="text-muted">{{ card.description }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="row mt-3">
            <div class="col-12">
              <div class="card flex-fill w-100">
                <div class="card-header">
                  <h5 class="card-title mb-0">Rezerwacje na najbliższe 7 dni</h5>
                </div>
                <div class="card-body py-3">
                  <div class="chart w-100">
                    <DashboardReservation />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-xl-6 col-xxl-7">
        <div class="card">
          <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="card-title">Przyszłe rezerwacje</h5>
            <div class="d-flex align-items-center gap-3">
              <select v-model="bookingsPerPage" class="form-select">
                <option value="5">5</option>
                <option value="10">10</option>
                <option value="25">25</option>
                <option value="50">50</option>
                <option value="100">100</option>
              </select>
              <span>{{ $t('show_entries') }}</span>
            </div>
          </div>
          <div class="card-body">
            <table class="table table-hover my-0">
              <thead>
                <tr>
                  <th scope="col">{{ $t('guest') }}</th>
                  <th scope="col">{{ $t('room') }}</th>
                  <th scope="col">Data</th>
                  <th scope="col">Status</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="booking in bookings" :key="booking.id">
                  <td>{{ booking.guest.name }} {{ booking.guest.surname }}</td>
                  <td>{{ booking.room.number }}</td>
                  <td>{{ new Date(booking.startDate).toLocaleDateString() }}</td>
                  <td>
                    <span
                      class="badge"
                      :class="{
                        'bg-warning': booking.status === 'PENDING',
                        'bg-success': booking.status === 'CONFIRMED',
                        'bg-primary': booking.status === 'CHECKED_IN',
                        'bg-secondary': booking.status === 'CHECKED_OUT',
                        'bg-danger': booking.status === 'CANCELLED',
                      }"
                    >
                      {{ translateStatus(booking.status) }}
                    </span>
                  </td>
                </tr>
                <tr v-if="bookings.length === 0">
                  <td colspan="4" class="text-center">
                    {{ $t('no_future_bookings') }}
                  </td>
                </tr>
              </tbody>
            </table>
            <nav aria-label="Page navigation example">
              <ul class="pagination justify-content-center">
                <li class="page-item" :class="{ disabled: currentPage === 1 }">
                  <a class="page-link" href="#" @click="previousPage">Poprzednia</a>
                </li>
                <li class="page-item" :class="{ active: currentPage === 1 }">
                  <a class="page-link" href="#" @click="goToPage(1)">1</a>
                </li>
                <li class="page-item" :class="{ active: currentPage === 2 }">
                  <a class="page-link" href="#" @click="goToPage(2)">2</a>
                </li>
                <li class="page-item" :class="{ disabled: currentPage === 2 }">
                  <a class="page-link" href="#" @click="nextPage">{{ $t('next') }}</a>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
    </div>

    <div class="row mt-3">
      <div class="col-12">
        <div class="card">
          <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="card-title">Przychody z rezerwacji</h5>
            <div class="d-flex align-items-center gap-3">
              <select v-model="revenuePerPage" class="form-select">
                <option value="5">5</option>
                <option value="10">10</option>
                <option value="25">25</option>
                <option value="50">50</option>
                <option value="100">100</option>
              </select>
              <span>{{ $t('show_entries') }}</span>
            </div>
          </div>
          <div class="card-body">
            <table class="table table-hover my-0">
              <thead>
                <tr>
                  <th scope="col">{{ $t('guest') }}</th>
                  <th scope="col">{{ $t('room') }}</th>
                  <th scope="col">Data</th>
                  <th scope="col">Cena</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="revenue in revenues" :key="revenue.id">
                  <td>{{ revenue.guest.name }} {{ revenue.guest.surname }}</td>
                  <td>{{ revenue.room.number }}</td>
                  <td>{{ new Date(revenue.startDate).toLocaleDateString() }}</td>
                  <td>{{ revenue.totalPrice }} zł</td>
                </tr>
                <tr v-if="revenues.length === 0">
                  <td colspan="4" class="text-center">
                    {{ $t('no_revenue_data') }}
                  </td>
                </tr>
              </tbody>
            </table>
            <nav aria-label="Page navigation example">
              <ul class="pagination justify-content-center">
                <li class="page-item" :class="{ disabled: revenueCurrentPage === 1 }">
                  <a class="page-link" href="#" @click="revenuePreviousPage">Poprzednia</a>
                </li>
                <li class="page-item" :class="{ active: revenueCurrentPage === 1 }">
                  <a class="page-link" href="#" @click="revenueGoToPage(1)">1</a>
                </li>
                <li class="page-item" :class="{ active: revenueCurrentPage === 2 }">
                  <a class="page-link" href="#" @click="revenueGoToPage(2)">2</a>
                </li>
                <li class="page-item" :class="{ disabled: revenueCurrentPage === 2 }">
                  <a class="page-link" href="#" @click="revenueNextPage">{{ $t('next') }}</a>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { useMyDashboardStore } from "~/stores/dashboard";
import { useMyBookingsStore } from "~/stores/bookings";

const store = useMyDashboardStore();
const bookingStore = useMyBookingsStore();

const currentDay = ref("");
const currentDate = ref("");

const bookings = ref([]);
const currentPage = ref(1);
const bookingsPerPage = ref(5);

const revenues = ref([]);
const revenueCurrentPage = ref(1);
const revenuePerPage = ref(5);

const translateStatus = (status: string) => {
  switch (status) {
    case "PENDING":
      return $t('pending');
    case "CONFIRMED":
      return $t('confirmed');
    case "CHECKED_IN":
      return $t('checked_in');
    case "CHECKED_OUT":
      return $t('checked_out');
    case "CANCELLED":
      return $t('cancelled');
    default:
      return status;
  }
};

const daysOfWeek = [
  $t('sunday'),
  $t('monday'),
  $t('tuesday'),
  $t('wednesday'),
  $t('thursday'),
  $t('friday'),
  $t('saturday'),
];

const months = [
  "stycznia",
  "lutego",
  "marca",
  "kwietnia",
  "maja",
  "czerwca",
  "lipca",
  "sierpnia",
  "września",
  "października",
  "listopada",
  "grudnia",
];

const updateCurrentDate = () => {
  const today = new Date();
  const dayOfWeek = today.getDay();
  const dayOfMonth = today.getDate();
  const month = today.getMonth();

  currentDay.value = daysOfWeek[dayOfWeek];
  currentDate.value = `${dayOfMonth} ${months[month]}`;
};

const loadBookings = async () => {
  try {
    await bookingStore.fetchBookings();
    bookings.value = bookingStore.bookings.slice(0, bookingsPerPage.value);
  } catch (error) {
    console.error("Error loading bookings:", error);
  }
};

const loadRevenues = async () => {
  try {
    await store.fetchRevenues();
    revenues.value = store.revenues.slice(0, revenuePerPage.value);
  } catch (error) {
    console.error("Error loading revenues:", error);
  }
};

const previousPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    const startIndex = (currentPage.value - 1) * bookingsPerPage.value;
    bookings.value = bookingStore.bookings.slice(
      startIndex,
      startIndex + bookingsPerPage.value
    );
  }
};

const nextPage = () => {
  if (currentPage.value < 2) {
    currentPage.value++;
    const startIndex = (currentPage.value - 1) * bookingsPerPage.value;
    bookings.value = bookingStore.bookings.slice(
      startIndex,
      startIndex + bookingsPerPage.value
    );
  }
};

const goToPage = (page: number) => {
  currentPage.value = page;
  const startIndex = (page - 1) * bookingsPerPage.value;
  bookings.value = bookingStore.bookings.slice(
    startIndex,
    startIndex + bookingsPerPage.value
  );
};

const revenuePreviousPage = () => {
  if (revenueCurrentPage.value > 1) {
    revenueCurrentPage.value--;
    const startIndex = (revenueCurrentPage.value - 1) * revenuePerPage.value;
    revenues.value = store.revenues.slice(
      startIndex,
      startIndex + revenuePerPage.value
    );
  }
};

const revenueNextPage = () => {
  if (revenueCurrentPage.value < 2) {
    revenueCurrentPage.value++;
    const startIndex = (revenueCurrentPage.value - 1) * revenuePerPage.value;
    revenues.value = store.revenues.slice(
      startIndex,
      startIndex + revenuePerPage.value
    );
  }
};

const revenueGoToPage = (page: number) => {
  revenueCurrentPage.value = page;
  const startIndex = (page - 1) * revenuePerPage.value;
  revenues.value = store.revenues.slice(
    startIndex,
    startIndex + revenuePerPage.value
  );
};

onMounted(async () => {
  updateCurrentDate();
  await store.fetchData();
  await loadBookings();
  await loadRevenues();
});
</script>

<style lang="scss" scoped>
.info-card {
  border-left: 3px solid #3b7ddd;
}

.pagination {
  margin-top: 20px;
}

.card-title {
  margin-bottom: 0.5rem;
}

.chart {
  height: 300px;
}

.text-center {
  text-align: center;
}
</style>
