$(function () {

    $('#side-menu').metisMenu();



    var overlayScrollbarsFn = $('.overlay-scroll').overlayScrollbars;
    if (overlayScrollbarsFn) {
        overlayScrollbarsFn({});
    }
    

    $('.nav-tabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

});