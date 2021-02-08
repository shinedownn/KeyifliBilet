$(function () {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
$(function () {
    $(window).bind("load resize", function () {
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.sidebar-collapse').addClass('collapse')
        } else {
            $('div.sidebar-collapse').removeClass('collapse')
        }
    })
})

function sendFile(file, editor, welEditable, posturl) {

    $("#fileprogress").fadeIn(500);
    data = new FormData();
    data.append("file", file);//You can append as many data as you want. Check mozilla docs for this
    $.ajax({
        data: data,
        type: "POST",
        url: posturl + 'page/savetheuploadedfile',
        cache: false,
        contentType: false,
        processData: false,
        success: function (url) {
            $("#fileprogress").fadeOut(500);

            $('#summernote').summernote('editor.insertImage', url);
        }
    });
}


function deleteFile(file, posturl) {

    $.ajax({
        data: 'filesrc=' + file,
        type: "POST",
        url: posturl + 'page/deletetheuploadedfile',
        success: function (msg) {

        }
    });
}