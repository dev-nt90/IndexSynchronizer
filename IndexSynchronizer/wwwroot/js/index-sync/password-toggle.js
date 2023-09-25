function togglePasswordVisibility(targetId) {
    const passwordInput = document.getElementById(targetId);

    if (passwordInput) {
        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
        }
        else {
            passwordInput.type = 'password';
        }
    }
}

// Initial state: Ensure password input fields start as password fields
const passwordInputA = document.getElementById('passwordInputSource');
const passwordInputB = document.getElementById('passwordInputTarget');

if (passwordInputA) {
    passwordInputA.type = 'password';
}

if (passwordInputB) {
    passwordInputB.type = 'password';
}

document.addEventListener("click", function (event) {
    if (event.target.classList.contains("show-password")) {
        var targetId = event.target.getAttribute("data-target");
        togglePasswordVisibility(targetId);
    }
});