$(document).ready(function () {
     
    $("#savLoan").click(function () {
        loadPanel("tabSide", 1);
        var win = $("#sideWindow");
        if (!win.data("kendoWindow")) {
            win.kendoWindow({
                modal: true,
                title: "Savings & Loans Links",
                cssClass: "col-md-5"
            });

        }
        else {
            win.data("kendoWindow").open();
        }
        win.parent().css("top", "105px");
        win.parent().css("left", "50px");
        win.parent().css("width", "350px");
    });

    $("#gl").click(function () {
        loadPanel("tabSide", 2);
        var win = $("#sideWindow");
        if (!win.data("kendoWindow")) {
            win.kendoWindow({
                modal: true,
                title: "General Ledger Links",
                cssClass: "col-md-5"
            });

        }
        else {
            win.data("kendoWindow").open();
        }
        win.parent().css("top", "105px");
        win.parent().css("left", "50px");
        win.parent().css("width", "350px");
    });

    $("#pdf").click(function () {
        window.open("/dash/exportPage.aspx?exportType=pdf&pageTitle=" + $(document).find("title").text(), "exportPage", "width=200, height=100");
    });

    $("#img").click(function () {
        window.open("/dash/exportPage.aspx?exportType=img&pageTitle=" + $(document).find("title").text(), "exportPage", "width=200, height=100");
    });

    $("#svg").click(function () {
        window.open("/dash/exportPage.aspx?exportType=svg&pageTitle=" + $(document).find("title").text(), "exportPage", "width=200, height=100");
    });

    $("#ul-sticky-toolbar").kendoTooltip({
        filter: "li",
        content: function (e) {
            var target = e.target; // the element for which the tooltip is shown
            var title = target.attr('mytitle');
            return title; // set the element text as content of the tooltip
        }
    });
});