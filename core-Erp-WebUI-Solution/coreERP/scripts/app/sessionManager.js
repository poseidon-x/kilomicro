window.addEventListener("onbeforeunload", function () {
    $.ajax({
        url: "/security/logMeOut.aspx" 
    });
});