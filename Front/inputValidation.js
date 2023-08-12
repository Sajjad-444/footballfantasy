function userNameValidator(userName) {
    return /^[a-zA-Z0-9_-]+$/.test(userName);
}

function emailValidator(email) {
    return /^[a-zA-Z0-9._-]+@[a-z]+\.[a-z]{3}$/.test(email);
}

function passwordValidator(password) {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()-_=+,<.>/?\\|;:'"])(?=.*[0-9])(?=.{8,}).*$/.test(password);
}

function signUp0Validator() {
    var submitButton = document.getElementById("submitButton");
    submitButton.addEventListener("click", function () {
        var firstNameValue = document.getElementsByName("firstName")[0].value;
        var lastNameValue = document.getElementsByName("lastName")[0].value;
        var isValidFirstName = /^[a-zA-Z]+$/.test(firstNameValue);
        var isValidLastName = /^[a-zA-Z]+$/.test(lastNameValue);
        firstNameError = document.getElementById("firstNameError");
        lastNameError = document.getElementById("lastNameError");
        if (!isValidFirstName && !isValidLastName) {
            firstNameError.classList.add("show-error");
            lastNameError.classList.add("show-error");
            return;
        }
        if (!isValidFirstName) {
            firstNameError.classList.add("show-error");
            lastNameError.classList.remove("show-error");

            return;
        }
        if (!isValidLastName) {
            firstNameError.classList.remove("show-error");
            lastNameError.classList.add("show-error");
            return;
        }
        firstNameError.classList.remove("show-error");
        lastNameError.classList.remove("show-error");
        localStorage.setItem("firstName", firstNameValue);
        localStorage.setItem("lastName", lastNameValue);
        window.location.href = "signUp1.html";
    });
}

function signUp1Validator() {
    var isValid = true;
    var valueList = document.getElementsByTagName("input");
    var errorList = document.getElementsByClassName("error-message");
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-z]+\.[a-z]{3}$/;
    var passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()-_=+,<.>/?\\|;:'"])(?=.*[0-9])(?=.{8,}).*$/;
    var mainPasswordError = document.getElementsByClassName("mainError")[0];
    var mainErrorMessage = document.getElementById("mainErrorMessage");
    var message = "Your password must contain at least : <br> &nbsp;- One uppercase and lowercase letter <br> &nbsp;- One number <br> &nbsp;- One special character, such as @, #, or $ <br> &nbsp;- 8 characters"
    if (!emailPattern.test(valueList[0].value)) {
        errorList[0].classList.add("show-error");
        isValid = false;
    } else {
        errorList[0].classList.remove("show-error");
    }
    if (!/^[a-zA-Z0-9_-]+$/.test(valueList[1].value)) {
        errorList[1].classList.add("show-error");
        isValid = false;
    } else {
        errorList[1].classList.remove("show-error");
    }
    if (!passwordPattern.test(valueList[2].value)) {
        errorList[2].classList.add("show-error");
        mainErrorMessage.innerHTML = message;
        mainPasswordError.classList.add("show-main-error");
        isValid = false;
    } else {
        errorList[2].classList.remove("show-error");
        mainPasswordError.classList.remove("show-main-error");
    }
    if (valueList[2].value !== valueList[3].value) {
        errorList[3].classList.add("show-error");
        isValid = false;
    }
    else {
        errorList[3].classList.remove("show-error");
    }
    return isValid;

}

function logInValidation() {
    var inputs = document.getElementsByTagName("input");
    var errors = document.getElementsByClassName("error-message");
    var validation = true;
    if (!userNameValidator(inputs[0].value) && !emailValidator(inputs[0].value)) {
        errors[0].classList.add("show-error");
        validation = false;
    }
    else {
        errors[0].classList.remove("show-error");
    }
    if (!passwordValidator(inputs[1].value)) {
        errors[1].classList.add("show-error");
        validation = false;
    }
    else {
        errors[1].classList.remove("show-error");
    }
    return validation;

}