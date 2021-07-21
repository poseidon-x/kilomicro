<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="default3.aspx.cs" Inherits="coreERP.ln.cashier._default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Cashier's Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cashier's Home</h3>
   <table>
        <tr>
            <td colspan="7">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested" 
                    LoadingMessage="Loading client data: type name or account number"
                    ToolTip="Begin typing any of the surname, other names or account number of the client to process" ></telerik:RadComboBox>
            </td>
        </tr>
       <tr> 
           <td style="height:30px">Surname</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" Width="150"
                    ToolTip="Enter the surname of the client to search by it"></telerik:RadTextBox></td>
           <td style="height:30px">Other Names</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" Width="150"
                    ToolTip="Enter the other names of the client to search by it"></telerik:RadTextBox></td> 
           <td style="height:30px">Account No.</td>
           <td style="height:30px"><telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccNo" runat="server" Width="100"
                    ToolTip="Enter the account number of the client to search by it"></telerik:RadTextBox></td> 
           <td style="height:30px"><telerik:RadButton ID="btnFind" runat="server" Text="Find Client" OnClick="btnFind_Click"
                    ToolTip="Click to search for the client with the details entered"></telerik:RadButton></td>
       </tr>
       <tr runat="server" id="trLoan">
           <td colspan="7">
               <telerik:RadGrid ID="grid" runat="server" AutoGenerateColumns="false" OnDetailTableDataBind="grid_DetailTableDataBind"
                    OnItemCreated="grid_ItemCreated" OnItemDataBound="grid_ItemDataBound">
                   <MasterTableView DataKeyNames="loanID">
                       <Columns>      
                           <telerik:GridImageColumn UniqueName="" DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                  
                            <telerik:GridHyperLinkColumn DataTextField="loanNo"  HeaderText="Disburse Loan" UniqueName="Disburse"
                                DataNavigateUrlFields="loanID,client.categoryID" DataNavigateUrlFormatString="/ln/cashier/disburse.aspx?id={0}&catID={1}"></telerik:GridHyperLinkColumn> 
                            <telerik:GridHyperLinkColumn UniqueName="Receipt" DataTextField="loanNo"  HeaderText="Receive Repayment"
                                DataNavigateUrlFields="loanID,client.categoryID" DataNavigateUrlFormatString="/ln/cashier/receipt.aspx?id={0}&catID={1}"></telerik:GridHyperLinkColumn> 
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="loanType.loanTypeName" HeaderText="Product"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="applicationDate" HeaderText="Application Date"  DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="finalApprovalDate" HeaderText="Approval Date"  DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn> 
                            <telerik:GridBoundColumn DataField="disbursementDate" HeaderText="Disbursement Date"  DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn> 
                            <telerik:GridNumericColumn DataField="amountRequested" HeaderText="Amount Requested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="amountApproved" HeaderText="Amount Approved"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="amountDisbursed" HeaderText="Amount Disbursed"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
       <tr><td colspan="7">&nbsp;</td></tr> 
       <tr runat="server" id="trDeposit">
           <td colspan="7">
               <telerik:RadGrid ID="grid2" runat="server" AutoGenerateColumns="false" OnItemDataBound="grid2_ItemDataBound">
                   <MasterTableView DataKeyNames="client.clientID, depositID">
                       <Columns> 
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                         
                            <telerik:GridHyperLinkColumn DataTextField="depositNo"  HeaderText="Additional Deposit" 
                                DataNavigateUrlFields="depositID" DataNavigateUrlFormatString="/ln/deposit/add.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                            <telerik:GridHyperLinkColumn DataTextField="depositNo"  HeaderText="Withdraw from Deposit" 
                                DataNavigateUrlFields="depositID" DataNavigateUrlFormatString="/ln/deposit/with.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="depositType.depositTypeName" HeaderText="Product"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="firstDepositDate" HeaderText="First Deposit" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                           <telerik:GridNumericColumn DataField="amountInvested" HeaderText="Amount Invested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
       <tr><td colspan="7">&nbsp;</td></tr> 
       <tr runat="server" id="trSavings">
           <td colspan="7">
               <telerik:RadGrid ID="grid4" runat="server" AutoGenerateColumns="false" OnItemDataBound="grid4_ItemDataBound">
                   <MasterTableView DataKeyNames="client.clientID, savingID">
                       <Columns>   
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                       
                            <telerik:GridHyperLinkColumn DataTextField="savingNo"  HeaderText="Make Deposit" 
                                DataNavigateUrlFields="savingID" DataNavigateUrlFormatString="/ln/saving/add.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                            <telerik:GridHyperLinkColumn DataTextField="savingNo"  HeaderText="Withdraw from Savings" 
                                DataNavigateUrlFields="savingID" DataNavigateUrlFormatString="/ln/saving/with.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="savingType.savingTypeName" HeaderText="Product"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="firstSavingDate" HeaderText="First Deposit" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                           <telerik:GridNumericColumn DataField="amountInvested" HeaderText="Amount Invested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
       <tr><td colspan="7">&nbsp;</td></tr> 
       <tr runat="server" id="trInvestment">
           <td colspan="7">
               <telerik:RadGrid ID="grid3" runat="server" AutoGenerateColumns="false" OnItemDataBound="grid3_ItemDataBound">
                   <MasterTableView DataKeyNames="client.clientID,investmentID">
                       <Columns>   
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                       
                            <telerik:GridHyperLinkColumn DataTextField="investmentNo"  HeaderText="Additional investment" 
                                DataNavigateUrlFields="investmentID" DataNavigateUrlFormatString="/ln/investment/add.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                            <telerik:GridHyperLinkColumn DataTextField="investmentNo"  HeaderText="Withdraw from investment" 
                                DataNavigateUrlFields="investmentID" DataNavigateUrlFormatString="/ln/investment/with.aspx?id={0}"></telerik:GridHyperLinkColumn> 
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="firstinvestmentDate" HeaderText="First investment" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                           <telerik:GridNumericColumn DataField="amountInvested" HeaderText="Amount Invested"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="principalBalance" HeaderText="Principal Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="interestBalance" HeaderText="Interest Balance"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
       <tr><td colspan="7">&nbsp;</td></tr> 
       <tr runat="server" id="trSusuAccount">
           <td colspan="7">
               <telerik:RadGrid ID="grid5" runat="server" AutoGenerateColumns="false" OnItemDataBound="grid5_ItemDataBound">
                   <MasterTableView DataKeyNames="client.clientID, susuAccountID">
                       <Columns>      
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                     
                            <telerik:GridHyperLinkColumn DataTextField="susuAccountNo"  HeaderText="Disburse Susu Account"
                                DataNavigateUrlFields="susuAccountID,client.categoryID" DataNavigateUrlFormatString="/ln/susu/disburse.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="amountEntitled" HeaderText="Amount Entitled"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="netAmountEntitled" HeaderText="Net Amount Entitled"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>  
                            <telerik:GridDateTimeColumn DataField="applicationDate" HeaderText="Date Applied"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                            <telerik:GridDateTimeColumn DataField="approvalDate" HeaderText="Date Approved"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>  
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
       <tr><td colspan="7">&nbsp;</td></tr> 
       <tr runat="server" id="trRegularSusu">
           <td colspan="7">
               <telerik:RadGrid ID="grid6" runat="server" AutoGenerateColumns="false" OnItemDataBound="grid6_ItemDataBound">
                   <MasterTableView DataKeyNames="client.clientID, regularSusuAccountID">
                       <Columns>      
                           <telerik:GridImageColumn DataImageUrlFields="client.clientID" DataImageUrlFormatString="/ln/loans/image.aspx?cid={0}&w=48&h=48" HeaderText="P"
                                ImageWidth="48" ImageHeight="48" ItemStyle-Width="48"></telerik:GridImageColumn>                                     
                            <telerik:GridHyperLinkColumn DataTextField="regularSusuAccountNo"  HeaderText="Disburse Normal Susu Account"
                                DataNavigateUrlFields="regularSusuAccountID,client.categoryID" DataNavigateUrlFormatString="/ln/regularSusu/regularSusuDisburse.aspx?id={0}&amp;catID={1}"></telerik:GridHyperLinkColumn>
                           <telerik:GridBoundColumn DataField="client.accountNumber" HeaderText="Acc. Num"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.surname" HeaderText="Surname"></telerik:GridBoundColumn>
                           <telerik:GridBoundColumn DataField="client.otherNames" HeaderText="Other Names"></telerik:GridBoundColumn>
                            <telerik:GridNumericColumn DataField="amountEntitled" HeaderText="Amount Entitled"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn> 
                            <telerik:GridNumericColumn DataField="netAmountEntitled" HeaderText="Net Amount Entitled"  DataFormatString="{0:#,###.00}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>  
                            <telerik:GridDateTimeColumn DataField="applicationDate" HeaderText="Date Applied"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn> 
                            <telerik:GridDateTimeColumn DataField="approvalDate" HeaderText="Date Approved"  DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>  
                        </Columns>  
                   </MasterTableView>
               </telerik:RadGrid>
           </td>
       </tr>
   </table>    
</asp:Content>
