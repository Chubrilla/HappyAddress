(function () {
    "use strict";

    function formatPhoneNumber(value) {
        let digits = String(value || "").replace(/\D/g, "");

        if (digits.startsWith("8")) {
            digits = "7" + digits.slice(1);
        }

        if (digits.length === 10) {
            digits = "7" + digits;
        }

        if (!digits.startsWith("7") && digits.length > 0) {
            digits = "7" + digits;
        }

        digits = digits.substring(0, 11);

        if (digits.length === 0) {
            return "";
        }

        let formatted = "+7";

        if (digits.length > 1) {
            formatted += " (" + digits.substring(1, 4);
        }

        if (digits.length >= 4) {
            formatted += ") " + digits.substring(4, 7);
        }

        if (digits.length >= 7) {
            formatted += "-" + digits.substring(7, 9);
        }

        if (digits.length >= 9) {
            formatted += "-" + digits.substring(9, 11);
        }

        return formatted;
    }

    function bindPhoneFormatter(selector, formatInitialValue) {
        const phoneInput = document.querySelector(selector || 'input[name="PhoneNumber"]');

        if (!phoneInput) {
            return;
        }

        if (formatInitialValue) {
            phoneInput.value = formatPhoneNumber(phoneInput.value);
        }

        phoneInput.addEventListener("input", function () {
            phoneInput.value = formatPhoneNumber(phoneInput.value);
        });
    }

    window.HappyAddressPhone = {
        bind: bindPhoneFormatter,
        format: formatPhoneNumber
    };
})();
