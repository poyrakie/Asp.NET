function hideSuccessMessage() {
    var successDiv = document.getElementById("successMessage");
    if (successDiv) {
        successDiv.style.display = "none";
    }
}

function showSuccessMessage() {
    var successDiv = document.getElementById("successMessage");
    successDiv.style.display = "block";

    setTimeout(function () {
        hideSuccessMessage();
    }, 3000);
}

showSuccessMessage();