<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="approve.aspx.cs" Inherits="coreERP.ln.loans.approve" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Approval
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Loan Approval</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" 
                    runat="server" ID="rotator1" ItemHeight="216" ItemWidth="216" 
                    FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnMiddle"> 
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
                Amount Approved
            </div>
            <div class="subFormInput">
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountApproved" runat="server" CssClass="inputControl"
                        AutoPostback="true" OnTextChanged="txt_TextChanged"></telerik:RadNumericTextBox>
                    <asp:Label runat="server" ID="lblInvalidAmt" Text="!" ForeColor="Red" Font-Size="Larger"></asp:Label>
            </div>
            <div class="subFormLabel">
                Loan Approval Date
            </div>
            <div class="subFormInput">
               <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Interest Rate
            </div>
            <div class="subFormInput">
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRate" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                &nbsp;per Month
            </div>
            <div class="subFormLabel">
                Tenure of Loan
            </div>
            <div class="subFormInput">
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTenure" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                &nbsp;Months
            </div>
           <div class="subFormLabel">
                    Processing Fee
           </div> 
           <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtProcFee" runat="server" CssClass="inputControl" Value="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
            </div> 
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Loan ID:
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblLoanID" Text=""></asp:Label>
            </div>
            <div class="subFormLabel">
                Amount Requested
            </div>
            <div class="subFormInput">
               <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtAmountRequested" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <asp:Panel ID="pnlMD" runat="server" Visible="false">
                <div class="subFormLabel">
                    Monthly Deduction
                </div>
                <div class="subFormInput">
                    <asp:Label ID="lblMD" CssClass="inputControl" runat="server"></asp:Label> 
                 </div> 
            </asp:Panel>
           <div class="subFormLabel">
                    Loan Type
           </div> 
           <div class="subFormInput">
                <telerik:RadComboBox ReadOnly="true" id="cboLoanType" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                <telerik:RadComboBox id="cboLoanProduct" runat="server" visible="false" AutoPostBack="true" 
                    OnSelectedIndexChanged="cboLoanProduct_SelectedIndexChanged" Width="300"></telerik:RadComboBox>
           </div> 
            <div class="subFormLabel">
                 Add Fees to Principal
            </div>
            <div class="subFormInput">
               <asp:CheckBox runat="server" CssClass="inputControl" ID="chkAddFees" />
            </div>
            <asp:Panel ID="pnlFees" runat="server"> 
                  <div class="subFormLabel"> 
                      Interest Calculation
                  </div>
                  <div class="subFormInput">
                        <telerik:RadComboBox id="cboInterestType" CssClass="inputControl" runat="server"></telerik:RadComboBox>
                   </div> 
                   <div class="subFormLabel">
                       Repayment Mode
                    </div>
                    <div class="subFormInput">
                           <telerik:RadComboBox id="cboRepaymentMode" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                     </div> 
                </asp:Panel>
                <asp:Panel runat="server" ID="trInsurance" Visible="false">
                    <div class="subFormLabel">
                         Insurance
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtInsurance" runat="server" CssClass="inputControl" 
                            Value="0" NumberFormat-DecimalDigits="2"
                        ToolTip="Insurance on the loan is entered here"></telerik:RadNumericTextBox>
                    </div>
                </asp:Panel>            
                <div class="subFormLabel">
                    Scheme Loan
                </div>      
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboLoanScheme" runat="server" AutoPostBack="true" CausesValidation="false"
                    CssClass="inputControl"
                        ToolTip="Select the type of scheme loan"></telerik:RadComboBox>                    
                </div> 
        </div>
    </div> 
    <asp:Repeater ID="rpCheckList" runat="server">
        <HeaderTemplate>
            <table>
                <tr  style="border-bottom: 1px solid #00B4FF; font-weight:bold;">
                    <td style="width:300px; border-bottom: 1px solid #00B4FF; font-weight:bold;">Check List Item</td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF; font-weight:bold;">M?</td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF; font-weight:bold;">P?</td>
                    <td style="width:300px; border-bottom: 1px solid #00B4FF; font-weight:bold;">Comments</td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemTemplate>
            <table>
                <tr>
                    <td style="width:300px; border-bottom: 1px solid #00B4FF">
                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("loanCheckListID") %>' Visible="false">
                        </asp:Label>                                       
                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("description") %>'>
                        </asp:Label>
                    </td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF">
                        <asp:CheckBox ID="chkMandatory" runat="server" Checked='<%# GetMandatory(Eval("categoryCheckList")) %>' Enabled="false" />
                    </td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF">
                        <asp:CheckBox ID="chkPassed" runat="server" Checked='<%# Bind("passed") %>' Enabled="false" />
                    </td>
                    <td style="width:300px; border-bottom: 1px solid #00B4FF">
                        <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("comments") %>' Enabled="false" 
                            Width="300" /></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <div class="subForm">
        <telerik:RadButton ID="btnApprove" Text="Approve Loan Application" runat="server" OnClick="btnApprove_Click"></telerik:RadButton>
                   <telerik:RadButton ID="btnDeny" Text="Deny Loan Application" runat="server" OnClick="btnDeny_Click"></telerik:RadButton>
    </div> 
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Allowances" Visible="false">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Payroll Details" Visible="false">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Guarantors" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Collaterals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Post Dated Checks"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Credit Manager Comments"  
                 ToolTip="Click to view the check list comments for this loan entered by the credit manager"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Approval Comments"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Repayment Schedule"> 
            </telerik:RadTab>  
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server"  ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView10" runat="server">  
            <telerik:RadGrid ID="gridAllowances" ShowFooter="true" runat="server" AutoGenerateColumns="false" >
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="prAllowanceType.allowanceTypeName" HeaderText="Financial Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="amount" HeaderText="Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="prAllowanceType.isPermanent" HeaderText="Permanent?"></telerik:GridCheckBoxColumn>                         
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView9" runat="server"> 
            <div class="subForm">
                <div class="subFormColumnLeft">
                    <div class="subFormLabel">
                         Basic Salary:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtBasicSalary" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the basic salary of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Tax:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTax" value="0" CssClass="inputControl" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the tax deduction the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Net Salary:
                    </div>
                    <div class="subFormInput">
                         <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtNetSalary" CssClass="inputControl"  value="0" runat="server"
                                ToolTip="The net salary of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                         Authority Note No:
                    </div>
                    <div class="subFormInput">
                         <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAuthorityNoteNo" CssClass="inputControl" runat="server"
                                ToolTip="Enter the authority note number here"></telerik:RadTextBox>
                    </div>
                    <div class="subFormLabel">
                          Loan Detail Purpose
                    </div>
                    <div class="subFormInput">
                         <telerik:RadComboBox runat="server" id="cboLoanPurposeDetail" CssClass="inputControl"
                             Height="350px"
                                ToolTip="Select the detail purpose for this loan application"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="subFormColumnMiddle">
                    <div class="subFormLabel">
                        Gross Salary:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtGrossSalary" CssClass="inputControl" runat="server" enabled="false"
                                ToolTip="Enter the gross salary of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Third Party Deductions:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDeductions" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the third party deductions of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        AMD:   
                    </div>                 
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" value="0" ID="txtAMD" CssClass="inputControl" runat="server"
                                ToolTip="The affordable monthly deduction of the employee taking this loan appears here"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Loan Advance No:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtLoanAdvanceNo" CssClass="inputControl" runat="server"
                                ToolTip="Enter the loan advance number here"></telerik:RadTextBox>
                    </div> 
                </div>
                <div class="subFormColumnRight">
                    <div class="subFormLabel">
                        Soc Sec. /Welfare:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtSSWelfare" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the social security and/or welfare deduction of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Deductions Not on Payslip:
                    </div>
                    <div class="subFormInput">
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtTotalDedNotOnPR" CssClass="inputControl" value="0" runat="server" AutoPostBack="true" OnTextChanged="txt_TextChanged"
                                ToolTip="Enter the social security and/or welfare deduction of the employee taking this loan"></telerik:RadNumericTextBox>
                    </div>
                    <div class="subFormLabel">
                        Pre-audit verified
                    </div>
                    <div class="subFormInput">
                        <asp:CheckBox runat="server" ID="chkPreAudit" CssClass="inputControl"
                                ToolTip="Tick if preaudit has been verified for this loan application"/>
                    </div>
                    <div class="subFormLabel">
                         Loan Purpose
                    </div>
                    <div class="subFormInput">
                        <telerik:RadComboBox runat="server" autopostback="true" CssClass="inputControl"
                              id="cboLoanPurpose" Height="350px"
                                ToolTip="Select the purpose for this loan application"></telerik:RadComboBox>
                    </div>
                </div>
         </div> 
        </telerik:RadPageView>
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
            <telerik:RadGrid ID="gridCollateral" runat="server" OnDetailTableDataBind="gridCollateral_DetailTableDataBind" AutoGenerateColumns="false" OnItemCommand="gridCollateral_ItemCommand">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="collateralType.collateralTypeName" HeaderText="Collateral Type"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="fairValue" HeaderText="Fair Value"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Collateral" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" Visible="false" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Collateral" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                    <DetailTables>
                        <telerik:GridTableView DataMember="collateralImages">
                            <Columns>
                                <telerik:GridBinaryImageColumn DataField="image.image1"></telerik:GridBinaryImageColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView7" runat="server"> 
            <table>
                <tr>
                    <td style="width:150px">
                        Bank Name
                    </td>
                    <td style="width:200px">
                        <telerik:RadComboBox ID="cboBank" runat="server"></telerik:RadComboBox>
                    </td> 
                </tr>
                <tr>
                    <td>
                        Check Amount
                    </td>
                    <td>
                        <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtCheckAmount" runat="server"></telerik:RadNumericTextBox>
                    </td> 
                </tr>  
                <tr>
                    <td>
                        Check Number
                    </td>
                    <td>
                        <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server"></telerik:RadTextBox>
                    </td> 
                </tr>  
                <tr>
                    <td>
                        Check Date
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" DateInput-DateFormat="dd-MMM-yyyy" ID="dtCheckDate" ></telerik:RadDatePicker>
                    </td> 
                </tr>   
                <tr>
                    <td> 
                    </td>
                    <td>
                        <telerik:RadButton ID="btnAddCheck" runat="server" Text="Add Check" OnClick="btnAddCheck_Click"></telerik:RadButton>  
                    </td> 
                </tr> 
            </table>
            <telerik:RadGrid ID="gridChecks" ShowFooter="true" runat="server" AutoGenerateColumns="false" OnItemCommand="gridChecks_ItemCommand">
                <MasterTableView>
                    <Columns> 
                        <telerik:GridBoundColumn DataField="checkDate" HeaderText="Check Date" DataFormatString="{0:dd-MMM-yyyy}" ></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="checkAmount" HeaderText="Check Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="checkNumber" HeaderText="Check No" ></telerik:GridBoundColumn>
                        <telerik:GridCheckBoxColumn DataField="cashed" HeaderText="Cashed"></telerik:GridCheckBoxColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Check" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Check" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server"> 
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="document.description" HeaderText="Surname"></telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn DataTextField="document.filename"  HeaderText="Download Document"
                            DataNavigateUrlFields="document.documentID" DataNavigateUrlFormatString="/ln/loans/document.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView12" runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCreditManagerNotes" Enabled="false" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView> 
        <telerik:RadPageView ID="RadPageView4" runat="server">
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAppComments" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView>    
        <telerik:RadPageView ID="RadPageView3" runat="server">             
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
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
