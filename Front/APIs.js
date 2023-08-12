function requestByJson(method, pattern, data) {
    return fetch('http://localhost:3001/' + pattern, {
        method: method,
        body: JSON.stringify(data),
        headers: data ? { 'content-type': 'application/json', 'mode': 'no-cors' } : {}
    })
        .then(response => {
            return response.json();
        })
        .catch(error => {
            console.log(error);
            throw Error("Something went wrong, please try again later");
        })
}

function requestByParams(method, pattern, queryParams) {
    const queryString = new URLSearchParams(queryParams).toString();
    const url = `http://localhost:3001/${pattern}?${queryString}`;

    return fetch(url, {
        method: method,
        headers: queryParams ? { 'content-type': 'application/x-www-form-urlencoded', 'mode': 'no-cors' } : {},
        // credentials: 'include'
    })
        .then(response => {
            return response.json();
        })
        .catch(error => {
            console.log(error);
            throw Error("Something went wrong, please try again later");
        })
}

function requestByParamsAndJson(method, pattern, queryParams, data) {
    const queryString = new URLSearchParams(queryParams).toString();
    const url = `http://localhost:3001/${pattern}?${queryString}`;

    return fetch(url, {
        method: method,
        body: JSON.stringify(data),
        headers: {
            'content-type': 'application/json',
            'mode': 'no-cors'
        }
    })
        .then(response => {
            return response.json();
        })
        .catch(error => {
            console.log(error);
            throw new Error("Something went wrong, please try again later");
        });
}


function setCookie(name, value, days) {
    const expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + days);

    const cookieValue = value + "; expires=" + expirationDate.toUTCString() + "; path=/";
    document.cookie = name + "=" + cookieValue;
}

function getCookie(name) {
    let cookies = document.cookie.split(";");
    for (let i = 0; i < cookies.length; i++) {
        let pair = cookies[i].split("=");
        if (pair[0] === name) {
            return pair[1];
        }
    }
    return null;
}

function userSignUp() {
    var btn = document.getElementById("submitButton");
    var mainError = document.getElementById("mainError");
    var mainErrorMessage = document.getElementById("mainErrorMessage");
    var loader = document.getElementById("loader");
    var inputs = document.getElementsByTagName("input");
    var firstName = localStorage.getItem("firstName");
    var lastName = localStorage.getItem("lastName");
    if (firstName === null || lastName === null) {
        alert("Something went wrong. Please sign up again.");
        return;
    }
    loader.style.display = "block";
    btn.disabled = true;
    btn.style.display = "none";
    var user = {
        firstName: firstName,
        lastName: lastName,
        emailAddress: inputs[0].value,
        userName: inputs[1].value,
        password: inputs[2].value,
    }
    requestByJson('POST', "signUp", user)
        .then(response => {
            if (response.success) {
                localStorage.setItem("userName", user.userName);
                localStorage.setItem("codeExpTime", response.data[1].message.substring(3, 8));
                if (response.data.length === 3) {
                    setCookie("FootballFantasyToken", response.data[2].message, 7);
                }
                window.location.href = "EmailVerification.html?source=signUp";
            }
            else {
                loader.style.display = "none";
                btn.disabled = false;
                btn.style.display = "block";
                mainErrorMessage.innerHTML = response.data[0].message;
                mainError.classList.add("show-main-error");
            }
        })
        .catch(error => {
            mainError.classList.remove("show-main-error");
            loader.style.display = "none";
            btn.disabled = false;
            btn.style.display = "block";
            console.log(error);
            setTimeout(() => {
                alert(error.message);
            }, 100);
        });
}

function userSignUpByToken() {
    var buttons = document.getElementById("buttons");
    var frontToken = getCookie("FootballFantasyToken");
    var userName = localStorage.getItem("userName");
    var signUpBtn = document.getElementById("signUp");
    var loader = document.getElementById("loader");
    signUpBtn.disabled = true;
    buttons.style.display = "none";
    loader.style.display = "block";
    if (frontToken === null || userName === null) {
        window.location.href = ("signUp0.html")
        return;
    }
    const params = {
        frontToken: frontToken
    }
    requestByParams('POST', "autoSignUp", params)
        .then(response => {
            if (response.success) {
                localStorage.setItem("codeExpTime", response.data[1].message.substring(3, 8));
                if (response.data.lenght === 3) {
                    setCookie("FootballFantasyToken", response.data[2].message, 7);
                }
                window.location.href = "EmailVerification.html?source=signUp"
            }
            else {
                window.location.href = "signUp0.html";
            }
        })
        .catch(error => {
            signUpBtn.disabled = false;
            buttons.style.display = "block";
            loader.style.display = "none";
            console.log(error);
            setTimeout(() => {
                alert(error.message);
            }, 100);
        })
}

