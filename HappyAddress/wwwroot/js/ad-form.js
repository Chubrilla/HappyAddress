(function () {
    "use strict";

    const DEFAULT_LATITUDE = 55.751244;
    const DEFAULT_LONGITUDE = 37.618423;
    const WORLD_BOUNDS = L.latLngBounds([[-85, -180], [85, 180]]);

    let map = null;
    let marker = null;

    function parseNumber(value, fallback) {
        if (!value) {
            return fallback;
        }

        const parsed = Number(String(value).replace(",", "."));
        return Number.isNaN(parsed) ? fallback : parsed;
    }

    function setDisplay(element, isVisible, displayValue) {
        if (element) {
            element.style.display = isVisible ? displayValue : "none";
        }
    }

    function setCoordinateInputs(lat, lng) {
        const latitudeInput = document.getElementById("latitudeInput");
        const longitudeInput = document.getElementById("longitudeInput");

        if (latitudeInput) {
            latitudeInput.value = String(lat).replace(".", ",");
        }

        if (longitudeInput) {
            longitudeInput.value = String(lng).replace(".", ",");
        }
    }

    async function getAddressFromCoordinates(lat, lng) {
        const addressInput = document.getElementById("addressInput");
        const cityInput = document.getElementById("cityInput");

        if (!addressInput || !cityInput) {
            return;
        }

        const url = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&addressdetails=1`;
        const response = await fetch(url);
        const data = await response.json();

        if (!data || !data.address) {
            return;
        }

        const address = data.address;
        const parts = [
            address.country,
            address.city || address.town || address.village || address.hamlet,
            address.city_district || address.suburb || address.county,
            address.road
        ].filter(Boolean);

        if (address.house_number) {
            parts.push(address.house_number);
        }

        addressInput.value = parts.join(", ");
        cityInput.value = address.city || address.town || address.village || address.hamlet || "";
    }

    async function setMarker(lat, lng, shouldUpdateAddress) {
        setCoordinateInputs(lat, lng);

        if (marker) {
            marker.setLatLng([lat, lng]);
        } else {
            marker = L.marker([lat, lng], { draggable: true }).addTo(map);

            marker.on("dragend", async function () {
                const position = marker.getLatLng();
                setCoordinateInputs(position.lat, position.lng);
                await getAddressFromCoordinates(position.lat, position.lng);
            });
        }

        if (shouldUpdateAddress) {
            await getAddressFromCoordinates(lat, lng);
        }
    }

    async function findAddressOnMap() {
        if (!map) {
            return;
        }

        const addressInput = document.getElementById("addressInput");
        const query = addressInput ? addressInput.value : "";

        if (!query || query.trim() === "") {
            alert("Введите адрес");
            return;
        }

        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}`;
        const response = await fetch(url);
        const data = await response.json();

        if (!Array.isArray(data) || data.length === 0) {
            alert("Адрес не найден. Попробуйте уточнить адрес.");
            return;
        }

        const lat = parseFloat(data[0].lat);
        const lng = parseFloat(data[0].lon);

        map.setView([lat, lng], 16);
        await setMarker(lat, lng, true);
    }

    function updatePropertyOptionsByDealType() {
        const dealTypeSelect = document.getElementById("dealTypeSelect");
        const propertyTypeSelect = document.getElementById("propertyTypeSelect");

        if (!dealTypeSelect || !propertyTypeSelect) {
            return;
        }

        const isDailyRent = dealTypeSelect.value === "Посуточная аренда";

        propertyTypeSelect.querySelectorAll("option").forEach(function (option) {
            const shouldHide = isDailyRent && (option.value === "Участок" || option.value === "Гараж");

            option.disabled = shouldHide;
            option.hidden = shouldHide;
            option.style.display = shouldHide ? "none" : "";
        });

        if (isDailyRent && (propertyTypeSelect.value === "Участок" || propertyTypeSelect.value === "Гараж")) {
            propertyTypeSelect.value = "";
        }
    }

    function updateDynamicFields() {
        const propertyTypeSelect = document.getElementById("propertyTypeSelect");
        const dealTypeSelect = document.getElementById("dealTypeSelect");

        if (!propertyTypeSelect) {
            return;
        }

        const propertyValue = propertyTypeSelect.value;
        const dealValue = dealTypeSelect ? dealTypeSelect.value : "";
        const isDailyRent = dealValue === "Посуточная аренда";

        setDisplay(document.getElementById("flatFields"), propertyValue === "Квартира" && !isDailyRent, "grid");
        setDisplay(
            document.getElementById("houseFields"),
            ["Дом", "Коттедж", "Таунхаус"].includes(propertyValue) && !isDailyRent,
            "grid"
        );
        setDisplay(document.getElementById("landFields"), propertyValue === "Участок" && !isDailyRent, "grid");
        setDisplay(document.getElementById("garageFields"), propertyValue === "Гараж" && !isDailyRent, "grid");
    }

    function updateDealFields() {
        const dealTypeSelect = document.getElementById("dealTypeSelect");

        if (!dealTypeSelect) {
            return;
        }

        setDisplay(document.getElementById("rentFields"), dealTypeSelect.value === "Долгосрочная аренда", "grid");
        setDisplay(document.getElementById("dailyRentFields"), dealTypeSelect.value === "Посуточная аренда", "grid");

        updatePropertyOptionsByDealType();
        updateDynamicFields();
    }

    function initMap() {
        const mapElement = document.getElementById("map");

        if (!mapElement || typeof L === "undefined") {
            return;
        }

        const initialLatitudeRaw = mapElement.dataset.initialLatitude || "";
        const initialLongitudeRaw = mapElement.dataset.initialLongitude || "";
        const hasInitialCoordinates = initialLatitudeRaw !== "" && initialLongitudeRaw !== "";
        const initialLatitude = parseNumber(initialLatitudeRaw, DEFAULT_LATITUDE);
        const initialLongitude = parseNumber(initialLongitudeRaw, DEFAULT_LONGITUDE);

        map = L.map("map", {
            minZoom: 2,
            maxBounds: WORLD_BOUNDS,
            maxBoundsViscosity: 1
        }).setView([initialLatitude, initialLongitude], hasInitialCoordinates ? 16 : 10);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: "",
            noWrap: true,
            bounds: WORLD_BOUNDS
        }).addTo(map);

        if (hasInitialCoordinates) {
            setMarker(initialLatitude, initialLongitude, false);
        }

        map.on("click", async function (event) {
            await setMarker(event.latlng.lat, event.latlng.lng, true);
        });

        setTimeout(function () {
            map.invalidateSize();
        }, 300);
    }

    document.addEventListener("DOMContentLoaded", function () {
        initMap();

        const addressSearchButton = document.querySelector("[data-action='find-address']");
        const dealTypeSelect = document.getElementById("dealTypeSelect");
        const propertyTypeSelect = document.getElementById("propertyTypeSelect");

        if (addressSearchButton) {
            addressSearchButton.addEventListener("click", findAddressOnMap);
        }

        if (window.HappyAddressPhone) {
            window.HappyAddressPhone.bind('input[name="PhoneNumber"]', false);
        }

        if (dealTypeSelect) {
            dealTypeSelect.addEventListener("change", updateDealFields);
        }

        if (propertyTypeSelect) {
            propertyTypeSelect.addEventListener("change", updateDynamicFields);
        }

        updateDealFields();
        updateDynamicFields();
    });
})();
