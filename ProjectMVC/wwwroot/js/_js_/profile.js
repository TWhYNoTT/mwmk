document.getElementById('update-profile').addEventListener('click', function() {
    const email = document.getElementById('email').value;
    const firstName = document.getElementById('first-name').value;
    const lastName = document.getElementById('last-name').value;
    const phone = document.getElementById('phone').value;
    const birthday = document.getElementById('birthday').value;
    const gender = document.querySelector('input[name="gender"]:checked').value;
    const nationality = document.getElementById('nationality').value;

    if (email && firstName && lastName && phone && birthday && gender && nationality) {
        alert("Profile updated successfully!");
        // هنا يمكن إضافة وظيفة لحفظ البيانات في قاعدة بيانات أو localStorage
    } else {
        alert("Please fill all the fields.");
    }
});
