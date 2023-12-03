function MoveToRegister(){
    var login = document.getElementById("login");
    var register = document.getElementById("register");
    login.style = "display :none !important";
    register.style = "display :flex !important";
}

function MoveToLogin(){
    var login = document.getElementById("login");
    var register = document.getElementById("register");
    login.style = "display :flex !important";
    register.style = "display :none !important";
}