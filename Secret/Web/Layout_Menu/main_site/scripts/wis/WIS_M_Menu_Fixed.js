var WIS_M_Menu_Fixed;
(function (WIS_M_Menu_Fixed) {
    var $Elem = $('.wis-js-menu-fixed');
    var previousScroll = 0;
    if ($Elem !== undefined && $Elem.length > 0) {
        var Scroll_1 = $(window).scrollTop();
        var ElemTop_1 = $Elem.offset().top - 80;
        if ($(window).width() < 768) {
            if (Scroll_1 > ElemTop_1) {
                $Elem.addClass('wis-jscss-menu-fixed');
            }
            else {
                $Elem.removeClass('wis-jscss-menu-fixed');
            }
            $(window).scroll(function () {
                Scroll_1 = $(window).scrollTop();
                var currentScroll = $(this).scrollTop();
                if (Scroll_1 > ElemTop_1) {
                    $Elem.addClass('wis-jscss-menu-fixed');
                    if (currentScroll > previousScroll) {
                        $('.wis-js-search').slideUp();
                    }
                    else {
                        $('.wis-js-search').slideDown();
                    }
                }
                else {
                    $Elem.removeClass('wis-jscss-menu-fixed');
                }
                previousScroll = currentScroll;
            });
        }
        $(document).ready(function () {
            if ($(window).width() < 768) {
                $Elem.animate({ scrollLeft: $('.wis-menu-scroll-block-elem_active').position().left - 100 }, 500);
            }
            else {
                $Elem.animate({ scrollLeft: $('.wis-menu-scroll-block-elem_active').position().left - 500 }, 500);
            }
        });
        var scrollItems = $('.wis-js-anchor');
        var $Menu_Items_1 = $('.wis-js-menu-scroll-elem');
        $(window).scroll(function () {
            var scrollPos = $(this).scrollTop();
            $Menu_Items_1.each(function () {
                var currLink = $(this);
                var refElement = $(currLink.children().attr("href"));
                if (refElement !== undefined) {
                    if (refElement.position().top <= scrollPos) {
                        $Menu_Items_1.removeClass("wis-menu-scroll-block-elem_active");
                        currLink.addClass("wis-menu-scroll-block-elem_active");
                        if ($Elem.scrollLeft() !== $('.wis-menu-scroll-block-elem_active').position().left) {
                            $Elem.scrollLeft($('.wis-menu-scroll-block-elem_active').position().left - 100);
                        }
                    }
                    else {
                    }
                }
            });
        });
    }
})(WIS_M_Menu_Fixed || (WIS_M_Menu_Fixed = {}));
//# sourceMappingURL=WIS_M_Menu_Fixed.js.map