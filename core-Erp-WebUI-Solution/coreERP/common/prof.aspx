<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="prof.aspx.cs" Inherits="coreERP.common.prof" %>
<%@ Register Assembly="Microsoft.AspNet.EntityDataSource" 
    Namespace="Microsoft.AspNet.EntityDataSource" TagPrefix="ef" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Company Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .style1
        {
            width: 180px;
            height: 15px;
            font-weight: bold;
            font-size:10pt;
        }
        .style2
        {
            width: 300px;
            height: 15px;
        }
    </style> 

<script type="text/javascript">
    function showProcessing() {
        var divEl = document.getElementById('<%= divProc.ClientID %>');
        if (divEl != null) divEl.style["visibility"] = "visible";
        
        return true;
    }
        </script>

 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
             
        <div style="vertical-align:middle;padding-left:20px;padding-top:10px">
            <h5>Company Profile</h5>            
            <div id="divError" runat="server" style="visibility:hidden" visible="false">
                <asp:Image ID="Image2" ImageUrl="~/images/errors/error.jpg" runat="server" />
                <span id="spanError" class="error" runat="server"></span>
            </div> 
	        <table cellpadding="0" cellspacing="2">
	            <tr>
	                <td class="style1">
	                    Company Name
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtCompName" Width="280px"
	                        MaxLength="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                             ControlToValidate="txtCompName"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the company Name.">
                        </asp:RequiredFieldValidator> 
                    </td>
	                <td class="style1">
	                    Local Currency
	                </td>
	                <td class="style2">
	                <telerik:RadComboBox ID="cboCurrency" runat="server" Height="200px" Width=" 150px"
                            DropDownWidth=" 260px" EmptyMessage="Select a Currency" HighlightTemplatedItems="true"
                            Filter="Contains" AutoPostBack="true"
                            DataTextField="major_name" DataValueField="currency_id">
                            <HeaderTemplate>
                                <table style="width: 260px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 80px;">
                                            Major Name</td>
                                        <td style="width: 50px;">
                                            Symbol</td>
                                        <td style="width: 80px;">
                                            Minor Name</td>
                                        <td style="width: 50px;">
                                            Rate</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 250px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 100px;" >
                                            <%# Eval("major_name")%>
                                        </td>
                                        <td style="width: 150px;">
                                            <%# Eval("major_symbol")%>
                                        </td>  
                                        <td style="width: 150px;">
                                            <%# Eval("minor_name")%>
                                        </td>  
                                        <td style="width: 150px;">
                                            <%# ((double)Eval("current_buy_rate")).ToString("#,###.#0;(#,###.#0);0") %>
                                        </td>  
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Address Line 1
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtAddressLine1" Width="280px"
	                        MaxLength="250"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                             ControlToValidate="txtAddressLine1"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the company's address.">
                        </asp:RequiredFieldValidator> 
                    </td>
	                <td class="style1">
	                    VAT Reg. No.
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtVATNo" Width="200px"
	                        MaxLength="20"></asp:TextBox>
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Address Line 2
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtAddressLine2" Width="280px"
	                        MaxLength="250"></asp:TextBox> 
                    </td>
	                <td class="style1">
	                    VAT Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neVATRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                             ControlToValidate="neVATRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the VAT Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Phone
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtPhoneNumber" Width="200px"
	                        MaxLength="250"></asp:TextBox> 
                    </td>
	                <td class="style1">
	                    NHIL Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neNHILRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                             ControlToValidate="neNHILRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the NHIL Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr> 
	            <tr>
	                <td class="style1">
	                    Website
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtWebsite" Width="280px"
	                        MaxLength="250"></asp:TextBox> 
                    </td>
	                <td class="style1">
	                    WithHolding Tax Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neWithTaxRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                             ControlToValidate="neWithTaxRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the WithHolding Tax Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr> 
	            <tr>
	                <td class="style1">
	                    City/Town
	                </td>
	                <td class="style2">
	                    <telerik:RadComboBox ID="cboCity" runat="server" Height="200px" Width=" 150px"
                            DropDownWidth=" 250px" EmptyMessage="Select a City/Town" HighlightTemplatedItems="true"
                            Filter="Contains" AutoPostBack="true"
                            DataTextField="city_name" DataValueField="city_id">
                            <HeaderTemplate>
                                <table style="width: 250px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 100px;">
                                            City</td>
                                        <td style="width: 150px;">
                                            Country</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 250px" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 100px;" >
                                            <%# Eval("city_name")%>
                                        </td>
                                        <td style="width: 150px;">
                                            <%# Eval("country_name")%>
                                        </td>  
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                    </td>
	                <td class="style1">
	                    VAT Flat Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neVATFlatRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                             ControlToValidate="neVATFlatRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the VAT Flat Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Country
	                </td>
	                <td class="style2">
	                    <telerik:RadComboBox ID="cboCountry" runat="server" Height="200px" Width=" 150px"
                            DropDownWidth=" 250px" EmptyMessage="Select a Country" HighlightTemplatedItems="true"
                            Filter="Contains" AutoPostBack="true"
                            DataTextField="country_name" DataValueField="country_id">
                            <HeaderTemplate>
                                <table style="width: 250px" cellspacing="0" cellpadding="0">
                                    <tr>  
                                        <td style="width: 250px;">
                                            Country</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 250px" cellspacing="0" cellpadding="0">
                                    <tr> 
                                        <td style="width: 250px;">
                                            <%# Eval("country_name")%>
                                        </td>  
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                    </td>
	                <td class="style1">
	                    Employee SSF Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neEmployeeSSFRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                             ControlToValidate="neEmployeeSSFRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Employee SSF Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Email Address
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtEmail" Width="280px"
	                        MaxLength="250"></asp:TextBox> 
                    </td>
	                <td class="style1">
	                    Employer SSF Rate
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="neEmployerSSFRate" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                             ControlToValidate="neEmployerSSFRate"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Employer SSF Rate.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr> 
	            <tr>
	                <td class="style1">
	                    Fax
	                </td>
	                <td class="style2">
	                    <asp:TextBox runat="server" ID="txtFax" Width="280px"
	                        MaxLength="250"></asp:TextBox> 
                    </td>
	                <td class="style1">
	                    Petty Cash Ceiling
	                </td>
	                <td class="style2">
	                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="nePettyCashCeiling" Width="100px"
	                        MaxLength="20" Value="0"></telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                             ControlToValidate="nePettyCashCeiling"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly enter the Petty Cash Ceiling.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr>  
	            <tr>
	                <td class="style1">
	                    GL # Precedes Name
	                </td>
	                <td class="style2">
	                    <asp:CheckBox runat="server" ID="chkGLNoPrecedesName" /> 
                    </td>
	                <td class="style1">
	                    First Month of Fiscal Year
	                </td>
	                <td class="style2">
	                    <telerik:RadComboBox ID="cboFMOY" runat="server" Height="200px" Width=" 150px"
                            DropDownWidth=" 250px" EmptyMessage="Select a month" HighlightTemplatedItems="true"
                            Filter="Contains" AutoPostBack="true" > 
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="January" />
                                <telerik:RadComboBoxItem Value="2" Text="February" />
                                <telerik:RadComboBoxItem Value="3" Text="March" />
                                <telerik:RadComboBoxItem Value="4" Text="April" />
                                <telerik:RadComboBoxItem Value="5" Text="May" />
                                <telerik:RadComboBoxItem Value="6" Text="June" />
                                <telerik:RadComboBoxItem Value="7" Text="July" />
                                <telerik:RadComboBoxItem Value="8" Text="August" />
                                <telerik:RadComboBoxItem Value="9" Text="September" />
                                <telerik:RadComboBoxItem Value="10" Text="October" />
                                <telerik:RadComboBoxItem Value="11" Text="November" />
                                <telerik:RadComboBoxItem Value="12" Text="December" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                             ControlToValidate="cboFMOY"
                             ErrorMessage="!" Font-Bold="true" ToolTip="You must kindly select first month of fiscal year.">
                        </asp:RequiredFieldValidator> 
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Allow Editing of Posted Journal
	                </td>
	                <td class="style2">
	                    <asp:CheckBox runat="server" ID="chkAllowEditPostedJnl" /> 
                    </td>
	                <td class="style1">
                        Price Per Bag 
	                </td>
	                <td class="style2">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtPPB" runat="server" MaxLength="20" Value="0.0"></telerik:RadNumericTextBox>  
	                </td>
	            </tr>
                <tr>
                    <td>Company Logo</td>
                    <td colspan="3">
                        <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="50" Width="269px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>
                    </td>
                </tr>
	            <tr>
	                <td colspan="4">
	                    <asp:Button runat="server" ID="btnSave" Text="Save Changes" Width="118px" 
                            onclick="btnSave_Click"  
                            OnClientClick="showProcessing()"/>
                            <span id="divProc" runat="server" style="visibility:hidden">
                                <img ID="Image1" src="~/images/animated/processing.gif" runat="server"
                                 width="32" height="32" alt="PROCESING ....."  />
                            </span>
	                </td> 
	            </tr>
	            <tr>
	                <td colspan="4">
	                    <h5>Cost Center Security</h5>
	                </td>
	            </tr>
	            <tr>
	                <td class="style1">
	                    Enforce Cost Center Security
	                </td>
	                <td class="style2">
	                    <asp:CheckBox runat="server" ID="chkEnforceCCSecurity" /> 
                    </td>
	                <td class="style1">
	                    Enforce Cost Center Usage
	                </td>
	                <td class="style2"> 
	                    <asp:CheckBox runat="server" ID="chkEnforceCCUsage" /> 
	                </td>
	            </tr>
	            <tr>
	                <td colspan="2">
	                    <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="EntityDataSource1"
        GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticDeletes="true" AllowAutomaticUpdates="true"
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false" 
        OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand"
        OnItemInserted="RadGrid1_ItemInserted" OnItemCreated="RadGrid1_ItemCreated"
        OnUpdateCommand="RadGrid1_UpdateCommand">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
