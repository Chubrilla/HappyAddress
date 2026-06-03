(function () {
    "use strict";

    let currentImageIndex = 0;
    let detailsModalMap = null;

    function getGalleryImages() {
        const modal = document.getElementById("galleryModal");

        if (!modal || !modal.dataset.galleryImages) {
            return [];
        }

        try {
            return JSON.parse(modal.dataset.galleryImages);
        } catch (error) {
            return [];
        }
    }

    function updateGalleryImage() {
        const galleryImage = document.getElementById("galleryImage");
        const galleryImages = getGalleryImages();

        if (galleryImage && galleryImages[currentImageIndex]) {
            galleryImage.src = galleryImages[currentImageIndex];
        }
    }

    function openGallery(index) {
        const modal = document.getElementById("galleryModal");
        const galleryImages = getGalleryImages();

        if (!modal || galleryImages.length === 0) {
            return;
        }

        currentImageIndex = Number(index) || 0;
        modal.style.display = "flex";
        updateGalleryImage();
    }

    function closeGallery() {
        const modal = document.getElementById("galleryModal");

        if (modal) {
            modal.style.display = "none";
        }
    }

    function setImage(index) {
        currentImageIndex = Number(index) || 0;
        updateGalleryImage();
    }

    function nextImage() {
        const galleryImages = getGalleryImages();

        if (galleryImages.length === 0) {
            return;
        }

        currentImageIndex = (currentImageIndex + 1) % galleryImages.length;
        updateGalleryImage();
    }

    function prevImage() {
        const galleryImages = getGalleryImages();

        if (galleryImages.length === 0) {
            return;
        }

        currentImageIndex = currentImageIndex - 1;

        if (currentImageIndex < 0) {
            currentImageIndex = galleryImages.length - 1;
        }

        updateGalleryImage();
    }

    function showPhone(button) {
        const phone = button.getAttribute("data-phone");

        if (!phone || phone.trim() === "" || phone === "null") {
            button.textContent = "Телефон не указан";
            return;
        }

        button.textContent = "Телефон: " + phone.trim();
        button.disabled = true;
    }

    function getMapModalConfig() {
        const modal = document.getElementById("detailsMapModal");

        if (!modal) {
            return null;
        }

        const latitude = Number(modal.dataset.latitude);
        const longitude = Number(modal.dataset.longitude);

        if (Number.isNaN(latitude) || Number.isNaN(longitude)) {
            return null;
        }

        return {
            modal,
            latitude,
            longitude,
            title: modal.dataset.title || "Объявление"
        };
    }

    function initDetailsModalMap() {
        const config = getMapModalConfig();
        const mapElement = document.getElementById("detailsModalMap");

        if (!config || !mapElement || typeof L === "undefined") {
            return;
        }

        if (detailsModalMap) {
            detailsModalMap.invalidateSize();
            detailsModalMap.setView([config.latitude, config.longitude], 16);
            return;
        }

        detailsModalMap = L.map("detailsModalMap").setView([config.latitude, config.longitude], 16);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: ""
        }).addTo(detailsModalMap);

        L.marker([config.latitude, config.longitude])
            .addTo(detailsModalMap)
            .bindPopup(config.title)
            .openPopup();

        setTimeout(function () {
            detailsModalMap.invalidateSize();
        }, 250);
    }

    function openDetailsMapModal() {
        const config = getMapModalConfig();

        if (!config) {
            return;
        }

        config.modal.style.display = "flex";
        document.body.classList.add("modal-open");
        setTimeout(initDetailsModalMap, 150);
    }

    function closeDetailsMapModal() {
        const modal = document.getElementById("detailsMapModal");

        if (!modal) {
            return;
        }

        modal.style.display = "none";
        document.body.classList.remove("modal-open");
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll("[data-gallery-open]").forEach(function (element) {
            element.addEventListener("click", function () {
                openGallery(element.dataset.galleryOpen);
            });
        });

        document.querySelectorAll("[data-gallery-set]").forEach(function (element) {
            element.addEventListener("click", function () {
                setImage(element.dataset.gallerySet);
            });
        });

        document.querySelectorAll("[data-action]").forEach(function (element) {
            element.addEventListener("click", function () {
                switch (element.dataset.action) {
                    case "close-gallery":
                        closeGallery();
                        break;
                    case "next-gallery-image":
                        nextImage();
                        break;
                    case "previous-gallery-image":
                        prevImage();
                        break;
                    case "show-phone":
                        showPhone(element);
                        break;
                    case "open-details-map":
                        openDetailsMapModal();
                        break;
                    case "close-details-map":
                        closeDetailsMapModal();
                        break;
                }
            });
        });

        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeDetailsMapModal();
                closeGallery();
            }
        });

        document.addEventListener("click", function (event) {
            const modal = document.getElementById("detailsMapModal");

            if (modal && event.target === modal) {
                closeDetailsMapModal();
            }
        });
    });
})();
