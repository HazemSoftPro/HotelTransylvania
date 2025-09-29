<template>
  <li class="nav-item dropdown">
    <a
      id="alertsDropdown"
      class="nav-icon dropdown-toggle"
      href="#"
      data-bs-toggle="dropdown"
    >
      <div class="position-relative">
        <i class="align-middle material-icons-sharp">notifications</i>
        <span class="indicator">4</span>
      </div>
    </a>
    <div
      class="dropdown-menu dropdown-menu-lg dropdown-menu-end py-0"
      aria-labelledby="alertsDropdown"
    >
      <div class="dropdown-menu-header">4 Nowe powiadomienia</div>
      <div class="list-group">
        <a href="#" class="list-group-item">
          <div class="row g-0 align-items-center">
            <div class="col-2">
              <i class="text-danger material-icons-sharp">warning</i>
            </div>
            <div class="col-10">
              <div class="text-dark">Lorem ipsum</div>
              <div class="text-muted small mt-1">
                Aliquam ex eros, imperdiet vulputate hendrerit et.
              </div>
              <div class="text-muted small mt-1">30m temu</div>
            </div>
          </div>
        </a>
        <a href="#" class="list-group-item">
          <div class="row g-0 align-items-center">
            <div class="col-2">
              <i class="text-warning material-icons-sharp">notifications</i>
            </div>
            <div class="col-10">
              <div class="text-dark">Lorem ipsum</div>
              <div class="text-muted small mt-1">
                Aliquam ex eros, imperdiet vulputate hendrerit et.
              </div>
              <div class="text-muted small mt-1">2h temu</div>
            </div>
          </div>
        </a>
        <a href="#" class="list-group-item">
          <div class="row g-0 align-items-center">
            <div class="col-2">
              <i class="text-primary material-icons-sharp">home</i>
            </div>
            <div class="col-10">
              <div class="text-dark">Lorem ipsum</div>
              <div class="text-muted small mt-1">
                Aliquam ex eros, imperdiet vulputate hendrerit et.
              </div>
              <div class="text-muted small mt-1">5h temu</div>
            </div>
          </div>
        </a>
        <a href="#" class="list-group-item">
          <div class="row g-0 align-items-center">
            <div class="col-2">
              <i class="text-success material-icons-sharp">person_add_alt</i>
            </div>
            <div class="col-10">
              <div class="text-dark">Lorem ipsum</div>
              <div class="text-muted small mt-1">
                Aliquam ex eros, imperdiet vulputate hendrerit et.
              </div>
              <div class="text-muted small mt-1">14h temu</div>
            </div>
          </div>
        </a>
      </div>
      <div class="dropdown-menu-footer">
        <a href="#" class="text-muted">Pokaż wszystkie powiadomienia</a>
      </div>
    </div>
  </li>
</template>

<script lang="ts" setup></script>

<style></style>
