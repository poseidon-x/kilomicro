<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="noteSideBar.ascx.cs" Inherits="coreERP.uc.noteSideBar" %>
    <link type="text/css" href="/styles/noteSidebar.css" rel="stylesheet" /> 
    <div id="messageBoard" ><div id="tabNote2"></div></div> 
    <!--Add script to update the page and send messages.--> 
    <script type="text/javascript">
        var launched = false;
        var noOfNewMessages = 0;
        var currentMessage = "";
        $(function () {
            $("#note").hide();

            // Declare a proxy to reference the hub. 
            var hub = $.connection.notificationsHub;
            // Create a function that the hub can call to broadcast messages.
            hub.client.sendMessage = function (userName, message, noOfMessages) {
                // Add the message to the page. 
                $('#tabNote2').html(message);
                $('#tabNote').html(message);
                currentMessage = message;
                var result = $("#tabNote2").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                });
                var result = $("#tabNote").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                });
                launched = hub.server.getAck('<%= HttpContext.Current.User.Identity.Name %>');
                if (/*(null == launched || false == launched) &&*/ message.length>10) {
                    flashMessage(); 
                } 
                noOfNewMessages = noOfMessages;
                $("#noOfMessages").text(noOfNewMessages.toString());
            };
            $('#tabNote').hide();
            $.connection.hub.start().done(function () {
                hub.server.registerConId('<%= HttpContext.Current.User.Identity.Name %>');
            });
            $("#clickableNote").click(function (e) {
                launchMessageWindow();
            });
             
            $(".close").click(function () {
                $("#note").css('visibility', 'hidden');
                hub.server.sendAck('<%= HttpContext.Current.User.Identity.Name %>');
            }); 
        });

        function launchMessageWindow() {
            $('#tabNote').show();
            if (!$("#tabNote").data("kendoTabStrip")) {
                var result = $("#tabNote").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                }); 
            }  
            var win = $("#window");
            if (!win.data("kendoWindow")) {
                win.kendoWindow({
                    modal : true, 
                    title: "New Notifications"
                });
                
            }
            else {
                win.data("kendoWindow").open();                
            }
            $("#window").closest(".k-window").css({
                top: 200,
                left: screen.width/2-300
            });
        }

        function closeMessageWindow() {
            var win = $("#window");
            if (win.data("kendoWindow")) { 
                win.data("kendoWindow").close();
            }
            $('#tabNote').hide();
        }

        function flashMessage() {
            $("#note").show();
            $("#note").css('visibility', 'visible');
        }
    </script>