(function () {
    "use strict";

    function getNationalDigits(value) {
        let digits = String(value || "").replace(/\D/g, "");

        // Ignore the country code already rendered by this formatter.
        if (digits.startsWith("7")) {
            digits = digits.slice(1);
        } else if (digits.startsWith("8") && digits.length === 11) {
            digits = digits.slice(1);
        }

        return digits.substring(0, 10);
    }

    function formatPhoneNumber(value) {
        const digits = getNationalDigits(value);

        if (!digits) {
            return "";
        }

        let formatted = "+7";

        if (digits.length > 0) {
            formatted += " (" + digits.substring(0, 3);
        }

        if (digits.length > 3) {
            formatted += ") " + digits.substring(3, 6);
        }

        if (digits.length > 6) {
            formatted += "-" + digits.substring(6, 8);
        }

        if (digits.length > 8) {
            formatted += "-" + digits.substring(8, 10);
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
            const formatted = formatPhoneNumber(phoneInput.value);
            phoneInput.value = formatted;
            phoneInput.setSelectionRange(formatted.length, formatted.length);
        });
    }

    window.HappyAddressPhone = {
        bind: bindPhoneFormatter,
        format: formatPhoneNumber
    };
})();
