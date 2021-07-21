<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLDash.aspx.cs" Inherits="SLDash" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">       
        .liveTileTitle
        {
            width: 280px;
            height: 30px;
            color: #fff;
            font-size: 12px; 
            font-family:'Segoe Print';
            font-weight:bold;
            background-repeat: repeat;
            padding: 10px;
            margin-left:10px;
        }
      
        .liveTileTitleImg
        {
            width: 280px;
            height: 280px; 
            font-size: 2px; 
            background-repeat: repeat;
            padding: 10px;
        }
 
        .weatherIcon
        {
            margin-top: 40px;
            margin-left: 60px;
        }
 
        .weatherInforWrapper
        {
            padding: 25px;
            float: right;
        }
        .wrapper {
         width:1350px;
         overflow:hidden;
         height:100%;
        }

        .left {
         width:320px;
         float:left;
         margin-right:10px;
        }

        .right {
         width:320px;
         float:right;
        }
        
        .middle {
         width:320px; 
         float:left;
         margin-left:10px;
        }
        
        .middle2 {
         width:320px; 
         float:left;
         margin-left:10px;
        }
    </style>    
    <script type="text/javascript">
        var index = 0;
        var index2 = 15;
        var index3 = 30;
        var index4 = 45;
        function LiveTileOnClientDataLoadingError(sender, args) {
            args.set_cancelErrorAlert(true);
        }
        function liveTileOnClientTemplateDataBound(sender, args) { 
            if (index > 15) {
                index = 0;
            }
            sender.set_value(index.toString());
        }
        function liveTileOnClientTemplateDataBound2(sender, args) { 
            if (index2 > 30) {
                index2 = 15;
            }
            sender.set_value(index2.toString());
        }
        function liveTileOnClientTemplateDataBound3(sender, args) { 
            if (index3 > 45) {
                index3 = 30;
            }
            sender.set_value(index3.toString());
        }
        function liveTileOnClientTemplateDataBound4(sender, args) { 
            if (index4 > 60) {
                index4 = 45;
            }
            sender.set_value(index4.toString());
        }
        function OnClientDataLoaded(sender, args) {
            var data = args.get_data();
            if (data.navigateUrl != null) {
                sender.set_navigateUrl(data.navigateUrl);
                index = data.nextIndex;
            } 
        }
        function OnClientDataLoaded2(sender, args) {
            var data = args.get_data();
            if (data.navigateUrl != null) {
                sender.set_navigateUrl(data.navigateUrl);
                index2 = 15 + data.nextIndex;
            }
        }
        function OnClientDataLoaded3(sender, args) {
            var data = args.get_data();
            if (data.navigateUrl != null) {
                sender.set_navigateUrl(data.navigateUrl);
                index3 = 30 + data.nextIndex;
            }
        }
        function OnClientDataLoaded4(sender, args) {
            var data = args.get_data();
            if (data.navigateUrl != null) {
                sender.set_navigateUrl(data.navigateUrl);
                index4 = 45 + data.nextIndex;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="wrapper">
        <div class="left">
            <telerik:RadLiveTile ID="LiveTile1" OnClientDataLoadingError="LiveTileOnClientDataLoadingError" 
                runat="server" Width="310" Height="340" Value="0" UpdateInterval="15000" 
                OnClientTemplateDataBound="liveTileOnClientTemplateDataBound" NavigateUrl="#= navigateUrl #"
               OnClientDataLoaded="OnClientDataLoaded">
                <WebServiceSettings Path="SLDash.aspx" Method="GetTileData" UseHttpGet="false" />
                <ClientTemplateAnimationSettings AnimationDuration="1000" Animation="Slide" />
                <ClientTemplate>
                    <img class="liveTileTitleImg" src="/ln/loans/image.aspx?id=#= imageID #" alt="#= imageName #"/>
                    <div class="liveTileTitle2">#= details #</div>
                </ClientTemplate>
            </telerik:RadLiveTile>
        </div>
        <div class="middle">
            <telerik:RadLiveTile ID="RadLiveTile4" OnClientDataLoadingError="LiveTileOnClientDataLoadingError" 
                runat="server" Width="310" Height="340" Value="45" UpdateInterval="15000" 
                OnClientTemplateDataBound="liveTileOnClientTemplateDataBound4" NavigateUrl="#= navigateUrl #"
               OnClientDataLoaded="OnClientDataLoaded4">
                <WebServiceSettings Path="SLDash.aspx" Method="GetTileData" UseHttpGet="false" />
                <ClientTemplateAnimationSettings AnimationDuration="1000" Animation="Slide" />
                <ClientTemplate>
                    <img class="liveTileTitleImg" src="/ln/loans/image.aspx?id=#= imageID #" alt="#= imageName #"/>
                    <div class="liveTileTitle2">#= details #</div>
                </ClientTemplate>
            </telerik:RadLiveTile>
        </div>
        <div class="middle2">
            <telerik:RadLiveTile ID="RadLiveTile3" OnClientDataLoadingError="LiveTileOnClientDataLoadingError" 
                runat="server" Width="310" Height="340" Value="30" UpdateInterval="15000" 
                OnClientTemplateDataBound="liveTileOnClientTemplateDataBound3" NavigateUrl="#= navigateUrl #"
               OnClientDataLoaded="OnClientDataLoaded3">
                <WebServiceSettings Path="SLDash.aspx" Method="GetTileData" UseHttpGet="false" />
                <ClientTemplateAnimationSettings AnimationDuration="1000" Animation="Slide" />
                <ClientTemplate>
                    <img class="liveTileTitleImg" src="/ln/loans/image.aspx?id=#= imageID #" alt="#= imageName #"/>
                    <div class="liveTileTitle2">#= details #</div>
                </ClientTemplate>
            </telerik:RadLiveTile>
        </div>
        <div class="right"> 
        </div>
        <div class="left">
            <telerik:RadLiveTile ID="RadLiveTile2" OnClientDataLoadingError="LiveTileOnClientDataLoadingError" 
                runat="server" Width="310" Height="340" Value="15" UpdateInterval="15000" 
                OnClientTemplateDataBound="liveTileOnClientTemplateDataBound2" NavigateUrl="#= navigateUrl #"
               OnClientDataLoaded="OnClientDataLoaded2">
                <WebServiceSettings Path="SLDash.aspx" Method="GetTileData" UseHttpGet="false" />
                <ClientTemplateAnimationSettings AnimationDuration="1000" Animation="Slide" />
                <ClientTemplate>
                    <img class="liveTileTitleImg" src="/ln/loans/image.aspx?id=#= imageID #" alt="#= imageName #"/>
                    <div class="liveTileTitle2">#= details #</div>
                </ClientTemplate>
            </telerik:RadLiveTile>
        </div>
    </div>
</asp:Content>