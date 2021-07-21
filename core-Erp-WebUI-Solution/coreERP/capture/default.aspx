<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.capture._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Capture Picture Using Web-Cam | coreERP(TM)</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="outer" style="width:1210px;">
            <video autoplay="autoplay" width="480" height="480"></video>
            <img src="" style="max-height: 480px; max-width: 480px;" width="480" height="480" />
        </div>
        <canvas style="display:none;" height="460" width="640"></canvas>
        <asp:HiddenField runat="server" ID="pictureB64" />
        <button id="btnSnap" onclick="snapshot()" type="button">Snap Picture</button>
        <button id="btnSubmit" onclick="SubmitPicture()" type="button">Submit Picture</button>
       <img src="../images/animated/processing.gif" style="visibility:hidden;max-height: 48px; max-width: 48px;" width="48" height="48" id="proc" />
        <script>
            navigator.getUserMedia = navigator.getUserMedia ||
                        navigator.webkitGetUserMedia ||
                        navigator.mozGetUserMedia ||
                        navigator.msGetUserMedia;
              var video = document.querySelector('video');
              var canvas = document.querySelector('canvas');
              var ctx = canvas.getContext('2d');
              var localMediaStream = null;
              var parentWindow;

              function snapshot() {
                    if (localMediaStream) {
                      ctx.drawImage(video, 0, 0, 640, 480);
                      // "image/webp" works in Chrome.
                        // Other browsers will fall back to image/png.
                      var pic = canvas.toDataURL('image/png'); 
                      document.getElementById('<%= pictureB64.ClientID %>').value = pic.substring(pic.indexOf(",")+1);
                      document.querySelector('img').src = pic;
                    }
                }

              video.addEventListener('click', snapshot, false);
              var errorCallback = function (e) {
                  console.log('Reeeejected!', e);
              };
              var posted = <%= posted %>;
              if (posted != true) {
                  // Not showing vendor prefixes or code that works cross-browser.
                  navigator.getUserMedia({ video: true }, function (stream) {
                      video.src = window.URL.createObjectURL(stream);
                      localMediaStream = stream;
                  }, errorCallback);
              }
              else{
                  window.opener.AddRotatorItem();
                  window.close();
              }

              function SubmitPicture() {
                  document.getElementById('btnSubmit').disabled=true;
                  document.getElementById('btnSnap').disabled=true;
                  document.getElementById('proc').style["visibility"] = "visible";
                  document.forms[0].submit();
              }
        </script>
    </form>
</body>
</html>
