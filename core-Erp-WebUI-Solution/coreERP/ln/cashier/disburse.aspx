<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="disburse.aspx.cs" Inherits="coreERP.ln.cashier.disburse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Disbursements
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Disbursements</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="134" Height="134" 
                    runat="server" ID="rotator1" ItemHeight="134" ItemWidth="134" 
                    FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
            <asp:Panel ID="pnlRegular" runat="server" Visible="true">
                <div class="subFormLabel">
                    Surname
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Other Names
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlJoint" runat="server" Visible="false"> 
                <div class="subFormLabel">
                    Account Name
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName" runat="server" CssClass="inputControl"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <div class="subFormLabel">
                Loan ID:
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblLoanID" Text=""></asp:Label>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Disbursement Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtDisbDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Calendar-RangeMinDate="1/1/1900 12:00:00 AM" Calendar-RangeSelectionStartDate="1/1/1900 12:00:00 AM" Calendar-RegisterWithScriptManager="True" DateInput-MinDate="1/1/1900 12:00:00 AM"
                    DateInput-ToolTip="Select the date on which the client was paid. Your till for that date must be openned by the admistrator in order to proceed"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Disbursment Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountPaid" runat="server" CssClass="inputControl"
                    ToolTip="Enter the amount to be disbursed. It must be less than or equal to the approved amount"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" runat="server" CssClass="inputControl"
                        ToolTip="Select the method by which the client was paid"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Check No
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" CssClass="inputControl"
                        ToolTip="Enter the check number (if applicable)"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"
                        ToolTip="Select the bank  account from which then client was paid (if applicable)"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Post to Saving's Account
            </div>
            <div class="subFormInput">
                <asp:CheckBox runat="server" CssClass="inputControl" ID="chkPostToSavings" Checked="false" />
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtBalance" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Add Fees to Principal
            </div>
            <div class="subFormInput">
                <asp:CheckBox runat="server" ID="chkAddFees" CssClass="inputControl"
                    ToolTip="Tick if you want to add the processing fees to the principal amount"/>
            </div>
            <div class="subFormLabel">
                Amount Approved
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" 
                    ID="txtAmountApproved" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Amount Disbursed So Far
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtDisbursed" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnDisburse" Text="Disburse Loan Amount" runat="server" OnClick="btnDisburse_Click" 
                    ToolTip="Click to disburse the loan. Your till must be posted by the administrator before it will reflect on the client account"></telerik:RadButton>
    </div>  
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="973px">
        <Tabs>
            <telerik:RadTab runat="server" Text="Loan Guarantors" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Collaterals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Repayment Schedule"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Loan Repayments"> 
            </telerik:RadTab> 
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server"> 
           <telerik:RadGrid ID="gridGuarantor" runat="server" AutoGenerateColumns="false" OnItemCommand="gridGuarantor_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="surName" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="idNo.idNo1" HeaderText="ID No"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn HeaderText="Picture" ResizeMode="Fit" DataField="image.image1" ImageWidth="48px" ImageHeight="48px"></telerik:GridBinaryImageColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Guarantor" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Guarantor" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server"> 
            <telerik:RadGrid ID="gridCollateral" runat="server" AutoGenerateColumns="false" OnItemCommand="gridCollateral_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="collateralType.collateralTypeName" HeaderText="Collateral Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="fairValue" HeaderText="Fair Value"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Collateral" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Collateral" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView3" runat="server">             
            <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                        <telerik:GridNumericColumn DataField="balanceCD" HeaderText="Principal C/D"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Remaining" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Remaining" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">             
            <telerik:RadGrid ID="gridRepayment" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="repaymentType.repaymentTypeName" HeaderText="Payment Type" ></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="amountPaid" HeaderText="Amount Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalPaid" HeaderText="Principal Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPaid" HeaderText="Interest Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="feePaid" HeaderText="Fee Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="penaltyPaid" HeaderText="Penalty Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="commission_paid" HeaderText="Commission Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
