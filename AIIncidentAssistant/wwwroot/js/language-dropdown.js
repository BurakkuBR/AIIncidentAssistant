
document.addEventListener("DOMContentLoaded", function () {
    const dropdown = document.getElementById("languageDropdown");
    const trigger = document.getElementById("languageTrigger");
    const menu = document.getElementById("languageMenu");
    const form = document.getElementById("languageForm");
    const cultureInput = document.getElementById("selectedCulture");

    if (!dropdown || !trigger || !menu || !form || !cultureInput) {
        return;
    }

    const options = Array.from(
        menu.querySelectorAll(".language-option")
    );

    function isOpen() {
        return dropdown.classList.contains("is-open");
    }

    function getSelectedIndex() {
        const index = options.findIndex(
            option => option.dataset.culture === cultureInput.value
        );

        return index >= 0 ? index : 0;
    }

    function focusOption(index) {
        if (options.length === 0) {
            return;
        }

        const normalizedIndex =
            (index + options.length) % options.length;

        options[normalizedIndex].focus();
    }

    function setOpen(open, focusIndex = null) {
        dropdown.classList.toggle("is-open", open);
        trigger.setAttribute("aria-expanded", String(open));

        // Prevent keyboard and assistive-technology interaction
        // while the animated menu is closed.
        menu.inert = !open;

        if (open && focusIndex !== null) {
            focusOption(focusIndex);
        }
    }

    function closeMenu(restoreFocus = false) {
        setOpen(false);

        if (restoreFocus) {
            trigger.focus();
        }
    }

    function selectLanguage(option) {
        const culture = option.dataset.culture;

        if (!culture) {
            return;
        }

        if (culture === cultureInput.value) {
            closeMenu(true);
            return;
        }

        cultureInput.value = culture;
        closeMenu();

        // Submit the existing ASP.NET Core form.
        form.requestSubmit();
    }

    // Mark the currently selected language.
    options.forEach(function (option) {
        const selected =
            option.dataset.culture === cultureInput.value;

        option.classList.toggle("is-selected", selected);
        option.setAttribute("aria-pressed", String(selected));

        option.addEventListener("click", function () {
            selectLanguage(option);
        });
    });

    // Mouse, Enter and Space are supported natively
    // by this button.
    trigger.addEventListener("click", function () {
        if (isOpen()) {
            closeMenu();
        } else {
            setOpen(true, getSelectedIndex());
        }
    });

    // Arrow keys can open the menu directly.
    trigger.addEventListener("keydown", function (event) {
        if (event.key === "ArrowDown") {
            event.preventDefault();
            setOpen(true, getSelectedIndex());
        }

        if (event.key === "ArrowUp") {
            event.preventDefault();
            setOpen(true, options.length - 1);
        }
    });

    // Keyboard navigation between languages.
    menu.addEventListener("keydown", function (event) {
        const currentIndex = options.indexOf(
            document.activeElement
        );

        switch (event.key) {
            case "ArrowDown":
                event.preventDefault();
                focusOption(currentIndex + 1);
                break;

            case "ArrowUp":
                event.preventDefault();
                focusOption(currentIndex - 1);
                break;

            case "Home":
                event.preventDefault();
                focusOption(0);
                break;

            case "End":
                event.preventDefault();
                focusOption(options.length - 1);
                break;

            // Enter and Space already activate native buttons.
        }
    });

    // Close when clicking outside.
    document.addEventListener("pointerdown", function (event) {
        if (!dropdown.contains(event.target)) {
            closeMenu();
        }
    });

    // Close when focus leaves the dropdown via Tab.
    dropdown.addEventListener("focusout", function () {
        requestAnimationFrame(function () {
            if (!dropdown.contains(document.activeElement)) {
                closeMenu();
            }
        });
    });

    // Escape works from both the trigger and the options.
    dropdown.addEventListener("keydown", function (event) {
        if (event.key === "Escape" && isOpen()) {
            event.preventDefault();
            closeMenu(true);
        }
    });

    // Ensure the menu starts closed and inaccessible.
    setOpen(false);
});
