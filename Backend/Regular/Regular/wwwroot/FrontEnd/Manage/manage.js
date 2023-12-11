window.addEventListener("resize", function () {
    if (this.window.innerWidth <= 1200)
        HideoffcanvasMenu();
    else
        ShowoffcanvasMenu();
});

window.addEventListener("load", function () {
    if (this.window.innerWidth <= 1200)
        HideoffcanvasMenu();
    else
        ShowoffcanvasMenu();
});

function HideoffcanvasMenu() {
    document.getElementById("btnShowMenuToggler").classList.remove("visually-hidden");
    document.getElementById("offcanvasMenu").classList.remove("show");

    document.getElementById("menuBox-Temp").classList.remove("col-xxl-4");
    document.getElementById("mainBox").classList.remove("col-xxl-6");
    document.getElementById("mainBox").classList.add("col-xxl-11");

    document.getElementById("menuBox-Temp").classList.remove("col-lg-4");
    document.getElementById("mainBox").classList.remove("col-lg-11");
    document.getElementById("mainBox").classList.add("col-lg-11");
}

function ShowoffcanvasMenu() {
    document.getElementById("btnShowMenuToggler").classList.add("visually-hidden");
    document.getElementById("offcanvasMenu").classList.add("show");

    document.getElementById("menuBox-Temp").classList.add("col-xxl-4");
    document.getElementById("mainBox").classList.add("col-xxl-6");
    document.getElementById("mainBox").classList.remove("col-xxl-11");

    document.getElementById("menuBox-Temp").classList.add("col-lg-4");
    document.getElementById("mainBox").classList.add("col-lg-7");
    document.getElementById("mainBox").classList.remove("col-lg-11");
}

function ToggleSecondOffcanvasMenu() {
    document.getElementById("SecondOffcanvasMenu").classList.toggle("show");
}