
$.fn.setCursorPosition = function (pos) {
    if ($(this).get(0).setSelectionRange) {
        $(this).get(0).setSelectionRange(pos, pos);
    } else if ($(this).get(0).createTextRange) {
        var range = $(this).get(0).createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
};

$(".wis-js-login-input_phone").click(function () {
    $(this).setCursorPosition(3);
}).mask("+7(999)999-99-99");

$('.wis-js-login-input_phone').mask('+7(999)999-99-99');