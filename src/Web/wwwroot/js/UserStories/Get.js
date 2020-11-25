// Toggle comments dropdown menu
function myFunction(dropdownnumber) {
    document.getElementById("myDropdown-" + dropdownnumber).classList.toggle("show");
}

document.getElementById("edit-comment").onclick = function () {
    $.ajax({
        type: 'GET',
        url: $(this).attr('href'),
        success: function (res) {
            $('#form-modal .modal-body').html(res.html);
            $('#form-modal .modal-title').html("Edit Comment");
            $('#form-modal').modal('show');
        }
    })
    // prevent default
    return false;
};