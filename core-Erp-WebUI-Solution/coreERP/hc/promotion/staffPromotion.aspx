<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="staffPromotion.aspx.cs" Inherits="coreERP.hc.promotion.staffPromotion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Staff Promotion
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        fieldset ul, fieldset li{
            border:0; margin:0; padding:0; list-style:none;
        }
        fieldset li{
            clear:both;
            list-style:none;
            padding-bottom:10px;
        }

        .input{
            float:left;
        }
        .label{
            width:140px;
            float:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Staff Promotion</h3>
    <telerik:RadSplitter runat="server" ID="splitter1">
        <telerik:RadPane runat="server" ID="pane1" Width="30%">            
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
        </telerik:RadPane>
        <telerik:RadSplitBar runat="server" ID="bar1"></telerik:RadSplitBar>
         <telerik:RadPane runat="server" ID="pane2" Width="70%">           
               <fieldset>
                    <legend>Promotion and Staff Details</legend> 
                    <ul>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Select Staff</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboStaff" 
                                 DropDownAutoWidth="Enabled" AutoPostBack="true"
                                 OnSelectedIndexChanged="cboStaff_SelectedIndexChanged"
                                 Width="300px"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Promotion Date</asp:Label>
                            <telerik:RadDatePicker runat="server" ID="dtPromotionDate"
                                 DateInput-DateFormat="dd-MMM-yyyy" CssClass="input"></telerik:RadDatePicker>
                        </li> 
                    </ul> 
            </fieldset>          
               <fieldset runat="server" id="fsOldDetails" visible="false">
                    <legend>Old Staff Details</legend> 
                    <ul>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Staff Level</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboOldLevel" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px" Enabled="false"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Staff Notch</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboOldNotch" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px" Enabled="false"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Job Title</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboOldJobTitle" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px" Enabled="false"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Line Manager</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboOldManagerStaff" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px" Enabled="false"></telerik:RadComboBox>
                        </li>
                    </ul> 
            </fieldset>         
               <fieldset runat="server" id="fsNewDetails" visible="false">
                    <legend>New Staff Details</legend> 
                    <ul>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Staff Level</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboNewLevel" 
                                 DropDownAutoWidth="Enabled" AutoPostBack="true" 
                                OnSelectedIndexChanged="cboNewStaffLevel_SelectedIndexChanged"
                                 Width="300px"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Staff Notch</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboNewNotch" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Job Title</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboNewJobTitle" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px"></telerik:RadComboBox>
                        </li>
                        <li> 
                            <asp:Label cssClass="label" runat="server">Line Manager</asp:Label>
                            <telerik:RadComboBox CssClass="input" runat="server" ID="cboNewManagerStaff" 
                                 DropDownAutoWidth="Enabled"
                                 Width="300px"></telerik:RadComboBox>
                        </li>
                        <li>
                            <telerik:RadButton runat="server" ID="btnPromote" Text="Promote Selected Staff"
                                 OnClick="btnPromote_Click" CssClass="input"></telerik:RadButton>
                        </li>
                    </ul> 
            </fieldset>
        </telerik:RadPane>
    </telerik:RadSplitter>
</asp:Content>