async function tokenAuthentication() {
    let frontToken = getCookie("FootballFantasyToken");
    const params = {
        frontToken: frontToken
    }
    try {
        const response = await requestByParams('POST', "tokenAuthentication", params);
        return response.success;
    }
    catch (error) {
        return false;
    }
}

function userLogIn() {
    var btn = document.getElementById("submitButton");
    var mainError = document.getElementById("mainError");
    var mainErrorMessage = document.getElementById("mainErrorMessage");
    var loader = document.getElementById("loader");
    var inputs = document.getElementsByTagName("input");
    loader.style.display = "block";
    btn.disabled = true;
    btn.style.display = "none";
    const params = {
        input: inputs[0].value,
        password: inputs[1].value
    }
    requestByParams('POST', "login", params)
        .then(response => {
            if (response.success) {
                setCookie("FootballFantasyToken", response.data[1].message, 7);
                window.location.href = "Index.html";
            }
            else {
                loader.style.display = "none";
                btn.disabled = false;
                btn.style.display = "block";
                mainErrorMessage.innerHTML = response.data[0].message;
                mainError.classList.add("show-main-error");
            }
        }).catch(error => {
            loader.style.display = "none"
            btn.disabled = false;
            btn.style.display = "block";
            console.log(error);
            setTimeout(() => {
                alert(error.message);
            }, 100);
        })
}

function EmailVerification() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const source = urlParams.get('source');
    var resendBtn = document.getElementById("resendBtn");
    var mainError = document.getElementById("mainError");
    var mainErrorMessage = document.getElementById("mainErrorMessage");
    resendBtn.disabled = true;
    if (source === "signUp") {
        var userName = localStorage.getItem("userName");
        var digits = document.querySelectorAll('.OtpCode input');
        var otpCode = "";
        digits.forEach(element => {
            otpCode += element.value;
        });
        const params = {
            otpCode: otpCode,
            userName: userName
        }
        requestByParams('POST', "codeVerification", params)
            .then(response => {
                if (response.success) {
                    localStorage.clear();
                    window.location.href = "signedUpSuccessfully.html";
                }
                else {
                    resendBtn.disabled = false;
                    mainErrorMessage.innerHTML = response.data[0].message;
                    mainError.classList.add("show-main-error");
                }
            })
            .catch(error => {
                resendBtn.disabled = false;
                console.log(error);
                setTimeout(() => {
                    alert(error.message);
                }, 100);
            })
    }

}

async function resendOtpCode() {
    var resendBtn = document.getElementById("resendBtn");
    var userName = localStorage.getItem("userName");
    var mainError = document.getElementById("mainError");
    var mainErrorMessage = document.getElementById("mainErrorMessage");
    var loader = document.getElementById("loader");
    var submitForm = document.getElementById("submitForm");
    resendBtn.disabled = true;
    submitForm.style.display = "none";
    loader.style.display = "block";
    const params = {
        userName: userName
    }
    try {
        const response = await requestByParams('POST', "resendOtpCode", params)
        if (response.success) {
            mainError.classList.remove("show-main-error");
            localStorage.setItem("codeExpTime", response.data[1].message.substring(3, 8));
            setCookie("FootballFantasyToken", response.data[2].message);
            resendBtn.disabled = false;
            loader.style.display = "none";
            submitForm.style.display = "block";
            return true;
        }
        else {
            mainErrorMessage.innerHTML = response.data[0].message;
            mainError.classList.add("show-main-error");
            resendBtn.disabled = false;
            submitForm.display = "block";
            loader.style.display = "none";
            return false;
        }
    }
    catch (error) {
        resendBtn.disabled = false;
        submitForm.display = "block";
        loader.style.display = "none";
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
        return false;
    }
}

