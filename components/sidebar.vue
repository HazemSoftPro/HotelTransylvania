<template>
  <nav class="sidebar js-sidebar" :class="{ collapsed: isCollapsed }">
    <div class="sidebar-content">
      <nuxt-link to="/" class="sidebar-brand">
        <img
          src="/assets/img/logo.png"
          alt="Favicon"
          class="bi pe-none me-2"
          width="40"
          height="40"
        />
        <span class="algin-middle">HMSuite</span>
      </nuxt-link>

      <ul class="sidebar-nav">
        <li class="sidebar-header">Menu</li>

        <li class="sidebar-item">
          <NuxtLink to="/" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">dashboard</i>
            <span class="align-middle">Dashboard</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink to="/schedule" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">calendar_month</i>
            <span class="align-middle">Grafik rezerwacji</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink to="/checkinout" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">hotel</i>
            <span class="align-middle">Meldunki</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink to="/bookings" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">event</i>
            <span class="align-middle">Rezerwacje</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink to="/guest" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">people</i>
            <span class="align-middle">{{ $t('guest_book') }}</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink to="/rooms" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">bed</i>
            <span class="align-middle">Pokoje</span>
          </NuxtLink>
        </li>

        <li class="sidebar-item">
          <NuxtLink
            to="/housekeeping"
            class="sidebar-link"
            active-class="active"
          >
            <i class="align-middle material-icons-sharp">cleaning_services</i>
            <span class="align-middle">Housekeeping</span>
          </NuxtLink>
        </li>

        <!-- <li class="sidebar-item">
          <NuxtLink to="/invoices" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">receipt_long</i>
            <span class="align-middle">Faktury</span>
          </NuxtLink>
        </li> -->

        <!-- <li class="sidebar-item">
          <NuxtLink to="/reports" class="sidebar-link" active-class="active">
            <i class="align-middle material-icons-sharp">show_chart</i>
            <span class="align-middle">Raporty</span>
          </NuxtLink>
        </li> -->
      </ul>
    </div>
  </nav>
</template>

<script lang="ts" setup>
const sidebar = useSidebarStore();

const isCollapsed = computed(() => sidebar.$state.isCollapsed);
</script>

<style lang="scss">
.sidebar {
  min-width: 260px;
  max-width: 260px;
  transition:
    margin-left 0.35s ease-in-out,
    left 0.35s ease-in-out,
    margin-right 0.35s ease-in-out,
    right 0.35s ease-in-out;
  background: #1d2d40;

  &-content {
    transition:
      margin-left 0.35s ease-in-out,
      left 0.35s ease-in-out,
      margin-right 0.35s ease-in-out,
      right 0.35s ease-in-out;
    display: flex;
    height: 100vh;
    flex-direction: column;
    background: #1d2d40;
  }

  &-brand {
    font-weight: 600;
    font-size: 1.15rem;
    padding: 1.15rem 1.5rem;
    display: block;
    color: #f8f9fa;
    text-decoration: none;

    &:hover {
      text-decoration: none;
      color: #f8f9fa;
    }

    &:focus {
      outline: 0;
    }
  }

  &-nav {
    padding-left: 0;
    margin-bottom: 0;
    list-style: none;
    flex-grow: 1;
  }

  &-header {
    background: transparent;
    padding: 1.5rem 1.5rem 0.375rem;
    font-size: 0.75rem;
    color: #ced4da;
  }

  &-item {
    .active {
      color: #e9ecef;
      background: #{linear-gradient(
          90deg,
          rgba(#3b7ddd, 0.1) 0%,
          rgba(#3b7ddd, 0.0875) 50%,
          rgba(#000, 0) 100%
        )};
      border-left-color: #3b7ddd;

      i,
      span {
        color: #e9ecef;
      }

      &:hover {
        text-decoration: none;
        color: #e9ecef;
        background: #{linear-gradient(
            90deg,
            rgba(#3b7ddd, 0.1) 0%,
            rgba(#3b7ddd, 0.0875) 50%,
            rgba(#000, 0) 100%
          )};
        border-left-color: #3b7ddd;

        i,
        span {
          color: #e9ecef;
        }
      }
    }
  }

  &-link,
  NuxtLink {
    display: block;
    padding: 0.625rem 1.625rem;
    font-weight: 400;
    transition: background 0.1s ease-in-out;
    position: relative;
    text-decoration: none;
    cursor: pointer;
    border-left-style: solid;
    border-left-width: 3px;
    color: rgba(#e9ecef, 0.5);
    background: #1d2d40;
    border-left-color: transparent;

    i,
    span {
      margin-right: 0.75rem;
      color: rgba(#e9ecef, 0.5);
    }

    &:hover {
      text-decoration: none;
      color: rgba(#e9ecef, 0.75);
      background: #1d2d40;
      border-left-color: transparent;

      i,
      span {
        color: rgba(#e9ecef, 0.75);
      }
    }

    &:focus {
      outline: 0;
    }
  }
}

// Toggle states
.sidebar-toggle {
  cursor: pointer;
  width: 26px;
  height: 26px;
  display: flex;
  margin-right: 1rem;
}

.sidebar {
  &.collapsed {
    margin-left: -260px;
  }

  @media (min-width: 1px) and (max-width: 992px) {
    margin-left: -260px;

    &.collapsed {
      margin-left: 0;
    }
  }
}
</style>
