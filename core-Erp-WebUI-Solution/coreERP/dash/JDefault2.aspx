<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JDefault2.aspx.cs" Inherits="JDefault2" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
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
            <h3 class="headerTitle">Applications Pending Approval</h3>
            <div id="appGrid"></div>
        </div>
        <div class="left">
            <h3 class="headerTitle">Undisbursed Approved Loans</h3>
            <div id="undGrid"></div>
        </div>
    </div>
    
    <div class="wrapper">
        <div class="right">
            <h3 class="headerTitle">Due and Overdue Repayments</h3>
            <div id="dueGrid" ></div>
        </div>
        <div class="right">
            <h3 class="headerTitle">Top Ten Borrowers</h3>
            <div id="toptenGrid"></div>
        </div>
    </div>
     
    <script src="/scripts/app/Dash/jDefault.js"></script>

</asp:Content>