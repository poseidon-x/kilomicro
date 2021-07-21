<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="deposit.aspx.cs" Inherits="coreERP.ln.deposit.deposit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Client Investments Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Investments Management</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                 <telerik:RadBinaryImage ID="RadBinaryImage1" Height="216" Width="216" AlternateText="Client Photo"
                         runat="server" ResizeMode="Fit"></telerik:RadBinaryImage> 
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
                 Amount Invested
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountInvested" AutoPostBack="true" runat="server" CssClass="form-control" 
                    OnTextChanged="txtAmountInvested_Changed"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Deposit Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" AutoPostBack="true" OnSelectedDateChanged="dtAppDate_Changed" 
                    DateInput-DateFormat="dd-MMM-yyyy">
                    
                </telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                 Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" CssClass="inputControl" AutoPostBack="true" OnSelectedIndexChanged="cboPaymentType_SelectedIndexChanged" runat="server"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Check No
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" CssClass="form-control" runat="server"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                 Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Naration (Description)
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNaration" runat="server" CssClass="form-control" Rows="1" TextMode="MultiLine"
                         MaxLength="400"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                 Investment Product
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboDepositType" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="cboDepositType_SelectedIndexChanged"                       
                        DropDownAutoWidth="Enabled" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Principal Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPrincRepaymentMode" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Interest Repayment Frequency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboInterestRepaymentMode" CssClass="inputControl" runat="server"></telerik:RadComboBox>
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
                 Period of Deposit
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboPeriod" runat="server" CssClass="inputControl" AutoPostBack="true"
                    DropDownWidth="355px" EmptyMessage="Select deposit period" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select deposit period" OnSelectedIndexChanged="cboPeriod_SelectedIndexChanged"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Maturity Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtMaturyityDate"  CssClass="inputControl" runat="server" DateInput-DateFormat="dd-MMM-yyyy">
                    <DateInput ReadOnly="true" runat="server">
                    </DateInput>
                    <Calendar Enabled="false" runat="server">
                    </Calendar>
                </telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                 Annual Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRateA" runat="server" CssClass="form-control" NumberFormat-AllowRounding="false"
                    OnTextChanged="txtRateA_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Monthly Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox Enabled="false" WrapperCssClass="inputControl" ID="txtInterestRate" runat="server" CssClass="form-control" NumberFormat-DecimalDigits="6"
                    OnTextChanged="txtInterestRate_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel" ID="chkAutoHeader">
                 Auto Interest Calculation
            </div>
            <div class="subFormInput">
                <asp:CheckBox runat="server" CssClass="inputControl" ID="chkAuto" />
            </div>
            <div class="subFormLabel">
                 Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" 
                    ID="txtPrincBal" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Interest Expected
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" 
                    ID="txtIntExpected" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtIntBalance" 
                        runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
        </div>
    </div> 
    <div class="subForm">
        <telerik:RadButton ID="btnSave" Text="Save Deposit" 
            runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div>
    
<telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" SelectedIndex="4" Width="1024px" OnTabClick="tab1_TabClick">
        <Tabs>
            <telerik:RadTab runat="server" Text="Signatories">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Schedule">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Deposits">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Withdrawals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Interests" Selected="True">
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Multi Payments" Visible="false">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="4">
        <telerik:RadPageView ID="RadPageView1" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                        Signatory Name
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDocDesc" CssClass="inputControl" runat="server"></telerik:RadTextBox>
                    </div>
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Signature Image
                    </div>
                    <div class="subFormInput">
                        <telerik:RadAsyncUpload runat="server" ID="upload4" InputSize="20"  CssClass="inputControl" 
                            AllowedFileExtensions="jpg,jpeg,png,gif" Localization-Select="Select Pic" 
                            MaxFileSize="100024000" UploadedFilesRendering="BelowFileInput"></telerik:RadAsyncUpload>                        
                    </div>
                </div>
            </div>
            <div class="subForm">
                <telerik:RadButton ID="btnAddDcoument" runat="server" Text="Add Signatory"
                     OnClick="btnAddDcoument_Click"></telerik:RadButton>
            </div> 
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false" OnItemCommand="gridDocument_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="fullName" HeaderText="Fullname of Signatory"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn DataField="image.image1" HeaderText="Signature" ImageWidth="128px" ImageHeight="128px" ResizeMode="Fit"></telerik:GridBinaryImageColumn>
                        <telerik:GridHyperLinkColumn DataTextField="fullName"  HeaderText="Download Signature"
                            DataNavigateUrlFields="image.imageID" DataNavigateUrlFormatString="/ln/loans/image.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="fullName" HeaderText="Delete" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="fullName" HeaderText="Edit" Text="Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
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
                        <telerik:GridBoundColumn DataField="depositDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="depositAmount" HeaderText="Deposit Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
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
            <telerik:RadGrid ID="gridInt" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="interestDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="interestRate" HeaderText="Interest Rate" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestAmount" HeaderText="Interest Amount" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principal" HeaderText="Principal" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView9" runat="server">             
            <telerik:RadGrid ID="gridMultiPayment" runat="server" AutoGenerateColumns="false" Width="800px"
                
                GridLines="Both" AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AllowAutomaticDeletes="false"
        AllowSorting="true" ShowFooter="true" EnableLinqExpressions="false" OnItemDataBound="gridMultiPayment_OnItemDataBound"
        OnItemCommand="gridMultiPayment_OnItemCommand" OnInsertCommand="gridMultiPayment_OnInsertCommand"
        OnItemInserted="gridMultiPayment_OnItemInserted" OnItemCreated="gridMultiPayment_OnItemCreated"
        OnUpdateCommand="gridMultiPayment_OnUpdateCommand" OnDeleteCommand="gridMultiPayment_OnDeleteCommand" >
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="amount" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="paymentType" HeaderText="Interest Rate" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="checkNo" HeaderText="Interest Amount" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="bank" HeaderText="Principal" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
