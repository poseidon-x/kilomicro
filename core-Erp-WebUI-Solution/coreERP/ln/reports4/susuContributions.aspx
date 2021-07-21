<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="susuContributions.aspx.cs" Inherits="coreERP.ln.reports4.susuContributions" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=14.1.20.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Group Susu Contributions Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Group Susu Contributions Report</h3>
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
        </div> 
        <div class="subFormColumnRight">  
            <div class="subFormLabel"></div>
            <div class="subFormInput">
                <telerik:RadButton ID="btnRun" runat="server" OnClick="btnRun_Click" Text="Generate Report"></telerik:RadButton>
            </div>
            <div class="subFormLabel" id="divProc" runat="server" style="visibility:hidden">
            </div>
            <div class="subFormInput" id="divError" runat="server" style="visibility:hidden">
                <span id="spanError" class="error" runat="server"></span>
            </div>  
        </div>
    </div>  
    <telerik:ReportViewer ID="rvw" runat="server" Width="100%" Height="724px" ZoomMode="FullPage"
         ></telerik:ReportViewer>

    <script  type="text/javascript">
        $(document).load(function () {
            $('<%= rvw.ClientID %>').height(window.innerHeight * 0.65);
        });
    </script>
</asp:Content>
