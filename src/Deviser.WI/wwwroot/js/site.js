// Write your Javascript code.
(function () {
    $(function () {

        $('.popup-video').magnificPopup({
            disableOn: 0,
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            fixedContentPos: false,
            showCloseBtn: true
        });

        $(".navbar-nav .dropdown").hover(function () {
            $(this).addClass("show");
        }, function () {
            $(this).removeClass("show");
        });

        var $menu = $('nav#mainNav')

        $menu.mmenu({
            dragOpen: true,
            navbar: {
                add: true
            },
            extensions: ["position-right"],
            onClick: {
                close: true
            },
            hooks: {
                "open:finish": function ($panel) {
                    //console.log("This panel is now opening: #" + $panel.attr("id"));
                    $('.hamburger').addClass('is-active');
                },
                "close:finish": function ($panel) {
                    //console.log("This panel is now opened: #" + $panel.attr("id"));
                    $('.hamburger').removeClass('is-active');
                }
            }
        });
    })

})();
