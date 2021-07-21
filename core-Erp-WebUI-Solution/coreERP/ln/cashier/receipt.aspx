<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="receipt.aspx.cs" Inherits="coreERP.ln.cashier.receipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Receipts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Receipts</h3>
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
                Received Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountPaid" runat="server" CssClass="inputControl"
                    ToolTip="Enter the amount paid by the client"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" runat="server" CssClass="inputControl"
                    ToolTip="Select the type of payment made by client paid"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Payment Mode
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboPaymentMode" runat="server" CssClass="inputControl"
                        ToolTip="Select the method by which the client paid"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Check No
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" CssClass="inputControl"
                        ToolTip="Enter the check presented by the client (if applicable)"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Payment Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtMntDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" DateInput-MinDate="1/1/1900 12:00:00 AM"
                        DateInput-ToolTip="Select the date on which the client made the payment"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
               Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"
                    ToolTip="Select the bank account to which the client paid (if applicable)"></telerik:RadComboBox>
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
               Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtInterestBalnace" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel" id="lblPenalty">
               Penalty Balance
            </div>
            <div class="subFormInput" id="lblTxtPenalty">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtPenalty" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
               Proccessing Fee
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtProcessingFee" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div> 
            <div class="subFormLabel">
               Application Fee
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtApplicationFee" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel" id="lblInsurance">
               Insurance 
            </div>
            <div class="subFormInput" >
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtInsurance" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnReceive" Text="Receive Repayment Amount" runat="server" OnClick="btnReceive_Click"
                    ToolTip="Click to process the payment. Your till must be posted by the administrator before it will reflect on the client account."></telerik:RadButton>
    </div> 
    <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px">
        <MasterTableView ShowFooter="true">
            <Columns>
                <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                <telerik:GridNumericColumn DataField="balanceCD" HeaderText="Principal C/D" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="proposedInterestWriteOff" HeaderText="Prop. Int. Write-Off" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestWritenOff" HeaderText="Interest Written-Off" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
