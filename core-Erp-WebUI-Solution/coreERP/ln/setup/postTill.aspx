<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="postTill.aspx.cs" Inherits="coreERP.ln.setup.postTill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Post Cashiers Till
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Post Cashier's  Till</h3>
    <br />
    <table>
        <tr>
            <td>Start Date:</td>
            <td><telerik:RadDatePicker ID="dtStartDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" 
                AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"></telerik:RadDatePicker></td>
            <td>End Date:</td>
            <td><telerik:RadDatePicker ID="dtEndDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" 
                AutoPostBack="true" OnSelectedDateChanged="dtDate_SelectedDateChanged"></telerik:RadDatePicker></td>
        </tr>
        <tr>
            <td>Cashier:</td>
            <td><telerik:RadComboBox runat="server" Width="300px" DropDownAutoWidth="Enabled" MarkFirstMatch="true"  AutoPostBack="true"
                 AutoCompleteSeparator=" " ID="cboUserName" OnSelectedIndexChanged="cboUserName_SelectedIndexChanged"></telerik:RadComboBox></td>
            <td>
                Field Agent (Staff):
            </td>
            <td>
                <telerik:RadComboBox runat="server" CssClass="inputControl" DropDownAutoWidth="Enabled" MarkFirstMatch="true"
                    AutoCompleteSeparator=" " ID="cboFieldAgent" AutoPostBack="true" OnSelectedIndexChanged="cboFieldAgent_SelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr runat="server" id="trDisb" visible="false">
            <td colspan="2">Loan Account Disbursements</td>
        </tr>
        <tr runat="server" id="trDisb1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridDisbursment" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                   OnDeleteCommand="gridDisbursment_DeleteCommand" OnLoad="gridDisbursment_Load"
                  OnPageIndexChanged="gridDisbursment_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="cashierDisbursementID" AllowPaging="true" PageSize="1000" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" >
                        <Columns>
                            <telerik:GridClientSelectColumn  UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="txDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="loan.loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="bankID" HeaderText="Bank">
                                <ItemTemplate>
                                    <%# GetBankName(Eval("bankID")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridNumericColumn DataField="amount" HeaderText="Amount Disbursed" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected disbursement?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" >
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trRcpt" visible="false">
            <td colspan="2">Loan Account Repayments</td>
        </tr>
        <tr runat="server" id="trRcpt1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridReceipt" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridReceipt_DeleteCommand" OnLoad="gridReceipt_Load"
                 OnPageIndexChanged="gridReceipt_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="cashierReceiptID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns> 
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="txDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="loan.loanNo" HeaderText="Loan ID"></telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="bankID" HeaderText="Bank">
                                <ItemTemplate>
                                    <%# GetBankName(Eval("bankID")) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridNumericColumn DataField="amount" HeaderText="Amount Paid" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected receipt?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trCSC" visible="false">
            <td colspan="2">Client Charges</td>
        </tr>
        <tr runat="server" id="trCSC1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridCSC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridCSC_DeleteCommand" OnLoad="gridCSC_Load"
                 OnPageIndexChanged="gridDA_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="clientServiceChargeId" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="chargeDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                           <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="A/C No."></telerik:GridBoundColumn>
                             <telerik:GridNumericColumn DataField="chargeAmount" HeaderText="Amount Charged" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected deposit?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        
        
        
        

        <tr runat="server" id="trDA" visible="false">
            <td colspan="2">Deposits (Client Investments)</td>
        </tr>
        <tr runat="server" id="trDA1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridDA" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridDA_DeleteCommand" OnLoad="gridDA_Load"
                 OnPageIndexChanged="gridDA_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="depositAdditionalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="deposit.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="depositDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                           <telerik:GridBoundColumn DataField="deposit.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.depositNo" HeaderText="Deposit ID"></telerik:GridBoundColumn> 
                             <telerik:GridNumericColumn DataField="depositAmount" HeaderText="Amount Deposited" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected deposit?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trDW" visible="false">
            <td colspan="2">Withdrawals (Client Investments)</td>
        </tr>
        <tr runat="server" id="trDW1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridDW" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridDW_DeleteCommand" OnLoad="gridDW_Load"
                     OnPageIndexChanged="gridDW_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="depositWithdrawalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="deposit.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="deposit.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.depositNo" HeaderText="Deposit ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                            <telerik:GridNumericColumn DataField="principalWithdrawal" HeaderText="Principal Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected withdrawal?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trDWC" visible="false">
            <td colspan="2">Premature Withdrawal Charges (Client Investments)</td>
        </tr>
        <tr runat="server" id="trDWC1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridDWC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridDWC_DeleteCommand" OnLoad="gridDWC_Load"
                     OnPageIndexChanged="gridDWC_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="depositWithdrawalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="deposit.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="deposit.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="deposit.depositNo" HeaderText="Deposit ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                            <telerik:GridNumericColumn DataField="disInvestmentCharge" HeaderText="PreMature Invest Charge" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected withdrawal Charge?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trSA" visible="false">
            <td colspan="2">Deposits (Regular Deposit Accounts)</td>
        </tr>
        <tr runat="server" id="trSA1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridSA" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridSA_DeleteCommand" OnLoad="gridSA_Load"
                 OnPageIndexChanged="gridSA_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="savingAdditionalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="saving.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="savingDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="saving.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="saving.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="saving.savingNo" HeaderText="Savings ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="savingAmount" HeaderText="Amount Deposited" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected deposit?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trSW" visible="false">
            <td colspan="2">Withdrawals (Regular Deposit Accounts)</td>
        </tr>
        <tr runat="server" id="trSW1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridSW" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridSW_DeleteCommand" OnLoad="gridSW_Load"
                 OnPageIndexChanged="gridSW_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="savingWithdrawalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="saving.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="saving.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="saving.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="saving.savingNo" HeaderText="Savings ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                            <telerik:GridNumericColumn DataField="principalWithdrawal" HeaderText="Principal Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected withdrawal?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trIA" visible="false">
            <td colspan="2">Deposits (Investment Accounts)</td>
        </tr>
        <tr runat="server" id="trIA1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridIA" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridIA_DeleteCommand" OnLoad="gridIA_Load"
                 OnPageIndexChanged="gridIA_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="investmentAdditionalID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="investment.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="investmentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="investment.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="investment.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="investment.savingNo" HeaderText="Investment ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="investmentAmount" HeaderText="Amount Invested" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected deposit?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trIW" visible="false">
            <td colspan="2">Withdrawals (Investment Accounts)</td>
        </tr>
        <tr runat="server" id="trIW1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridIW" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridIW_DeleteCommand" OnLoad="gridSW_Load"
                 OnPageIndexChanged="gridIW_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="investmentWithdrawalID" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="investment.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="investment.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="investment.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="investment.savingNo" HeaderText="Investment ID"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                            <telerik:GridNumericColumn DataField="principalWithdrawal" HeaderText="Principal Withdrawn" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected withdrawal?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trSC" visible="false">
            <td colspan="2">Group Susu Account Contributions</td>
        </tr>
        <tr runat="server" id="trSC1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridSC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridSC_DeleteCommand" OnLoad="gridSC_Load"
                 OnPageIndexChanged="gridSC_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="susuContributionID" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="susuAccount.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="contributionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="susuAccount.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="susuAccount.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="susuAccount.susuAccountNo" HeaderText="Susu Account No"></telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="contributionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="Amount" HeaderText="Amount Contributed" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>                 
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected contribution?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trSU" visible="false">
            <td colspan="2">Group Susu Account Disbursments</td>
        </tr>
        <tr runat="server" id="trSU1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridSU" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridSU_DeleteCommand" OnLoad="gridSU_Load"
                 OnPageIndexChanged="gridSU_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="susuAccountID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="disbursementDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="susuAccountNo" HeaderText="Susu Account No"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="netAmountEntitled" HeaderText="Disbursement Amount" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected disbursement?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trRSC" visible="false">
            <td colspan="2">Normal Susu Account Contributions</td>
        </tr>
        <tr runat="server" id="trRSC1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridRSC" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridRSC_DeleteCommand" OnLoad="gridRSC_Load"
                 OnPageIndexChanged="gridRSC_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="regularSusuContributionID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="regularSusuAccount.client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="contributionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="regularSusuAccount.client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="regularSusuAccount.client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="regularSusuAccount.regularSusuAccountNo" HeaderText="Normal Susu Account No"></telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="contributionDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="Amount" HeaderText="Amount Contributed" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>                 
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected regular contribution?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr runat="server" id="trRSU" visible="false">
            <td colspan="2">Normal Susu Account Disbursments</td>
        </tr>
        <tr runat="server" id="trRSU1" visible="false">
            <td colspan="2">
                <telerik:RadGrid ID="gridRSU" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnDeleteCommand="gridRSU_DeleteCommand" OnLoad="gridRSU_Load"
                 OnPageIndexChanged="gridRSU_PageIndexChanged"  AllowMultiRowSelection="true"
                   AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced" PageSize="1000">
                    <MasterTableView datakeynames="regularSusuAccountID" AllowPaging="true" PagerStyle-Mode="NextPrevNumericAndAdvanced"
                         PagerStyle-Position="TopAndBottom" PageSize="1000"> 
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn"></telerik:GridClientSelectColumn>
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>
                            <telerik:GridDateTimeColumn DataField="disbursementDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="client.surName" HeaderText="Surname"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="regularSusuAccountNo" HeaderText="Normal Susu Account No"></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="netAmountEntitled" HeaderText="Regular Disbursement Amount" DataFormatString="{0:#,###.#0}"
                                Aggregate="Sum" FooterAggregateFormatString="{0:#,###.##}"></telerik:GridNumericColumn>  
                              <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" 
                                ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                ConfirmText="Are you sure you want to delete the selected regular disbursement?">
                              </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting> 
                    </ClientSettings>
                 </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td><telerik:RadButton ID="btnOpen" runat="server" Text="Post Till" OnClick="btnOpen_Click" Enabled="false"></telerik:RadButton></td> 
        </tr>
    </table>
</asp:Content>
