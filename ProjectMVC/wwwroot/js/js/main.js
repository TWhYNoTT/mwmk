document.addEventListener("DOMContentLoaded", function () {
    fetch('/html/header.html')
        .then(response => response.text())
        .then(data => {
            document.body.insertAdjacentHTML('afterbegin', data);
        });


    fetch('/html/footer.html')
        .then(response => response.text())
        .then(data => {
            document.body.insertAdjacentHTML('beforeend', data);
        });
});

function viewProduct(productId) {
    localStorage.setItem('selectedProduct', productId);
    window.location.href = 'product-details.html';
}

function changeImage(newSrc) {
    const mainImage = document.getElementById('main-image');
    mainImage.src = newSrc;
}

const colors = document.querySelectorAll('.color');
const selectedColorText = document.getElementById('selected-color');

colors.forEach(color => {
    color.addEventListener('click', () => {
        // إزالة الصفة "selected" من جميع الألوان
        colors.forEach(c => c.classList.remove('selected'));
        
        // إضافة الصفة "selected" للون المحدد
        color.classList.add('selected');
        
        // تحديث النص ليعكس اللون المحدد
        selectedColorText.textContent = color.classList[1]; // الحصول على اسم اللون
    });
});

// اختيار المقاس
const sizes = document.querySelectorAll('.size');
const selectedSizeText = document.getElementById('selected-size');

sizes.forEach(size => {
    size.addEventListener('click', () => {
        // إزالة الصفة "selected" من جميع المقاسات
        sizes.forEach(s => s.classList.remove('selected'));
        
        // إضافة الصفة "selected" للمقاس المحدد
        size.classList.add('selected');
        
        // تحديث النص ليعكس المقاس المحدد
        selectedSizeText.textContent = size.textContent; // الحصول على نص المقاس
    });
});

function decreaseQuantity() {
    const quantityInput = document.getElementById('quantity-input');
    let quantity = parseInt(quantityInput.value);
    if (quantity > 1) {
        quantity--;
        quantityInput.value = quantity;
        updateSubtotal(quantity);
    }
}

function increaseQuantity() {
    const quantityInput = document.getElementById('quantity-input');
    let quantity = parseInt(quantityInput.value);
    quantity++;
    quantityInput.value = quantity;
    updateSubtotal(quantity);
}

function updateSubtotal(quantity) {
    const price = 399.00;
    const subtotal = price * quantity;
    document.getElementById('subtotal').innerText = `LE ${subtotal.toFixed(2)}`;
}
function showHiddenPage(event) {
    event.stopPropagation();
    document.getElementById('hiddenPage').style.display = 'block';
}

function continueShopping() {
    document.getElementById('hiddenPage').style.display = 'none';
}

function proceedToCheckout() {
    window.location.href = '/html/checkout.html';
}
function showPopup() {
    document.getElementById('popup').style.display = 'flex';
}

function closePopup() {
    document.getElementById('popup').style.display = 'none';
}

function continueShopping() {
    closePopup();
    
}

function checkout() {
    closePopup();
    window.location.href = '/html/checkout.html'; 
}