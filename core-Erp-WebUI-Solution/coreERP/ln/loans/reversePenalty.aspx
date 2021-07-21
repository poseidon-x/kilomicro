<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="reversePenalty.aspx.cs" Inherits="coreERP.ln.loans.reversePenalty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    ::Reverse Loan Penalty
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 105px;
        }
        .auto-style2 {
            width: 67px;
        }
        .auto-style4 {
            width: 203px;
        }
        .auto-style5 {
            width: 19px
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Penalty Reversal</h3>
    
    <br />
    <table>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                &nbsp;</td>
            <td class="auto-style4">
                &nbsp;</td>
            <td class="auto-style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                
                &nbsp;</td>
            <td class="auto-style1">
                &nbsp;</td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                Client:</td>
            <td class="auto-style4">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="283px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number" ></telerik:RadComboBox>
                
            </td>
            <td class="auto-style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                
                Loan:</td>
            <td class="auto-style1">
                <telerik:RadComboBox ID="cboLoan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLoan_SelectedIndexChanged"
                                     EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                                     MarkFirstMatch="true" AutoCompleteSeparator="" Width="245px"/>
                
            </td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                &nbsp;</td>
            <td class="auto-style4">
                &nbsp;</td>
            <td class="auto-style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                &nbsp;</td>
            <td class="auto-style1">
                &nbsp;</td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                Penalty:</td>
            <td class="auto-style4" rowspan="3" valign="top">
                <telerik:RadComboBox ID="cboPenalty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboPenalty_SelectedIndexChanged"
                                     EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                                     MarkFirstMatch="true" AutoCompleteSeparator="" Width="283px" />
            <td class="auto-style5" >
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                
                Amount:</td>
            <td class="auto-style1" rowspan="3" valign="top">
                <telerik:RadNumericTextBox ID="txtReversalAmount" Runat="server" Culture="en-US" DbValueFactor="1" LabelWidth="64px" MinValue="1" Width="243px">
                </telerik:RadNumericTextBox>
            </td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                &nbsp;</td>
            <td class="auto-style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                &nbsp;</td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td>
                
                &nbsp;</td>
            <td class="auto-style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td class="auto-style2">
                &nbsp;</td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
            <td colspan="6" align="center">
                
                <telerik:RadButton runat="server" ID="btnReverse" Text="Reverse Penalty" OnClick="btnReverse_Click" style="top: 1px; left: 1px"></telerik:RadButton>
                    </td>
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
        </tr>
                <tr>
            <td>
                &nbsp;</td> 
        </tr>
    </table>
</asp:Content>
