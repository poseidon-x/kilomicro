<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="payrollLoanEnquiry2.aspx.cs" Inherits="coreERP.ln.loans.payrollLoanEnquiry2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Payroll Loans Enquiry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Payroll Loans Enquiry</h3>
       
            <table>  
                <tr>
                    <td style="width:150px">
                        Net Salary:
                    </td>
                    <td style="width:200px">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtNetSalary"  value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"></telerik:RadNumericTextBox>
                    </td>   
                    <td style="width:150px">
                        Third Party Deductions:
                    </td>
                    <td style="width:200px">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDeductions" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>  
                    <td style="width:150px">
                        Deductions Not on P/R:
                    </td>
                    <td style="width:200px">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDedNotOnPR" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"></telerik:RadNumericTextBox>
                    </td>
                    <td style="width:150px">
                        AMD:
                    </td>
                    <td style="width:200px">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" value="0" ID="txtAMD" runat="server" Enabled="false"></telerik:RadNumericTextBox>
                    </td>  
                </tr> 
                <tr>
                    <td> 
                    </td>
                    <td>
                        <telerik:RadButton ID="btnRecalc" runat="server" Text="Recalculate" OnClick="btnRecalculate_Click"></telerik:RadButton>  
                        <telerik:RadButton ID="btnReport" runat="server" Text="Report" OnClick="btnReport_Click"></telerik:RadButton>   
                    </td> 
                </tr> 
            </table> 
            <telerik:RadGrid ID="gridSchedule" runat="server" AutoGenerateColumns="false" Width="800px"
                OnNeedDataSource ="gridSchedule_NeedDataSource"
                OnDetailTableDataBind="gridSchedule_DetailTableDataBind" AllowSorting="true" >
                <MasterTableView DataKeyNames="ProductID">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ProductName" HeaderText="Product Name"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="tenure" HeaderText="Product Tenure" ></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="qualifiedAmount" HeaderText="Min. Qualified Principal" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="qualifiedAmountMax" HeaderText="Max. Qualified Principal" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="monthlyDeduction" HeaderText="Min. Monthly Deduction" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="processingFee" HeaderText="Proc. Fee" DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="netLoanAmount" HeaderText="Net Loan Amt." DataFormatString="{0:#,##0.#0}"></telerik:GridNumericColumn>
                    </Columns>
                    <DetailTables>
                       <telerik:GridTableView  ShowFooter="true" DataKeyNames="repaymentScheduleID">  
                            <Columns>
                                <telerik:GridBoundColumn DataField="repaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="principalPayment" HeaderText="Principal Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                                <telerik:GridNumericColumn DataField="interestPayment" HeaderText="Interest Payment" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                                <telerik:GridCalculatedColumn Expression="principalPayment+interestPayment" HeaderText="Total Payment" Aggregate="Sum" DataType="System.Double" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridCalculatedColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                </MasterTableView>
            </telerik:RadGrid>
</asp:Content>
