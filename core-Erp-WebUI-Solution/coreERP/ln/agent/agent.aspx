<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="agent.aspx.cs" Inherits="coreERP.ln.agent.agent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Agent Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Agent Management</h3>
       <table>
           <tr runat="server">
               <td Width="150px">
                   Agent Code
               </td>
               <td Width="400px">
                   <telerik:RadTextBox WrapperCssClass="inputControl" runat="server"  ID="txtAccNum"></telerik:RadTextBox>
               </td>
               <td Width="150px"> &nbsp;
               </td>               
               <td Width="400px">  &nbsp;
               </td>
                    <td rowspan="7">
                        <telerik:RadRotator Width="320" Height="180" runat="server" ID="rotator2" ItemHeight="180" ItemWidth="320" OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
                    </td>
           </tr>
           <tr runat="server">
               <td>
                   Surname
               </td>
               <td>
                   <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSurname" Width="220px"></telerik:RadTextBox>
               </td>
               <td>
                   Other Names
               </td>
               <td>
                   <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtOtherNames" Width="220px"></telerik:RadTextBox>
               </td>
           </tr>
           <tr ID="TableRow2" runat="server">
               <td>
                   DOB
               </td>
               <td>
                    <telerik:RadDatePicker runat="server" ID="dpDOB" DateInput-DateFormat="dd-MMM-yyyy" MinDate="1/1/1900 12:00:00 AM"></telerik:RadDatePicker>
               </td> 
               <td>
                   Sex
               </td>
               <td>
                   <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal">
                       <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                       <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                   </asp:RadioButtonList>
               </td>
           </tr> 
           <tr ID="TableRow4" runat="server">
               <td>
                   Branch
               </td>
               <td>
                   <telerik:RadComboBox runat="server" ID="cboBranch"></telerik:RadComboBox>
               </td> 
           </tr> 
                <tr>
                    <td>Bank Name</td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="cboBank" AutoPostBack="true" OnSelectedIndexChanged="cboBank_SelectedIndexChanged"></telerik:RadComboBox>
                    </td> 
                    <td>Bank Branch</td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="cboBankBranch"></telerik:RadComboBox>
                    </td> 
                </tr>
                <tr>
                    <td>Account Type</td>
                    <td>
                       <telerik:RadComboBox runat="server" ID="cboAccountType" AutoPostBack="true"></telerik:RadComboBox>
                    </td>   
                    <td >Account No.</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" Width="200"></telerik:RadTextBox>
                    </td> 
                </tr>
                <tr>
                    <td >AccountName</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountName" runat="server" Width="200"></telerik:RadTextBox>
                    </td>  
                </tr>   
            <tr>
                <td style="vertical-align:top">Agent Pictures</td>
                <td >
                    <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="50" Width="269px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>                        
                </td>
            </tr>
           <tr ID="TableRow6" runat="server">
               <td>&nbsp;</td>
               <td colspan="3">
                   <telerik:RadButton runat="server" ID="btnSave" Text="Save Agent Data" OnClick="btnSave_Click"></telerik:RadButton>
                   <telerik:RadButton runat="server" ID="btnSave2" Text="Save To Continue Later" OnClick="btnSave2_Click"></telerik:RadButton>
               </td>
           </tr>
       </table><br />
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="973px">
        <Tabs>
            <telerik:RadTab runat="server" Text="Physical Address" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Mailing Address"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Telephone Numbers"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Next of Kin"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView runat="server"> 
            <table width="100%">
                <tr>
                    <td style="width:150px">Address_Line_1</td>
                    <td style="width:500px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr1" runat="server" Width="400"></telerik:RadTextBox>
                    </td>
                    <td rowspan="7">
                        <telerik:RadRotator Width="320" Height="180" runat="server" ID="rotator1" ItemHeight="180" ItemWidth="320" OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
                    </td>
                </tr>
                <tr>
                    <td>Address Line 2</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr2" runat="server" Width="400"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Town/City</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyCityTown" runat="server" Width="150"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top">Location Picures</td>
                    <td >
                        <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="50" Width="469px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>                        
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <table width="100%">
                <tr>
                    <td style="width:150px">Address_Line_1</td>
                    <td style="width:500px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr1" runat="server" Width="400"></telerik:RadTextBox>
                    </td> 
                </tr>
                <tr>
                    <td>Address Line 2</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr2" runat="server" Width="400"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Town/City</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddrCity" runat="server" Width="150"></telerik:RadTextBox>
                    </td>
                </tr> 
            </table>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <table width="100%">
                <tr>
                    <td style="width:150px">Mobile Telephone</td>
                    <td style="width:500px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMobilePhone" runat="server" Width="200"></telerik:RadTextBox>
                    </td> 
                </tr>
                <tr>
                    <td>Work Telephone</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtWorkPhone" runat="server" Width="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>Home Telephone</td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtHomePhone" runat="server" Width="200"></telerik:RadTextBox>
                    </td>
                </tr> 
            </table>
        </telerik:RadPageView>     
        <telerik:RadPageView ID="RadPageView10" runat="server">
            <table width="100%">
               <tr>
                    <td style="width:150px">
                        Surname
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKSurname" runat="server"></telerik:RadTextBox>
                    </td>
                    <td style="width:150px">
                        Other Names
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKOtherNames" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:150px">
                        ID Type
                    </td>
                    <td style="width:200px">
                        <telerik:RadComboBox runat="server" ID="cboNOKIDType"></telerik:RadComboBox>
                    </td>
                    <td style="width:150px">
                        ID Number
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKIDNumber" runat="server"></telerik:RadTextBox>
                    </td>
                </tr> 
                <tr>
                    <td style="width:150px">
                        Email Address
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKEmailAddress" runat="server" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td style="width:150px">
                        Phone Number
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNOKPhoneNumber" runat="server"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:150px">
                       Relation to Agent
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtRelation" runat="server" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td style="width:150px"> 
                    </td>
                    <td style="width:200px"> 
                    </td>
                </tr>
                <tr>
                    <td>Picture</td>
                    <td  colspan="3">
                        <telerik:RadAsyncUpload runat="server" ID="upload5" InputSize="25"  Width="250px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </td>
                </tr>
                <tr>
                    <td style="width:150px">
                    </td>
                    <td style="width:200px"> 
                    </td> 
                    <td  colspan="4">
                        <telerik:RadButton ID="btnAddNOK" runat="server" Text="Add Next of Kin Details" OnClick="btnAddNOK_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gridNOK" runat="server" AutoGenerateColumns="false" OnItemCommand="gridNOK_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="idNo.idNo1" HeaderText="ID No"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="phone.phoneNo" HeaderText="Phone No."></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="email.emailAddress" HeaderText="Email Address"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn HeaderText="Picture" ResizeMode="Fit" DataField="image.image1" ImageWidth="48px" ImageHeight="48px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit NOK" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete NOK" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView> 
        <telerik:RadPageView ID="RadPageView8" runat="server"> 
            <table> 
                <tr>
                    <td style="width:150px">
                        Document Description
                    </td>
                    <td style="width:200px">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" runat="server"></telerik:RadTextBox>
                    </td> 
                    <td>Documents</td>
                    <td  colspan="3">
                        <telerik:RadAsyncUpload runat="server" ID="upload4" InputSize="25"  Width="250px" AllowedFileExtensions="pdf,txt,docx,xlsx,doc,xls,html,jpg,jpeg,png,gif" Localization-Select="Select Pic" MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </td>
                </tr>
                <tr>
                    <td style="width:150px">
                    </td>
                    <td style="width:200px"> 
                    </td> 
                    <td  colspan="4">
                        <telerik:RadButton ID="btnAddDcoument" runat="server" Text="Add Document" OnClick="btnAddDcoument_Click"></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDocument_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="document.description" HeaderText="Document Description"></telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn DataTextField="document.filename"  HeaderText="Download Document"
                            DataNavigateUrlFields="document.documentID" DataNavigateUrlFormatString="/ln/loans/document.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="document.description" HeaderText="Delete Document" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage> 
</asp:Content>
