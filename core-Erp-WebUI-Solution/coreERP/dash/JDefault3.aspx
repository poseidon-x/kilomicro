<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JDefault3.aspx.cs" Inherits="JDefault3" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Investment DashBoard</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:95%;
         overflow:hidden;
        }

        .left {
         width:98%;
         padding:10px;
         margin:10px;
        }

        .right {
         width:98%;
         float:right;
         padding:10px;
         margin:10px;
        }
         
        .headerTitle {
            font-family:Calibri;
            font-size:14pt;   
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="subForm"> 
        Display Results for (Branch): 
        <select id="cboBranch"></select> 
    </div>
    <div class="wrapper">
        <div class="left">
            <h3 class="headerTitle">Investment Maturing This Week</h3>
            <div id="investmentGrid"></div>
        </div>
    </div>

        <div class="wrapper">
        <div class="left">
            <h3 class="headerTitle">Over-Matured Investment</h3>
            <div id="overMaturedinvestmentGrid"></div>
        </div>
    </div>

    <div class="wrapper">
        <div class="left">
            <h3 class="headerTitle">Top Ten Investors</h3>
            <div id="topTenInvestorsGrid"></div>
        </div>
    </div>
    
    <script src="/scripts/app/Dash/jDefault3.js"></script>

</asp:Content>