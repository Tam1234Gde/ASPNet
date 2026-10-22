/* Автор: Инфографика (Висерв) - https://wiserv.ru */

/**
 * Компонент "Вкладки"
 */
module WIS_M_Timer_Auth {

    export function Init(Time: number, Link: string) {

        let $Timer = $('.wis-js-timer');

        let $New_Code = $('.wis-js-new_code');

        var Time = Time;

        var downloadTimer: any;

        downloadTimer = setInterval(function () {

            let seconds = Time % 60 
            let minutes = Time / 60 % 60

            if (Time <= 0) { 

                clearInterval(downloadTimer);
                $Timer.parent().hide();

                if (Link != undefined) {
                    window.location.href = Link
                }
              

                // Автоматический переход на страницу со страницы анимации когда анимация завершена и таймер закончился
                let targetChat: HTMLElement = document.querySelector(".wis-js-animation__target_chat");
    

                $New_Code.show();

            }

            else if (Math.floor(minutes) <= 0) {
                if (seconds < 10) {
                    let strTimer = `0${seconds} сек`;
                    $Timer.text(strTimer);
                }

                else {

                    let strTimer = `${seconds} сек`;

                    $Timer.text(strTimer);
                }
            }

            else {
                if (seconds < 10) {
                    let strTimer = `0${Math.floor(minutes)}:0${seconds}`;
                    $Timer.text(strTimer);
                }

                else {

                    let strTimer = `0${Math.floor(minutes)}:${seconds}`;

                    $Timer.text(strTimer);
                }
            }
             
            --Time;

        }, 1000)

    }
}