<template>
  <div class="wrapper">
    <Sidebar />
    <div class="main">
      <Header />
      <main class="content">
        <div class="container-fluid p-0">
          <slot />
        </div>
      </main>
      <footer class="footer">
        <div class="container-fluid">
          <div class="row text-muted">
            <div class="col-6 text-start">
              <p class="mb-0">
                <span class="text-muted">HMSuite</span>
              </p>
            </div>
            <div class="col-6 text-end">
              <ul class="list-inline">
                <li class="list-inline-item">
                  <a
                    class="text-muted"
                    href="https://github.com/kbcode85"
                    target="_blank"
                    >Github</a
                  >
                </li>
              </ul>
            </div>
          </div>
        </div>
      </footer>
    </div>
    <SlideoutpanelGuest />
    <ModalGuest />
    <ModalRoom />
    <ModalBookings />
  </div>
</template>

<script lang="ts" setup></script>

<style lang="scss">
.wrapper {
  align-items: stretch;
  display: flex;
  width: 100%;
  background: #1d2d40;
}

.main {
  display: flex;
  width: 100%;
  min-width: 0;
  min-height: 100vh;
  transition:
    margin-left 0.35s ease-in-out,
    left 0.35s ease-in-out,
    margin-right 0.35s ease-in-out,
    right 0.35s ease-in-out;
  background: #f5f7fb;
  flex-direction: column;
  overflow: hidden;
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
}

.content {
  padding: 1.5rem 1.5rem 0.75rem;
  flex: 1;
  width: 100vw;
  max-width: 100vw;
  direction: ltr;

  @media (min-width: 768px) {
    width: auto;
    max-width: auto;
  }

  @media (min-width: 992px) {
    padding: 3rem 3rem 1.5rem;
  }
}

footer.footer {
  padding: 1rem * 0.875;
  background: #fff !important;

  ul {
    margin-bottom: 0;
  }

  @media (max-width: 767px) {
    width: 100vw;
  }
}
</style>
