/* ------------------------------------------------------------------------------
 *
 *  # Internationalization - query strings
 *
 *  Demo JS code for internationalization_switch_query.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

const I18nextQuery = function() {


    //
    // Setup module components
    //

    // Change language without page reload
    const _componentI18nextQuery = function() {
        if (typeof i18next == 'undefined') {
            console.warn('Warning - i18next.min.js is not loaded.');
            return;
        }


        // Configuration
        // -------------------------

        // Define main elements
        const elements = document.querySelectorAll('.language-switch .dropdown-item'),
            selector = document.querySelectorAll('[data-i18n]'),
            englishLangClass = 'en',
            ukrainianLangClass = 'ua';

        // Add options
        i18next.use(i18nextHttpBackend).use(i18nextBrowserLanguageDetector).init({
            backend: {
                loadPath: 'demo/locales/{{lng}}.json'
            },
            debug: true,
            fallbackLng: 'en'
        },
        function (err, t) {
            selector.forEach(function(item) {
                item.innerHTML = i18next.t(item.getAttribute("data-i18n"));
            });
        });


        // Change languages in dropdown
        // -------------------------

        // Do stuff when i18Next is initialized
        i18next.on('initialized', function() {

            // English
            if(i18next.language == "en") {
                document.querySelector('.' + englishLangClass).classList.add('active');
                document.querySelector('.language-switch .navbar-nav-link').innerHTML = document.querySelector('.' + englishLangClass).innerHTML;
            }

            // Ukrainian
            if(i18next.language == "ua") {
                document.querySelector('.' + ukrainianLangClass).classList.add('active');
                document.querySelector('.language-switch .navbar-nav-link').innerHTML = document.querySelector('.' + ukrainianLangClass).innerHTML;
            }

            // Add responsive classes if toggler is not hidden on mobile
            document.querySelector('.language-switch .navbar-nav-link span').classList.add('d-none', 'd-lg-inline-block', 'me-1');
        });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _componentI18nextQuery();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    I18nextQuery.init();
});
