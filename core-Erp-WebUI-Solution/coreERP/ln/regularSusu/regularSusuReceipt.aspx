<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="regularSusuReceipt.aspx.cs" Inherits="coreERP.ln.susu.regularSusuReceipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Normal Susu Account Receipts
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Normal Susu Account Receipts</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Payment Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtMntDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" DateInput-MinDate="1/1/1900 12:00:00 AM"
                        DateInput-ToolTip="Select the date on which the client made the payment"
                     AutoPostBack="true" DateInput-CausesValidation="false" OnSelectedDateChanged="dtMntDate_SelectedDateChanged"></telerik:RadDatePicker>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Cashier Name
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblCashier"></asp:Label>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Cashier's Till Status
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblCashierStatus"></asp:Label>
            </div>
        </div>
    </div>
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
                Client Account N<u>o</u>:
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblLoanID" Text=""></asp:Label>
            </div>
        </div>
        <div class="subFormColumnMiddle">       
            <div class="subFormLabel">
                    Selected Client
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client"  
                MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false"
                        EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"
                    ToolTip="Select the  client whose application is being entered"></telerik:RadComboBox>
            </div>        
            <div class="subFormLabel">
                    Selected Susu Account
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboAccount" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboAccount_SelectedIndexChanged" CssClass="inputControl"
                    DropDownAutoWidth="Enabled" EmptyMessage="Type account number"  
                    MarkFirstMatch="true" AutoCompleteSeparator="" CausesValidation="false" 
                    ToolTip="Select the  Normal Susu account for which payment is being entered"></telerik:RadComboBox>
            </div>  
            <div class="subFormLabel">
                Received Amount
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountPaid" runat="server" CssClass="inputControl"
                    ToolTip="Enter the amount paid by the client"></telerik:RadNumericTextBox>
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
               Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"
                    ToolTip="Select the bank account to which the client paid (if applicable)"></telerik:RadComboBox>
            </div>                     
            <div class="subFormLabel">
                Narration
            </div>
            <div class="subFormInput">
                <asp:TextBox runat="server" ID="txtNarration" TextMode="MultiLine" Rows="2" MaxLength="255"></asp:TextBox>
            </div>                           
                <div class="subFormLabel">
                    Collected By (Staff)
                </div>
               <div class="subFormInput">
                    <telerik:RadComboBox ID="cboStaff" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select a Staff" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the staff who collected this contribution"></telerik:RadComboBox>
                </div> 
                <div class="subFormLabel">
                    Collected By (Agent)
                </div>
                <div class="subFormInput">
                    <telerik:RadComboBox ID="cboAgent" runat="server" CssClass="inputControl"
                        DropDownWidth="355px" EmptyMessage="Select an Agent" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select the agent who collected this contribution"></telerik:RadComboBox>
                </div> 
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                     Contribution Amount
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2" ID="txtContribAmount" Enabled="false"></telerik:RadNumericTextBox>
                </div>  
                <div class="subFormLabel">
                     Remaining to be Contributed
                </div>   
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" 
                        NumberFormat-DecimalDigits="2" ID="txtRemainder" Enabled="false"></telerik:RadNumericTextBox>
                </div> 
                <div class="subFormLabel">
                     Amount Contributed
                </div> 
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="inputControl" 
                        NumberFormat-DecimalDigits="2" ID="txtAmountContributed" Enabled="false"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                     Date Started
                </div> 
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtStarted" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>
                <div class="subFormLabel">
                    Date Approved
                </div> 
                <div class="subFormInput">
                    <telerik:RadDatePicker runat="server" CssClass="inputControl" ID="dtApproved" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>
        </div>
    </div>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <telerik:RadButton ID="btnReceive" Text="Receive Normal Susu Contribution" runat="server" OnClick="btnReceive_Click"
                    ToolTip="Click to process the payment. Your till must be posted by the administrator before it will reflect on the client account."></telerik:RadButton>
        </div>
        <div class="subFormColumnMiddle">
            <telerik:RadButton ID="btnSave" Text="Save Received Contributions Below" runat="server" OnClick="btnSave_Click"
                    ToolTip="Click to process the payment. Your till must be posted by the administrator before it will reflect on the client account."></telerik:RadButton>
        </div>        
    </div> 
    <telerik:RadGrid ID="gridContrib" runat="server" AutoGenerateColumns="false" Width="1000px">
        <MasterTableView ShowFooter="true">
            <Columns>
                <telerik:GridImageColumn DataImageUrlFields="regularSusuAccount.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                    ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                <telerik:GridBoundColumn DataField="regularSusuAccount.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="regularSusuAccount.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="regularSusuAccount.regularSusuAccountNo" HeaderText="Normal Susu Account No"></telerik:GridBoundColumn> 
                <telerik:GridBoundColumn DataField="contributionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="Amount" HeaderText="Amount Contributed" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>                
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
