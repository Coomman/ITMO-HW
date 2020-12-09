const popupWrapper = document.getElementById("contact-popup-wrapper");
const popup = document.getElementById("contact-popup");

function openPopup() {
    popupWrapper.classList.add("popup-wrapper_active");
}

function closePopup() {
    popupWrapper.classList.remove("popup-wrapper_active");
}


popup.addEventListener("click", e => {e.stopPropagation()})