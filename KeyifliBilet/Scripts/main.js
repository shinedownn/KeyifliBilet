



function executeAjax(url, method, data, dataType, async) {
    return $.ajax({
        type: method,
        url: url,
        data: data,
        dataType: dataType,
        contentType: "application/json; charset-utf-8",
        async: async,
        cache: false
    });
}




//function getAirports() {

//    var selectBoardingTerminal = $('#selectBoardingTerminal');
//    var selectLandingTerminal = $('#selectLandingTerminal');

//    selectBoardingTerminal.html('').append($('<option></option>').val('').html('Yükleniyor...'));

//    selectLandingTerminal.html('').append($('<option></option>').val('').html('Yükleniyor...'));

//    var jsonData = {
//        param: 'airports',
//        requestUrl: '',
//        optionalParam: '' // 'TR' if you use this parameter comes to Turkish translations.
//    };

//    jsonData = JSON.stringify(jsonData);
//    var result = executeAjax("/scripts/airports-tr.json", "GET", jsonData, "json", true); // local source, türkçe
//    result.success(function (response) {

//        //response = JSON.parse(response); // if you are using remote source, this line should be open..

//        selectBoardingTerminal.html('').append();
//        selectLandingTerminal.html('').append();

//        if (selectBoardingTerminal.children('option').length < 2 || selectLandingTerminal.children('option').length < 2) {
//            for (var key in response.result) {
//                selectBoardingTerminal.append($('<option></option>').val(response.result[key].code).html(response.result[key].name));
//                selectLandingTerminal.append($('<option></option>').val(response.result[key].code).html(response.result[key].name));
//            }
//        }
//    });

//}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


function get_time_difference(earlierDate, laterDate) {
    
    var now = moment(laterDate); 
    var end = moment(earlierDate);
    var duration = moment.duration(now.diff(end));
    var x = duration.asMilliseconds();
    var d = moment.duration(x, 'milliseconds');
    var hours = Math.floor(d.asHours());
    var mins = Math.floor(d.asMinutes()) - hours * 60;
    return hours + "sa. " + mins + "dk.";

}


function formatTime(time) {
    var formattedDate = moment(time).format('HH:mm');
    return formattedDate;

}

function formatDate(date) {
    var formattedDate = moment(date).format('DD.MM.YYYY');
    return formattedDate;
}

function onSuccessPnr(response) {
    if (response.result == "success") {
        window.location = "/Home/PnrDetail?orderId=" + response.orderId;
    } else {
        return _toastr(response.message, "top-left", "warning", false);
    }
}

if ($(window).width() >= 768) {
	var valueautoApply = false;
} else {
    var valueautoApply = false;
}

function twoWayPicker() {
    var today = new Date();
    $('#inpRangePicker').daterangepicker({
        locale: {
            format: 'DD.MM.YYYY',
            //"separator": " - ",
            "applyLabel": "Tamam",
            "cancelLabel": "Temizle",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Pt",
                "Sa",
                "Ça",
                "Pe",
                "Cu",
                "Ct",
                "Pz"
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
        autoApply: valueautoApply,
        autoUpdateInput: valueautoApply,
        changeMonth: false,
        minDate: today,
        showDropdowns: false, 

    });

    $('#inpRangePicker').on('apply.daterangepicker', function (ev, picker) {
        
        setTimeout(function () {
            $('#dvPerson').find(".custom-input").trigger("click");
            var body = $("html, body");
            body.stop().animate({ scrollTop: 0 }, 500, 'swing', function () {});
            //$("#selectLandingTerminal").focus();
        }, 100);
    });
}

function oneWayPicker() {
    var today = new Date();
    $('#inpRangePicker').daterangepicker({
        locale: {
            format: 'DD.MM.YYYY',
            //"separator": " - ",
            "applyLabel": "Tamam",
            "cancelLabel": "Temizle",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "weekLabel": "W",
            "daysOfWeek": [
                "Pt",
                "Sa",
                "Ça",
                "Pe",
                "Cu",
                "Ct",
                "Pz"
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
        singleDatePicker: true,
        autoApply: valueautoApply,
        autoUpdateInput: valueautoApply,
        changeMonth: false,
        showDropdowns: false,
        minDate: today
    });

    $('#inpRangePicker').on('apply.daterangepicker', function (ev, picker) {
       
        setTimeout(function () {
            $('#dvPerson').find(".custom-input").trigger("click");
            var body = $("html, body");
            body.stop().animate({ scrollTop: 0 }, 500, 'swing', function () { });
            //$("#selectLandingTerminal").focus();
        }, 100);
    });

     
}