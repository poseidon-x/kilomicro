<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="rollOver.aspx.cs" Inherits="coreERP.ln.saving.rollOver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Cashier's Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cashier's Home</h3>
   <table>
        <tr>
            <td>Select Client</td>
            <td >
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td>Select Savings</td>
            <td >
                <telerik:RadComboBox ID="cboSavings" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboSavings_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="200px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
       <tr><td>&nbsp;</td></tr> 
       <tr><td>&nbsp;</td></tr> 
       <tr><td>&nbsp;</td></tr> 
       <tr>
           <td>Amount Invested</td>
           <td><asp:Label ID="lblAmount" runat="server"></asp:Label></td>
       </tr>
       <tr>
           <td>First Savings Date</td>
           <td><asp:Label ID="lblDepDate" runat="server"></asp:Label></td>
       </tr>
       <tr>
           <td>Maturity Date</td>
           <td><asp:Label ID="lblMaturityDate" runat="server"></asp:Label></td>
       </tr>
       <tr>
           <td>Principal Balance</td>
           <td><asp:Label ID="lblPrinc" runat="server"></asp:Label></td>
       </tr>
       <tr>
           <td>Interest Balance</td>
           <td><asp:Label ID="lblInte" runat="server"></asp:Label></td>
       </tr>
       <tr>
           <td>New Savings Date</td>
           <td>
               <telerik:RadDatePicker ID="dtAppDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
           </td>
       </tr>
       <tr>
           <td>New Interest Rate (per Annum)</td>
           <td>
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtRate" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
           </td>
       </tr>
       <tr>
           <td>New Period (months)</td>
           <td>
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtPeriod" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
           </td>
       </tr>
       <tr>
           <td colspan="2">
               <telerik:RadButton ID="btnRollOverInt" runat="server" Text="Rollover Interest" Enabled="false" OnClick="btnRollOverInt_Click"></telerik:RadButton>
           </td>
       </tr>
   </table>    
</asp:Content>
