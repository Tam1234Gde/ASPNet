

//#region јнимаци€ кофейных стаканчиков

/**
 * ¬ыбираем стаканчики, статусы и пар.
 */
const coffeeGreen = document.querySelector(".wis-mood-carousel__green"),
    coffeeBlack = document.querySelector(".wis-mood-carousel__black"),
    coffeeViolet = document.querySelector(".wis-mood-carousel__violet"),
    statusGreen = document.querySelector(".wis-mood-carousel__green_status"),
    statusBlack = document.querySelector(".wis-mood-carousel__black_status"),
    statusViolet = document.querySelector(".wis-mood-carousel__violet_status"),
    vaporGreen = document.querySelector(".wis-mood-carousel__green_vapor"),
    vaporBlack = document.querySelector(".wis-mood-carousel__black_vapor"),
    vaporViolet = document.querySelector(".wis-mood-carousel__violet_vapor");

if (coffeeGreen) {
    coffeeGreen.onmouseover = () => {
        coffeeGreen.style.transform = 'scale(1.2) rotate(-2deg)';
        statusGreen.style.transform = 'scale(1.3) rotate(8deg)';
        vaporGreen.style.transform = 'translateY(-140px)';
        coffeeBlack.style.transform = 'scale(0.8) rotate(8deg)';
        vaporBlack.style.opacity = '0';
    }

    coffeeGreen.onmouseleave = () => {
        coffeeGreen.style.transform = 'scale(1) rotate(-8deg)';
        statusGreen.style.transform = 'scale(1) rotate(8deg)';
        vaporGreen.style.transform = 'translateY(0px)';
        coffeeBlack.style.transform = 'scale(1) rotate(0deg)';
        vaporBlack.style.opacity = '1';
    }
}

if (coffeeBlack) {
    coffeeBlack.onmouseover = () => {
        coffeeBlack.style.transform = 'scale(1.1) ';
        statusBlack.style.transform = 'scale(1.2)';
        vaporBlack.style.transform = 'translateY(-30px)';
    }

    coffeeBlack.onmouseleave = () => {
        coffeeBlack.style.transform = 'scale(1)';
        statusBlack.style.transform = 'scale(1)';
        vaporBlack.style.transform = 'translateY(0px)';
    }
}

if (coffeeViolet) {
    coffeeViolet.onmouseover = () => {
        coffeeViolet.style.transform = 'scale(1.2)';
        statusViolet.style.transform = 'scale(1.3) rotate(-8deg)';
        vaporViolet.style.transform = 'translateY(-120px)';
        coffeeBlack.style.transform = 'scale(0.8) rotate(-8deg)';
        vaporBlack.style.opacity = '0';
    }

    coffeeViolet.onmouseleave = () => {
        coffeeViolet.style.transform = 'scale(1) rotate(8deg)';
        statusViolet.style.transform = 'scale(1) rotate(-8deg)';
        vaporViolet.style.transform = 'translateY(0px)';
        coffeeBlack.style.transform = 'scale(1)';
        vaporBlack.style.opacity = '1';
    }
}


    
//#endregion


//#region ”становка скролла стаканчиков по средине


const carouselContainer = document.querySelector(".wis-mood-container");

const mediaQuery = window.matchMedia('(max-width: 767.98px)');

    function handleTabletChange(e) {
        if (carouselContainer != undefined) {
            if (e.matches) {
                carouselContainer.scrollTo({ left: Math.round(380 - carouselContainer.offsetWidth / 2) });
                addEventListener('resize', () => {
                    carouselContainer.scrollTo({ left: Math.round(380 - carouselContainer.offsetWidth / 2) });
                })
            }
        }
    }


mediaQuery.addListener(handleTabletChange);
handleTabletChange(mediaQuery);


//#endregion


//#region ѕлавна€ прокрутка до €кор€

const anchors = document.querySelectorAll('.wis-js-menu-scroll-elem a[href*="#"]');

if (anchors.length > 0) {
    for (let anchor of anchors) {
        anchor.addEventListener("click", event => {
            event.preventDefault();
            const blockId = anchor.getAttribute("href");
            document.querySelector("" + blockId).scrollIntoView({
                behavior: "smooth",
                block: "start",
            });
        });
    }
}


//#endregion


//#region «акрытие мобильного меню при клике на ссылки

const mobileMenuAnchors = document.querySelectorAll(".wis-menu_mobile a");

if (mobileMenuAnchors.length > 0) {
    mobileMenuAnchors.forEach(elem => {
        elem.onclick = closeMobileMenu
    });
}


function closeMobileMenu() {
    const mobileBackground = document.querySelector(".wis-js-menu_mobile-bg"),
        mobileMenu = document.querySelector(".wis-js-menu_mobile-box");

        mobileBackground.classList.remove("wis-jscss-menu_mobile-bg__active");
        mobileMenu.classList.remove("wis-jscss-menu_mobile-box__active");
}
    
//#endregion


//#region ќпредел€ем мобильное устройство и убираем горизонтальный скролл страницы
// при по€влении скролла у контейнера со стаканчиками
 
const moodContainer = document.querySelector(".wis-mood-container");

if (moodContainer != undefined) {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|BB|PlayBook|IEMobile|Windows Phone|Kindle|Silk|Opera Mini/i.test(navigator.userAgent)) {
        moodContainer.style.width = "100vw";
    } else moodContainer.style.width = "calc(100vw - 12px)";
}

//#endregion