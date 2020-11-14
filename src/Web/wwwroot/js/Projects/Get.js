$('#del').on('click', function deleteItem() {
    if (confirm("Are you sure you want to delete this item?")) {
        $.dialog.bind()
        return true;
    }
    return false;
});

document.getElementById("edit").onclick = function () {
    $.ajax({
        type: 'GET',
        url: $(this).attr('href'),
        success: function (res) {
            $('#form-modal .modal-body').html(res.html);
            $('#form-modal .modal-title').html(document.getElementById("project-name"));
            $('#form-modal').modal('show');
        }
    })
    // prevent default
    return false;
};

EditAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#description').text(res.newDescription)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}