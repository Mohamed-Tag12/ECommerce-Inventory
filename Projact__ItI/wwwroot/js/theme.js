(function () {
    function apply(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        try { localStorage.setItem('theme', theme); } catch (e) { }
        document.dispatchEvent(new CustomEvent('themechange', { detail: theme }));
    }
    window.toggleTheme = function () {
        var current = document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'dark' : 'light';
        apply(current === 'dark' ? 'light' : 'dark');
    };
})();