async function filterSoccerPlayers(pageNum) {
    function findActivePosition(positions) {
        let i = 0;
        for (i = 0; i < positions.length; i++) {
            if (positions[i].getAttribute("active") === "1") {
                return i;
            }
        }
        return 0;
    }

    function findActiveOrder(orders) {
        let i = 0;
        for (i = 0; i < orders.length; i++) {
            if (orders[i].getAttribute("active") === "1") {
                return i + 1;
            }
        }
        return 0;
    }

    function findActiveOrderType(orderTypes) {
        let i = 0;
        for (i = 0; i < orderTypes.length; i++) {
            if (orderTypes[i].getAttribute("active") === "1") {
                return i + 1;
            }
        }
        return 0;
    }

    let name = document.querySelector(".filterByName input");
    let positions = document.querySelectorAll('.positions input[type="submit"]');
    let minPrice = document.querySelectorAll(".input-min")[0];
    let maxPrice = document.querySelectorAll(".input-max")[0];
    let minScore = document.querySelectorAll(".input-min")[1];
    let maxScore = document.querySelectorAll(".input-max")[1];
    let orders = document.querySelectorAll('.order-of input[type="submit"]');
    let orderTypes = document.querySelectorAll('.order-type input[type="submit"]');

    const params = {
        name: name.value,
        priceStart: minPrice.value,
        priceEnd: maxPrice.value,
        scoreStart: minScore.value,
        scoreEnd: maxScore.value,
        position: findActivePosition(positions),
        trend: findActiveOrderType(orderTypes),
        trendOf: findActiveOrder(orders),
        pageNum: pageNum
    }
    try {
        const response = await requestByParams('GET', "filterSoccerPlayersList", params);
        if (response.success) {
            let temp = {
                soccerPlayers: response.soccerPlayers,
                pageCount: response.pageCount
            }
            return temp;
        }
        console.log(response.data[0]);
        setTimeout(() => {
            alert(response.data[0].message);
        }, 100);

    }
    catch (error) {
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
    }

}

async function getUsers(pageNum) {
    const params = {
        pageNum: pageNum
    }
    try {
        const response = await requestByParams('GET', "getUsers", params);
        if (response.success) {
            let temp = {
                users: response.users,
                pageCount: response.pageCount
            }
            return temp;
        }
        console.log(response.data[0]);
        setTimeout(() => {
            alert(response.data[0].message);
        }, 100);
    }
    catch (error) {
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
    }
}

async function addSoccerPlayer(soccerPlayer) {
    var token = getCookie("FootballFantasyToken");
    if (token === null) {
        alert("Why are you here, Sign in First")
        return;
    }
    let params = {
        frontToken: token
    }
    let data = soccerPlayer;
    try {
        const response = await requestByParamsAndJson('POST', "addSoccerPlayer", params, data);
        if (response.success) {
            return true;
        }
        else {
            alert("You are not allowed to add this Soccer Player");
            return false;
        }

    }
    catch (error) {
        return false;
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
    }
}

async function getUserSoccerPlayers() {
    var token = getCookie("FootballFantasyToken");
    if (token === null) {
        alert("Why are you here, Sign in First")
        return;
    }
    let params = {
        frontToken: token
    }
    try {
        const response = await requestByParams("GET", "getUserSoccerPlayers", params);
        if (response.success) {
            return response.soccerPlayers;
        }
        else {
            alert("table couldn't get updated")
            return null;
        }
    }
    catch (error) {
        return null;
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
    }
}

async function removeSoccerPlayer(soccerPlayer) {
    var token = getCookie("FootballFantasyToken");
    if (token === null) {
        alert("Why are you here, Sign in First")
        return;
    }
    let params = {
        frontToken: token
    }
    let data = soccerPlayer
    try {
        const response = await requestByParamsAndJson("POST", "removeSoccerPlayer", params, data);
        if (response.success) {
            return true;
        }
        else {
            alert("The SoccerPlayer isn't removed successfully")
            return false;
        }
    }
    catch (error) {
        return null;
        console.log(error);
        setTimeout(() => {
            alert(error.message);
        }, 100);
    }
}