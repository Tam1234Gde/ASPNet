var WIS_M_Timer_Auth;
(function (WIS_M_Timer_Auth) {
    function Init(Time, Link) {
        var $Timer = $('.wis-js-timer');
        var $New_Code = $('.wis-js-new_code');
        var Time = Time;
        var downloadTimer;
        downloadTimer = setInterval(function () {
            var seconds = Time % 60;
            var minutes = Time / 60 % 60;
            if (Time <= 0) {
                clearInterval(downloadTimer);
                $Timer.parent().hide();
                if (Link != undefined) {
                    window.location.href = Link;
                }
                var targetChat = document.querySelector(".wis-js-animation__target_chat");
                $New_Code.show();
            }
            else if (Math.floor(minutes) <= 0) {
                if (seconds < 10) {
                    var strTimer = "0".concat(seconds, " \u0441\u0435\u043A");
                    $Timer.text(strTimer);
                }
                else {
                    var strTimer = "".concat(seconds, " \u0441\u0435\u043A");
                    $Timer.text(strTimer);
                }
            }
            else {
                if (seconds < 10) {
                    var strTimer = "0".concat(Math.floor(minutes), ":0").concat(seconds);
                    $Timer.text(strTimer);
                }
                else {
                    var strTimer = "0".concat(Math.floor(minutes), ":").concat(seconds);
                    $Timer.text(strTimer);
                }
            }
            --Time;
        }, 1000);
    }
    WIS_M_Timer_Auth.Init = Init;
})(WIS_M_Timer_Auth || (WIS_M_Timer_Auth = {}));
//# sourceMappingURL=WIS_M_Timer_Auth.js.map