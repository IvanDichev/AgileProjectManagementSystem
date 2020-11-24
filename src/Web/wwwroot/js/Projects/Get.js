// Ajax GET to open modal for editing project.
document.getElementById("edit").onclick = function () {
    $.ajax({
        type: 'GET',
        url: $(this).attr('href'),
        success: function (res) {
            $('#form-modal .modal-body').html(res.html);
            $('#form-modal .modal-title').html($("#project-name").html());
            $('#form-modal').modal('show');
        }
    })
    // prevent default
    return false;
};

// Ajax POST to edit project.
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