<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="controllerFileAnalyzer.aspx.cs" Inherits="coreERP.ln.loans.controllerFileAnalyzer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Controller Output File Processing
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Controller File Analyzer</h3>
    <telerik:RadMultiPage runat="server" ID="multi" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView" runat="server"> 
            <h2>Please select the controller file to continue</h2>
            <table>
                <tr>
                    <td style="width:200px">Controller Output File</td>
                    <td style="width:200px">
                        <telerik:RadAsyncUpload runat="server" ID="upload"  InputSize="25"  Width="250px" AllowedFileExtensions="txt,csv,dat" Localization-Select="Select Controller File" MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>    
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadButton Enabled="true" runat="server" id="btnNext" OnClick="btnNext1_Click" Text="Next"></telerik:RadButton>
                    </td>
                </tr>  
            </table>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    
    </asp:Content>
