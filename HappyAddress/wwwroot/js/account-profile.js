(function () {
    "use strict";

    document.addEventListener("DOMContentLoaded", function () {
        if (window.HappyAddressPhone) {
            window.HappyAddressPhone.bind('input[name="PhoneNumber"]', true);
        }
    });
})();
