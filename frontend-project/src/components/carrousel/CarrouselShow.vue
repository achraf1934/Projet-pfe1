<script lang="ts">
export default {
  name: 'CarrouselShow'
}
</script>
<template>
  <div class="container-fluid p-0">
    <div class="owl-carousel header-carousel position-relative owl-loaded owl-drag">
      <div class="owl-stage-outer">
        <div
          class="owl-stage"
          style="
            transform: translate3d(-3960px, 10px, 0px);
            transition: all 1.5s ease 0s;
            width: 7920px;
          "
        >
          <div class="owl-item cloned" style="width: 1320px"></div>
          <div class="owl-item cloned" style="width: 1320px"></div>
          <div class="owl-item" style="width: 1320px"></div>
          <div class="owl-item active" style="width: 1320px">
            <div class="owl-carousel-item position-relative">
              <img class="img-fluid" src="/src/assets/img/istockphoto.jpg" alt="" />
              <div
                class="position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center"
                style="background: rgba(43, 57, 64, 0.5)"
              >
                <div class="container">
                  <div class="row justify-content-start">
                    <div class="col-10 col-lg-8">
                      <h1 class="display-3 text-white animated slideInDown mb-4">
                        Find The Best Startup Job That Fit You
                      </h1>
                  
                      <router-link to="/jobs" class="btn btn-primary py-md-3 px-md-5 me-3 animated slideInLeft"
                        >Search A Job</router-link
                      >
                   
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style></style>
