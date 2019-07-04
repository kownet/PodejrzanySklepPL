APP.home = (function () {

    var checkStore = function (opts) {

        $(opts.spinner).hide();

        $(document).on('click', opts.btn, function () {

            var url = $(opts.cnt).val();

            if (url === "") {

                swal("Błąd", "Podaj adres sklepu do sprawdzenia", "error");

            } else {

                $(opts.spinner).show();

                var params = JSON.stringify({ url: url });

                $.ajax({
                    contentType: 'application/json',
                    type: 'POST',
                    url: opts.url,
                    data: params,
                    success: function (data) {

                        if (data.isSuspicious) {
                            swal("Uwaga", data.description, "warning");
                        }
                        else {
                            swal("OK", "Tego sklepu nie ma na liście podejrzanych", "success");
                        }
                    }
                }).done(function () {

                    $(opts.spinner).hide();

                });

            }

        });

    };

    return {
        checkStore: function (opts) {
            checkStore(opts);
        }
    };

})();