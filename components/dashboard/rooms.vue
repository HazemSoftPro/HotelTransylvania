<template>
  <Pie :data="chartData" :options="chartOptions" />
</template>

<script lang="ts" setup>
import { Pie } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  CategoryScale,
} from "chart.js";
import { ref } from "vue";

ChartJS.register(Title, Tooltip, Legend, ArcElement, CategoryScale);

const store = useMyDashboardStore();

const roomStatusCount = computed(() => {
  const statusCount = {
    CLEAN: 0,
    DIRTY: 0,
    SERVICE: 0,
    OCCUPIED: 0,
    ARRIVAL: 0,
  };

  if (Array.isArray(store.rooms)) {
    store.rooms.forEach((room) => {
      statusCount[room.status] += 1;
    });
  }

  return [
    statusCount["CLEAN"],
    statusCount["DIRTY"],
    statusCount["SERVICE"],
    statusCount["OCCUPIED"],
    statusCount["ARRIVAL"],
  ];
});

const chartData = ref({
  labels: ["Czysty", "Brudny", "Serwis", "Pobyt", "Przyjazd"],
  datasets: [
    {
      data: roomStatusCount.value,
      backgroundColor: ["#4CAF50", "#F44336", "#FFEB3B", "#3F51B5", "#9C27B0"],
    },
  ],
});

const chartOptions = ref({
  maintainAspectRatio: false,
});
</script>

<style></style>
