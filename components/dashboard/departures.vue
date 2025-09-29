<template>
  <table v-if="staysForToday.length > 0" class="table">
    <thead>
      <tr style="position: sticky; top: 0; background: white; z-index: 1">
        <th>{{ $t('guest') }}</th>
        <th class="text-right">{{ $t('room') }}</th>
        <th class="text-right">Status</th>
      </tr>
    </thead>
    <tr
      v-for="(bookings, index) in staysForToday"
      :key="index"
      :class="{ 'bg-light': index % 2 === 1 }"
    >
      <td>{{ bookings.guest.name + " " + bookings.guest.name }}</td>
      <td class="text-right">{{ bookings.room.number }}</td>
      <td class="text-right">
        <i
          v-if="bookings.status === 'CHECKED_OUT'"
          class="material-icons-sharp"
          style="color: #2ca58d"
          >check_circle</i
        >
      </td>
    </tr>
  </table>
  <div v-else class="d-flex justify-content-center align-items-center m-3 p-3">
    {{ $t('no_departures_today') }}
  </div>
</template>

<script lang="ts" setup>
const store = useMyDashboardStore();

function stripTime(date: Date): Date {
  return new Date(date.getFullYear(), date.getMonth(), date.getDate());
}

const today = stripTime(new Date());

const staysForToday = computed(() =>
  store.stays.filter((booking) => {
    const endDate = stripTime(new Date(booking.endDate));
    return (
      endDate.getTime() === today.getTime() &&
      (booking.status === "CHECKED_IN" || booking.status === "CHECKED_OUT")
    );
  }),
);
</script>

<style></style>