<MasterTableView autogeneratecolumns="False" EditMode="InPlace"
            datakeynames="user_gl_ou_id" datasourceid="EntityDataSource1" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
               CommandItemDisplay="Top" Width="500px" PageSize="5">
               <CommandItemSettings AddNewRecordText="Add Cost Center Permission" />
    <Columns>
        <telerik:GridBoundColumn DataField="user_gl_ou_id" DataType="System.Int32" 
            DefaultInsertValue="" HeaderText="user_gl_ou_id" ReadOnly="True" 
            SortExpression="user_gl_ou_id" UniqueName="user_gl_ou_id" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="user_name" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="User Name" 
            SortExpression="user_name" UniqueName="user_name" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">            
        </telerik:GridBoundColumn>   
        <telerik:GridTemplateColumn DataField="gl_ou_id" DataType="System.Double" 
            DefaultInsertValue="" HeaderText="Cost Center" 
            SortExpression="gl_ou_id" UniqueName="gl_ou_id" ItemStyle-Width="200px"
            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            <ItemTemplate>
                <span style="width:200px"><%# CostCenterName(int.Parse(DataBinder.Eval(Container.DataItem, "user_gl_ou_id").ToString()))%></span>
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                 <telerik:RadComboBox ID="cboCC" runat="server" Height="200px" Width=" 125px"
                    DropDownWidth=" 155px" EmptyMessage="Select a Cost Center" HighlightTemplatedItems="true"
                     
                    EnableLoadOnDemand="true" AutoPostBack="true">
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
                                    <%# DataBinder.Eval(Container, "Attributes['ou_name']")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="allow" DataType="System.Boolean" 
            DefaultInsertValue="" HeaderText="Allow?" 
            SortExpression="allow" UniqueName="allow" ItemStyle-Width="80px"
            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chkAllow"
                    Checked='<%# (bool)Eval("allow")%>' />
            </ItemTemplate>
            <EditItemTemplate>
                <span>
                    <asp:CheckBox ID="chkAllow" runat="server" 
                        Checked='<%# Bind("allow") %>' 
                        Width="80px">
                    </asp:CheckBox>
                </span>
            </EditItemTemplate>
        </telerik:GridTemplateColumn> 
        <telerik:GridBoundColumn DataField="creation_date" DataType="System.DateTime" 
            DefaultInsertValue="" HeaderText="creation_date" SortExpression="creation_date" 
            UniqueName="creation_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="creator" DefaultInsertValue="" 
            HeaderText="creator" SortExpression="creator" UniqueName="creator" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="modification_date" 
            DataType="System.DateTime" DefaultInsertValue="" HeaderText="modification_date" 
            SortExpression="modification_date" UniqueName="modification_date" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="last_modifier" DefaultInsertValue="" 
            HeaderText="last_modifier" SortExpression="last_modifier" 
            UniqueName="last_modifier" Visible="false">
        </telerik:GridBoundColumn>
        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
            ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
            ItemStyle-Width="32px" ItemStyle-Height="32px" >
        </telerik:GridEditCommandColumn>
          <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
            ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
            ConfirmText="Are you sure you want to delete the selected currency?">
          </telerik:GridButtonColumn>
    </Columns>  
</MasterTableView>
       <ClientSettings>
           <ClientEvents />
           <Selecting AllowRowSelect="true" />
           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
           <Selecting AllowRowSelect="true" />
       </ClientSettings>
    </telerik:RadGrid>
	                </td>
	            </tr>
	        </table>            
	    </div> 
	        
    <ef:EntityDataSource ID="EntityDataSource1" runat="server" 
        ConnectionString="name=core_dbEntities" DefaultContainerName="core_dbEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="user_gl_ou_gl_ou">
    </ef:EntityDataSource>     
</asp:Content>
