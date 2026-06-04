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

        phoneInput.addEventListener("input", function (event) {
            const valueBeforeFormatting = phoneInput.value;
            const cursorBeforeFormatting = phoneInput.selectionStart ?? valueBeforeFormatting.length;
            const digitsBeforeCursor = valueBeforeFormatting
                .slice(0, cursorBeforeFormatting)
                .replace(/\D/g, "")
                .length;
            const digits = valueBeforeFormatting.replace(/\D/g, "");

            // Allow the user to remove the last remaining country-code digit.
            if (event.inputType?.startsWith("delete") && digits === "7") {
                phoneInput.value = "";
                return;
            }

            const formatted = formatPhoneNumber(valueBeforeFormatting);
            phoneInput.value = formatted;

            if (cursorBeforeFormatting >= valueBeforeFormatting.length) {
                phoneInput.setSelectionRange(formatted.length, formatted.length);
                return;
            }

            let digitsSeen = 0;
            let cursorAfterFormatting = formatted.length;

            for (let index = 0; index < formatted.length; index++) {
                if (/\d/.test(formatted[index])) {
                    digitsSeen++;
                }

                if (digitsSeen >= digitsBeforeCursor) {
                    cursorAfterFormatting = index + 1;
                    break;
                }
            }

            phoneInput.setSelectionRange(cursorAfterFormatting, cursorAfterFormatting);
        });
    }

    window.HappyAddressPhone = {
        bind: bindPhoneFormatter,
        format: formatPhoneNumber
    };
})();
