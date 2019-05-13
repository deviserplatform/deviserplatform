$(function () {

    $('#side-menu').metisMenu();

    $('#menu1').metisMenu();

    $('.nav-tabs a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });

});