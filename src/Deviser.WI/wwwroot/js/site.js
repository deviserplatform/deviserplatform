// Write your Javascript code.
(function () {
    $('.popup-video').magnificPopup({
        disableOn: 0,
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        fixedContentPos: false,
        showCloseBtn: true
    });

    //slick carousel
    var mySwiper = new Swiper('.swiper-container', {
        // Optional parameters
        //autoplay: {
        //    delay: 2500,
        //    disableOnInteraction: false,
        //},       
        slidesPerView: 1,
        spaceBetween: 0,        
        direction: 'horizontal',
        loop: true,
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        pagination: {
            el: '.swiper-pagination',
        }
    });

})();
