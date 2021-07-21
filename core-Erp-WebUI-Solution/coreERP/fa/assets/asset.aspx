<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="asset.aspx.cs" Inherits="coreERP.ln.asset.asset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Asset Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Asset Management</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="132" Height="132" runat="server" ID="rotator2" 
                    ItemHeight="132" ItemWidth="132" 
                    OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="imageFrame">
                <telerik:RadAsyncUpload runat="server" ID="upload3" 
                    InputSize="20" Width="200px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" 
                    Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>                        
            </div>
            <div class="subFormLabel">
                Asset Tag
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Enabled="false" ID="txtAccNum"
                    ToolTip="The asset tag will be generated and placed here. You can override it by manually typing"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Asset Description
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtDesc" CssClass="inputControl"
                    ToolTip="Enter the name/description of the asset"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Asset Category
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboAssetCategory" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboAssetCategory_SelectedIndexChanged" CssClass="inputControl"
                    ToolTip="Select the category of the asset"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Asset SubCategory
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboSubCategory" CssClass="inputControl"
                    ToolTip="Select the sub-category of the asset"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Asset Depreciation Method
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblDepMeth"></asp:Label>
            </div>
            <div class="subFormLabel">
                 Asset Owner
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboStaff" CssClass="inputControl"
                    ToolTip="Select the staff/employee assigned to this the asset"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Asset Org. Unit
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboOU" CssClass="inputControl"
                    ToolTip="Select the organizational unit which owns the asset"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Asset Purchase Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtPurchDate" MinDate="1-1-1900" DateInput-DateFormat="dd-MMM-yyyy"
                    DateInput-ToolTip="Select the purchase date of the asset"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                 Life time (Years)
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Type="Number" ID="txtLifetime" NumberFormat-DecimalDigits="0"
                    ToolTip="Enter the life time years of the asset"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                 Asset Price
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Type="Number" ID="txtAssetPrice"
                    ToolTip="Enter the price of the asset"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Current Value
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Type="Number" ID="txtCurrentValue"
                    ToolTip="The current value of the asset will be placed here"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Last Depreciation Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtLastDep" MinDate="1-1-1900" 
                    DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                 Depreciation Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" Type="Number" ID="txtRate" Value="0"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Acc. Depreciation Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboADA" runat="server" Height="150px" CssClass="inputControl" DataTextField="fullname" DataValueField="acct_id"
                    DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 320px;">
                                    Acc Name</td>
                                <td style="width: 100px;">
                                    Acc Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 320px;">
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 100px;">
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Fixed Assets Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboFAA" runat="server" Height="150px" CssClass="inputControl" DataTextField="fullname" DataValueField="acct_id"
                    DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 320px;">
                                    Acc Name</td>
                                <td style="width: 100px;">
                                    Acc Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 320px;">
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 100px;">
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Depreciation Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboDA" runat="server" Height="150px" CssClass="inputControl" DataTextField="fullname" DataValueField="acct_id"
                    DropDownWidth=" 500px" EmptyMessage="Type the number, name of a GL Account" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator="" OnItemsRequested="cboGLAcc_ItemsRequested"   
                    EnableLoadOnDemand="true" AutoPostBack="true">
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 80px;">
                                    Acc Num</td>
                                <td style="width: 320px;">
                                    Acc Name</td>
                                <td style="width: 100px;">
                                    Acc Currency</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 80px;" >
                                    <%# Eval("acc_num")%>
                                </td>
                                <td style="width: 320px;">
                                    <%# Eval("acc_name")%>
                                </td> 
                                <td style="width: 100px;">
                                    <%# Eval("major_name")%>
                                </td> 
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton runat="server" ID="btnSave" Text="Save Asset Data" OnClick="btnSave_Click"   
                    ToolTip="Click to save this asset"></telerik:RadButton>
    </div>
    <br />
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs> 
            <telerik:RadTab runat="server" Text="Depreciation Schedule" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Asset Notes" Selected="True">   
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Asset Depreciation" Selected="True">   
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <telerik:RadGrid ID="gridSched" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="drepciationDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="depreciationAmount" HeaderText="Amount" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="assetValue" HeaderText="Value After" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
       <telerik:RadPageView ID="RadPageView8" runat="server"> 
           <div class="subForm">
               <div class="subFormColumnLeft">
                   <div class="subFormLabel">
                       Document Description
                   </div>
                   <div class="subFormInput">
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                   </div>
               </div>
               <div class="subFormColumnMiddle">
                   <div class="subFormLabel">
                       Documents
                   </div>
                   <div class="subFormInput">
                       <telerik:RadAsyncUpload runat="server" ID="upload4" InputSize="20"  CssClass="inputControl"
                            AllowedFileExtensions="pdf,txt,docx,xlsx,doc,xls,html,jpg,jpeg,png,gif" Localization-Select="Select Pic" MaxFileSize="100024000"
                            UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                   </div>
               </div>
               <div class="subFormColumnRight">
                   <div class="subFormLabel">
                       <telerik:RadButton ID="btnAddDcoument" runat="server" CssClass="inputControl" Text="Add Document" OnClick="btnAddDcoument_Click"></telerik:RadButton>
                   </div>
                   <div class="subFormInput">                     
                   </div>
               </div>
           </div> 
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDocument_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="document.description" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn DataTextField="document.filename"  HeaderText="Download Document"
                            DataNavigateUrlFields="document.documentID" DataNavigateUrlFormatString="/ln/loans/document.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Guarantor" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
       <telerik:RadPageView runat="server">
           <telerik:RadTextBox WrapperCssClass="inputControl" TextMode="MultiLine" runat="server" ID="txtNotes" Rows="20" Width="800px"></telerik:RadTextBox>
       </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <telerik:RadGrid ID="gridDep" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="drepciationDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="depreciationAmount" HeaderText="Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="assetValue" HeaderText="Value After" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>  
</asp:Content>
