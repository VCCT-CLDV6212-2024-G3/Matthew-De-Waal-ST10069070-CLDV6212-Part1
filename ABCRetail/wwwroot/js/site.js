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