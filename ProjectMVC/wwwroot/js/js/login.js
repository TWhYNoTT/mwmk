
 const toggleCurrentPassword = document.querySelector('#toggleCurrentPassword');
 const currentPasswordInput = document.querySelector('#current-password');

 toggleCurrentPassword.addEventListener('click', function () {
     const type = currentPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
     currentPasswordInput.setAttribute('type', type);
     this.classList.toggle('fa-eye-slash');
 });

 const toggleNewPassword = document.querySelector('#toggleNewPassword');
 const newPasswordInput = document.querySelector('#new-password');

 toggleNewPassword.addEventListener('click', function () {
     const type = newPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
     newPasswordInput.setAttribute('type', type);
     this.classList.toggle('fa-eye-slash');
 });


 const toggleConfirmNewPassword = document.querySelector('#toggleConfirmNewPassword');
 const confirmNewPasswordInput = document.querySelector('#confirmnew-password');

 toggleConfirmNewPassword.addEventListener('click', function () {
     const type = confirmNewPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
     confirmNewPasswordInput.setAttribute('type', type);
     this.classList.toggle('fa-eye-slash');
 });
 const hidePassword = document.querySelector('#hidePassword');
    const passwordInput = document.querySelector('#password');
    
    hidePassword.addEventListener('click', function () {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);
        this.classList.toggle('fa-eye-slash');
    });

    document.getElementById("loginButton").addEventListener("click", function() {
     
        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        if(username && password) {  
            localStorage.setItem("isLoggedIn", "true");
            window.location.href = "/index.html"; 
        } else {
            alert("Please enter your username and password."); 
        }
    });
