// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function RedirectToHomePage() {
    window.location = "/Home/Index";
}

function SignUp(userName, firstName, lastName, password, confirmedPassword) {

    let validation = userName.length > 0 &&
        firstName.length > 0 &&
        lastName.length > 0 &&
        password.length >= 8 &&
        password == confirmedPassword;

    if (validation) {
        let registration = { "UserName": userName, "FirstName": firstName, "LastName": lastName, "Password": password };

        let request = new XMLHttpRequest();
        request.open("POST", "/Home/SignUp", true);
        request.send(JSON.stringify(registration));

        request.onloadend = function () {
            if (request.status == 1) {
                window.location = "/Home/SignIn"
            }
        }
    }
}

function SignIn(userName, password) {

    let validation = userName.length > 0 && password.length > 0;

    if (validation) {
        let signInRequest = { "UserName": userName, "Password": password };

        let request = new XMLHttpRequest();
        request.open("POST", "/Home/SignIn", true);
        request.send(JSON.stringify(signInRequest));

        request.onloadend = function () {
            if (request.status == 1) {
                window.location = "/Home/Products";
            }
        }
    }
}

function SendMessage(message) {
    let request = new XMLHttpRequest();
    request.open("POST", "/Home/Contacts", true);
    request.send(message);

    request.onloadend = function () {
        if (request.status == 1) {
            document.getElementById("txtMessage").value = "";

            document.getElementById("content1").style.display = "none";
            document.getElementById("content2").style.display = "block";
        }
    }
}

function CloseMessage() {
    document.getElementById("content1").style.display = "block";
    document.getElementById("content2").style.display = "none";
}

function QuickNav_MouseHover(e) {
    e.srcElement.style.backgroundColor = "#286995"
    e.srcElement.style.color = "white";
}

function QuickNav_MoveLeave(e) {
    e.srcElement.style.backgroundColor = 'white';
    e.srcElement.style.color = "black";
}
