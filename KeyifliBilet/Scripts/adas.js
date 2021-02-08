function GetAjax(url,parameter, async, method, callback) {    
    $.ajax({
        url: url,
        method: method,
        data: JSON.stringify(parameter),
        async: async,
        contentType: "application/json; charset=utf-8",
        success: callback
    })
}