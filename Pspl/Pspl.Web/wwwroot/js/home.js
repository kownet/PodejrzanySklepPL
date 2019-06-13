APP.home = (function () {

    var checkStore = function (opts) {

        $(document).on('click', opts.btn, function () {

            var url = $(opts.cnt).val();

            if (url === "") {

                swal("Błąd", "Podaj adres sklepu do sprawdzenia", "error");

            } else {

                var params = JSON.stringify({ url: url });

                $.ajax({
                    contentType: 'application/json',
                    type: 'POST',
                    url: opts.url,
                    data: params,
                    success: function (data) {

                        if (data.isSuspicious) {
                            swal("Uwaga", "Sklep jest podejrzany", "warning");
                        }
                        else {
                            swal("OK", "Tego sklepu nie ma na liście podejrzanych", "success");
                        }
                    }
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