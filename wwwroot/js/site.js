// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getCollapseInstance(element) {
    return bootstrap.Collapse.getOrCreateInstance(element, { toggle: false });
}

// close search and products panels when clicking outside
document.addEventListener('click', function (event) {
    const searchBar = document.querySelector('.search-bar-collapse');
    const searchInput = document.querySelector('.search-input');
    const searchToggleButtons = Array.from(document.querySelectorAll('.search-icon-btn'));
    const productsBar = document.querySelector('.products-bar-collapse');
    const productsToggleButtons = Array.from(document.querySelectorAll('.product-menu-toggle'));

    const clickedInsideSearch = searchBar && searchBar.contains(event.target);
    const clickedSearchToggle = searchToggleButtons.some(function (button) {
        return button.contains(event.target);
    });
    const clickedInsideProducts = productsBar && productsBar.contains(event.target);
    const clickedProductsToggle = productsToggleButtons.some(function (button) {
        return button.contains(event.target);
    });

    if (searchBar && searchBar.classList.contains('show') && !clickedInsideSearch && !clickedSearchToggle) {
        getCollapseInstance(searchBar).hide();
        if (searchInput) {
            searchInput.value = ''; // Clear the search input when closing
        }
    }

    if (productsBar && productsBar.classList.contains('show') && !clickedInsideProducts && !clickedProductsToggle) {
        getCollapseInstance(productsBar).hide();
    }
});

// Optional: Close panels when pressing the Escape key
document.addEventListener('keydown', function (event) {
    const searchBar = document.querySelector('.search-bar-collapse');
    const searchInput = document.querySelector('.search-input');
    const productsBar = document.querySelector('.products-bar-collapse');

    if (event.key === 'Escape') {
        if (searchBar && searchBar.classList.contains('show')) {
            getCollapseInstance(searchBar).hide();
            if (searchInput) {
                searchInput.value = ''; // Clear the search input when closing
            }
        }

        if (productsBar && productsBar.classList.contains('show')) {
            getCollapseInstance(productsBar).hide();
        }
    }
});

