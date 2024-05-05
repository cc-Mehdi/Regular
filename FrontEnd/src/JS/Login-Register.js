var login = document.getElementById("login");
var register = document.getElementById("register");
var registerBox_banner = document.getElementById("registerBox-banner");
var register_box = document.getElementById("register-box");


function MoveToRegister(){
    login.style = "display :none !important";
    register.style = "display :flex !important";
    registerBox_banner.classList.toggle("registerBox-banner");
    register_box.classList.toggle("register-box");
    document.getElementsByTagName("title")[0].innerHTML = "Regular - ثبت نام";
}

function MoveToLogin(){
    login.style = "display :flex !important";
    register.style = "display :none !important";
    registerBox_banner.classList.toggle("registerBox-banner");
    register_box.classList.toggle("register-box");
    document.getElementsByTagName("title")[0].innerHTML = "Regular - ورود";
}

function isRegister(){
    const searchParams = new URLSearchParams(window.location.search);
    if(searchParams == "isRegister=true")
        MoveToRegister();
}