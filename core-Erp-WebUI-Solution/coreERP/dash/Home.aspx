<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Home" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:100%;
         overflow:hidden;
        }

        .left {
         width:32%;
         float:left;
         margin-right:1%;
        }

        .right {
         width:33%;
         float:right;
        }
        
        .middle {
         width:32%; 
         float:left;
         margin-left:1%;
        }

        .headerTitle {
            font-family:sans-serif;
            font-size:14pt; 
            text-decoration:overline underline;
        }
 
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Welcome, <%= UserFullname() %>, <%= DisplayUserBranchName() %></h3>
    <h4 id="roleName" runat="server"></h4><h6 id="instruction" runat="server">Select one of the below options</h6>
    <telerik:RadTileList runat="server" ID="tile1"  RenderMode="Mobile"  
            SelectionMode="Multiple" EnableDragAndDrop="true"> 
    </telerik:RadTileList>    
</asp:Content>