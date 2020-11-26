// Toggle comments dropdown menu.
function myFunction(dropdownnumber) {
    document.getElementById("myDropdown-" + dropdownnumber).classList.toggle("show");

    // Close the dropdown menu if the user clicks outside of it
    window.onclick = function (event) {
        if (!event.target.matches(".comment-menu-" + dropdownnumber)) {
            var dropdowns = document.getElementsByClassName("dropdown-content-" + dropdownnumber);
            var i;
            for (i = 0; i < dropdowns.length; i++) {
                var openDropdown = dropdowns[i];
                if (openDropdown.classList.contains('show')) {
                    openDropdown.classList.remove('show');
                }
            }
        }
    }
}

// Ajax GET to open modal for each comment to edit.
const commentsEditNavigation = document.querySelectorAll("#edit-comment-a");
const commentsEditNavigationArr = Array.from(commentsEditNavigation);
commentsEditNavigationArr.forEach(element => element.onclick = function () {
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
});