// Push main content with leftNav
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main-container").style.marginLeft = "0";
}

function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    if (screen.width > 550) {
        document.getElementById("main-container").style.marginLeft = "250px";
    }
}

// Toggle left/rigth arrow icon for open/close leftNav
$(document).ready(function () {
    $(".close-nav").click(function (event) {
        event.preventDefault();
        $(".open-nav").show();
        $(".close-nav").hide();
    });
    $(".open-nav").click(function (event) {
        event.preventDefault();
        $(".open-nav").hide();
        $(".close-nav").show();
    });
});