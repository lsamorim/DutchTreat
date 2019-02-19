$(document).ready(function () {
    //var theForm = document.getElementById("the-form");
    //theForm.hidden = true;

    var button = document.getElementById("buy-button");
    button.addEventListener("click", onBuyClicked);

    var productProps = $(".product-props li");
    productProps.on("click", function () {
        console.log("Clicked on " + $(this).text());
    });

    function onBuyClicked() {
        console.log("Buying Item");
    }

    var $loginToggle = $("#login-toggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.fadeToggle(1000);
    });
});