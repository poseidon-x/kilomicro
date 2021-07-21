<%@ Page Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="coreERP.hc.loans.post" EnableEventValidation="false"  %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loans Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Post Loans</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="132" Height="132" runat="server" ID="rotator1" ItemHeight="132" ItemWidth="132" 
                    FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Staff No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
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
            <div class="subFormLabel">
                Staff Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboStaff" runat="server" AutoPostBack="true" Enabled="false"
                    OnSelectedIndexChanged="cboStaff_SelectedIndexChanged" CssClass="inputControl"
                        DropDownWidth="300px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Loan Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtAmount" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application amount for the loan"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Loan Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dtAppDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the application date for the loan"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtPrincBalance" runat="server" 
                    CssClass="inputControl" Enabled="false"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtInterestBalance" 
                    runat="server" CssClass="inputControl" Enabled="false"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Loan Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboLoanType" CssClass="inputControl" runat="server" Enabled="false"></telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboLoanType" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please select the type of the loan"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Deduction Starts Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtpDeductionStartsDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dtpDeductionStartsDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the deduction starts date for the loan"></asp:RequiredFieldValidator>
            </div>
            <div class="subFormLabel">
                Attracts Interest
            </div>
            <div class="subFormInput">
               <asp:CheckBox runat="server" ID="chkAttractsInterest" CssClass="inputControl" Enabled="false" />
            </div>
            <div class="subFormLabel">
                Interest Rate
            </div>
            <div class="subFormInput">
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtInterestRate" runat="server" 
                   CssClass="inputControl" Value="0" Enabled="false"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Approval Date
            </div>
            <div class="subFormInput">
               <telerik:RadDatePicker ID="dtApprovalDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="dtApprovalDate" ErrorMessage="!"
                         ForeColor="Red" Font-Bold="true" ToolTip="Please enter the approval date for the loan"></asp:RequiredFieldValidator>
            </div> 
            <div class="subFormLabel">
                Check No
            </div>
            <div class="subFormInput">
               <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" CssClass="inputControl" runat="server"></telerik:RadTextBox>
            </div> 
            <div class="subFormLabel">
                Bank
            </div>
            <div class="subFormInput">
              <telerik:RadComboBox ID="cboBank" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div> 
        </div>
    </div>
    <br />
     <div class="subForm">
        <telerik:RadButton ID="btnSave" Text="Post Loan Application" runat="server" OnClick="btnSave_Click"></telerik:RadButton> 
    </div>
    <br />    
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="1224px">
        <Tabs> 
            <telerik:RadTab runat="server" Text="Memo / Notes" Selected="true"> 
            </telerik:RadTab>    
            <telerik:RadTab runat="server" Text="Deduction Schedule"> 
            </telerik:RadTab>  
            <telerik:RadTab runat="server" Text="Loan Deductions"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="0"> 
        <telerik:RadPageView runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCNotes" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView>         
        <telerik:RadPageView ID="RadPageView3" runat="server">             
            <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="deductionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="principalDeduction" HeaderText="Principal Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestDeduction" HeaderText="Interest Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridCalculatedColumn Expression="principalDeduction+interestDeduction" HeaderText="Total Deduction" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                        <telerik:GridNumericColumn DataField="balanceAfter" HeaderText="Principal C/D" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">             
            <telerik:RadGrid ID="gridRepayment" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="principalPaid" HeaderText="Principal Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPaid" HeaderText="Interest Paid" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="balanceAfter" HeaderText="Balance After" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView> 
    </telerik:RadMultiPage>
</asp:Content>
