<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="rollOverBulk.aspx.cs" Inherits="coreERP.ln.saving.rollOverBulk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    RollOver Matured Savings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>RollOver Matured Savings</h3>
   <table>  
       <tr>
           <td>Roll Over Date</td>
           <td>
               <telerik:RadDatePicker ID="dtAppDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
           </td>
       </tr> 
       <tr>
           <td colspan="2">
               <telerik:RadButton ID="btnRollOverInt" runat="server" Text="Rollover Interest" Enabled="false" OnClick="btnRollOverInt_Click"></telerik:RadButton>
           </td>
       </tr>
   </table>    
</asp:Content>
