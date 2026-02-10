function createTimeoutPopup() {
    $('body').append(`
        <div class='popup'> 
            <div class='popupContent'>
                <div>
                    <div class='popupHearder'> Attention!</div> 
                    <h4 id='popUpMessage'></h4>
                </div>
                <div onclick='closePopup(); ' class='close-btn fa fa-close'></div> 
            </div>
           
        </div> 
    `);
}
let currentTimeouID = undefined;
let initialized = false;
let timeBeforeRedirect = 5;
let timeoutCallBack = () => { window.location.replace("/Accounts/ExpiredSession"); };
let infinite = -1;
let timeLeft = infinite;
let maxStallingTime = infinite;

function initSessionTimeout(stallingTime = infinite, callback = timeoutCallBack) {
    maxStallingTime = stallingTime;
    timeoutCallBack = callback;
    createTimeoutPopup();
    initialized = true;
    startCountdown();
}
function noTimeout() {
    //clearTimeout(currentTimeouID);
    initSessionTimeout((20 * 60) - 60 ); // Default ASP.Net session is 20 minutes allow 60 secondes for user to decide
}

function startCountdown() {
    if (!initialized) initTimeout();
    clearTimeout(currentTimeouID);
    $(".popup").hide();
    timeLeft = maxStallingTime;
    if (timeLeft != infinite) {
        currentTimeouID = setInterval(() => {
            timeLeft = timeLeft - 1;
            if (timeLeft > 0) {
                if (timeLeft <= 10) {
                    $(".popup").show();
                    $("#popUpMessage").text("Expiration dans " + timeLeft + " secondes");
                }
            } else {
                $("#popUpMessage").text('Redirection dans ' + (timeBeforeRedirect + timeLeft) + " secondes");
                if (timeLeft <= -timeBeforeRedirect) {
                    clearTimeout(currentTimeouID);
                    closePopup();
                    timeoutCallBack();
                }
            }
        }, 1000);
    }
}
function closePopup() {
    $(".popup").hide();
    startCountdown();
} 