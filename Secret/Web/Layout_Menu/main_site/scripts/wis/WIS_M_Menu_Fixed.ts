/**
 * Fixed меню в мобильной версии
 */
module WIS_M_Menu_Fixed {

    let $Elem: JQuery = $('.wis-js-menu-fixed');

    let previousScroll = 0;

    if ($Elem !== undefined && $Elem.length > 0) {

        let Scroll = $(window).scrollTop();
        let ElemTop = $Elem.offset().top - 80;

        if ($(window).width() < 768) {

            if (Scroll > ElemTop) {
                $Elem.addClass('wis-jscss-menu-fixed');
            }
            else {
                $Elem.removeClass('wis-jscss-menu-fixed');
            }

            $(window).scroll(function () {
                Scroll = $(window).scrollTop();

                let currentScroll = $(this).scrollTop();

                if (Scroll > ElemTop) {
                    $Elem.addClass('wis-jscss-menu-fixed');

                    if (currentScroll > previousScroll) {

                        $('.wis-js-search').slideUp()

                    }

                    else {
                        $('.wis-js-search').slideDown()
                    }
                }
                else {
                    $Elem.removeClass('wis-jscss-menu-fixed');
                }

               

                previousScroll = currentScroll;


            })
        }

        //Скролл до активного пункта меню

        $(document).ready(function () {
 


            if ($(window).width() < 768) {
                $Elem.animate({ scrollLeft: $('.wis-menu-scroll-block-elem_active').position().left - 100 }, 500);
            }
            else {

                $Elem.animate({ scrollLeft: $('.wis-menu-scroll-block-elem_active').position().left - 500 }, 500);
            }
     
        })

        let scrollItems = $('.wis-js-anchor');

        let $Menu_Items = $('.wis-js-menu-scroll-elem');

        $(window).scroll(function () {

            var scrollPos = $(this).scrollTop();

            $Menu_Items.each(function () {

                var currLink = $(this);

                var refElement = $(currLink.children().attr("href"));


                if (refElement !== undefined) {

                    if (refElement.position().top <= scrollPos) {

                        $Menu_Items.removeClass("wis-menu-scroll-block-elem_active");

                        currLink.addClass("wis-menu-scroll-block-elem_active");

                        if ($Elem.scrollLeft() !== $('.wis-menu-scroll-block-elem_active').position().left ) {
                            $Elem.scrollLeft($('.wis-menu-scroll-block-elem_active').position().left - 100)

                        }

                    }

                    else {
                      
                    }
                                        
                }

            });




        });​

    }


  

}

