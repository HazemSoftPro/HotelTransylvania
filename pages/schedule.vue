<template>
  <div>
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>Grafik rezerwacji</h3>
      </div>
    </div>

    <div
      v-if="isLoaded"
      class="table-responsive"
      style="overflow-y: auto; overflow-x: auto; height: 900px"
    >
      <table class="table table-bordered">
        <thead class="thead-light">
          <tr>
            <th>{{ $t('room') }}</th>
            <th v-for="date in dateRange" :key="date">{{ date }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="room in rooms" :key="room.id" class="align-middle">
            <td class="text-center">{{ room.number }}</td>
            <td v-for="date in dateRange" :key="date">
              <template v-if="getBooking(room.id, date)">
                <td
                  class="reservation"
                  :style="getCellStyle(getBooking(room.id, date)?.status)"
                >
                  {{ getBooking(room.id, date)?.id }}
                  <i
                    v-if="getBooking(room.id, date)?.status === 'PENDING'"
                    class="bi bi-hourglass-split"
                  ></i>
                  <i
                    v-if="getBooking(room.id, date)?.status === 'CONFIRMED'"
                    class="bi bi-check2-circle"
                  ></i>
                  <i
                    v-if="getBooking(room.id, date)?.status === 'GUARANTEED'"
                    class="bi bi-shield-lock"
                  ></i>
                  <i
                    v-if="getBooking(room.id, date)?.status === 'CHECKED_IN'"
                    class="bi bi-person-check"
                  ></i>
                </td>
              </template>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="legend">
      <h3>Legenda:</h3>
      <div>
        <i class="bi bi-hourglass-split icon-large"></i> - {{ $t('pending') }}
        <i class="bi bi-check2-circle icon-large"></i> - Potwierdzona
        <i class="bi bi-shield-lock icon-large"></i> - Gwarantowana
        <i class="bi bi-person-check icon-large"></i> - Meldunek
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
const bookingStore = useMyBookingsStore();
const roomStore = useMyRoomStore();

const isLoaded = ref(false);

const rooms = computed(() => roomStore.rooms);
const bookings = computed(() => {
  return bookingStore.bookings.filter((booking) =>
    ["PENDING", "CONFIRMED", "GUARANTEED", "CHECKED_IN"].includes(
      booking.status,
    ),
  );
});

function getCellStyle(status: string | undefined) {
  let color;
  switch (status) {
    case "PENDING":
      color = "grey";
      break;
    case "CONFIRMED":
      color = "blue";
      break;
    case "GUARANTEED":
      color = "green";
      break;
    case "CHECKED_IN":
      color = "red";
      break;
    default:
      color = "white";
  }
  return { backgroundColor: color };
}

function getBooking(roomId: number, date: string) {
  const targetDate = new Date(date);
  const booking = bookings.value.find(
    (booking) =>
      booking.roomId === roomId &&
      new Date(booking.startDate) <= targetDate &&
      new Date(booking.endDate) >= targetDate,
  );
  // console.log(`Checking booking for room ${roomId} on date ${date}:`, booking);
  return booking;
}

function generateDateRange(start: Date, end: Date): string[] {
  const dateRange: string[] = [];
  const currentDate = new Date(start);

  while (currentDate <= end) {
    dateRange.push(formatDate(currentDate));
    currentDate.setDate(currentDate.getDate() + 1);
  }

  return dateRange;
}

// Calculate the maximum end date from the bookings
const maxEndDate = computed(() => {
  return bookings.value.reduce((max, booking) => {
    const endDate = new Date(booking.endDate);
    return endDate > max ? endDate : max;
  }, new Date());
});

// Generate date range from today to the maximum end date
const dateRange = computed(() => {
  return generateDateRange(new Date(), maxEndDate.value);
});

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

onMounted(async () => {
  await bookingStore.fetchBookings(
    undefined,
    undefined,
    undefined,
    undefined,
    startDate.value,
  );
  await roomStore.fetchRooms();
  isLoaded.value = true;
});
</script>

<style>
.reservation {
  border-radius: 5px;
  padding: 5px;
  margin-bottom: 5px;
  color: #fff;
  min-width: 80px;
  text-align: center;
}
.icon-large {
  font-size: 1.5em;
}
</style>
