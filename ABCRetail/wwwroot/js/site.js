// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Redirects the user to the homepage.
function RedirectToHomePage() {
    window.location = "/Home/Index";
}

// Registers a user with ABC Retail.
function SignUp(userName, firstName, lastName, password, confirmedPassword) {

    // Perform validation on the parameters.
    let validation = userName.length > 0 &&
        firstName.length > 0 &&
        lastName.length > 0 &&
        password.length >= 8 &&
        password == confirmedPassword;

    // Continue if the validation succeeds.
    if (validation) {
        let registration = { "UserName": userName, "FirstName": firstName, "LastName": lastName, "Password": password };

        // Declare and instantiate a XMLHttpRequest object.
        let request = new XMLHttpRequest();
        // Open a new POST request.
        request.open("POST", "/Home/SignUp", true);
        // Send the data to the POST request.
        request.send(JSON.stringify(registration));

        request.onloadend = function () {
            if (request.status == 1) {
                // The request succeeded/
                window.location = "/Home/SignIn"
            }

            if (request.status == 2) {
                // The request failed.
                document.getElementById("content1").style.display = "none";
                document.getElementById("content2").style.display = "block";
            }
        }
    }
    else {
        // The validation failed.
        document.getElementById("content1").style.display = "none";
        document.getElementById("content2").style.display = "block";
    }
}

// Attempts to sign in a user with the provided credentials.
function SignIn(userName, password) {
    // Perform validation on the parameters.
    let validation = userName.length > 0 && password.length > 0;

    // Continue if the validation succeeds.
    if (validation) {
        let signInRequest = { "UserName": userName, "Password": password };

        // Declare and instantiate a XMLHttpRequest object.
        let request = new XMLHttpRequest();
        // Open a new POST request.
        request.open("POST", "/Home/SignIn", true);
        // Send the data to the POST request.
        request.send(JSON.stringify(signInRequest));

        request.onloadend = function () {
            if (request.status == 1) {
                // The request succeeded.
                window.location = "/Home/Products";
            }

            if (request.status == 2) {
                // The request failed.
                document.getElementById("content1").style.display = "none";
                document.getElementById("content2").style.display = "block";
            }
        }
    }
    else {
        // The validation failed.
        document.getElementById("content1").style.display = "none";
        document.getElementById("content2").style.display = "block";
    }
}

// This method sends a message to ABC Retail's storage system.
function SendMessage(message) {
    // Declare and instantiate a XMLHttpRequest object.
    let request = new XMLHttpRequest();
    // Open a new POST request.
    request.open("POST", "/Home/Contacts", true);
    // Send the data to the POST request.
    request.send(message);

    request.onloadend = function () {
        if (request.status == 1) {
            // The request succeeded.
            document.getElementById("txtMessage").value = "";

            document.getElementById("content1").style.display = "none";
            document.getElementById("content2").style.display = "block";
        }
    }
}

// Closes the confirmation message for sending a messsage.
function CloseMessage() {
    document.getElementById("content1").style.display = "block";
    document.getElementById("content2").style.display = "none";
}

// Highlights the Quick Navigation menu.
function QuickNav_MouseHover(e) {
    e.srcElement.style.backgroundColor = "#286995"
    e.srcElement.style.color = "white";
}

// Removes the highlights from the Quick Navigation menu.
function QuickNav_MouseLeave(e) {
    e.srcElement.style.backgroundColor = 'white';
    e.srcElement.style.color = "black";
}

// Performs the operation of buying a product.
function BuyProduct(productId, userName, mode) {
    // Declare and instantiate a XMLHttpRequest object.
    let request = new XMLHttpRequest();
    // Open a new POST request.
    request.open("POST", "/Home/BuyProduct", true);

    let requestData = { "ProductId": productId, "UserName": userName, "Mode": mode };
    // Send the data to the POST request.
    request.send(JSON.stringify(requestData));

    request.onloadend = function () {
        // Redirect the user to the BuyProduct page.
        window.location = "/Home/BuyProduct/?Action=" + mode + "&ProductId=" + productId;
    }
}