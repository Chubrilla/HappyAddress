(function () {
    "use strict";

    let homeMap = null;
    let homeClusterGroup = null;
    const WORLD_BOUNDS = L.latLngBounds([[-85, -180], [85, 180]]);

    function parseMapAds() {
        const mapBox = document.getElementById("homeMapBox");

        if (!mapBox || !mapBox.dataset.mapAds) {
            return [];
        }

        try {
            return JSON.parse(mapBox.dataset.mapAds);
        } catch (error) {
            return [];
        }
    }

    function escapeHtml(value) {
        return String(value || "")
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    function openFilters() {
        const filtersModal = document.getElementById("filtersModal");

        if (!filtersModal) {
            return;
        }

        filtersModal.style.display = "flex";
        document.body.classList.add("modal-open");
    }

    function closeFilters() {
        const filtersModal = document.getElementById("filtersModal");

        if (!filtersModal) {
            return;
        }

        filtersModal.style.display = "none";
        document.body.classList.remove("modal-open");
    }

    function toggleFilters() {
        const filtersModal = document.getElementById("filtersModal");

        if (!filtersModal) {
            return;
        }

        if (filtersModal.style.display === "flex") {
            closeFilters();
        } else {
            openFilters();
        }
    }

    function clearClusterAdsList() {
        const listBox = document.getElementById("clusterAdsList");
        const itemsBox = document.getElementById("clusterAdsItems");

        if (listBox) {
            listBox.style.display = "none";
        }

        if (itemsBox) {
            itemsBox.innerHTML = "";
        }
    }

    function showClusterAdsList(ads) {
        const listBox = document.getElementById("clusterAdsList");
        const itemsBox = document.getElementById("clusterAdsItems");

        if (!listBox || !itemsBox) {
            return;
        }

        if (!ads || ads.length === 0) {
            clearClusterAdsList();
            return;
        }

        listBox.style.display = "block";

        itemsBox.innerHTML = ads.map(function (ad) {
            const price = ad.price
                ? Number(ad.price).toLocaleString("ru-RU") + " ₽"
                : "Цена не указана";

            const title = escapeHtml(ad.title || "Объявление");
            const city = escapeHtml(ad.city || "");
            const address = escapeHtml(ad.address || "");
            const imagePath = escapeHtml(ad.imagePath || "");
            const imageMarkup = imagePath
                ? `<img src="${imagePath}" alt="Фото объявления" class="cluster-ad-image" />`
                : `<div class="cluster-ad-image cluster-ad-image-empty">Нет фото</div>`;

            return `
                <div class="cluster-ad-card">
                    ${imageMarkup}

                    <div class="cluster-ad-content">
                        <h5>${title}</h5>
                        <p class="cluster-ad-price">${price}</p>
                        <p class="cluster-ad-address">${city}${address ? ", " + address : ""}</p>
                    </div>

                    <a href="/Ads/Details/${encodeURIComponent(ad.id)}" class="cluster-ad-link">
                        Подробнее
                    </a>
                </div>
            `;
        }).join("");
    }

    function initAdsMap() {
        const mapElement = document.getElementById("adsMap");
        const mapAds = parseMapAds();

        if (!mapElement || typeof L === "undefined") {
            return;
        }

        if (!homeMap) {
            homeMap = L.map("adsMap", {
                minZoom: 2,
                maxBounds: WORLD_BOUNDS,
                maxBoundsViscosity: 1
            }).setView([55.751244, 37.618423], 10);

            L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
                attribution: "",
                noWrap: true,
                bounds: WORLD_BOUNDS
            }).addTo(homeMap);
        }

        homeMap.invalidateSize();

        if (homeClusterGroup) {
            homeMap.removeLayer(homeClusterGroup);
        }

        homeClusterGroup = L.markerClusterGroup({
            showCoverageOnHover: false,
            spiderfyOnMaxZoom: true,
            zoomToBoundsOnClick: false
        });

        if (!mapAds.length) {
            homeMap.setView([55.751244, 37.618423], 10);
            clearClusterAdsList();
            return;
        }

        const bounds = [];

        mapAds.forEach(function (ad) {
            const lat = Number(String(ad.latitude).replace(",", "."));
            const lng = Number(String(ad.longitude).replace(",", "."));

            if (Number.isNaN(lat) || Number.isNaN(lng)) {
                return;
            }

            const marker = L.marker([lat, lng]);
            const price = ad.price ? Number(ad.price).toLocaleString("ru-RU") : "Цена не указана";
            const title = escapeHtml(ad.title || "Объявление");
            const city = escapeHtml(ad.city || "");
            const address = escapeHtml(ad.address || "");

            marker.adData = ad;
            marker.bindPopup(`
                <div class="map-popup">
                    <strong>${title}</strong>
                    <br />
                    <span>${price} ₽</span>
                    <br />
                    <small>${city}${address ? ", " + address : ""}</small>
                    <br />
                    <a href="/Ads/Details/${encodeURIComponent(ad.id)}">Подробнее</a>
                </div>
            `);

            marker.on("click", function () {
                showClusterAdsList([ad]);
            });

            homeClusterGroup.addLayer(marker);
            bounds.push([lat, lng]);
        });

        homeClusterGroup.on("clusterclick", function (event) {
            const ads = event.layer.getAllChildMarkers()
                .map(function (marker) {
                    return marker.adData;
                })
                .filter(Boolean);

            showClusterAdsList(ads);
            homeMap.fitBounds(event.layer.getBounds(), { padding: [40, 40] });
        });

        homeMap.addLayer(homeClusterGroup);

        if (bounds.length > 0) {
            homeMap.fitBounds(bounds, { padding: [40, 40] });
        }
    }

    function toggleMap() {
        const mapBox = document.getElementById("homeMapBox");
        const adsSection = document.querySelector(".ads-section");

        if (!mapBox) {
            return;
        }

        if (mapBox.style.display === "none" || mapBox.style.display === "") {
            mapBox.style.display = "block";

            if (adsSection) {
                adsSection.style.display = "none";
            }

            setTimeout(initAdsMap, 200);
        } else {
            mapBox.style.display = "none";

            if (adsSection) {
                adsSection.style.display = "block";
            }

            clearClusterAdsList();
        }
    }

    function updatePropertyOptionsByDealType() {
        const propertyTypeFilter = document.getElementById("propertyType");
        const dealTypeFilter = document.getElementById("dealType");

        if (!dealTypeFilter || !propertyTypeFilter) {
            return;
        }

        const isDailyRent = dealTypeFilter.value === "Посуточная аренда";

        propertyTypeFilter.querySelectorAll("option").forEach(function (option) {
            const shouldHide = isDailyRent && (option.value === "Участок" || option.value === "Гараж");

            option.disabled = shouldHide;
            option.hidden = shouldHide;
            option.style.display = shouldHide ? "none" : "";
        });

        if (isDailyRent && (propertyTypeFilter.value === "Участок" || propertyTypeFilter.value === "Гараж")) {
            propertyTypeFilter.value = "";
        }
    }

    function updateFilterFields() {
        const propertyTypeFilter = document.getElementById("propertyType");
        const dealTypeFilter = document.getElementById("dealType");

        if (!propertyTypeFilter) {
            return;
        }

        const propertyValue = propertyTypeFilter.value;
        const dealValue = dealTypeFilter ? dealTypeFilter.value : "";
        const isDailyRent = dealValue === "Посуточная аренда";
        const isLongRent = dealValue === "Долгосрочная аренда";

        setField("houseFilterFields", ["Дом", "Коттедж", "Таунхаус"].includes(propertyValue) && !isDailyRent);
        setField("flatFilterFields", propertyValue === "Квартира" && !isDailyRent);
        setField("landFilterFields", propertyValue === "Участок" && !isDailyRent);
        setField("garageFilterFields", propertyValue === "Гараж" && !isDailyRent);
        setField("dailyRentFilterFields", isDailyRent);
        setField("longRentFilterFields", isLongRent);
    }

    function setField(id, isVisible) {
        const element = document.getElementById(id);

        if (element) {
            element.style.display = isVisible ? "block" : "none";
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        const searchInput = document.querySelector('input[name="searchText"]');
        const searchSubmitButton = document.querySelector("[data-action='submit-text-search']");
        const filtersButtons = document.querySelectorAll("[data-action='toggle-filters']");
        const mapButtons = document.querySelectorAll("[data-action='toggle-map']");
        const propertyTypeFilter = document.getElementById("propertyType");
        const dealTypeFilter = document.getElementById("dealType");

        filtersButtons.forEach(function (button) {
            button.addEventListener("click", toggleFilters);
        });

        mapButtons.forEach(function (button) {
            button.addEventListener("click", toggleMap);
        });

        if (searchSubmitButton) {
            searchSubmitButton.addEventListener("click", function () {
                const searchMode = document.getElementById("searchMode");

                if (searchMode) {
                    searchMode.value = "text";
                }
            });
        }

        if (searchInput) {
            searchInput.addEventListener("keydown", function (event) {
                if (event.key === "Enter") {
                    const searchMode = document.getElementById("searchMode");

                    if (searchMode) {
                        searchMode.value = "text";
                    }
                }
            });
        }

        if (propertyTypeFilter) {
            propertyTypeFilter.addEventListener("change", function () {
                updatePropertyOptionsByDealType();
                updateFilterFields();
            });
        }

        if (dealTypeFilter) {
            dealTypeFilter.addEventListener("change", function () {
                updatePropertyOptionsByDealType();
                updateFilterFields();
            });
        }

        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                closeFilters();
            }
        });

        document.addEventListener("click", function (event) {
            const filtersModal = document.getElementById("filtersModal");

            if (filtersModal && event.target === filtersModal) {
                closeFilters();
            }
        });

        updatePropertyOptionsByDealType();
        updateFilterFields();
    });
})();
