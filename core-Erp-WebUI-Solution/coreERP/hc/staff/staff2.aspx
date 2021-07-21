<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="staff2.aspx.cs" Inherits="coreERP.hc.staff.staff2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Staff Master Data Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Staff Master Data Management</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator2" ItemHeight="216" ItemWidth="216" 
                    OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="imageFrame">
                <telerik:RadAsyncUpload runat="server" ID="upload3" InputSize="20" 
                    Width="200px" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" Localization-Select="Select Pic" MaxFileSize="102400000"
                            ToolTip="Select the picture to upload the staff here"></telerik:RadAsyncUpload> 
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Staff ID
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server"  ID="txtAccNum" CssClass="inputControl"
                    ToolTip="The staff number will be generated and placed here. To override, you can manually type it here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Surname
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtSurname" CssClass="inputControl"
                    ToolTip="Enter the surname of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Other Names
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtOtherNames" CssClass="inputControl"
                    ToolTip="Enter the other names (firstname) of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Date of Birth
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dpDOB" DateInput-DateFormat="dd-MMM-yyyy" MinDate="1/1/1900 12:00:00 AM" CssClass="inputControl"
                    ToolTip="Select the date of birth the staff here"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
               Marital Status
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboMaritalStatus" CssClass="inputControl"
                    ToolTip="Select the marital status the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Sex (Gender)
            </div>
            <div class="subFormInput">
                <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal" CssClass="inputControl"
                    ToolTip="Select the gender the staff here">
                       <asp:ListItem Value="F" Text="Female"></asp:ListItem>
                       <asp:ListItem Value="M" Text="Male"></asp:ListItem>
                   </asp:RadioButtonList>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
               Staff Category
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboCategory" AutoPostBack="True" CssClass="inputControl"
                       OnSelectedIndexChanged="cboCategory_SelectedIndexChanged" 
                    ToolTip="Select the employee category the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Job Title
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboJobTitle" CssClass="inputControl"
                    ToolTip="Select the job title the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Branch
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboBranch" CssClass="inputControl"
                        ToolTip="Select the branch of the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Username
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtUserName" CssClass="inputControl" MaxLength="30"
                    ToolTip="Enter the system (coreERP) user name of the staff here"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
               Employment Status
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" ID="cboEmploymentStatus" CssClass="inputControl"
                    ToolTip="Select the current employment status the staff here"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
               Employment Start Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker runat="server" ID="dtpEmploymentStartDate" DateInput-DateFormat="dd-MMM-yyyy" CssClass="inputControl"
                    DateInput-ToolTip="Select the start date of employment the staff here"></telerik:RadDatePicker>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton runat="server" ID="btnSave" Text="Save Staff Data" OnClick="btnSave_Click"
                    ToolTip="Click to save the staff's records into the system"></telerik:RadButton>
    </div> 
    <br />
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Staff Level Details" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Qualifications">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Relationship">   
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Physical Address">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Mailing Address"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Telephone Numbers"> 
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Line Manager
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboStaffManager"  CssClass="inputControl"
                            DropDownAutoWidth="Enabled" MaxHeight="300"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Employment Start Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                            ID="dtpEmploymentStartDate2" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Level
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboLevel" DropDownAutoWidth="Enabled"
                             MaxHeight="300" AutoPostBack="true" CssClass="inputControl"
                             OnSelectedIndexChanged="cboLevel_SelectedIndexChanged"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Notch
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" ID="cboNotch"  CssClass="inputControl"
                            DropDownAutoWidth="Enabled" MaxHeight="300"></telerik:RadComboBox>
                    </div>
                </div>
            </div> 
        </telerik:RadPageView>     
        <telerik:RadPageView ID="RadPageView3" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Qualification Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboQualificationType" runat="server"  CssClass="inputControl"
                            DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Completion Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                        ID="dtEndDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Qualification Subject
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboQualificationSubject" CssClass="inputControl"
                             runat="server" DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Institution Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtInstitutionName" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Start Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                            ID="dtStartDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                    <div class="subFormLabel">
                        Expiry Date
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                            ID="dtExpiryDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
            </div> 
            <div class="subForm">
                <telerik:RadButton ID="btnAddQualification" runat="server" 
                    Text="Add Qualification" OnClick="btnAddQualification_Click"></telerik:RadButton>
            </div>
            <br />
            <telerik:RadGrid ID="gridQualification" runat="server" AutoGenerateColumns="false" OnItemCommand="gridQualification_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="qualificationType.qualificationTypeName" HeaderText="Qualification Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="qualificationSubject.qualificationSubjectName" HeaderText="Qualification Subject"></telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="startDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="endDate" HeaderText="Completion Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="expiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                        <telerik:GridBoundColumn DataField="institutionName" HeaderText="Institution Name"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Relation Type
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox ID="cboRelationType" runat="server" CssClass="inputControl"
                             DropDownAutoWidth="Enabled" MaxHeight="400"></telerik:RadComboBox>
                    </div>
                    <div class="subFormLabel">
                        Date of Birth
                    </div>
                    <div class="subFormInput">
                        <telerik:RadDatePicker runat="server" CssClass="inputControl"
                            ID="dtRelationDOB"  DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Surname
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtRelationSurname"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Other Names
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" ID="txtRelationOtherNames"></telerik:RadTextBox>
                    </div> 
                </div>
            </div> 
            <div class="subForm">
               <telerik:RadButton ID="btnAddRelation" runat="server" 
                   Text="Add Relation" OnClick="btnAddRelation_Click"></telerik:RadButton>
            </div>
            <br /> 
            <telerik:RadGrid ID="gridRelation" runat="server" AutoGenerateColumns="false" OnItemCommand="gridRelation_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="relationType.relationTypeName" HeaderText="Relation Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="dob" HeaderText="Date of Birth" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView> 
        <telerik:RadPageView runat="server">  
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="imageFrame">
                        <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" 
                            ItemHeight="216" ItemWidth="216" 
                            OnItemDataBound="rotator1_ItemDataBound" FrameDuration="30000"></telerik:RadRotator>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Map (Location Pic)
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="20" 
                            CssClass="inputControl" AllowedFileExtensions="png,jpg,jpeg,gif,tiff" 
                            Localization-Select="Select Pic" MaxFileSize="102400000"></telerik:RadAsyncUpload>                        
                    </div>
                    <div class="subFormLabel">
                        Address_Line_1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr1" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyAddr2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtPhyCityTown" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div> 
            </div> 
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView1" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Address_Line_1
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr1" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                        Address Line 2
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddr2" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Town/City
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMailAddrCity" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
            </div>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Mobile Telephone
                    </div>
                    <div class="subFormInput">
                       <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtMobilePhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Work Telephone
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtWorkPhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Home Telephone
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtHomePhone" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
            </div> 
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
                         <telerik:RadAsyncUpload runat="server" ID="upload4" InputSize="20"  
                             CssClass="inputControl" AllowedFileExtensions="pdf,txt,docx,xlsx,doc,xls,html,jpg,jpeg,png,gif"
                              Localization-Select="Select Pic" MaxFileSize="100024000"
                              UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="imageFrame">  
                         <telerik:RadButton ID="btnAddDcoument" runat="server" 
                             Text="Add Document" OnClick="btnAddDcoument_Click"></telerik:RadButton>
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
    </telerik:RadMultiPage> 
</asp:Content>
