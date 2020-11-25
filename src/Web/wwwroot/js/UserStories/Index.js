// Hide selected sort arrow from table
window.onload = function () {
    const allSortingArrows = document.getElementsByClassName("sort-arrow");
    for (var i = 0; i < allSortingArrows.length; i++) {
        if (allSortingArrows[i].href === window.location.href) {
            allSortingArrows[i].remove();
        }
    }
};