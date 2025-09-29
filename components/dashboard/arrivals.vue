<template>
  <table v-if="store.arrivals.length > 0" class="table">
    <thead>
      <tr style="position: sticky; top: 0; background: white; z-index: 1">
        <th>{{ $t('guest') }}</th>
        <th class="text-right">{{ $t('room') }}</th>
        <th class="text-right">Status</th>
      </tr>
    </thead>
    <tr
      v-for="(bookings, index) in store.arrivals"
      :key="index"
      :class="{ 'bg-light': index % 2 === 1 }"
    >
      <td>{{ bookings.guest.name + " " + bookings.guest.name }}</td>
      <td class="text-right">{{ bookings.room.number }}</td>
      <td class="text-right">
        <i
          v-if="bookings.status === 'CHECKED_IN'"
          class="material-icons-sharp"
          style="color: #2ca58d"
          >check_circle</i
        >
      </td>
    </tr>
  </table>
  <div v-else class="d-flex justify-content-center align-items-center m-3 p-3">
    {{ $t('no_arrivals_today') }}
  </div>
</template>

<script lang="ts" setup>
const store = useMyDashboardStore();
</script>

<style></style>
