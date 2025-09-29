<template>
  <div>
    <div class="row mb-2 mb-xl-3">
      <div class="col-auto d-none d-sm-block">
        <h3>{{ $t('guest_book') }}</h3>
      </div>
    </div>

    <div class="row align-items-center">
      <div class="col">
        <button
          class="btn btn-primary me-2"
          aria-label="Edit"
          @click="openPanel('add')"
        >
          <span>{{ $t('new_guest') }}</span>
        </button>
        <input v-model="searchQuery" placeholder="Szukaj" type="input" />
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

    <div class="table-responsive">
      <table class="table table-striped">
        <thead>
          <tr class="align-middle">
            <th class="w-25">
              <div class="d-flex justify-content-between align-items-center">
                Imię
                <div class="d-flex flex-column">
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('name', 'asc') }"
                    @click="sort('name', 'asc')"
                  >
                    arrow_drop_up
                  </span>
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('name', 'desc') }"
                    @click="sort('name', 'desc')"
                  >
                    arrow_drop_down
                  </span>
                </div>
              </div>
            </th>
            <th class="w-25">
              <div class="d-flex justify-content-between align-items-center">
                Nazwisko
                <div class="d-flex flex-column">
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('surname', 'asc') }"
                    @click="sort('surname', 'asc')"
                  >
                    arrow_drop_up
                  </span>
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('surname', 'desc') }"
                    @click="sort('surname', 'desc')"
                  >
                    arrow_drop_down
                  </span>
                </div>
              </div>
            </th>
            <th class="w-25">
              <div class="d-flex justify-content-between align-items-center">
                Telefon
                <div class="d-flex flex-column">
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('phone', 'asc') }"
                    @click="sort('phone', 'asc')"
                  >
                    arrow_drop_up
                  </span>
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('phone', 'desc') }"
                    @click="sort('phone', 'desc')"
                  >
                    arrow_drop_down
                  </span>
                </div>
              </div>
            </th>
            <th class="w-25">
              <div class="d-flex justify-content-between align-items-center">
                Email
                <div class="d-flex flex-column">
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('email', 'asc') }"
                    @click="sort('email', 'asc')"
                  >
                    arrow_drop_up
                  </span>
                  <span
                    class="material-icons-sharp"
                    :class="{ 'text-primary': isSorted('email', 'desc') }"
                    @click="sort('email', 'desc')"
                  >
                    arrow_drop_down
                  </span>
                </div>
              </div>
            </th>
            <th class="w-25">Akcje</th>
          </tr>
        </thead>
        <tbody class="align-middle">
          <tr
            v-for="(guest, index) in guests"
            :key="guest.id"
            :class="{ 'bg-light': index % 2 === 0 }"
          >
            <td>{{ guest.name }}</td>
            <td>{{ guest.surname }}</td>
            <td>{{ guest.phone }}</td>
            <td>{{ guest.email }}</td>
            <td>
              <div class="d-flex justify-content-between align-items-center">
                <button
                  class="btn btn-primary p-1"
                  aria-label="Edit"
                  @click="openPanel('edit', guest.id)"
                >
                  <span class="material-icons-sharp">edit</span>
                </button>
                <button
                  class="btn btn-danger p-1"
                  aria-label="Delete"
                  @click="openModal(guest.id)"
                >
                  <span class="material-icons-sharp">delete</span>
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
            Następna
          </a>
        </li>
      </ul>
    </nav>
  </div>
</template>

<script lang="ts" setup>
import { useMyGuestStore } from "~/stores/guest";

definePageMeta({
  middleware: ["auth"],
});

const store = useMyGuestStore();
const guests = computed(() => store.guests);
const entriesPerPage = ref(10);
const currentPage = ref(1);
const totalPages = computed(() => store.totalPages);
const searchQuery = ref("");

const modalStore = useMyModalStore();
const slideoutStore = useMySlideoutStore();
const { openPanel } = useMySlideoutStore();

// Zmieniamy openModal na funkcję, która przyjmuje id gościa jako argument
const openModal = (guestId: number) => {
  // Tutaj możesz zrobić coś z guestId, na przykład zapisać go do sklepu
  useMyModalStore().openModal(guestId);
};

const sort = (field: string, direction: "asc" | "desc") => {
  if (store.sortField === field && store.sortDirection === direction) {
    store.sortField = null;
    store.sortDirection = null;
  } else {
    store.sortGuests(field, direction);
  }
};

const isSorted = (field: string, direction: "asc" | "desc") => {
  return store.sortField === field && store.sortDirection === direction;
};

onMounted(async () => {
  await store.fetchGuests(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
  );
});

watch([entriesPerPage, currentPage, searchQuery], async () => {
  await store.fetchGuests(
    entriesPerPage.value,
    currentPage.value,
    searchQuery.value,
  );
});

watch(
  [() => store.sortField, () => store.sortDirection],
  async (
    [newSortField, newSortDirection],
    [oldSortField, oldSortDirection],
  ) => {
    if (
      newSortField !== oldSortField ||
      newSortDirection !== oldSortDirection
    ) {
      await store.fetchGuests(
        entriesPerPage.value,
        currentPage.value,
        searchQuery.value,
      );
    }
  },
);

watch(
  () => modalStore.isModalOpen,
  async (newVal, oldVal) => {
    if (!newVal && oldVal) {
      await store.fetchGuests(
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
      await store.fetchGuests(
        entriesPerPage.value,
        currentPage.value,
        searchQuery.value,
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
