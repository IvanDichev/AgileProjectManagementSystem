// Push main content with leftNav.
function openNav() {
    $.cookie('isNavOpen', '1', { path: '/'});
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    $.cookie('isNavOpen', '0', { path: '/' });
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main-container").style.marginLeft = "0";
}

function openNav() {
    $.cookie('isNavOpen', '1', { path: '/' });
    document.getElementById("mySidenav").style.width = "250px";
    if (screen.width > 550) {
        document.getElementById("main-container").style.marginLeft = "250px";
    }
}

// Toggle left/rigth arrow icon for open/close leftNav.
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

// Manage last leftNav state.
$(function () {
    if ($.cookie('isNavOpen') == '1') {
        $(".sidenav").css("width", "250px").css("transition", "0s");
        if (window.location.pathname.split('/')[1].toLocaleLowerCase() !== 'identity'
            && window.location.pathname.split('/')[2] != null) {
            $("#main-container").css("margin-left", "250px");
        }
        $('.open-nav').hide();
        $('.close-nav').show();
    }
    else {
        $(".sidenav").css("width", "0px").css("transition", "0s");
        $("#main-container").css("margin-left", "0px")
        $('.open-nav').show();
        $('.close-nav').hide();
    }
});