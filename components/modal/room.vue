<template>
  <div class="overlay" :class="{ active: isOpen }">
    <div
      class="modal fade"
      :class="{ show: isOpen }"
      :style="{ display: isOpen ? 'block' : 'none' }"
    >
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div v-if="action === 'info'">
            <div class="modal-header">
              <h5 class="modal-title">{{ $t('room_info') }} {{ room.number }}</h5>
              <button
                type="button"
                class="btn-close"
                @click="store.closeModal"
              ></button>
            </div>
            <div class="modal-body">
              <div v-if="room">
                <p class="card-text">
                  <strong>{{ $t('equipment') }} </strong>
                  <span class="m-1">
                    <span v-if="equipment.hasWifi" class="icon-with-text">
                      <span class="material-icons-sharp">wifi</span> Wi-Fi
                    </span>
                    <span v-if="equipment.hasTV" class="icon-with-text">
                      <span class="material-icons-sharp">tv</span> TV
                    </span>
                    <span v-if="equipment.hasKitchen" class="icon-with-text">
                      <span class="material-icons-sharp">kitchen</span> {{ $t('kitchen') }}
                    </span>
                    <span v-if="equipment.hasFridge" class="icon-with-text">
                      <span class="material-icons-sharp">kitchen</span> {{ $t('fridge') }}
                    </span>
                    <span v-if="equipment.hasBalcony" class="icon-with-text">
                      <span class="material-icons-sharp">deck</span> {{ $t('balcony') }}
                    </span>
                    <span
                      v-if="equipment.hasAirConditioning"
                      class="icon-with-text"
                    >
                      <span class="material-icons-sharp">ac_unit</span>
                      {{ $t('air_conditioning') }}
                    </span>
                    <span v-if="equipment.hasWardrobe" class="icon-with-text">
                      <span class="material-icons-sharp">checkroom</span> {{ $t('wardrobe') }}
                    </span>
                    <span v-if="equipment.hasHairDryer" class="icon-with-text">
                      <span class="material-icons-sharp">dry</span> {{ $t('hair_dryer') }}
                    </span>
                    <span
                      v-if="equipment.hasCoffeeAndTeaSet"
                      class="icon-with-text"
                    >
                      <span class="material-icons-sharp">coffee_maker</span>
                      {{ $t('coffee_tea_set') }}
                    </span>
                    <span v-if="equipment.hasCosmetics" class="icon-with-text">
                      <span class="material-icons-sharp">spa</span> {{ $t('cosmetics') }}
                    </span>
                    <span v-if="equipment.hasTowels" class="icon-with-text">
                      <span class="material-icons-sharp">clean_hands</span>
                      {{ $t('towels') }}
                    </span>
                  </span>
                </p>
                <p
                  v-if="equipment.hasBathroom"
                  class="card-text d-flex flex-column"
                >
                  <strong>{{ $t('bathroom') }} </strong>
                  <span
                    v-if="equipment.bathroomType === 'SHOWER'"
                    class="icon-with-text"
                  >
                    <span class="material-icons-sharp">shower</span> {{ $t('shower') }}
                  </span>
                  <span
                    v-if="equipment.bathroomType === 'BATHTUB'"
                    class="icon-with-text"
                  >
                    <span class="material-icons-sharp">bathtub</span> {{ $t('bathtub') }}
                  </span>
                  <span
                    v-if="equipment.bathroomType === 'BOTH'"
                    class="icon-with-text"
                  >
                    <span class="material-icons-sharp">bathtub shower</span>
                    {{ $t('bathtub_and_shower') }}
                  </span>
                </p>
                <p class="card-text d-flex flex-column">
                  <strong>{{ $t('beds') }}</strong>
                  <span v-if="equipment.singleBeds" class="icon-with-text">
                    <span class="material-icons-sharp">single_bed</span>
                    {{ equipment.singleBeds }} {{ $t('single_bed') }}
                  </span>
                  <span v-if="equipment.doubleBeds" class="icon-with-text">
                    <span class="material-icons-sharp">king_bed</span>
                    {{ equipment.doubleBeds }} {{ $t('double_bed') }}
                  </span>
                </p>
              </div>
              <div v-else>
                <p>{{ $t('room_not_found') }}</p>
              </div>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                {{ $t('close') }}
              </button>
            </div>
          </div>

          <div v-if="action === 'delete'">
            <div class="modal-header">
              <h5 class="modal-title">{{ $t('delete_room') }}</h5>
              <button
                type="button"
                class="btn-close"
                @click="store.closeModal"
              ></button>
            </div>
            <div class="modal-body">
              <p>{{ $t('confirm_delete_room', { number: room.number }) }}</p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                {{ $t('no') }}
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="store.deleteRoom(room.id)"
              >
                {{ $t('yes') }}
              </button>
            </div>
          </div>

          <div v-if="action === 'clean'">
            <div class="modal-header">
              <h5 class="modal-title">{{ $t('change_status') }}</h5>
              <button
                type="button"
                class="btn-close"
                @click="store.closeModal"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                {{ $t('confirm_change_room_status', { number: room.number }) }}
                {{ $t('clean') }} ?
              </p>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                {{ $t('no') }}
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="store.editRoom(room.id, RoomStatus.CLEAN)"
              >
                {{ $t('yes') }}
              </button>
            </div>
          </div>

          <div v-if="action === 'edit'">
            <div class="modal-header">
              <h5 class="modal-title">{{ $t('edit_room') }}</h5>
              <button
                type="button"
                class="btn-close"
                @click="store.closeModal"
              ></button>
            </div>
            <div class="modal-body">
              <form>
                <div class="col-12 mb-3">
                  <label for="name">{{ $t('room_number') }}:</label>
                  <input
                    id="name"
                    v-model="room.number"
                    type="text"
                    class="form-control"
                    required
                  />
                </div>
                <div class="col-12 mb-3">
                  <label for="type">{{ $t('room_type') }}:</label>
                  <select
                    id="type"
                    v-model="room.type"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_room_type') }}</option>
                    <option
                      v-for="types in roomStandards"
                      :key="types"
                      :value="types"
                    >
                      {{ translateStandard(types) }}
                    </option>
                  </select>
                </div>

                <div class="col-12 mb-3">
                  <label for="maxGuests">{{ $t('max_guests') }}:</label>
                  <input
                    id="maxGuests"
                    v-model.number="room.maxGuests"
                    type="number"
                    min="1"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="pricePerNight">{{ $t('price_per_night') }}:</label>
                  <input
                    id="pricePerNight"
                    v-model.number="room.pricePerNight"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="status">{{ $t('room_status') }}:</label>
                  <select
                    id="status"
                    v-model="room.status"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_room_status') }}</option>
                    <option
                      v-for="status in roomStatuses"
                      :key="status"
                      :value="status"
                    >
                      {{ translateStatus(status) }}
                    </option>
                  </select>
                </div>

                <div class="col-12 mb-3">
                  <label for="singleBeds">{{ $t('single_beds_count') }}:</label>
                  <input
                    id="singleBeds"
                    v-model.number="equipment.singleBeds"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="doubleBeds">{{ $t('double_beds_count') }}:</label>
                  <input
                    id="doubleBeds"
                    v-model.number="equipment.doubleBeds"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasTV">{{ $t('has_tv') }}</label>
                  <input id="hasTV" v-model="equipment.hasTV" type="checkbox" />
                </div>

                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasWifi">{{ $t('has_wifi') }}</label>
                  <input
                    id="hasWifi"
                    v-model="equipment.hasWifi"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasBathroom">{{ $t('has_bathroom') }}</label>
                  <input
                    id="hasBathroom"
                    v-model="equipment.hasBathroom"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasKitchen">{{ $t('has_kitchen') }}</label>
                  <input
                    id="hasKitchen"
                    v-model="equipment.hasKitchen"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasFridge">{{ $t('has_fridge') }}</label>
                  <input
                    id="hasFridge"
                    v-model="equipment.hasFridge"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasBalcony">{{ $t('has_balcony') }}</label>
                  <input
                    id="hasBalcony"
                    v-model="equipment.hasBalcony"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasAirConditioning"
                    >{{ $t('has_ac') }}</label
                  >
                  <input
                    id="hasAirConditioning"
                    v-model="equipment.hasAirConditioning"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasWardrobe">{{ $t('has_wardrobe') }}</label>
                  <input
                    id="hasWardrobe"
                    v-model="equipment.hasWardrobe"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasHairDryer">{{ $t('has_hair_dryer') }}</label>
                  <input
                    id="hasHairDryer"
                    v-model="equipment.hasHairDryer"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasCoffeeAndTeaSet"
                    >{{ $t('has_coffee_tea') }}</label
                  >
                  <input
                    id="hasCoffeeAndTeaSet"
                    v-model="equipment.hasCoffeeAndTeaSet"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasCosmetics">{{ $t('has_cosmetics') }}</label>
                  <input
                    id="hasCosmetics"
                    v-model="equipment.hasCosmetics"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasTowels">{{ $t('has_towels') }}</label>
                  <input
                    id="hasTowels"
                    v-model="equipment.hasTowels"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3">
                  <label for="bathroomType">{{ $t('bathroom_type') }}:</label>
                  <select
                    id="bathroomType"
                    v-model="equipment.bathroomType"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_bathroom_type') }}</option>
                    <option
                      v-for="types in bathroomTypes"
                      :key="types"
                      :value="types"
                    >
                      {{ types }}
                    </option>
                  </select>
                </div>
              </form>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                {{ $t('cancel') }}
              </button>
              <button
                type="button"
                class="btn btn-success"
                @click="store.editRoom(room.id)"
              >
                {{ $t('edit') }}
              </button>
            </div>
          </div>

          <div v-if="action === 'add'">
            <div class="modal-header">
              <h5 class="modal-title">{{ $t('add_room') }}</h5>
              <button
                type="button"
                class="btn-close"
                @click="store.closeModal"
              ></button>
            </div>
            <div class="modal-body">
              <form>
                <div class="col-12 mb-3">
                  <label for="name">{{ $t('room_number') }}:</label>
                  <input
                    id="name"
                    v-model="room.number"
                    type="text"
                    class="form-control"
                    required
                  />
                </div>
                <div class="col-12 mb-3">
                  <label for="type">{{ $t('room_type') }}:</label>
                  <select
                    id="type"
                    v-model="room.type"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_room_type') }}</option>
                    <option
                      v-for="types in roomStandards"
                      :key="types"
                      :value="types"
                    >
                      {{ translateStandard(types) }}
                    </option>
                  </select>
                </div>

                <div class="col-12 mb-3">
                  <label for="maxGuests">{{ $t('max_guests') }}:</label>
                  <input
                    id="maxGuests"
                    v-model.number="room.maxGuests"
                    type="number"
                    min="1"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="pricePerNight">{{ $t('price_per_night') }}:</label>
                  <input
                    id="pricePerNight"
                    v-model.number="room.pricePerNight"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="status">{{ $t('room_status') }}:</label>
                  <select
                    id="status"
                    v-model="room.status"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_room_status') }}</option>
                    <option
                      v-for="status in roomStatuses"
                      :key="status"
                      :value="status"
                    >
                      {{ translateStatus(status) }}
                    </option>
                  </select>
                </div>

                <div class="col-12 mb-3">
                  <label for="singleBeds">{{ $t('single_beds_count') }}:</label>
                  <input
                    id="singleBeds"
                    v-model.number="equipment.singleBeds"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3">
                  <label for="doubleBeds">{{ $t('double_beds_count') }}:</label>
                  <input
                    id="doubleBeds"
                    v-model.number="equipment.doubleBeds"
                    type="number"
                    min="0"
                    class="form-control"
                    required
                  />
                </div>

                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasTV">{{ $t('has_tv') }}</label>
                  <input id="hasTV" v-model="equipment.hasTV" type="checkbox" />
                </div>

                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasWifi">{{ $t('has_wifi') }}</label>
                  <input
                    id="hasWifi"
                    v-model="equipment.hasWifi"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasBathroom">{{ $t('has_bathroom') }}</label>
                  <input
                    id="hasBathroom"
                    v-model="equipment.hasBathroom"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasKitchen">{{ $t('has_kitchen') }}</label>
                  <input
                    id="hasKitchen"
                    v-model="equipment.hasKitchen"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasFridge">{{ $t('has_fridge') }}</label>
                  <input
                    id="hasFridge"
                    v-model="equipment.hasFridge"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasBalcony">{{ $t('has_balcony') }}</label>
                  <input
                    id="hasBalcony"
                    v-model="equipment.hasBalcony"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasAirConditioning"
                    >{{ $t('has_ac') }}</label
                  >
                  <input
                    id="hasAirConditioning"
                    v-model="equipment.hasAirConditioning"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasWardrobe">{{ $t('has_wardrobe') }}</label>
                  <input
                    id="hasWardrobe"
                    v-model="equipment.hasWardrobe"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasHairDryer">{{ $t('has_hair_dryer') }}</label>
                  <input
                    id="hasHairDryer"
                    v-model="equipment.hasHairDryer"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasCoffeeAndTeaSet"
                    >{{ $t('has_coffee_tea') }}</label
                  >
                  <input
                    id="hasCoffeeAndTeaSet"
                    v-model="equipment.hasCoffeeAndTeaSet"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasCosmetics">{{ $t('has_cosmetics') }}</label>
                  <input
                    id="hasCosmetics"
                    v-model="equipment.hasCosmetics"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3 d-flex justify-content-between">
                  <label for="hasTowels">{{ $t('has_towels') }}</label>
                  <input
                    id="hasTowels"
                    v-model="equipment.hasTowels"
                    type="checkbox"
                  />
                </div>
                <div class="col-12 mb-3">
                  <label for="bathroomType">{{ $t('bathroom_type') }}:</label>
                  <select
                    id="bathroomType"
                    v-model="equipment.bathroomType"
                    class="form-control"
                    required
                  >
                    <option disabled value="">{{ $t('select_bathroom_type') }}</option>
                    <option
                      v-for="types in bathroomTypes"
                      :key="types"
                      :value="types"
                    >
                      {{ types }}
                    </option>
                  </select>
                </div>
              </form>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                @click="store.closeModal"
              >
                {{ $t('cancel') }}
              </button>
              <button
                type="button"
                class="btn btn-success"
                @click="store.addRoom()"
              >
                {{ $t('add') }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { RoomStatus, RoomStandard, BathroomType } from "~/server/models/room";
const store = useMyRoomStore();

const isOpen = computed(() => store.$state.isModalOpen);

const action = computed(() => store.activeAction);

const room = computed(() => store.$state.room);

const equipment = computed(() => store.$state.equipment);

const roomStandards = Object.values(RoomStandard);
const roomStatuses = Object.values(RoomStatus);
const bathroomTypes = Object.values(BathroomType);

function translateStatus(status: string) {
  switch (status) {
    case "CLEAN":
      return $t('room_clean');
    case "DIRTY":
      return $t('room_dirty');
    case "SERVICE":
      return $t('room_service');
    case "OCCUPIED":
      return $t('room_occupied');
    case "ARRIVAL":
      return $t('room_arrival');
    default:
      return status;
  }
}

function translateStandard(standard: string) {
  switch (standard) {
    case "ECONOMY":
      return $t('room_economy');
    case "STANDARD":
      return $t('room_standard');
    case "LUXURY":
      return $t('room_luxury');
    case "APARTMENT":
      return $t('room_apartment');
    default:
      return standard;
  }
}
</script>

<style lang="scss">
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: -2;
  opacity: 0;
  transition: opacity 0.35s ease-in-out;
}

.overlay.active {
  opacity: 1;
  z-index: 0;
}
</style>