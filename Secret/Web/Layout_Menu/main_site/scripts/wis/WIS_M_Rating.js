var WIS_M_Rating;
(function (WIS_M_Rating) {
    var $Elem = $('.wis-js-rating');
    if ($Elem != undefined && $Elem.length > 0) {
        var $Input_Full_1 = $Elem.find('.wis-js-rating-input_full');
        var $Input_Elem = $Elem.find('.wis-js-rating-input_elem');
        var $Text_1 = $Elem.find('.wis-js-rating-text');
        $Input_Elem.click(function () {
            var title = $(this).attr('title');
            var val = $(this).val();
            $Text_1.text(title);
            $Text_1.attr('data-rating', val);
            $Input_Full_1.val(val);
            if (Number(val) <= 3) {
                $('.wis-js-form').slideDown();
            }
            else {
                $('.wis-js-form').slideUp();
            }
            $('.wis-js-rating-button').show();
        });
    }
})(WIS_M_Rating || (WIS_M_Rating = {}));
//# sourceMappingURL=WIS_M_Rating.js.map