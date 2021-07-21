<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="saving.aspx.cs" Inherits="coreERP.ln.saving.saving" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Open Regular Deposit Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Open Regular Deposit Account</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" ItemHeight="216" 
                    ItemWidth="216" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel"> 
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
            </div>
            <asp:Panel ID="pnlRegular" runat="server" Visible="true">
                <div class="subFormLabel">
                    Surname
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Other Names
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlJoint" runat="server" Visible="false"> 
                <div class="subFormLabel">
                    Account Name
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <div class="subFormLabel">
                    <telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"></telerik:RadButton>
            </div>
            <div class="subFormInput"> 
            </div>
            <div class="subFormLabel">
                    Client Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client"
                     HighlightTemplatedItems="true" CausesValidation="false"
                     EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox> 
            </div> 
        </div>
        <div class="subFormColumnMiddle"> 
            <div class="subFormLabel">
                  Account Open Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div> 
            <div class="subFormLabel">
                 Deposit Product
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboSavingsType" runat="server" AutoPostBack="true"
                         OnSelectedIndexChanged="cboSavingsType_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" CssClass="inputControl" ></telerik:RadComboBox>
            </div> 
            <div class="subFormLabel">
                 Annual Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRateA" runat="server" CssClass="form-control" 
                    OnTextChanged="txtRateA_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Monthly Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtInterestRate" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="6" 
                    OnTextChanged="txtInterestRate_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Deposit Plan (Frequency)
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboSavingPlan" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Deposit Plan (Amount)
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtPlanAmount" runat="server" CssClass="form-control" Value="0"></telerik:RadNumericTextBox>
            </div>
        </div>
        <div class="subFormColumnRight">           
            <div class="subFormLabel">
                Relationship Officer
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboStaff" runat="server" CssClass="inputControl"
                    DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the staff assigned to the loan"></telerik:RadComboBox>
            </div> 
            <div class="subFormLabel">
                Field Agent
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboAgent" runat="server" CssClass="inputControl"
                    DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                MarkFirstMatch="true" AutoCompleteSeparator=""
                ToolTip="Select the agent who processed this loan application"></telerik:RadComboBox>
            </div>    
            <div class="subFormLabel">
                 Principal Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPrincRepaymentMode" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                  Interest Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboInterestRepaymentMode" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtPrincBal" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtIntBalance" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div> 
        </div>    
     </div>
    <div class="subForm">
        <telerik:RadButton ID="btnSave" Text="Save Deposit Account" 
            runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div> 
<telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Authorized Signatories" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Next Of Kin Details">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Schedule" Visible="false">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Deposit History">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Withdrawals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Interests Accumulated">
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Savings Plan">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Signatory Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Signature Image
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload1" InputSize="20"  CssClass="inputControl" 
                            AllowedFileExtensions="jpg,jpeg,png,gif" Localization-Select="Select Pic" 
                            MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>
                </div>
            </div>
            <div class="subForm">
                <telerik:RadButton ID="btnAddDcoument" runat="server" Text="Add Signatory"
                     OnClick="btnAddDcoument_Click"></telerik:RadButton>
            </div> 
            <telerik:RadGrid ID="gridGuarantor" runat="server" AutoGenerateColumns="false" OnItemCommand="gridGuarantor_ItemCommand" Width="600px">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn ItemStyle-Width="250px" DataField="fullName" HeaderText="Full Name"></telerik:GridBoundColumn> 
                        <telerik:GridBinaryImageColumn HeaderText="Signature" ResizeMode="Fit" DataField="image.image1" ImageWidth="96px" ImageHeight="96px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ItemStyle-Width="100px" ButtonType="PushButton" CommandName="EditItem" CommandArgument="fullName" HeaderText="View/Edit Signatory" Text="View/Edit" ></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ItemStyle-Width="100px" ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="fullName" HeaderText="Delete Signatory" Text="Delete"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView runat="server" ID="rpvNextOfKin">
            <telerik:RadGrid ID="gridNextOfKin" runat="server" GridLines="Both" AllowAutomaticInserts="true" 
                AllowAutomaticDeletes="true" AllowAutomaticUpdates="true" AllowSorting="true" ShowFooter="true"  
                OnItemCommand="RadGrid1_ItemCommand" OnInsertCommand="RadGrid1_InsertCommand" OnItemInserted="RadGrid1_ItemInserted"
                OnUpdateCommand="RadGrid1_UpdateCommand" OnDeleteCommand="RadGrid1_OnDeleteCommand">
                    <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                    <MasterTableView autogeneratecolumns="False" EditMode="InPlace"
                                datakeynames="savingNextOfKinId" AllowAutomaticDeletes="True" 
                                AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
                                CommandItemDisplay="Top" Width="1200px">
                        <Columns>
                            <telerik:GridBoundColumn DataField="savingNextOfKinId" DataType="System.Int32" 
                                DefaultInsertValue="" HeaderText="savingNextOfKinId" ReadOnly="True" 
                                SortExpression="savingNextOfKinId" UniqueName="savingNextOfKinId" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="surName" DefaultInsertValue="" 
                                HeaderText="Surname" SortExpression="surName" UniqueName="surName" ItemStyle-Width="120px">
                            </telerik:GridBoundColumn>  
                            <telerik:GridBoundColumn DataField="otherNames" DefaultInsertValue="" 
                                HeaderText="Other Names" SortExpression="otherNames" UniqueName="otherNames" ItemStyle-Width="120px">
                            </telerik:GridBoundColumn>  
                            <telerik:GridDateTimeColumn DataField="dob" DefaultInsertValue="" DataFormatString="{0:dd-MMM-yyyy}"
                                HeaderText="Date Of Birth" SortExpression="dob" UniqueName="dob" ItemStyle-Width="100px"
                                EditDataFormatString="dd-MMM-yyyy">
                            </telerik:GridDateTimeColumn>  
                            <telerik:GridDropDownColumn DataField="relationshipType" HeaderText="Relation"
                                ListValueField="relationTypeName" ListTextField="relationTypeName" DataSourceID="relDataSource">
                            </telerik:GridDropDownColumn>
                            <telerik:GridDropDownColumn DataField="idTypeId" HeaderText="ID Type"
                                ListValueField="idNoTypeID" ListTextField="idNoTypeName" DataSourceID="idTypeDataSource">
                            </telerik:GridDropDownColumn> 
                            <telerik:GridBoundColumn DataField="idNumber" DefaultInsertValue="" 
                                HeaderText="ID Number" SortExpression="idNumber" UniqueName="idNumber" ItemStyle-Width="80px">
                            </telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="phoneNumber" DefaultInsertValue="" 
                                HeaderText="Contact Number" SortExpression="phoneNumber" UniqueName="phoneNumber" ItemStyle-Width="180px">
                            </telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="percentageAllocated" DefaultInsertValue="" DataFormatString="{0:#,##0.#0}"
                                HeaderText="% Allocated" SortExpression="percentageAllocated" UniqueName="percentageAllocated" 
                                ItemStyle-Width="80px" MaxValue="100" MinValue="0" FooterAggregateFormatString="{0:#,##0.#0}" Aggregate="Sum">
                            </telerik:GridNumericColumn>  
                            <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" 
                                ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" 
                                ItemStyle-Width="32px" ItemStyle-Height="32px" >
                            </telerik:GridEditCommandColumn>
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected institution?">
                              </telerik:GridButtonColumn>
                        </Columns> 
                    </MasterTableView>
               <ClientSettings> 
                   <Selecting AllowRowSelect="true" />
                   <KeyboardNavigationSettings AllowActiveRowCycle="true" />
                   <Selecting AllowRowSelect="true" />
               </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView5" runat="server"> 
            <table>
                <tr>
                    <td style="width:150px">
                        Repayment Date
                    </td>
                    <td style="width:200px">
                        <telerik:RadDatePicker ID="dtRepaymentDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                    </td>
                </tr>
                <tr> 
                    <td>
                        Principal
                    </td>
                    <td>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtPrincipal" runat="server"></telerik:RadNumericTextBox>
                    </td> 
                    <td>
                        Interest
                    </td>
                    <td>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtInterest" runat="server"></telerik:RadNumericTextBox>
                    </td>
                </tr>   
                <tr>
                    <td> 
                    </td>
                    <td>
                        <telerik:RadButton ID="btnAddSchedule" runat="server" Text="Add Schedule" OnClick="btnAddSchedule_Click"></telerik:RadButton>  
                    </td> 
                </tr> 
            </table>
            <telerik:RadGrid ID="gridSchedule" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnItemCommand="gridSchedule_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Repayment Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="principalPayment" HeaderText="Principal" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="interestPayment" HeaderText="Interest" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="expensed" HeaderText="Expensed"></telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn DataField="authorized" HeaderText="Authorized"></telerik:GridCheckBoxColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Schedule" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Schedule" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">             
            <telerik:RadGrid ID="gridDep" runat="server" AutoGenerateColumns="false" Width="1000px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="savingDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="savingAmount" HeaderText="Savings Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="checkNo" HeaderText="CheckNo"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="modeOfPayment.modeOfPaymentName" HeaderText="Payment Mode"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView6" runat="server">             
            <telerik:RadGrid ID="gridWith" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalWithdrawal" HeaderText="Principal Withdrawn" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="checkNo" HeaderText="CheckNo"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="modeOfPayment.modeOfPaymentName" HeaderText="Payment Mode"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server">             
            <telerik:RadGrid ID="gridInt" runat="server" AutoGenerateColumns="false" Width="500px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="interestDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="interestRate" HeaderText="Interest Rate" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestAmount" HeaderText="Interest Amount" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="deposited" HeaderText="Cum. Depositss" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Princ. Balance" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principal" HeaderText="Principal" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server">             
            <telerik:RadGrid ID="gridPlan" runat="server" AutoGenerateColumns="false" Width="600px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="plannedDate" HeaderText="Planned Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="plannedAmount" HeaderText="Planned Amount" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridCheckBoxColumn DataField="deposited" HeaderText="Deposited?"></telerik:GridCheckBoxColumn>
                        <telerik:GridNumericColumn DataField="amountDeposited" HeaderText="Deposited Amount" DataFormatString="{0:#,###.#0}"
                             Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    
    <asp:ObjectDataSource runat="server" DataObjectTypeName="coreERP.RelationTypeDataSource" SelectMethod="Get"
        ID="relDataSource" TypeName="coreERP.RelationTypeDataSource" />
    <asp:ObjectDataSource runat="server" DataObjectTypeName="coreERP.IdTypeDataSource" SelectMethod="Get"
        ID="idTypeDataSource" TypeName="coreERP.IdTypeDataSource">
    </asp:ObjectDataSource>
</asp:Content>
