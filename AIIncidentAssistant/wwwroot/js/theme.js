(function () {
    const storageKey = "ai-incident-theme";

    function applyTheme(theme) {
        const selectedTheme = theme === "light" ? "light" : "dark";
        const isLight = selectedTheme === "light";

        document.documentElement.setAttribute(
            "data-theme",
            selectedTheme
        );

        const button = document.getElementById("themeToggle");
        const icon = document.getElementById("themeIcon");
        const label = document.getElementById("themeLabel");

        if (button) {
            button.setAttribute("aria-checked", String(isLight));
            button.setAttribute(
                "aria-label",
                isLight ? "Light Mode" : "Dark Mode"
            );
        }

        if (icon) {
            icon.textContent = isLight ? "☀" : "☾";
        }

        if (label) {
            label.textContent = isLight ? "Light Mode" : "Dark Mode";
        }
    }

    let savedTheme = "dark";

    try {
        savedTheme = localStorage.getItem(storageKey) || "dark";
    } catch {
        // Keep the default theme.
    }

    applyTheme(savedTheme);

    document.addEventListener("DOMContentLoaded", function () {
        const button = document.getElementById("themeToggle");

        if (!button) {
            return;
        }

        button.addEventListener("click", function () {
            const currentTheme =
                document.documentElement.getAttribute("data-theme");

            const nextTheme =
                currentTheme === "dark" ? "light" : "dark";

            applyTheme(nextTheme);

            try {
                localStorage.setItem(storageKey, nextTheme);
            } catch {
                // Theme still works for the current page.
            }
        });

        applyTheme(savedTheme);
    });
})();