<template>
  <table v-if="stays.length > 0" class="table">
    <thead>
      <tr style="position: sticky; top: 0; background: white; z-index: 1">
        <th>{{ $t('guest') }}</th>
        <th class="text-right">{{ $t('room') }}</th>
      </tr>
    </thead>
    <tr
      v-for="(bookings, index) in stays"
      :key="index"
      :class="{ 'bg-light': index % 2 === 1 }"
    >
      <td>{{ bookings.guest.name + " " + bookings.guest.name }}</td>
      <td class="text-right">{{ bookings.room.number }}</td>
    </tr>
  </table>
  <div v-else class="d-flex justify-content-center align-items-center m-3 p-3">
    {{ $t('no_stays') }}
  </div>
</template>

<script lang="ts" setup>
const store = useMyDashboardStore();

const stays = computed(() =>
  store.stays.filter((stay) => stay.status !== "CHECKED_OUT"),
);
</script>

<style></style>
