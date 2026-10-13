document.addEventListener("DOMContentLoaded", function () {

    const form = document.querySelector("form[method='post']");
    const button = document.getElementById("analyzeButton");
    const buttonText = document.getElementById("buttonText");
    const loadingIndicator = document.getElementById("loadingIndicator");

    if (!form || !button || !buttonText || !loadingIndicator) {
        return;
    }

    form.addEventListener("submit", function (event) {

        if (typeof $ !== "undefined" &&
            $.validator &&
            !$(form).valid()) {
            return;
        }

        if (button.disabled) {
            event.preventDefault();
            return;
        }

        buttonText.classList.add("d-none");
        loadingIndicator.classList.remove("d-none");

        button.disabled = true;
    });

});