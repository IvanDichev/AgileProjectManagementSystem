// Pop-up to ask for delete confirmation for elements with id del on the screen.
$(document).ready(function () {
    $('*[id*=delete]').on('click', function deleteItem() {
        if (confirm("Are you sure you want to delete this item?")) {
            return true;
        }
        return false;
    });
})

// Add loader to indicate loading for ajax call.
$(document).ready(function () {
    $("#loaderbody").addClass('hide');
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});