<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="applyToLoan.aspx.cs" Inherits="coreERP.ln.loans.applyToLoan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Receipts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loans Receipts</h3>
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
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Loan ID:
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboLoan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLoan_SelectedIndexChanged"
                        DropDownWidth="355px" EmptyMessage="Select a Loan" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" runat="server" CssClass="inputControl"
                    ToolTip="Select the type of payment made by client paid"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Received Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountPaid" runat="server" CssClass="inputControl" Enabled="false"
                    ToolTip="Enter the amount paid by the client"></telerik:RadNumericTextBox>
            </div>  
        </div> 
        <div class="subFormColumnRight"> 
            <div class="subFormLabel">
                Payment Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtMntDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" DateInput-MinDate="1/1/1900 12:00:00 AM"
                        DateInput-ToolTip="Select the date on which the client made the payment" Enabled="false"></telerik:RadDatePicker>
            </div> 
            <div class="subFormLabel">
               Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtBalance" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div> 
        </div> 
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnReceive" Text="Receive Repayment Amount" runat="server" OnClick="btnReceive_Click" Enabled="false"
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
