
(function (app, $) {

    app.init = function () {
       // app.setValidatorDefaults();
        $(document).ready(function () {
            app.fixBootstrapModalPadding();
            //app.setLocale();
            app.addModal();
            app.configureAlerts();
            app.configureFilterForm();
            app.disableSecondaryMenuClick();
            //app.setCollapseToggle();
            //app.addSelectPickers();
            //app.addCurrencyMask();
            //app.initPlugins();
            if (app.isTouchDevice()) {
                app.addSubmenuClickEvent();
            }
        });
    };


    app.fixBootstrapModalPadding = function () {
        var oldSSB = $.fn.modal.Constructor.prototype.setScrollbar;
        $.fn.modal.Constructor.prototype.setScrollbar = function () {
            //oldSSB.apply(this);
            //if (this.scrollbarWidth) $('.navbar-fixed-top').css('padding-right', this.scrollbarWidth);
        }

        var oldRSB = $.fn.modal.Constructor.prototype.resetScrollbar;
        $.fn.modal.Constructor.prototype.resetScrollbar = function () {
            //oldRSB.apply(this);
            //$('.navbar-fixed-top').css('padding-right', '');
        }
    };

    app.addSubmenuClickEvent = function () {
        $('li.dropdown-submenu > a').on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $(this).parent().siblings().removeClass('open');
            $(this).parent().toggleClass('open');
        });
    };

    app.initPlugins = function (parent) {
        app.addTooltips(parent);
        app.addContextualDates(parent);
        app.addDatePickers(parent);
        app.addTextboxMaxLength(parent);
        //app.addEditors(parent);
        app.addReportPrintButtons(parent);
        app.addSwitches(parent);
        app.addVideoPlayers(parent);
        app.addViewers(parent);
    };

    app.guid = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    app.getUniqueId = function () {
        var charstoformid = '_0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz'.split('');
        var idlength = Math.floor(Math.random() * charstoformid.length);

        var uniqid = '';
        for (var i = 0; i < length; i++) {
            uniqid += charstoformid[Math.floor(Math.random() * charstoformid.length)];
        }
        // one last step is to check if this ID is already taken by an element before 
        if ($("#" + uniqid).length == 0)
            return uniqid;
        else
            return app.getUniqueId();
    };

    app.getBrowser = function () {
        var nav = 'unknown';

        if (navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0) {
            nav = 'ie';
        }
        else if (navigator.userAgent.indexOf('Chrome') !== -1) {
            nav = 'chrome';
        }
        else if (navigator.userAgent.indexOf('Firefox') !== -1) {
            nav = 'firefox';
        }
        else if (navigator.userAgent.indexOf('Safari') !== -1) {
            nav = 'safari';
        }
        else if (navigator.userAgent.indexOf('Opera') !== -1) {
            nav = 'opera';
        }

        return nav;
    };

    app.isTouchDevice = function () {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    };

    
    app.alert = function (message, title, cb) {
        if (typeof swal !== 'undefined') {
            swal({
                title: title || '',
                text: message,
                //type: type,//"warning", "error", "success" and "info".
                confirmButtonText: 'Aceptar',
                html: true
            }, cb);
        }
    };

    app.confirm = function (message, title, cbSuccess, cbFailed) {
        if (typeof swal !== 'undefined') {
            swal({
                title: title || '',
                text: message,
                //type: type,//"warning", "error", "success" and "info".
                confirmButtonText: 'Aceptar',
                cancelButtonText: "Cancelar",
                showCancelButton: true,
                html: true
            }, function (isConfirmed) {
                if (isConfirmed) {
                    if (cbSuccess) cbSuccess();
                } else {
                    if (cbFailed) cbFailed();
                }
            });
        }
    };




    app.addVideoPlayers = function (parent) {
        if (typeof jwplayer !== 'undefined') {
            var element = parent || "body";
            $(element).find('div[data-type="jwplayer"]').each(function () {
                var $elem = $(this);

                if (!$elem.attr('id')) {
                    $elem.attr('id', app.getUniqueId());
                    setTimeout(function () {
                        jwplayer($elem.attr("id")).setup({
                            file: $elem.data("src"),
                            image: $elem.data("img"),
                            width: $elem.attr("width"),
                            height: $elem.attr("height")
                        });
                    }, 100);
                }
                else {
                    jwplayer($elem.attr("id")).setup({
                        file: $elem.data("src"),
                        image: $elem.data("img"),
                        width: $elem.attr("width"),
                        height: $elem.attr("height")
                    });
                }


            });
        }
    };

    app.addSwitches = function (parent) {
        if (typeof $().bootstrapSwitch !== 'undefined') {
            var element = parent || "body";
            $(element).find('.make-switch').bootstrapSwitch();
        }
    };

    app.addTextboxMaxLength = function (parent) {
        var element = parent || "body";
        $(element).find("input[data-val-length-max]").each(function () {
            var $this = $(this);
            var data = $this.data();
            $this.attr("maxlength", data.valLengthMax);
        });
    };

    app.addDatePickers = function (parent) {
        if (typeof $().datetimepicker !== 'undefined') {
            var element = parent || "body";
            $(element).find(".input-group.date").each(function () {
                var el = $(this);
                el.datetimepicker({
                    language: app.getLocale(),
                    startDate: new moment({ y: 1900 }),
                    //useStrict: true,
                    icons: {
                        time: "fa fa-clock-o",
                        date: "fa fa-calendar",
                        up: "fa fa-arrow-up",
                        down: "fa fa-arrow-down"
                    }
                });
            });
        }
    };

    app.addContextualDates = function (parent) {
        if (typeof moment !== 'undefined') {
            var element = parent || "body";
            $(element).find(".contextual-date").each(function (index, element) {
                var el = $(this);
                var date = el.data("date");
                var title = el.html();
                el.html(moment(date, 'YYYY-MM-DD HH:mm:ss').fromNow());
                el.attr("title", title);
            });
        }
    };

    app.addSelectPickers = function (parent, props, additionalClasses) {
        props = props || { width: '100%' };
        if (typeof $().selectpicker !== 'undefined') {
            var element = parent || "body";

            $(element).find("select:not(.selectize):not(.monthselect):not(.yearselect):not(.plainselect)").each(function () {
                var sel = $(this);
                var firstOpt = sel.children(":first");
                var title = sel.attr("title");
                if (firstOpt.attr("value") === "") {
                    //firstOpt.data("hidden", true);
                    title = firstOpt.text();
                }

                if (sel.data("container")) {
                    props.container = sel.data("container");
                }

                sel.attr("title", title)
                .addClass(additionalClasses || '')
                .removeClass("form-control input-sm")
                .selectpicker(props);

                if (sel.hasClass("force-val")) {
                    sel.on("change blur", function () {
                        $(this).valid();
                    });
                }

                if (sel.attr('data-parent')) {
                    app.bootstrapselect.initSelect(this);
                }
            });
        }
    };

    app.setCollapseToggle = function () {
        $(".collapsible").off("show.bs.collapse hide.bs.collapse").on("show.bs.collapse hide.bs.collapse", function () {
            var id = $(this).attr("id");
            $('*[data-toggle="collapse"][data-target="#' + id + '"] i.fa-chevron-up, *[data-toggle="collapse"][data-target="#' + id + '"] i.fa-chevron-down').toggleClass("fa fa-chevron-up fa fa-chevron-down");
            //$(this).prev().find("h1 > i,h2 > i,h3 > i,h4 > i,h5 > i,h6 > i,span > i").toggleClass("fa fa-chevron-up fa fa-chevron-down");
        });
    };

    app.disableSecondaryMenuClick = function () {
        $("nav.navbar li.dropdown-submenu").on("click", "a[href='#']", function (e) {
            $(this).blur();
            e.stopPropagation();
            e.preventDefault();
        });
    };

    app.setLocale = function () {
        var data = $("meta[name='accept-language']").attr("content");

        $.validator.methods.date = function (value, element) {
            if (value === '' || !isNaN(Globalize.parseDate(value))) {
                return true;
            }

            return false;
        };

        $.validator.methods.number = function (value, element) {
            return this.optional(element) ||
                !isNaN(Globalize.parseFloat(value));
        };

        if (data.toLowerCase() !== "en-us" && data.toLowerCase() !== "en") {
            $.getScript("/Scripts/globalize/cultures/globalize.culture." + data + ".js", function () {
                Globalize.culture(data);

                if ($.datepicker) {
                    $.datepicker.setDefaults($.datepicker.regional[Globalize.culture().name] || $.datepicker.regional[Globalize.culture().language]);
                }
            });

            if (typeof moment !== 'undefined') {
                moment.locale(data);
            }
        }
    };

    app.getLocale = function () {
        var data = $("meta[name='accept-language']").attr("content");
        return data;
    };

    app.setValidatorDefaults = function () {
        // setup defaults for $.validator outside domReady handler
        $.validator.setDefaults({
            ignore: ":hidden:not(.force-val),[data-val-ignore]",
            highlight: function (element) {
                $(element).closest(".form-group, .col-group").addClass("has-error");
            },
            unhighlight: function (element) {
                $(element).closest(".form-group, .col-group").removeClass("has-error");
            }
        });

        $.validator.unobtrusive.adapters.add('datecompare', ['propertytocompare', 'comparetype', 'format'],
            function (options) {
                options.rules['datecompare'] = options.params;
                if (options.message) {
                    options.messages['datecompare'] = options.message;
                }
            }
        );

        $.validator.addMethod('datecompare', function (value, element, params) {
            var valToCompare = $("#" + params.propertytocompare).val();
            var date = moment(value, params.format);
            var dateToCompare = moment(valToCompare, params.format);

            if (!date.isValid() || !dateToCompare.isValid()) return true;

            if (params.comparetype === "Equals") {
                return date.isSame(dateToCompare);
            }
            else if (params.comparetype === "GreaterThan") {
                return date.isAfter(dateToCompare);
            }
            else if (params.comparetype === "LessThan") {
                return date.isBefore(dateToCompare);
            }

            return true;
        }, '');

        $.validator.unobtrusive.adapters.add('decimalplaces', ['separator', 'thousandsseparator', 'mindecimalplaces', 'maxdecimalplaces', 'decimalsproperty'],
            function (options) {
                options.rules['decimalplaces'] = options.params;
                if (options.message) {
                    options.messages['decimalplaces'] = $.validator.format(options.message, "0", $('#' + options.params.decimalsproperty).val());
                }
            }
        );

        $.validator.addMethod('decimalplaces', function (value, element, params) {
            if (value === null || $.trim(value) === '') return true;

            var min = params.mindecimalplaces;
            var max = params.maxdecimalplaces;
            var propdecimal = params.decimalsproperty;

            if (propdecimal !== null && typeof (propdecimal) !== 'undefined') {
                max = $('#' + propdecimal).val();
            }

            var separator = params.separator === '.' ? '\\.' : params.separator;
            var thousandsSeparator = params.thousandsseparator === '.' ? '\\.' : params.thousandsseparator;

            var regex = new RegExp("^(" + (min === "0" ? "(\\d|" + thousandsSeparator + ")+|" : "") + "((\\d|" + thousandsSeparator + ")+" + separator + "\\d{" + min + "," + max + "}))$");

            return regex.test(value);

        }, '');

        jQuery.extend(jQuery.validator.methods, {
            range: function (value, element, param) {
                //Use the Globalization plugin to parse the value
                var val = Globalize.parseFloat(value);
                return this.optional(element) || (val >= param[0] && val <= param[1]);
            }
        });
    };

    app.showMessage = function (type, message, parentElement, sticky) {
        var msg = $("<div class='alert alert-" + type +
            "' style='display:none;'><button type='button' class='close' data-dismiss='alert'>x</button>" +
            message + "</div>");

        var parent = parentElement || $("div.messages > div");

        var func = sticky ? function () { parent.append(msg.fadeIn('slow')); } : function () { parent.append(msg.fadeIn('slow').delay(8000).slideUp('slow', function () { $(this).remove(); })); };

        if (parent.find("div").length > 0) {
            parent.find("div").stop(true).fadeOut('normal', function () {
                func();
                $(this).remove();
            });
        }
        else {
            func();
        }
    };

    app.showSuccessMessage = function (message, parent) {
        app.showMessage("success", message, parent);
    };

    app.showErrorMessage = function (message, parent) {
        app.showMessage("danger", message, parent);
    };

    app.configureAlerts = function () {
        $("div.alert:not(.sticky)").fadeIn('slow').delay(8000).slideUp('slow');
    };

    app.addTooltips = function (parent) {
        var element = parent || "body";
        $(element).find("*[data-toggle='tooltip']").tooltip();
    };
    app.showPleaseWait= function () {
        var pleaseWaitDiv = $('#pleaseWaitDialog');
        var options = {};
        var settings = $.extend({
            dialogSize: 'm',
            progressType: ''
        }, options);
        pleaseWaitDiv.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
        pleaseWaitDiv.find('.progress-bar').attr('class', 'progress-bar');
        if (settings.progressType) {
            $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
        }
        pleaseWaitDiv.find('h3').addClass("text-center");
        pleaseWaitDiv.find('h3').text("Loading...");

        pleaseWaitDiv.modal();

    },
    app.hidePleaseWait = function () {
        var pleaseWaitDiv = $('#pleaseWaitDialog');
        pleaseWaitDiv.modal('hide');
    },

    app.waitingDialog = function ($) {
        'use strict';

        // Creating modal dialog's DOM
        var $dialog = $(
            '<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
            '<div class="modal-dialog modal-m">' +
            '<div class="modal-content">' +
                '<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
                '<div class="modal-body">' +
                    '<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
                '</div>' +
            '</div></div></div>');

        return {
            /**
             * Opens our dialog
             * @param message Custom message
             * @param options Custom options:
             * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
             * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
             */
            show: function (message, options) {
                // Assigning defaults
                if (typeof options === 'undefined') {
                    options = {};
                }
                var settings = $.extend({
                    dialogSize: 'm',
                    progressType: ''
                }, options);
                if (typeof message === 'undefined') {
                    message = 'Loading';
                }
                // Configuring dialog
                $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
                $dialog.find('.progress-bar').attr('class', 'progress-bar');
                if (settings.progressType) {
                    $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
                }
                $dialog.find('h3').text(message);
                // Opening dialog
                $dialog.modal();
            },
            /**
             * Closes dialog
             */
            hide: function () {
                $dialog.modal('hide');
            }
        };

    };
    app.configureFilterForm = function () {
        var form = $("#filters_form,.form-subfiltros");

        $("body").on("click", ".pagination li a", function (e) {
            if (!$(this).parent().hasClass("active")) {
                var page = $(this).data("page");
                var form = $(this).closest("form");
                if (form.length < 1) form = $("#filters_form");
                form.data("caller", "paging");
                form.find("#pagina").val(page);
                form.submit();
            }
            e.preventDefault();
        });

        form
            .on("click", ".filter input[type='checkbox']", function (e) {
                $(this).closest("form").find("#pagina").val(1);
                $(this).closest("form").submit();
            })
            .on("change", "select.filter", function (e) {
                $(this).closest("form").find("#pagina").val(1);
                $(this).closest("form").submit();
            })
            .on("apply", ".filter.daterange, .filter.multi-select", function (e) {
                $(this).closest("form").find("#pagina").val(1);
                $(this).closest("form").submit();
            })
            .on("click", ".filter-open", function (e) {
                var button = $(this);
                var panel = button.closest("form").find('.other-filters');
                button.find("i").toggleClass("fa-angle-double-down fa-angle-double-up");
                panel.slideToggle();
                e.preventDefault();
            })
            .on("click", "a.export", function (e) {
                var link = $(this);
                var url = link.data("url");
                var formData = link.closest("form").serialize();
                formData = (url.indexOf("?") < 0 ? "?" : "&") + formData;
                link.attr("href", url + formData);
            }).on("click", "button[type='submit']", function (e) {
                $(this).closest("form").find("#pagina").val(1);
            });

        //form.find("select.filter:not(.selectize)").selectpicker({ width: '100%' });
        form.find("select.filter.selectize").each(function () {
            app.selectize.initAjaxSelect($(this));
        });

    };

    app.submitFilterForm = function () {
        $("#filters_form").submit();
    };

    app.disableCurrency = function (element) {
        var hdn = $("#" + element);
        var txt = hdn.prev(".currency");
        txt.data("disabled", true);
        txt.val("");
        hdn.val("");
    };

    app.reenableCurrency = function (element) {
        var hdn = $("#" + element);
        var txt = hdn.prev(".currency");
        txt.data("disabled", null);
        txt.val("");
        hdn.val("");
    };

    app.formatCurrency = function (number) {
        var region = app.getLocale();
        var opts = { colorize: false, region: region, roundToDecimalPlace: -2 };
        if (region.toLowerCase() !== "es-co") opts.roundToDecimalPlace = 2;

        return $("<span/>").html(number).formatCurrency(opts).text();
    };

    app.addCurrencyMask = function (parent) {
        var element = parent || "body";
        var region = app.getLocale();

        var opts = { colorize: false, region: region, roundToDecimalPlace: -2 };

        if (region.toLowerCase() !== "es-co") opts.roundToDecimalPlace = -1;

        var setValue = function (el, round) {
            var finalOpts = $.extend({}, opts, round && opts.roundToDecimalPlace === -1 ? { roundToDecimalPlace: 2 } : {});

            var txt = $(el);
            var hdn = txt.next("#" + txt.data("hdn"));

            var different = txt.val() !== hdn.val();
            if (!txt.data("disabled")) {
                if (txt.data("percentage")) { //Custom option for percentage
                    var num = $.trim(txt.val().replace("%", ""));
                    txt.val(num + " %");
                    hdn.val(num);
                }
                else { //Currency
                    txt.formatCurrency(finalOpts);
                    if (txt.val().trim() === $.formatCurrency.regions[finalOpts.region].symbol || new RegExp("^[^\\d" + $.formatCurrency.regions[finalOpts.region].symbol.trim() + "]", "i").test(txt.val().trim())) {
                        txt.val('');
                    }

                    hdn.val(txt.val() === '' ? '' : txt.asNumber({ region: region, parse: false }));
                }
            } else {
                hdn.val(txt.val());
            }

            if (different && round && hdn.length > 0) hdn.valid();
        };

        $(element).find("input.currency").each(function () {
            setValue(this, true);
        });

        $(element).on("blur", "input.currency", function (e) {
            setValue(this, true);
        }).on("keyup", "input.currency", function (e) {
            if (e.keyCode !== undefined && !e.ctrlKey) {
                switch (e.keyCode) {
                    case 16: break; // Shift
                    case 17: break; // Ctrl
                    case 18: break; // Alt
                    case 27: this.value = ''; break; // Esc: clear entry
                    case 35: break; // End
                    case 36: break; // Home
                    case 37: break; // cursor left
                    case 38: break; // cursor up
                    case 39: break; // cursor right
                    case 40: break; // cursor down
                    case 78: break; // N (Opera 9.63+ maps the "." from the number key section to the "N" key too!) (See: http://unixpapa.com/js/key.html search for ". Del")
                    case 110: break; // . number block (Opera 9.63+ maps the "." from the number block to the "N" key (78) !!!)
                    case 190: break; // .
                    default: setValue(this);
                }
            }
        });
    };

    app.addChat = function (id, name, image) {
        $.chat({
            // your user information
            user: {
                Id: id,
                Name: name,
                ProfilePictureUrl: image
            },
            // text displayed when the other user is typing
            typingText: ' está escribiendo...',
            // the title for the user's list window
            titleText: 'Test chat',
            // text displayed when there's no other users in the room
            emptyRoomText: "No hay usuarios contectados",
            // the adapter you are using
            adapter: new SignalRAdapter()
        });
    };

    app.onError = function (error) {
        app.showErrorMessage(error.responseText);
        app.closeModal();
    };

    app.onSuccess = function (result, status, xhr, isDeletion) {
        $("#filters_form").data("caller", "modal");
        $("#filters_form").data("msg", result);
        app.refreshMainContent(isDeletion, xhr);
    };


    app.refreshMainContent = function (isDeletion, xhr) {

        if (typeof (app.slidingLayout) !== 'undefined') {
            if (isDeletion) {
                app.slidingLayout.closePanel();
            }
            else {
                var newLocation = xhr.getResponseHeader("Location");
                if (newLocation) $("#itemDetails").data("url", newLocation);
                app.slidingLayout.refreshPanel(true);
            }
        }

        app.submitFilterForm();
    };

    app.onFormError = function (result) {
        app.refreshModal(result);
    };

    app.clearForm = function (form) {
        form[0].reset();
        if (typeof ($().selectpicker) !== 'undefined') {
            form.find("select:not(.selectize)").selectpicker("deselectAll");
        }
        if (typeof ($().selectize) !== 'undefined') {
            form.find("select.selectize").each(function () {
                app.selectize.refreshSelect(this);
            });
        }

        form.find(".field-validation-error").empty();
        form.find(".has-error").removeClass("has-error");
        form.trigger('reset.unobtrusiveValidation');
    };

    app.clearModalForm = function () {
        app.clearForm($("#master-modal form"));
    };

    app.addReportPrintButtons = function (parentElement) {
        var parent = parentElement || "body";

        $(parent).on("click", ".print-report", function (e) {
            var url = $(this).attr('href');
            var copies = $(this).data('copies');

            app.printPage(url, copies);

            e.preventDefault();
        });
    };

    app.printPageAndReload = function (data) {
        app.printPage(data.url, data.copies, function () { location.reload(); });
    }

    app.printPage = function (url, copies, cb) {
        $.ajax({
            url: url,
            contentType: 'text/html',
            type: 'GET',
            cache: false,
            success: function (data) {
                app.printDocument(data, copies, cb);
            }
        });
    };

    app.printDocument = function (content, copies, cb) {
        copies = copies || 1;
        if (app.getBrowser() === 'ie') {
            var printWindow = window.open('about:blank', 'impresionPago', 'left=' + (screen.width - 100) + ',top=' + (screen.height - 100) + ',width=100,height=100,titlebar=no,toolbar=no,menubar=no,location=no,directories=no,status=no');

            printWindow.document.write(content);
            printWindow.document.close();

            printWindow.focus();

            for (var i = 0; i < copies; i++) {
                printWindow.print();
            }
            printWindow.close();
            if (cb) cb();
        }
        else {
            var iframe = document.createElement("IFRAME");
            iframe.setAttribute("width", "0");
            iframe.setAttribute("height", "0");

            document.body.appendChild(iframe);

            var win = iframe.contentWindow || iframe;
            var doc = iframe.contentDocument;

            iframe.onload = function () {
                win.focus();

                for (var i = 0; i < copies; i++) {
                    win.print();
                }
                win.parent.document.body.removeChild(iframe);
                if (cb) cb();
            };

            doc.write(content);
            doc.close();
        }
    };

    app.addModal = function (parentElement) {
        var parent = parentElement || "body";

        $(parent).on("click", ".modal-form", function (e) {
            var url = $(this).attr('href');
            app.openModal(url);
            e.preventDefault();
        });
    };

    app.openModal = function (url, container, cb, errorcb) {
        debugger;
        container = container || "master-modal";
        app.load(container, url, function (data, item) {
            if (cb) cb();
            var callback = item.find("form").data('onload');
            if (typeof callback !== 'undefined' && callback !== null) {
                app.callFunction(callback);
            }

            item.modal({ backdrop: 'static', keyboard: false });
            app.setModalHeight("#" + container);
           // app.addModalSelects("#" + container);
           // app.addCurrencyMask("#" + container);
        }, errorcb);
    };

    app.addModalSelects = function (modalId) {
        app.addSelectPickers(modalId, { width: '100%', style: 'btn-sm btn-default', container: 'body' }, 'modal-select force-val');
    };

    app.setModalHeight = function (modalId) {
        var heightFunc = function () {
            var height = ($(window).height() - 190);
            $(modalId + " .modal-body").css("max-height", "" + height + "px");
            $(modalId + " .modal-body").find('.set-height').css("height", "" + (height - 60) + "px");

            //$.each(CKEDITOR.instances, function (index, editor) {
            //    if (editor.instanceReady && $(editor.element.$).hasClass('set-height')) {
            //        editor.resize('100%', height - 60);
            //    }
            //});
        };

        heightFunc();

        //CKEDITOR.on('instanceReady', function (evt) {
        //    heightFunc();
        //});

        $(window).off("resize").on("resize", function (e) {
            heightFunc();
        });
    };

    app.load = function (itemId, url, successCallback, errorCallback) {
        debugger;
        $.ajax({
            cache: false,
            url: url,
            type: 'GET',
            success: function (data) {
                var item = $("#" + itemId);
                item.html(data);
             //   $.validator.unobtrusive.parse(item);
               // app.initPlugins("#" + itemId);
                //app.setCollapseToggle();
                if (successCallback) successCallback(data, item);
            },
            error: function (xhr, status, error) {
             //   app.handleAjaxError(xhr, status, error);
                if (errorCallback) {
                    var item = $("#" + itemId);
                    errorCallback(xhr, status, error, item);
                }
            }
        });
    };

    app.handleAjaxError = function (xhr, status, error, messageElement) {
        if (xhr.status === 401) {
            location.reload();
        }
        else if (xhr.status === 403) {
            app.showErrorMessage("No tiene permisos para acceder a la función seleccionada", messageElement);
        }
        else if (xhr.status === 400) {
            var msg = xhr.responseText;
            if (msg.length > 1 && msg[0] === '"' && msg[msg.length - 1] === '"') msg = msg.slice(1, -1);
            app.showErrorMessage(msg, messageElement);
        }
        else {
            app.showErrorMessage("Ocurrió un error desconocido", messageElement);
        }
    };

    app.callFunction = function (functionName) {
        var i = 0,
            obj = window,
            parts = functionName.split("."),
            length = parts.length;

        for (i = 0; i < length; i++) {
            obj = obj[parts[i]];
            if (typeof obj === 'undefined') return;
        }

        obj();
    };

    app.refreshModal = function (xhr) {
        var $modal = $("#master-modal");
        $modal.html(xhr.responseText || xhr);
        $.validator.unobtrusive.parse("#master-modal");
        var $form = $modal.find("form");

        app.addModalSelects("#master-modal");
        app.addCurrencyMask("#master-modal");

        $form.find("span.field-validation-error").closest(".form-group").addClass("has-error");
    };

    app.closeModal = function () {
        var modal = $('#master-modal');
        modal.modal('hide');
    };

    app.closeModalAndRefresh = function (result, status, xhr) {
        if (xhr && xhr.getResponseHeader("Location") !== null && xhr.getResponseHeader("Location") !== "") {
            var newLocation = xhr.getResponseHeader("Location");
            if (xhr.getResponseHeader("LocationType") !== null && xhr.getResponseHeader("LocationType") === "modal") {
                var closeFunc = xhr.getResponseHeader("CloseFunction");
                var fn = closeFunc ? app.getFunctionFromString(closeFunc) : null;
                app.openModal(newLocation, "master-modal", function () {
                    if (typeof fn === 'function') $("#master-modal").on("hide.bs.modal", fn);
                });
            }
            else {
                location.href = newLocation;
            }
        }
        else {
            location.reload();
        }
    };

    app.getFunctionFromString = function (string) {
        var scope = window;
        var scopeSplit = string.split('.');
        for (i = 0; i < scopeSplit.length - 1; i++) {
            scope = scope[scopeSplit[i]];

            if (scope == undefined) return;
        }

        return function () {
            scope[scopeSplit[scopeSplit.length - 1]].call(scope);
        };
        //return scope[scopeSplit[scopeSplit.length - 1]];
    };

    app.getNumber = function (value) {
        var region = jack.getLocale();

        var opts = { colorize: false, region: region, roundToDecimalPlace: -2, symbol: '' };

        if (region.toLowerCase() === "es-pe" || region.toLowerCase() === "en-us") opts.roundToDecimalPlace = -1;

        return $("<div/>").html(value).asNumber({ region: region })
    };

    var customsText = [];
    app.setCustomsText = function (object) {
        customsText = object;
    }

    app.customText = function (text) {
        var arrayText = text.split(" ");
        var resultado = "";
        var ingreso = false;
        $.each(arrayText, function (index, value) {
            textTemporal = value.toLowerCase();
            for (var name in customsText) {
                var nametemportal = name.toLowerCase();
                if (nametemportal == textTemporal) {
                    resultado = resultado.concat(customsText[name] + " ");
                    ingreso = true;
                }
            }
            if (ingreso == false) {
                resultado = resultado.concat(value + " ");
            } else {
                ingreso = false;
            }
        });

        if (resultado.length > 0) {
            resultado = resultado.substring(0, resultado.length - 1);
        }
        return resultado;
    };
    app.init();
}(window.pornmaleon = window.pornmaleon || {}, jQuery));
