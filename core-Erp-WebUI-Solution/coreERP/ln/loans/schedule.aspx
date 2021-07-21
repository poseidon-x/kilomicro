<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="schedule.aspx.cs" Inherits="coreERP.ln.loans.schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Cashier's Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Cashier's Home</h3>
   <table>
        <tr>
            <td>Select Client</td>
            <td >
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr> 
        <tr>
            <td>Select Loan</td>
            <td >
                <telerik:RadComboBox ID="cboLoan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboLoan_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" EmptyMessage="Type name or account number of client" Width="300px" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </td>
        </tr>
       <tr><td>&nbsp;</td></tr> 
       <tr><td>&nbsp;</td></tr> 
       <tr><td>&nbsp;</td></tr> 
       <tr>
           <td colspan="4">           
            <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="1000px" DataSourceID="scheduleDS"
                OnUpdateCommand="gridSchedule_UpdateCommand">
                <MasterTableView ShowFooter="true" DataKeyNames="repaymentScheduleID">
                    <Columns>
                        <telerik:GridDateTimeColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridDateTimeColumn>
                        <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" DataFormatString="{0:#,###.#0}" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                        <telerik:GridNumericColumn ReadOnly="true" DataField="balanceCD" HeaderText="Principal C/D" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn ReadOnly="true" DataField="interestBalance" HeaderText="Interest Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn ReadOnly="true" DataField="principalBalance" HeaderText="Principal Remaining" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridEditCommandColumn  UniqueName="EditCommandColumn" ButtonType="ImageButton" EditImageUrl="~/images/edit.jpg" ItemStyle-Width="32px" ItemStyle-Height="32px" >
                        </telerik:GridEditCommandColumn> 
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
           </td>
       </tr>
   </table>  
    <ef:EntityDataSource ID="scheduleDS" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="False" EnableUpdate="True" 
        EntitySetName="repaymentSchedules" Where="it.loanID=@loanID">
        <WhereParameters>
            <asp:Parameter Name="loanID" DbType="Int32" />
        </WhereParameters>
    </ef:EntityDataSource>  
</asp:Content>
