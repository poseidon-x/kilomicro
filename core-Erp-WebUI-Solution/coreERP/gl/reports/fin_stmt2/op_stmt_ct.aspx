<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="op_stmt_ct.aspx.cs" Inherits="coreERP.gl.reports.fin_stmt2.op_stmt_ct" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=14.1.20.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Income Statement (Analytical)
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Income Statement (Analytical)</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Start Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtpStartDate"  
                    DateInput-DateFormat="dd-MMM-yyyy" >
                </telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Show Summary
            </div>
            <div class="subFormInput">
                <asp:CheckBox ID="chkSum" runat="server" Checked="false" />
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                End Date:
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtpEndDate"  
                    DateInput-DateFormat="dd-MMM-yyyy" >
                </telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Cost Center (Department)
            </div>
            <div class="subFormInput">
                 <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" Width=" 225px"
                    DropDownWidth=" 155px" EmptyMessage="Cost Center" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" 
                     DataTextField="ou_name" DataValueField="ou_id">
                    <HeaderTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr> 
                                <td style="width: 150px;">
                                    Cost Center</td> 
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 155px" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 150px;" >
                                    <%# Eval("ou_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Accounts W/O Tx
            </div>
            <div class="subFormInput">
                <asp:CheckBox ID="chkNoTx" runat="server" Checked="false" />
            </div>
            <div class="subFormLabel">
                Group Report By:
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboPeriodType">
                    <Items>
                        <telerik:RadComboBoxItem Value="365" Text="Yearly" />
                        <telerik:RadComboBoxItem Value="182" Text="Half-Yearly" />
                        <telerik:RadComboBoxItem Value="91" Text="Quarterly" />
                        <telerik:RadComboBoxItem Value="30" Text="Monthly" />
                        <telerik:RadComboBoxItem Value="7" Text="Weekly" />
                        <telerik:RadComboBoxItem Value="1" Text="Daily" />
                    </Items>
                </telerik:RadComboBox>
            </div>
        </div>
        <div class="subForm">
            <div class="subFormColumnLeft">
                <div>
                    <asp:Button runat="server" ID="btnRun" Text="Get Report" 
                        onclick="btnRun_Click" />
                    <telerik:ReportViewer ID="rvw" runat="server" Height="800px" Width="1800px" ZoomMode="FullPage"></telerik:ReportViewer>
                </div>
            </div>
            <div class="subFormColumnMiddle">
                <div class="subFormLabel" id="divProc" runat="server" style="visibility:hidden">
                </div>
                <div class="subFormInput" id="divError" runat="server" style="visibility:hidden">
                    <span id="spanError" class="error" runat="server"></span>
                </div> 
            </div>
        </div>
    </div>  
</asp:Content>
