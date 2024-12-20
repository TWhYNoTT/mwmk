document.addEventListener("DOMContentLoaded", function () {
    // تفعيل القائمة الجانبية في الشاشات الصغيرة
    function toggleMenu() {
        const navLinks = document.getElementById('navLinks');
        if (navLinks) {
            navLinks.classList.toggle('active');
        }
    }

    // تحقق من وجود عنصر navbarToggler
    const navbarToggler = document.getElementById('navbarToggler');
    if (navbarToggler) {
        navbarToggler.addEventListener('click', toggleMenu);
    } else {
        console.warn("navbarToggler غير موجود في الصفحة.");
    }

    // التحقق من وجود عناصر القائمة المنسدلة للمستخدم
    const userDropdownToggle = document.getElementById('userDropdownToggle');
    const userDropdownMenu = document.getElementById('userDropdownMenu');

    if (userDropdownToggle && userDropdownMenu) {
        userDropdownToggle.addEventListener('click', function(e) {
            e.preventDefault();
            userDropdownMenu.classList.toggle('active');
        });

        // إخفاء القائمة إذا تم النقر خارجها
        document.addEventListener('click', function(e) {
            if (!userDropdownToggle.contains(e.target) && 
                !userDropdownMenu.contains(e.target)) {
                userDropdownMenu.classList.remove('active');
            }
        });

        // التحقق من حالة تسجيل الدخول
        let isLoggedIn = localStorage.getItem("loggedIn") === "true";
        userDropdownMenu.innerHTML = isLoggedIn
            ? `
                <li><a class="dropdown-item" href="/html/profile.html">Profile</a></li>
                <li><a class="dropdown-item" href="#" id="logout">Logout</a></li>
            `
            : `
                <li><a class="dropdown-item" href="/html/login.html">Sign In</a></li>
                <li><a class="dropdown-item" href="/html/signup.html">Register</a></li>
            `;

        if (isLoggedIn) {
            const logoutButton = document.getElementById('logout');
            logoutButton?.addEventListener('click', function() {
                localStorage.removeItem("loggedIn");
                window.location.reload(); 
            });
        }
    } else {
        console.warn("userDropdownToggle أو userDropdownMenu غير موجودين في الصفحة.");
    }
});
