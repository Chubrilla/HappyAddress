document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("toastContainer");

    if (!container || !container.dataset.toastMessage) {
        return;
    }

    const toast = document.createElement("div");
    toast.classList.add("toast");

    if (container.dataset.toastType === "error") {
        toast.classList.add("toast-error");
    } else {
        toast.classList.add("toast-success");
    }

    toast.textContent = container.dataset.toastMessage;
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
