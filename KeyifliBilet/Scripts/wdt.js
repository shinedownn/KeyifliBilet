/// <reference path="../content/assets/plugins/moment.min.js" />
var dialogTimer;
var currentLang = "";
var WDT = {
    Init: function () {
        this.Helper.Import("WDT.Ajax");
        this.Helper.Import("WDT.Mask");
        this.Helper.Import("WDT.SearchBox");
        this.Helper.Import("WDT.SearchPnr");
    },
    Helper: {
        Import: function (Class) {
            var klass = eval(Class);
            if (klass === undefined) {
                throw new Error(Class + " is undefined.");
            }
            if (klass["Init"] === undefined) {
                throw new Error(Class + " has'nt 'Init' method.");
            } else {
                klass["Init"]();
            }
        },
        GetQueryString: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.href);
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        GetRandom: function (length) {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (var i = 0; i < length; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        },
        GetValue: function (element, id) {
            var control = $("#" + element + "-" + id);
            if ($(control).is(":checkbox")) {
                return $(control).is(":checked");
            }
            else {
                return $("#" + element + "-" + id).val()
            }
        },
        LoadingHtml: '<div id="preloaderTable"><div class="inner"><span class="loader"></span></div></div>',
        ShowLoading: function (element) {
            $("#" + element).append(WDT.Helper.LoadingHtml);
        },
        ConvertToDate: function (jsonDate) {
            var currentTime = new Date(parseInt(jsonDate.substr(6)));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;
            return date;
        },
        SetNumberInput: function (element) {
            $(element).attr('onpaste', 'return false;');
            $(element).keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode == 44) {
                    return true;
                }
                else if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            });
        }
    },
    Ajax: {
        Init: function () { },
        GetJSON: function (url, data, callback) {
            url = currentLang + url;
            $.ajax({
                type: 'GET',
                url: url,
                data: data,
                async: false,
                cache: false,
                success: function (data) { callback(data); },
                error: function (xhr) { console.log(xhr.responseText); }
            });
        },
        GetJSONAsync: function (url, data, callback) {
            url = currentLang + url;
            $.ajax({
                type: 'GET',
                url: url,
                data: data,
                async: true,
                cache: false,
                success: function (data) { callback(data); },
                error: function (xhr) { console.log(xhr.responseText); }
            });
        },
        PostJSON: function (url, data, callback) {
            url = currentLang + url;
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                type: 'POST',
                contentType: 'application/json',
                async: false,
                success: function (result) {
                    callback(result);
                },
                error: function (xhr) {
                    console.log(JSON.stringify(xhr));
                    console.log(xhr.responseText);
                }
            });
        },
        PostJSONAsync: function (url, data, callback) {
            url = currentLang + url;
            $.ajax({
                url: url,
                data: JSON.stringify(data),
                type: 'POST',
                contentType: 'application/json',
                async: true,
                cache: false,
                success: function (result) {
                    callback(result);
                },
                error: function (xhr) { console.log(xhr.responseText); }
            });
        }
    },
    Mask: {
        Init: function () { },
        Type: {
            PHONE: 1,
            BIRTHDAY: 2,
            EMAIL: 3
        },
        SetMask: function (type, selector) {
            switch (type) {
                case WDT.Mask.Type.PHONE:
                    $(selector).mask("+09 (999) 999-99-99", { placeholder: "+__(___) __-__-__" });
                    break;
                case WDT.Mask.Type.BIRTHDAY:
                    $(selector).mask("99/99/9999", { placeholder: "dd.mm.yyyy" });
                    break;
                case WDT.Mask.Type.EMAIL:
                    //Email Mask
                    break;
                default:
                    break;
            }
        }
    },
    SearchBox: {
        Init: function () {

        },
        CreateBox: function () {
            //WDT.Ajax.GetJSONAsync('/Flight/GetMainSearchBox', null, function (result) {
                //$("#tabQuickFlights").html(result);
                BindDatepickerScripts();
                $("#tekyon-tab").click(function () {
                    $("#hdnIsOneWay").val(true)
                    WDT.SearchBox.SetPicker(true);
                });
                $("#gidisdonus-tab").click(function () {
                    $("#hdnIsOneWay").val(false)
                    WDT.SearchBox.SetPicker(false);
                });
                setTimeout(function () {
                    if ($("#hdnIsOneWay").val() == 'True') {
                        $("#tekyon-tab").click();
                    } else {
                        $("#gidisdonus-tab").click();
                    }
                }, 500);


                if ($(".fancy-form.mobile.departure").length > 0) {
                    $(".fancy-form.mobile.departure").focusin(function () {
                        $("#slider").animate({ "margin-bottom": "200px" }, 1000, 'easeInOutExpo');
                        $("body").animate({ scrollTop: 80 }, 1000, 'easeInOutExpo');
                        $(this).toggleClass("active");
                    });
                    $(".fancy-form.mobile.departure").focusout(function () {
                        $("#slider").animate({ "margin-bottom": "0" }, 1000, 'easeInOutExpo');
                        $("body").animate({ scrollTop: 0 }, 1000, 'easeInOutExpo');
                        $(this).toggleClass("active");
                    });

                }
                var $popoverItem = $('#dvPerson .custom-input').popover({
                    container: '.search-engine',
                    content: $(".flight-information"),
                    html: true,
                    placement: 'bottom'
                });
                var hasStepper = false;
                $popoverItem.on('shown.bs.popover', function () {
                    if (!hasStepper) {
                        hasStepper = true;
                        //_stepper();
                    }
                });


                $("#btn_FSB_OK").click(WDT.SearchBox.SearchFlight);

                $(document).on("change", "#adult, #child, #infant", WDT.SearchBox.CalcPersonCount);
                WDT.SearchBox.SetAutocomplete();
                $('#dvDeparture').on('show.bs.dropdown', function () {
                    setTimeout(function () { $("#selectBoardingTerminal").focus(); }, 100);
                });
                $('#dvReturn').on('show.bs.dropdown', function () {
                    setTimeout(function () { $("#selectLandingTerminal").focus(); }, 100);
                });
                var isOneWay = $("#hdnflightType").val();
                $("#btnGoFlightSearch").click(WDT.SearchBox.SearchFlight);

                $("#btnChangePorts").click(function () {
                    var depCity = $("#dvReturn").find('span[class="label-short"]').text();
                    var depAirport = $("#dvReturn").find('span[class="label-airport"]').text();
                    var depCode = $("#dvReturn").find('span[class="label-code"]').text();

                    var arrCity = $("#dvDeparture").find('span[class="label-short"]').text();
                    var arrAirport = $("#dvDeparture").find('span[class="label-airport"]').text();
                    var arrCode = $("#dvDeparture").find('span[class="label-code"]').text();

                    $("#dvDeparture").find('span[class="label-short"]').text(depCity);
                    $("#dvDeparture").find('span[class="label-airport"]').text(depAirport);
                    $("#dvDeparture").find('span[class="label-code"]').text(depCode);
                    $('#selectBoardingTerminalCode').val(depCode);


                    $("#dvReturn").find('span[class="label-short"]').text(arrCity);
                    $("#dvReturn").find('span[class="label-airport"]').text(arrAirport);
                    $("#dvReturn").find('span[class="label-code"]').text(arrCode);
                    $('#selectLandingTerminalCode').val(arrCode);

                });


            //});
        },
        SearchFlight: function () {
            var toDate = "";
            try {
                toDate = $('#inpRangePicker').data('daterangepicker').endDate.format('DD-MM-YYYY');
            } catch (e) {
                toDate = "";
            }

            var model = {
                IsOneWay: $("#hdnIsOneWay").val().toString().toLowerCase() == 'true' ? true : false,
                DEP_Airport_Code: $('#selectBoardingTerminalCode').val(),
                DEP_Airport_Name: '',
                DEP_Airport_CityName: '',
                DEP_Airport_IsCity: $('#selectAllInFromCity').val() == 'true' ? true : false,
                DepartureDate: $('#inpRangePicker').data('daterangepicker').startDate.format('DD-MM-YYYY'),
                ARR_Airport_Code: $('#selectLandingTerminalCode').val(),
                ARR_Airport_Name: '',
                ARR_Airport_CityName: '',
                ARR_Airport_IsCity: $('#selectAllInToCity').val() == 'true' ? true : false,
                ReturnDate: toDate,
                Adult: parseInt($("#txtAdultCount").val()),
                Child: parseInt($("#txtChildCount").val()),
                Infant: parseInt($("#txtInfantCount").val()),
                Total: 0,
                CabinClass: $("#ddlClass").val(),
                NoStop: $("#chkNoStop").is(':checked')
            };
            if (typeof (model.DEP_Airport_IsCity) !== "boolean") {
                model.DEP_Airport_IsCity = false;
            }
            if (typeof (model.ARR_Airport_IsCity) !== "boolean") {
                model.ARR_Airport_IsCity = false;
            }

            if (model.DEP_Airport_Code == "" || model.ARR_Airport_Code == "") {
                return _toastr("Nereden nereye gideceğinizi seçiniz.", "top-center", "info", false);
            } else if (model.DEP_Airport_Code == model.ARR_Airport_Code.ToAirport)
                return _toastr("Kalkış ve Varış yerleri aynı olamaz !", "top-center", "info", false);
            else if (model.Infant > model.Adult)
                return _toastr("Bebek yolcu sayısı yetişkin yolcu sayısına eşit ya da daha az olmalıdır.", "top-center", "warning", false);
            else {
                $('#preloader').show();
                WDT.Ajax.PostJSONAsync('/Flight/Search', { model: model }, function (result) {
                    if (result.Result) {
                        location.href = result.Url;
                    }
                });
            }

            $("#spnDeparture").html($("#dvDeparture").find('span[class="label-short"]').text() + "(" + $("#dvDeparture").find('span[class="label-code"]').text() + ")");
            $("#spnArrival").html($("#dvReturn").find('span[class="label-short"]').text() + "(" + $("#dvReturn").find('span[class="label-code"]').text() + ")");
            var date = $('#inpRangePicker').val();
            var adult = parseInt($("#txtAdultCount").val());
            var child = parseInt($("#txtChildCount").val());
            var infant = parseInt($("#txtInfantCount").val());
            var paxArray = [];
            if (adult > 0)
                paxArray.push(adult + " Yetişkin ");
            if (child > 0)
                paxArray.push(child + " Çocuk ");
            if (infant > 0)
                paxArray.push(infant + " Bebek ");
            $("#spnDateAndPax").html(date + " - " + paxArray.join(","));
        },
        SearchFlightFromBox: function (boxId) {
            WDT.Ajax.PostJSONAsync('/Flight/GetBoxModel', { boxId: boxId }, function (result) {
                $("#spnDeparture").html(result.DEP_Airport_CityName + "(" + result.DEP_Airport_Code + ")");
                $("#spnArrival").html(result.ARR_Airport_CityName + "(" + result.ARR_Airport_Code + ")");
                var date = result.DepartureDate;
                var adult = parseInt($("#txtAdultCount").val());
                var child = parseInt($("#txtChildCount").val());
                var infant = parseInt($("#txtInfantCount").val());
                var paxArray = [];
                if (adult > 0)
                    paxArray.push(adult + " Yetişkin ");
                if (child > 0)
                    paxArray.push(child + " Çocuk ");
                if (infant > 0)
                    paxArray.push(infant + " Bebek ");
                $("#spnDateAndPax").html(date + " - " + paxArray.join(","));




                $('#preloader').show();
                WDT.Ajax.PostJSONAsync('/Flight/Search', { model: result }, function (response) {
                    if (response.Result) {
                        location.href = response.Url;
                    }
                });
            });
        },
        CreateRow: function () { },
        SetPicker: function (isSingle) {
            var today = new Date();
            $('#inpRangePicker').daterangepicker({
                locale: {
                    format: 'DD.MM.YYYY',
                    "applyLabel": "Tamam",
                    "cancelLabel": "Kapat",
                    "fromLabel": "From",
                    "toLabel": "To",
                    "customRangeLabel": "Custom",
                    "weekLabel": "W",
                    "daysOfWeek": [
                        "Pz",
                        "Pt",
                        "Sa",
                        "Ça",
                        "Pe",
                        "Cu",
                        "Ct"
                    ],
                    "monthNames": [
                        "Ocak",
                        "Şubat",
                        "Mart",
                        "Nisan",
                        "Mayıs",
                        "Haziran",
                        "Temmuz",
                        "Ağustos",
                        "Eylül",
                        "Ekim",
                        "Kasım",
                        "Aralık"
                    ],
                    "firstDay": 1
                },
                singleDatePicker: isSingle,
                autoApply: valueautoApply,
                autoUpdateInput: valueautoApply,
                changeMonth: false,
                showDropdowns: false,
                minDate: today,
                autoApply: false
            });
            $('#inpRangePicker').on('apply.daterangepicker', function (ev, picker) {
                setTimeout(function () {
                    $('#dvPerson').find(".custom-input").trigger("click");
                    var body = $("html, body");
                    body.stop().animate({ scrollTop: 0 }, 500, 'swing', function () { });
                }, 100);
            });
            $(document).on("change", "#adult, #child, #infant", function () {
                var adultCount = $('#adult').val();
                var childCount = $('#child').val();
                var infantCount = $('#infant').val();
                $('#personCount').html(parseInt(adultCount) + parseInt(childCount) + parseInt(infantCount));
            });

        },
        CalcPersonCount: function () {
            var adultCount = $('#txtAdultCount').val();
            var childCount = $('#txtChildCount').val();
            var infantCount = $('#txtInfantCount').val();
            $('#personCount').html(parseInt(adultCount) + parseInt(childCount) + parseInt(infantCount));
        },
        SetAutocomplete: function () {

            $("#selectBoardingTerminal").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: currentLang + "/WDT/Autocomplete",
                        type: "GET",
                        data: { q: request.term },
                        success: function (data) {
                            response($.map(data, function (el) {
                                return {
                                    name: el.name,
                                    code: el.code,
                                    cityName: el.cityName,
                                    label: el.cityName + ' - ' + el.name + ' - (' + el.code + ') - ' + el.countryName,
                                    value: el.code,
                                    city: el.city
                                };
                            }));
                        }
                    });
                },
                appendTo: $("#dvDeparture").find(".dropdown-menu"),
                delay: 300,
                autoFocus: !0,
                minLength: 3,
                max: 7,
                classes: {
                    "ui-autocomplete": "tt-menu"
                },
                select: function (t, e) {
                    console.log(e.item);
                    $("#selectBoardingTerminalCode").val(e.item.code);
                    $("#selectAllInFromCity").val(e.item.city);
                    $("#dvDeparture").find(".label-short").html(e.item.cityName);
                    $("#dvDeparture").find(".label-airport").html(e.item.name);
                    $("#dvDeparture").find(".label-code").html(e.item.code);

                    setTimeout(function () {
                        $('#dvReturn').find(".custom-input").trigger("click");
                        $("#selectLandingTerminal").focus();
                    }, 100);
                },
                focus: function (t, e) {
                    if (void 0 !== t.keyCode) {
                        var a = $(this).parents("form")[0].getAttribute("data-name");
                        $(this).hasClass("complete-origin") ? (window.main[a].origin = e.item.id, window.main[a].originText = e.item.label) : (window.main[a].destination = e.item.id, window.main[a].destinationText = e.item.label)
                    }
                },
                create: function (t, e) {
                    $(this).data("ui-autocomplete")._resizeMenu = function () {
                        this.menu.element.outerWidth($(this.element[0]).parents(".row")[0].offsetWidth)
                    }, $(this).data("ui-autocomplete")._renderItem = function (t, e) {
                        var a = '<div class="item"><span class="avatar"><i class="icon-chevron-right"></i></span> <span class="airport">' + e.name + '<small>(' + e.code + ')</small>, </span> <span class="city">' + e.cityName + '</span> </div>';
                        return $("<li>").append(a).appendTo(t)
                    }, $(this).autocomplete("option", "position", {
                        my: "right top",
                        at: "right bottom",
                        of: $(this).parents(".row")[0]
                    })
                }
            });



            function split(val) {
                return val.split(/,\s*/);
            }
            function extractLast(term) {
                return split(term).pop();
            }

            $("#selectLandingTerminal").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: currentLang+"/WDT/Autocomplete",
                        type: "GET",
                        data: { q: request.term },
                        success: function (data) {
                            response($.map(data, function (el) {
                                return {
                                    name: el.name,
                                    code: el.code,
                                    cityName: el.cityName,
                                    label: el.cityName + ' - ' + el.name + ' - (' + el.code + ') - ' + el.countryName,
                                    value: el.code,
                                    city: el.city
                                };
                            }));
                        }
                    });
                },
                appendTo: $("#dvReturn").find(".dropdown-menu"),
                delay: 300,
                autoFocus: !0,
                minLength: 3,
                max: 7,
                classes: {
                    "ui-autocomplete": "tt-menu"
                },
                select: function (t, e) {
                    console.log(e.item);
                    $("#selectLandingTerminalCode").val(e.item.code);
                    $("#selectAllInToCity").val(e.item.city);
                    $("#dvReturn").find(".label-short").html(e.item.cityName);
                    $("#dvReturn").find(".label-airport").html(e.item.name);
                    $("#dvReturn").find(".label-code").html(e.item.code);
                    $('#selectLandingTerminal').blur();
                    setTimeout(function () {
                        $("#inpRangePicker").focus();
                        $('#selectLandingTerminal').blur();
                    }, 100);
                },
                focus: function (t, e) {
                    if (void 0 !== t.keyCode) {
                        var a = $(this).parents("form")[0].getAttribute("data-name");
                        $(this).hasClass("complete-origin") ? (window.main[a].origin = e.item.id, window.main[a].originText = e.item.label) : (window.main[a].destination = e.item.id, window.main[a].destinationText = e.item.label)
                    }
                },
                create: function (t, e) {
                    $(this).data("ui-autocomplete")._resizeMenu = function () {
                        this.menu.element.outerWidth($(this.element[0]).parents(".row")[0].offsetWidth)
                    }, $(this).data("ui-autocomplete")._renderItem = function (t, e) {
                        var a = '<div class="item"><span class="avatar"><i class="icon-chevron-right"></i></span> <span class="airport">' + e.name + '<small>(' + e.code + ')</small>, </span> <span class="city">' + e.cityName + '</span> </div>';
                        return $("<li>").append(a).appendTo(t)
                    }, $(this).autocomplete("option", "position", {
                        my: "right top",
                        at: "right bottom",
                        of: $(this).parents(".row")[0]
                    })
                }
            });



        },
        SearchResult: function () {
            WDT.Session.Init();
            var currentOrder = 'minPrice';

            $("#chkNoStopFilter").click(function () {
                WDT.SearchBox.SetFilter();
            });

            $("#pills-home-tab,#pills-profile-tab,#pills-contact-tab").click(function () {
                var currentOrder = $(this).data('orderby');
                var currentOrderType = $(this).data('orderType');
                console.log(currentOrder);
                var $items = $("#flight > li.flight-itinerary");
                var opOrder = $items.sort(function (a, b) {
                    var aPrice = parseFloat($(a).attr(currentOrder));
                    var bPrice = parseFloat($(b).attr(currentOrder));
                    if (currentOrderType == 'asc') {
                        return (aPrice < bPrice) ? -1 : 1;
                    }
                    else {
                        return (aPrice < bPrice) ? 1 : -1;
                    }
                });
                $("#flight").find('li').remove();
                $("#flight").html(opOrder);
                $items = $("#flightReturn > li.flight-itinerary");
                var opOrder = $items.sort(function (a, b) {
                    var aPrice = parseFloat($(a).attr(currentOrder));
                    var bPrice = parseFloat($(b).attr(currentOrder));
                    if (currentOrderType == 'asc') {
                        return (aPrice < bPrice) ? -1 : 1;
                    }
                    else {
                        return (aPrice < bPrice) ? 1 : -1;
                    }
                });
                $("#flightReturn").html(opOrder);
                $(this).data('orderType', currentOrderType == "asc" ? "desc" : "asc");
            });
            if ($("#hdnHidePreviousDay").val() == 'True') {
                $("#btnFromPreviousDay").hide();
            }
            $("#btnFromPreviousDay").click(function () { WDT.SearchBox.NewSearch(-1, false); });
            $("#btnFromNextDay").click(function () { WDT.SearchBox.NewSearch(1, false); });

            $("#btnReturnPreviousDay").click(function () { WDT.SearchBox.NewSearch(-1, true); });
            $("#btnReturnNextDay").click(function () { WDT.SearchBox.NewSearch(1, true); });


            $(".lblAirline").click(function () {
                WDT.SearchBox.SetFilter();
            });
            $(".lblFareType").click(function () {
                WDT.SearchBox.SetFilter();
            });
            setTimeout(function () {
                $("#pills-home-tab").click();
                setTimeout(function () {
                    $("#pills-home-tab").click();
                }, 10);
            }, 5);

        },
        SetFilter: function () {
            $("#sectionReturnFlightContainer").show();
            $("#sectionFlightContainer").show();
            $("#sectionReturnNoFlightInfo").remove();
            $("#sectionNoFlightInfo").remove();
            var noStop = $("#chkNoStopFilter").is(':checked');
            var airlineList = [];
            var cabinList = [];
            $(".lblAirline").each(function () {
                if ($(this).is(':checked'))
                    airlineList.push($(this).val());
            });
            $(".lblFareType").each(function () {
                if ($(this).is(':checked'))
                    cabinList.push($(this).val());
            });

            $(".flight-itinerary").hide();
            if (noStop) {
                $("li[data-hasStop='False']").show();
            } else {
                $(".flight-itinerary").show();
            }
            $(".flight-itinerary").each(function () {
                var isHide = false;
                if (noStop) {
                    isHide = $(this).attr('data-hasStop') != 'False';
                } else {
                    isHide = false;
                }
                if (!isHide) {
                    var airlineCode = $(this).attr('airline');
                    isHide = airlineList.indexOf(airlineCode) == -1;
                }
                if (isHide) {
                    $(this).hide();
                }
                $(".pricing > .content").show();
                $(".pricing").each(function () {
                    var status = $(this).attr('status');
                    var cabin = $(this).attr('fareType');
                    if (status == 'active') {
                        if (cabinList.indexOf(cabin) == -1)
                            $(this).find('div[class="content"]').hide();
                    }
                });
            });

            var goVisible = $("#flight > li.flight-itinerary").filter(':visible').length;
            var returnVisible = $("#flightReturn > li.flight-itinerary").filter(':visible').length;

            if (returnVisible < 1) {
                $("#sectionReturnFlightContainer").hide();
                var html = noFlightHTML.replace('XXXXX', 'sectionReturnNoFlightInfo');
                $("#sectionReturnFlightContainer").before(html);
            }
            else {
                $("#sectionReturnFlightContainer").show();
                $("#sectionReturnNoFlightInfo").remove();
            }

            if (goVisible < 1) {
                $("#sectionFlightContainer").hide();
                var html = noFlightHTML.replace('XXXXX', 'sectionNoFlightInfo');
                $("#sectionFlightContainer").before(html);
            }
            else {
                $("#sectionFlightContainer").show();
                $("#sectionNoFlightInfo").remove();
            }

            console.log(goVisible + " " + returnVisible);
        },
        NewSearch: function (addDays, isReturn) {
            WDT.Ajax.PostJSONAsync('/Flight/GetDaysModel', { addDays: addDays, isReturn: isReturn }, function (result) {
                $("#spnDeparture").html(result.DEP_Airport_CityName + "(" + result.DEP_Airport_Code + ")");
                $("#spnArrival").html(result.ARR_Airport_CityName + "(" + result.ARR_Airport_Code + ")");
                var date = result.DepartureDate;
                var adult = parseInt($("#txtAdultCount").val());
                var child = parseInt($("#txtChildCount").val());
                var infant = parseInt($("#txtInfantCount").val());
                var paxArray = [];
                if (adult > 0)
                    paxArray.push(adult + " Yetişkin ");
                if (child > 0)
                    paxArray.push(child + " Çocuk ");
                if (infant > 0)
                    paxArray.push(infant + " Bebek ");
                $("#spnDateAndPax").html(date + " - " + paxArray.join(","));
                $('#preloader').show();
                WDT.Ajax.PostJSONAsync('/Flight/Search', { model: result }, function (response) {
                    if (response.Result) {
                        location.href = response.Url;
                    }
                });
            });
        }
    },
    SearchResult: {
        Init: function () {

        },
        SelectFlight: function (searchId, flightId, fareType, flightType, pricingType, providerKey, packageId) {
            var model = {
                SearchId: searchId,
                FlightId: flightId,
                FareType: fareType,
                FlightType: flightType,
                Path: location.pathname
            };

            var packageModel = {
                PricingType: pricingType,
                ProviderKey: providerKey,
                PackageId: packageId
            };
            console.log(packageModel);

            WDT.Ajax.PostJSONAsync('/Flight/SelectFlight', model, function (result) {
                if (model.FlightType == 'Go') {
                    $("#flight").hide();
                    $("#selectedFlight").html(result);
                    $("#tabFlights").find(".navigate").hide();

                    $("#flightReturn > li.flight-itinerary").show();
                    $("#flightReturn > li.flight-itinerary").each(function (index, item) {
                        var returnPricingType = $(item).attr('pricingType');
                        var returnProviderKey = $(item).attr('providerKey');
                        var returnPackageId = $(item).attr('packageId');
                        if (returnPricingType == pricingType && returnProviderKey == providerKey && packageId == returnPackageId) {
                            $(item).show();
                        } else {
                            $(item).hide();
                        }
                    });
                } else {
                    $("#flightReturn").hide();
                    $("#selectedFlightReturn").html(result);
                    $("#tabFlightsReturn").find(".navigate").hide();
                }
            });
        },
        GoPayment: function (k) {
            location.href = location.href + "/odeme?k=" + k;
        }
    },
    SearchPnr: {
        Init: function () {
            WDT.SearchPnr.Render();
        },
        Render: function () {
            //
            //WDT.Ajax.GetJSONAsync('/Flight/GetSearchPnr', null, function (result) {
                //$("#tabQuickPNR").html(result);
                $("#txtSearchPnr_Mobile").inputmask('09999999999', { clearIncomplete: true });
                $.validator.unobtrusive.parse("#frmSearchPnr");
            //});
        },
        SearchSuccess: function (data) {

        }
    },
    Payment: {
        Init: function () {
            WDT.Session.Init();
        },
        GetPaymentArea: function (saleId) {
            WDT.Ajax.PostJSONAsync('/Flight/RenderPaymentForm', { saleId: saleId }, function (result) {
                $("#dvPaymentArea").html(result);
                $("#PassengerForm_Email").inputmask('email', { clearIncomplete: true });
                $("#PassengerForm_Mobile").CcPicker({ "countryCode": "tr" });
                $("#PassengerForm_Mobile").on("countrySelect", function (e, i) {
                    $("#PassengerForm_CountryCode").val(i.phoneCode);
                });


                $(".maskIdentity").inputmask('numeric', { clearIncomplete: true, rightAlign: false });
                $(".maskBirthdate").inputmask('99/99/9999', { clearIncomplete: true });
                $("#PaymentForm_CardNumber").inputmask('9999-9999-9999-9999', { clearIncomplete: true });
                $("#PaymentForm_CardCVV").inputmask('999', { clearIncomplete: true });
                $.validator.unobtrusive.parse("#frmSavePayment");
                $("#RightsConfirm").after("<i></i>");
                $("#CampaignConfirm").after("<i></i>");
                $(".tcPerson").click(function () {
                    var personId = parseInt($(this).attr('personId'));
                    var isChecked = $(this).is(':checked');
                    if (isChecked) {
                        $("#PassengerForm_PassengerList_" + personId + "__IsTurkish").val(false);
                        $("#PassengerForm_PassengerList_" + personId + "__IdentityNo").hide();
                    } else {
                        $("#PassengerForm_PassengerList_" + personId + "__IsTurkish").val(true);
                        $("#PassengerForm_PassengerList_" + personId + "__IdentityNo").show();
                    }
                });
                $(".hdnIsTurkish").each(function (index, item) {
                    var isTurkish = $(item).val() == 'True';
                    var personId = $(item).attr('personId');
                    if (!isTurkish) {
                        $(".tcPerson").filter('input[personId=' + personId + ']').click();
                    }
                });


            });
        },
        CheckForm: function () {
            $("#spnMobileError").html('');
            var isChecked = $("#RightsConfirm").is(':checked');
            if (isChecked) {
                $("#spnRightError").html('');
            } else {
                $("#spnRightError").html('Lütfen kuralları okuyup kabul ediniz !');
            }
            var countryCode = $("#PassengerForm_CountryCode").val();
            if (countryCode == "+90") {
                var phone = $("#PassengerForm_Mobile").val();
                if (phone.startsWith('0') || phone.length != 10) {
                    $("#spnMobileError").html('Cep telefonu numaranızı başında 0 olmadan 10 hane olarak giriniz !');
                    isChecked = false;
                }
            }

            return isChecked;
        },
        PaymentSuccess: function (data) {
            $("#clientid").val(data.Merchant.ClientId);
            $("#amount").val(data.Order.Amount);
            $("#oid").val(data.Order.OrderId);
            $("#okUrl").val(data.SuccessUrl);
            $("#failUrl").val(data.FailUrl);
            $("#islemtipi").val(data.ProcessType);
            $("#rnd").val(data.Random);
            $("#hash").val(data.Hash);
            $("#pan").val(data.CardNumber.replace(/[-]/g, ''));
            $("#Ecom_Payment_Card_ExpDate_Year").val(data.ExpireYear);
            $("#Ecom_Payment_Card_ExpDate_Month").val(data.ExpireMonth);
            $("#cv2").val(data.CVV);
            //console.log(data);
            setTimeout(function () { $("#frmPayment").submit(); }, 500);
        }
    },
    Member: {
        Init: function () {
            WDT.Member.LoginInit();
            WDT.Member.RegisterInit();

            $('.btnMember').click(function () {
                var tab = $(this).attr('tabContent');
                $('li > a[href="#' + tab + '"]').tab("show");
            });

            //$('.btnMember').click(function () {
            //	var tab = $(this).attr('tabContent');
            //	$('li > a[href="#' + tab + '"]').tab("show");
            //});

        },
        LoginRequired: function () {
            var path = WDT.Helper.GetQueryString('ReturnUrl');
            if (path.length > 2) {
                $(".bs-example-modal-sm").modal('show');
            } else {
                path = WDT.Helper.GetQueryString('approve');
                if (path.length == 6) {
                    $(".bs-example-modal-sm").modal('show');
                    $("#btnLoginTab").click();
                    $("#spnLoginError").html('Üyeliğiniz onaylandı, lütfen giriş yapınız ');
                }
                else {
                    path = WDT.Helper.GetQueryString('forgetPassword');
                    if (path.length == 6) {
                        $(".bs-example-modal-sm").modal('show');
                        //$("#btnCPTab").click();
                        $('li > a[href="#register"]').hide();
                        $('li > a[href="#login"]').hide();
                        $('li > a[href="#forgetPassword"]').hide();
                        $('li > a[href="#changePassword"]').show();
                        $('li > a[href="#changePassword"]').tab("show");
                    }
                }
            }
        },
        LoginInit: function () {
            $("#txtLogEmail,#txtFPEmail").inputmask('email', { clearIncomplete: true });
        },
        LoginSuccess: function (data) {
            if (data.Result) {
                var path = WDT.Helper.GetQueryString('ReturnUrl');
                if (path.length > 2)
                    location.href = path;
                else {
                    path = WDT.Helper.GetQueryString('approve');
                    if (path.length == 6)
                        location.href = "/";
                    else
                        location.reload();
                }

            } else {
                $("#spnLoginError").html('<h4>' + data.ErrorMsg + '</h4>');
            }
        },
        ChangePasswordSuccess: function (data) {
            if (data.Result) {
                var path = WDT.Helper.GetQueryString('forgetPassword');
                if (path.length == 6) {
                    $("#spnResultCP").html('<h4>' + data.Message + '</h4>');
                    setTimeout(function () {
                        location.href = "/";
                    }, 2000);
                }
                else
                    location.reload();
            } else {
                $("#spnResultCP").html('<h4>' + data.Message + '</h4>');
            }
        },
        RegisterInit: function () {
            //$("#txtReqMobile").inputmask('0 (999) 999-99-99', { clearIncomplete: true });
            $("#txtReqEmail").inputmask('email', { clearIncomplete: true });
            $("#txtReqMobile").CcPicker({ "countryCode": "tr", "countryFilter": false });
            $("#txtReqMobile").on("countrySelect", function (e, i) {
                $("#CountryCode").val(i.phoneCode);
            });
            $.validator.unobtrusive.parse("#frmRegister");
        },
        ForgetPasswordVisibility: function (isShow) {
            if (isShow) {
                $('li > a[href="#forgetPassword"]').tab("show");
            } else {
                $('li > a[href="#login"]').tab("show");
            }
        },
        ForgetPasswordSuccess: function (data) {
            $("#spnResultFP").html('<h4>' + data.Message + '</h4>');
        },
        RegisterContract: function () {
            var isValid = $("#frmRegister").valid();
            if (isValid) {
                isValid = $("#ConfirmContract").is(':checked');
                if (!isValid) {
                    $("#spnError").html('Üyelik sözleşmesini kabul ediniz !');
                }
                return isValid;
            }
            return isValid;
        },
        RegisterSuccess: function (data) {
            if (data.Result) {
                var resultHtml = '<br><p class="text-left padding-15"><strong class="text-success size-20 bold">Üyelik işleminiz başarıyla tamamlanmıştır.</strong><br/>Kayıt olurken kullanmış olduğunuz email adresinize göndermiş olduğumuz aktivasyon bağlantısına tıklayarak, üyeliğinizi aktifleyebilirsiniz.</p><p class="text-right padding-15"><button type="button" class="btn btn-secondary margin-right-20" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">TAMAM</span></button></p>';
                $("#frmRegister").html(resultHtml);
            } else {
                $("#spnError").html('<h4>' + data.ErrorMsg + '</h4>');
            }
        },
        ProfileInit: function () {
            $("#txtUpMobile").inputmask('0 (999) 999-99-99', { clearIncomplete: true });
        },
        SaveProfileSuccess: function (data) {
            $("#spnLoginError").html('<h4>' + data.ErrorMsg + '</h4>');
            setTimeout(function () {
                $("#spnLoginError").html('');
            }, 1500);
        },
        MailGroupSuccess: function (data) {
            $("#txtGREmail").val('');
            $("#spnResultMG").html(data.Message);
            setTimeout(function () { $("#spnResultMG").html(''); }, 1500);
        }
    },
    Session: {
        Timeout: 900000, //15dk,
        Init: function () {
            $('#timeout').modal({ backdrop: 'static', show: false });
            WDT.Session.Start();
        },
        Start: function () {
            dialogTimer = setTimeout(function () {
                $('#timeout').modal('show');
            }, WDT.Session.Timeout);
        },
        Resume: function () {
            $('#timeout').modal('hide');
            clearInterval(dialogTimer);
            WDT.Session.Start();
        },
        End: function () { location.href = '/'; }

    }
};

