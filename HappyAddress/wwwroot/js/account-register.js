(function () {
    "use strict";

    document.addEventListener("DOMContentLoaded", function () {
        if (window.HappyAddressPhone) {
            window.HappyAddressPhone.bind('input[name="PhoneNumber"]', false);
        }

        const displayInput = document.getElementById("BirthDateDisplay");
        const hiddenInput = document.getElementById("BirthDate");

        if (!displayInput || !hiddenInput) {
            return;
        }

        function formatDate(value) {
            const digits = String(value || "").replace(/\D/g, "").substring(0, 8);

            if (digits.length <= 2) {
                return digits;
            }

            if (digits.length <= 4) {
                return digits.substring(0, 2) + "." + digits.substring(2);
            }

            return digits.substring(0, 2) + "." + digits.substring(2, 4) + "." + digits.substring(4);
        }

        function syncHiddenDate() {
            const match = displayInput.value.match(/^(\d{2})\.(\d{2})\.(\d{4})$/);
            hiddenInput.value = "";

            if (!match) {
                return false;
            }

            const day = Number(match[1]);
            const month = Number(match[2]);
            const year = Number(match[3]);
            const date = new Date(Date.UTC(year, month - 1, day));
            const isValid = date.getUTCFullYear() === year
                && date.getUTCMonth() === month - 1
                && date.getUTCDate() === day;

            if (isValid) {
                hiddenInput.value = `${match[3]}-${match[2]}-${match[1]}`;
            }

            return isValid;
        }

        displayInput.addEventListener("input", function () {
            displayInput.value = formatDate(displayInput.value);
            displayInput.setSelectionRange(displayInput.value.length, displayInput.value.length);
            displayInput.setCustomValidity("");
            syncHiddenDate();
        });

        displayInput.addEventListener("blur", function () {
            if (!displayInput.value) {
                displayInput.setCustomValidity("Введите дату рождения");
            } else if (!syncHiddenDate()) {
                displayInput.setCustomValidity("Введите корректную дату в формате ДД.ММ.ГГГГ");
            }
        });

        displayInput.form?.addEventListener("submit", function (event) {
            if (!displayInput.value || !syncHiddenDate()) {
                event.preventDefault();
                displayInput.setCustomValidity(
                    displayInput.value
                        ? "Введите корректную дату в формате ДД.ММ.ГГГГ"
                        : "Введите дату рождения"
                );
                displayInput.reportValidity();
            }
        });
    });
})();
