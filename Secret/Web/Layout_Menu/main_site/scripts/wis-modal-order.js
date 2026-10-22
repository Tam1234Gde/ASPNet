

// Get the modal
var modal = document.getElementById("wis-modal-order");

// Get the button that opens the modal
var btn = document.getElementById("wis-open-order");

// Get the <span> element that closes the modal
var close = document.getElementById("wis-close");

// When the user clicks the button, open the modal


if (btn !== undefined && btn) {
    btn.onclick = function () {
        modal.style.display = "block";
    }
}

// When the user clicks on <span> (x), close the modal
if (close !== undefined && close) {
    close.onclick = function () {
        modal.style.display = "none";
    }
}


// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

let touchstartY = 0
let touchendY = 0

function checkDirection() {

    if (touchendY > touchstartY) {

        modal.style.display = "none";
       
    }
}


if (document.getElementById("wis-js-swipe")) {
    document.getElementById("wis-js-swipe").addEventListener('touchstart', e => {
        touchstartY = e.changedTouches[0].screenY

    })

    document.getElementById("wis-js-swipe").addEventListener('touchend', e => {
        touchendY = e.changedTouches[0].screenY;

        checkDirection()

    })
}