// focus search input when opening search panel
document.addEventListener('DOMContentLoaded', function () {
    const searchCollapse = document.getElementById('searchCollapse');
    const searchInput = document.getElementById('searchInput');
    const productsCollapse = document.getElementById('productsCollapse');
    const navbarCollapse = document.getElementById('navbarNav');
    const productToggleButton = document.querySelector('.product-menu-toggle');
    const productTypeLinks = Array.from(document.querySelectorAll('[data-products-type]'));
    const productPanes = Array.from(document.querySelectorAll('[data-products-pane]'));
    const header = document.querySelector('.ford-style-header.dark-theme.ultra-thin');
    const hasHero = !!document.querySelector('.hero');

    const setProductToggleState = function (isOpen) {
        if (!productToggleButton) {
            return;
        }

        productToggleButton.classList.toggle('show', isOpen);
        productToggleButton.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
    };

    const setActiveProductType = function (typeId) {
        if (!productTypeLinks.length || !productPanes.length) {
            return;
        }

        productTypeLinks.forEach(function (link) {
            link.classList.toggle('is-active', link.dataset.productsType === typeId);
        });

        productPanes.forEach(function (pane) {
            pane.classList.toggle('is-active', pane.dataset.productsPane === typeId);
        });
    };

    // Set is-home class based on hero presence
    if (hasHero) {
        document.documentElement.classList.add('is-home');
    } else {
        document.documentElement.classList.remove('is-home');
    }

    const syncHeroTopState = function () {
        if (!header) {
            return;
        }

        const isDesktop = window.matchMedia('(min-width: 992px)').matches;
        const isNavbarOpen = !isDesktop && !!(navbarCollapse && (navbarCollapse.classList.contains('show') || navbarCollapse.classList.contains('collapsing')));
        const isSearchOpen = !!(searchCollapse && (searchCollapse.classList.contains('show') || searchCollapse.classList.contains('collapsing')));
        const isProductsOpen = !!(productsCollapse && (productsCollapse.classList.contains('show') || productsCollapse.classList.contains('collapsing')));
        const anyPanelOpen = isSearchOpen || isProductsOpen || isNavbarOpen;

        if (window.scrollY > 20) {
            header.classList.add('scrolled');
            if (searchCollapse) {
                searchCollapse.classList.add('scrolled');
            }
        } else {
            header.classList.remove('scrolled');
            if (searchCollapse) {
                searchCollapse.classList.remove('scrolled');
            }
        }

        const shouldBeTransparent = hasHero && window.scrollY <= 20 && !anyPanelOpen;

        header.classList.toggle('search-open', anyPanelOpen);
        header.classList.toggle('menu-open', isNavbarOpen);
        header.classList.toggle('hero-at-top', shouldBeTransparent);
    };

    if (searchCollapse && searchInput) {
        searchCollapse.addEventListener('show.bs.collapse', function () {
            if (header) {
                header.classList.add('search-open');
                header.classList.remove('hero-at-top');
            }

            if (productsCollapse && productsCollapse.classList.contains('show')) {
                getCollapseInstance(productsCollapse).hide();
            }
            searchInput.focus();
        });
        searchCollapse.addEventListener('shown.bs.collapse', function () {
            syncHeroTopState();
            searchInput.focus();
        });
        searchCollapse.addEventListener('hide.bs.collapse', function () {
            syncHeroTopState();
        });
        searchCollapse.addEventListener('hidden.bs.collapse', function () {
            syncHeroTopState();
        });
    }

    if (navbarCollapse) {
        navbarCollapse.addEventListener('show.bs.collapse', function () {
            if (header && !window.matchMedia('(min-width: 992px)').matches) {
                header.classList.add('menu-open');
                header.classList.add('search-open');
                header.classList.remove('hero-at-top');
            }
        });

        navbarCollapse.addEventListener('shown.bs.collapse', function () {
            syncHeroTopState();
        });

        navbarCollapse.addEventListener('hide.bs.collapse', function () {
            syncHeroTopState();
        });

        navbarCollapse.addEventListener('hidden.bs.collapse', function () {
            syncHeroTopState();
        });
    }

    if (productsCollapse) {
        productsCollapse.addEventListener('show.bs.collapse', function () {
            if (header) {
                header.classList.add('search-open');
                header.classList.remove('hero-at-top');
            }

            if (searchCollapse && searchCollapse.classList.contains('show')) {
                getCollapseInstance(searchCollapse).hide();
                if (searchInput) {
                    searchInput.value = '';
                }
            }

            setProductToggleState(true);
            setActiveProductType('all');
        });
        productsCollapse.addEventListener('hide.bs.collapse', function () {
            setProductToggleState(false);
            syncHeroTopState();
        });
        productsCollapse.addEventListener('shown.bs.collapse', function () {
            syncHeroTopState();
        });
        productsCollapse.addEventListener('hidden.bs.collapse', function () {
            syncHeroTopState();
        });
    }

    if (productTypeLinks.length && productPanes.length) {
        productTypeLinks.forEach(function (link) {
            const typeId = link.dataset.productsType;

            link.addEventListener('mouseenter', function () {
                setActiveProductType(typeId);
            });

            link.addEventListener('focus', function () {
                setActiveProductType(typeId);
            });
        });
    }

    syncHeroTopState();
    window.addEventListener('scroll', syncHeroTopState, { passive: true });
    window.addEventListener('resize', function () {
        const isDesktop = window.matchMedia('(min-width: 992px)').matches;
        if (isDesktop && navbarCollapse) {
            navbarCollapse.classList.remove('show', 'collapsing');
            navbarCollapse.style.height = '';

            const toggler = document.querySelector('.navbar-toggler');
            if (toggler) {
                toggler.setAttribute('aria-expanded', 'false');
            }
        }

        syncHeroTopState();
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const slider = document.querySelector('[data-hero-slider]');
    if (!slider) {
        return;
    }

    const slides = Array.from(slider.querySelectorAll('[data-hero-slide]'));
    const prevButton = slider.querySelector('[data-hero-prev]');
    const nextButton = slider.querySelector('[data-hero-next]');

    if (slides.length === 0 || !prevButton || !nextButton) {
        return;
    }

    let currentIndex = slides.findIndex(function (slide) {
        return slide.classList.contains('is-active');
    });

    if (currentIndex < 0) {
        currentIndex = 0;
        slides[0].classList.add('is-active');
    }

    const fallbackDurationMs = 7000;
    const minDurationMs = 4000;
    const maxDurationMs = 12000;
    let autoRotateTimer = null;

    const clearAutoRotateTimer = function () {
        if (autoRotateTimer) {
            clearTimeout(autoRotateTimer);
            autoRotateTimer = null;
        }
    };

    const scheduleNextSlide = function () {
        clearAutoRotateTimer();

        const activeSlide = slides[currentIndex];
        if (!activeSlide) {
            autoRotateTimer = setTimeout(function () {
                showSlide(currentIndex + 1);
            }, fallbackDurationMs);
            return;
        }

        const activeVideo = activeSlide.querySelector('video');
        if (!activeVideo) {
            autoRotateTimer = setTimeout(function () {
                showSlide(currentIndex + 1);
            }, fallbackDurationMs);
            return;
        }

        const startTimerWithDuration = function () {
            const durationMs = Number.isFinite(activeVideo.duration) && activeVideo.duration > 0
                ? Math.min(maxDurationMs, Math.max(minDurationMs, activeVideo.duration * 1000))
                : fallbackDurationMs;

            clearAutoRotateTimer();
            autoRotateTimer = setTimeout(function () {
                showSlide(currentIndex + 1);
            }, durationMs);
        };

        if (Number.isFinite(activeVideo.duration) && activeVideo.duration > 0) {
            startTimerWithDuration();
            return;
        }

        activeVideo.addEventListener('loadedmetadata', function onLoadedMetadata() {
            if (slides[currentIndex] !== activeSlide) {
                return;
            }

            startTimerWithDuration();
        }, { once: true });

        autoRotateTimer = setTimeout(function () {
            showSlide(currentIndex + 1);
        }, fallbackDurationMs);
    };

    const showSlide = function (index) {
        currentIndex = (index + slides.length) % slides.length;

        slides.forEach(function (slide, slideIndex) {
            const isActive = slideIndex === currentIndex;
            slide.classList.toggle('is-active', isActive);

            const video = slide.querySelector('video');
            if (!video) {
                return;
            }

            if (isActive) {
                const playPromise = video.play();
                if (playPromise && typeof playPromise.catch === 'function') {
                    playPromise.catch(function () { });
                }
            } else {
                video.pause();
            }
        });

        scheduleNextSlide();
    };

    prevButton.addEventListener('click', function () {
        showSlide(currentIndex - 1);
    });

    nextButton.addEventListener('click', function () {
        showSlide(currentIndex + 1);
    });

    showSlide(currentIndex);
});

// Initialize TinyMCE for all textareas with class 'rich-editor'
document.addEventListener('DOMContentLoaded', function () {
    if (typeof tinymce !== 'undefined') {
        tinymce.init({
            selector: 'textarea.rich-editor',
            height: 400,
            plugins: [
                'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
                'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
                'insertdatetime', 'media', 'table', 'help', 'wordcount'
            ],
            toolbar: 'undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media | removeformat | help',
            branding: false,
            language: 'vi',
            content_style: 'body { font-family:Segoe UI,Helvetica,Arial,sans-serif; font-size:14px }',
            relative_urls: false,
            remove_script_host: false,
            document_base_url: '/',
            menubar: 'file edit view insert format tools table help',
            setup: function(editor) {
                editor.on('init', function() {
                    console.log('TinyMCE editor initialized');
                });
            }
        });
    }
});
