(function () {

    /*Event bindings*/
    $("form.sd-form").on("submit", formSubmit);
    $("a.sd-ajax-load").on("click", ajaxAnchorClick)

    //////////////////////////////////
    /*Function declarations only*/
    function formSubmit(event) {
        event.preventDefault();
        var $form = $(this),
        url = $form.attr('action'),
        fromData = $form.serialize(),
        formMethod = $form.attr('method').toUpperCase();

        console.log(fromData);

        var request = $.ajax({
            type: formMethod,
            url: url,
            data: fromData, // serializes the form's elements.
            headers: {
                IsAjaxRequest: true
            }
        });

        request.done(function (data, textStatus, jqXHR) {
            console.log(data); // show response from the server.

            if (typeof (data) === 'object') {
                // It is JSON
                console.log(data);
                if (data.value && data.statusCode === 302) {
                    //MVC Redirecting to another page
                    window.location = data.value;
                }
            }
            else {
                var $formContainer = $form.closest('.sd-module-container');
                $form.remove();
                //$formContainer.empty();
                $formContainer.html(data);
                $("form.sd-form").on("submit", formSubmit);
            }
        });

        request.fail(function (jqXHR, textStatus) {
            console.log(jqXHR + '' + textStatus);
        });
    }

    function ajaxAnchorClick(event) {
        event.preventDefault();
        var $a = $(this),
            url = $a.attr('href');

        var request = $.ajax({
            type: 'GET',
            url: url,
            headers: {
                IsAjaxRequest: true
            }
        });

        request.done(function (data, textStatus, jqXHR) {
            console.log(data); // show response from the server.    
            var $formContainer = $a.closest('.sd-module-container');
            //$formContainer.empty();
            $formContainer.html(data);
            $("form.sd-form").on("submit", formSubmit);
        });

        request.fail(function (jqXHR, textStatus) {
            console.log(jqXHR + '' + textStatus);
        });

    }
})();