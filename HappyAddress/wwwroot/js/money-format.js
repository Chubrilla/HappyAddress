(function () {
    "use strict";

    function getDigits(value) {
        return String(value || "").replace(/\D/g, "").substring(0, 15);
    }

    function formatMoney(value) {
        const digits = getDigits(value);

        if (!digits) {
            return "";
        }

        return digits.replace(/\B(?=(\d{3})+(?!\d))/g, " ");
    }

    function formatInitialMoney(value) {
        const normalized = String(value || "").trim();
        const integerPart = normalized.split(/[.,]/)[0];
        return formatMoney(integerPart);
    }

    function bindMoneyInputs() {
        const inputs = document.querySelectorAll("[data-money-input]");

        inputs.forEach(function (input) {
            input.value = formatInitialMoney(input.value);

            input.addEventListener("input", function () {
                input.value = formatMoney(input.value);
                input.setSelectionRange(input.value.length, input.value.length);
            });
        });

        document.querySelectorAll("form").forEach(function (form) {
            form.addEventListener("submit", function () {
                form.querySelectorAll("[data-money-input]").forEach(function (input) {
                    input.value = getDigits(input.value);
                });

                setTimeout(function () {
                    form.querySelectorAll("[data-money-input]").forEach(function (input) {
                        input.value = formatMoney(input.value);
                    });
                }, 0);
            }, true);
        });
    }

    document.addEventListener("DOMContentLoaded", bindMoneyInputs);
})();
