// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function setThemeMode(isDark) {
//    const body = document.body;
//    const checkboxes = document.querySelectorAll('.dark-mode-checkbox');

//    if (isDark) {
//        body.classList.add('dark-mode');
//        body.classList.remove('light-mode');
//        localStorage.setItem('theme', 'dark');
//    } else {
//        body.classList.remove('dark-mode');
//        body.classList.add('light-mode');
//        localStorage.setItem('theme', 'light');
//    }

//    checkboxes.forEach(cb => cb.checked = isDark);
//}

//document.addEventListener('DOMContentLoaded', () => {
//    const storedTheme = localStorage.getItem('theme');
//    const isDark = storedTheme !== 'light'; // default to dark

//    setThemeMode(isDark);

//    const checkboxes = document.querySelectorAll('.dark-mode-checkbox');
//    checkboxes.forEach(cb => {
//        cb.addEventListener('change', (e) => {
//            setThemeMode(e.target.checked);
//        });
//    });
//});



    window.addEventListener('load', function () {
    const preloader = document.getElementById('preloader');
    preloader.style.opacity = '0';
    setTimeout(() => {
        preloader.style.display = 'none';
    }, 500); // adjust delay if needed
});

document.addEventListener("DOMContentLoaded", function () {
    const quotes = window.quoteData;
    let index = 0;

    function showNextQuote() {
        if (!quotes || quotes.length === 0) return;

        const quote = quotes[index];
        const textEl = document.getElementById("quoteText");
        const authorEl = document.getElementById("quoteAuthor");

        if (textEl && authorEl) {
            textEl.textContent = quote.quoteText;
            authorEl.textContent = quote.author;
        }

        index = (index + 1) % quotes.length;
    }

    showNextQuote();
    setInterval(showNextQuote, 7000);
});
