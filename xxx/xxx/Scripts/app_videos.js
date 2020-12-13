(function (app, $) {
    var model;

    app.pagos = app.pagos || {};

    var PagoFormModel = function (data) {
        var self = this;
        var mapping = {
            'ListaFacturas': {
                create: function (options) {
                    return new FacturaViewModel(options.data);
                }
            },
            'Pago': {
                create: function (options) {
                    return new PagoViewModel(options.data, self);
                }
            },
        };

        ko.mapping.fromJS(data, mapping, this);


        self.guardar = function (form) {
            $('#submit-btn').button('loading');
            var url = form.action;
            var data = ko.toJSON({ pago: self.Pago, facturas: self.ListaFacturas });
            $.ajax({
                url: form.action,
                data: data,
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                error: function (xhr, status, error) {
                    jack.handleAjaxError(xhr, status, error);
                },
                success: function (data) {
                    jack.closeModal();
                    jack.showSuccessMessage("hola");
                },
                complete: function () {
                    $('#submit-btn').button('reset');
                }
            });
        };
    };

    var FacturaViewModel = function (data) {
        var self = this;
        var mapping = {};

        ko.mapping.fromJS(data, mapping, this);

        this.PorPagar = ko.computed(function () {
            return self.TotalFactura() - self.Pagado();
        }, this);
    };

    var PagoViewModel = function (data, parent) {

        var self = this;

        ko.mapping.fromJS(data, {}, this);

        this.Valor = ko.computed(function () {
            var total = 0;
            ko.utils.arrayForEach(parent.ListaFacturas(), function (item) {
                item.Abono(parseFloat((item.fac_seleccionada() === true ? item.Abono() : item.PorPagar())));

                total += item.fac_seleccionada() === true ? item.Abono() : 0;
            });
            return total;
        }, this);
    };


    app.pagos.init = function () {
        $(document).ready(function () {
        });
    };

    app.pagos.setModel = function (jsModel) {
        var form = $("#formPago");
        model = new PagoFormModel(jsModel);
        ko.applyBindings(model, form[0]);

        form.on("submit", function (e) {
            model.guardar(form[0]);
            e.preventDefault();
        });
    };

    app.pagos.init();

}(window.jack = window.jack || {}, jQuery));