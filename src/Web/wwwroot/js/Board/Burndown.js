// Get burndown chard data.
var path = window.location.pathname.split('/')
var projectId = path[path.length - 1]
var url = window.location.origin + '/Boards/GetBurndownData/' + projectId + window.location.search

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: url,
        contentType: false,
        processData: false,
        success: function (res) {
            var days = res.daysInSprint
            var tasks = res.tasksRemaining
            var scope = res.scopeChanges
            showBurnDown(
                "burndown43",
                tasks,
                scope,
                days,
            );
            var velocity = res.velocity
            $('#velocity-data').text = velocity;
        }
    })
});