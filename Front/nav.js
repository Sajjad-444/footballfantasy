function toggleNavBar() {
    let nav = document.getElementById("mobileNav");
    event.preventDefault();
    let computedStyle = window.getComputedStyle(nav);
    let visibility = computedStyle.height;
    if (visibility === "0px") {
        nav.classList.add("enableMobileNav");
    }
    else {
        nav.classList.remove("enableMobileNav");
    }
}

function goToSignIn() {
    window.location.href = "Login.html";
}