if ($(window).width() >= 768) {
    var valueautoApply = true;
} else {
    var valueautoApply = true;
}

var noFlightHTML = '<section id="XXXXX"><div class="container"><div class="row"><div class="col-md-12"><h3 class="margin-bottom-10">Aradığınız kriterlere uygun uçuş bulunamadı!</h3><p>Lütfen <a href="/">buraya tıklayarak</a> yeni arama yapınız</p></div></div></div></section>'

function BindDatepickerScripts() {
    var _container_1 = jQuery('.datepicker');
    if (_container_1.length > 0) {
        if (jQuery().datepicker) {
            _container_1.each(function () {
                var _t = jQuery(this),
                    _lang = _t.attr('data-lang') || 'en';

                //if(_lang != 'en' && _lang != '') { // load language file
                //	loadScript(plugin_path + 'bootstrap.datepicker/locales/bootstrap-datepicker.tr.min.js');
                //} 
                var _minDate = new Date(_t.attr('data-mindate'));
                var _maxDate = new Date(_t.attr('data-maxdate'));
                jQuery(this).datepicker({
                    format: 'DD-MM-YYYY',
                    language: _lang,
                    rtl: _t.attr('data-RTL') == "true" ? true : false,
                    changeMonth: _t.attr('data-changeMonth') == "false" ? false : true,
                    changeYear: _t.attr('data-changeYear') == "false" ? false : true,
                    todayBtn: _t.attr('data-todayBtn') == "false" ? false : "linked",
                    calendarWeeks: _t.attr('data-calendarWeeks') == "false" ? false : true,
                    autoclose: _t.attr('data-autoclose') == "false" ? false : true,
                    todayHighlight: _t.attr('data-todayHighlight') == "false" ? false : true,
                    yearRange: _t.attr('data-yearrange'),
                    minDate: _minDate,
                    maxDate: _maxDate,
                    onRender: function (date) {
                        // return date.valueOf() < nowDate.valueOf() ? 'disabled' : '';
                    }
                }).on('changeDate', function (ev) {
                    // AJAX POST - OPTIONAL
                }).data('datepicker');
            });
        }
    }
    /** Range Picker
        <input type="text" class="form-control rangepicker" value="2015-01-01 - 2016-12-31" data-format="yyyy-mm-dd" data-from="2015-01-01" data-to="2016-12-31">
     ******************* **/
    var _container_2 = jQuery('.rangepicker');
    if (_container_2.length > 0) {
        if (jQuery().datepicker == -1) {
            _container_2.each(function () {
                var _t = jQuery(this),
                    _format = _t.attr('data-format').toUpperCase() || 'DD-MM-YYYY';
                _t.daterangepicker({
                    locale: {
                        format: _format
                    },
                    startDate: _t.attr('data-from'),
                    endDate: _t.attr('data-to'),
                    "autoApply": true,
                    "autoUpdateInput": true,
                    //ranges: {
                    //   'Today': [moment(), moment()],
                    //   'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    //   'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    //   'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    //   'This Month': [moment().startOf('month'), moment().endOf('month')],
                    //   'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    //}
                },
                    function (start, end, label) {
                        // console.log("New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')");
                        // alert("A new date range was chosen: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
                    });
            });
        }
    }
}


var substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substringRegex;
        matches = [];
        substrRegex = new RegExp(q, 'i');
        $.each(strs, function (i, str) {
            if (substrRegex.test(str)) {
                matches.push(str);
            }
        });
        cb(matches);
    };
};