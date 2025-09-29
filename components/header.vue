<template>
  <nav class="navbar navbar-expand navbar-light navbar-bg">
    <a class="sidebar-toggle js-sidebar-toggle" @click="toggleSidebar">
      <i class="hamburger align-self-center"></i>
    </a>

    <div class="navbar-collapse collapse">
      <ul class="navbar-nav navbar-align">
        <!-- <HeaderMessages />
        <HeaderNotifications /> -->

        <li class="nav-item dropdown">
          <a
            class="nav-icon dropdown-toggle d-inline-block d-sm-none"
            href="#"
            data-bs-toggle="dropdown"
          >
            <i class="align-middle material-icons-sharp">settings</i>
          </a>

          <a
            class="nav-link dropdown-toggle d-none d-sm-inline-block"
            href="#"
            data-bs-toggle="dropdown"
          >
            <img
              src="/assets/img/user.png"
              style="width: 40px"
              class="avatar img-fluid rounded me-2"
              alt="user"
            />
            <span class="text-dark">{{ username }}</span>
          </a>
          <div class="dropdown-menu dropdown-menu-end">
            <!-- <NuxtLink to="/profile" class="dropdown-item"
              ><i class="align-middle me-1 material-icons-sharp">person</i>
              Profil</NuxtLink
            >
            <div class="dropdown-divider"></div>
            <NuxtLink to="/settings" class="dropdown-item"
              ><i class="align-middle me-1 material-icons-sharp"
                >manage_accounts</i
              >
              {{ $t('settings_privacy') }}</NuxtLink
            >
            <NuxtLink to="/support" class="dropdown-item"
              ><i class="align-middle me-1 material-icons-sharp">support</i>
              {{ $t('help') }}</NuxtLink
            > -->
            <!-- <div class="dropdown-divider"></div> -->
            <a class="dropdown-item" href="#" @click="logout"
              ><i class="align-middle me-1 material-icons-sharp">logout</i
              >{{ $t('logout') }}</a
            >
          </div>
        </li>
        
        <li class="nav-item">
          <LanguageSwitcher />
        </li>
      </ul>
    </div>
  </nav>
</template>

<script lang="ts" setup>
const store = useAuthStore();
const username = store.data.userdata?.username;

const logout = async () => {
  await store.logout();
};

const sidebar = useSidebarStore();
const toggleSidebar = sidebar.toggleSidebar;
</script>

<style lang="scss">
.navbar {
  border-bottom: 0;
  box-shadow: 0 0 2rem 0 rgba(#000, 0.1);

  @media (max-width: 767px) {
    width: 100vw;
  }
}

.navbar .avatar {
  margin-top: -15px;
  margin-bottom: -15px;
}

.navbar-align {
  margin-left: auto;
}

.navbar-bg {
  background: #fff;
}

.navbar-brand {
  font-weight: 400;
  font-size: 1.15rem;
  padding: 1rem * 0.875 0;
  color: #f8f9fa;
  display: block;

  svg,
  .feather {
    color: #3b7ddd;
    height: 24px;
    width: 24px;
    margin-left: -0.15rem;
    margin-right: 0.375rem;
    margin-top: -0.375rem;
  }
}

.nav-icon,
.nav-flag {
  padding: 0.1rem 0.8rem;
  display: block;
  font-size: 1.5rem;
  color: #6c757d;
  transition:
    background 0.1s ease-in-out,
    color 0.1s ease-in-out;
  line-height: 1.4;

  &:after {
    display: none !important;
  }

  &:hover,
  &.active {
    color: #3b7ddd;
  }

  svg,
  .feather {
    width: 20px;
    height: 20px;
  }
}

.nav-item {
  .indicator {
    background: #3b7ddd;
    box-shadow: 0 0.1rem 0.2rem rgba(#000, 0.05);
    border-radius: 50%;
    display: block;
    height: 18px;
    width: 18px;
    padding: 1px;
    position: absolute;
    top: 0;
    right: -8px;
    text-align: center;
    transition: top 0.1s ease-out;
    font-size: 0.675rem;
    color: #fff;
  }

  &:hover .indicator {
    top: -4px;
  }

  a:focus {
    outline: 0;
  }
}

@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .navbar .avatar {
    max-height: 47px;
  }
}

@media (max-width: 575px) {
  .navbar {
    padding: 0.75rem;
  }

  .nav-icon {
    padding: 0.1rem 0.75rem;
  }

  .dropdown,
  .dropleft,
  .dropright,
  .dropup {
    position: inherit;
  }

  .navbar-expand .navbar-nav .dropdown-menu-lg {
    min-width: 100%;
  }

  .nav-item .nav-link:after {
    display: none;
  }
}

.nav-flag img {
  border-radius: 50%;
  width: 20px;
  height: 20px;
  object-fit: cover;
}

.navbar input {
  direction: ltr;
}

.hamburger,
.hamburger:before,
.hamburger:after {
  cursor: pointer;
  border-radius: 1px;
  height: 3px;
  width: 24px;
  background: #495057;
  display: block;
  content: "";
  transition:
    background 0.1s ease-in-out,
    color 0.1s ease-in-out,
    transform 0.3s ease-in-out;
  position: relative;
}

.hamburger:before,
.hamburger:after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
}

.hamburger:before {
  transform: translateY(-7.5px);
}

.hamburger:after {
  transform: translateY(7.5px);
}

.sidebar-toggle:hover {
  .hamburger {
    background: transparent;
  }

  .hamburger:before,
  .hamburger:after {
    background: #3b7ddd;
    top: 0;
    width: 24px;
  }

  .hamburger:before {
    transform: rotate(45deg);
  }

  .hamburger:after {
    transform: rotate(-45deg);
  }
}
.navbar-nav .dropdown-menu {
  box-shadow: 0 0.1rem 0.2rem rgba(#fff, 0.05);
}

.dropdown .dropdown-menu.show {
  animation-name: dropdownAnimation;
  animation-duration: 0.25s;
  animation-iteration-count: 1;
  animation-timing-function: ease;
  animation-fill-mode: forwards;
}

@keyframes dropdownAnimation {
  from {
    opacity: 0;
    transform: translateY(-8px);
  }
  to {
    opacity: 1;
    transform: translate(0);
  }
}

.dropdown-toggle:after {
  border: solid;
  border-width: 0 2px 2px 0;
  display: inline-block;
  padding: 2px;
  transform: rotate(45deg);
}

.dropdown-item {
  transition:
    0.1s ease-in-out,
    color 0.1s ease-in-out;
}

.dropdown-menu {
  top: auto;
}

.dropdown-menu-lg {
  min-width: 200px * 2;
}

.dropdown .list-group .list-group-item {
  border-width: 0;
  border-bottom-width: 1px;
  margin-bottom: 0;

  &:first-child,
  &:last-child {
    border-radius: 0;
  }

  &:hover {
    background: #f8f9fa;
  }
}

.dropdown-menu-header {
  padding: 1rem * 0.75;
  text-align: center;
  font-weight: 600;
  border-bottom: 1px solid #dee2e6;
}

.dropdown-menu-footer {
  padding: 1rem * 0.5;
  text-align: center;
  display: block;
  font-size: 0.75rem;
}
</style>
