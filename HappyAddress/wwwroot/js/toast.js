document.addEventListener("DOMContentLoaded", function () {
    if (!window.toastMessage) {
        return;
    }

    const container = document.getElementById("toastContainer");

    if (!container) {
        return;
    }

    const toast = document.createElement("div");
    toast.classList.add("toast");

    if (window.toastType === "error") {
        toast.classList.add("toast-error");
    } else {
        toast.classList.add("toast-success");
    }

    toast.textContent = window.toastMessage;
    container.appendChild(toast);

    setTimeout(function () {
        toast.classList.add("show");
    }, 100);

    setTimeout(function () {
        toast.classList.remove("show");

        setTimeout(function () {
            toast.remove();
        }, 300);
    }, 3000);
});