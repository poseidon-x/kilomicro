<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="loanCheckList.aspx.cs" Inherits="coreERP.ln.loans.loanCheckList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Loan Approval
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Client Loan Approval</h3>
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
                Loan ID:
            </div>
            <div class="subFormInput">
                <asp:Label runat="server" CssClass="inputControl" ID="lblLoanID" Text=""></asp:Label>
            </div>
            <div class="subFormLabel">
                Loan Approval Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ReadOnly="true" ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Interest Calculation
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox Enabled="false" id="cboInterestType" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>            
                <div class="subFormLabel">
                    Scheme Loan
                </div>      
                <div class="subFormInput">
                    <telerik:RadComboBox id="cboLoanScheme" runat="server" AutoPostBack="true" CausesValidation="false"
                       CssClass="inputControl"
                        ToolTip="Select the type of scheme loan"></telerik:RadComboBox>                    
                </div> 
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
               Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtRate" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                    &nbsp;per Month
            </div>
            <div class="subFormLabel">
               Tenure of Loan
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtTenure" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                         &nbsp;Months
            </div>
            <div class="subFormLabel">
                Amount Requested
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtAmountRequested" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <asp:Panel ID="pnlFees" runat="server"> 
                <div class="subFormLabel">
                    Processing Fee
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtProcFee" runat="server" CssClass="inputControl" Value="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                            Repayment Mode
                </div>
                <div class="subFormInput">
                      <telerik:RadComboBox Enabled="false" id="cboRepaymentMode" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                </div> 
            </asp:Panel>
            <div class="subFormLabel">
                Loan Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox Enabled="false" id="cboLoanType" runat="server" CssClass="inputControl"></telerik:RadComboBox>
                <telerik:RadComboBox id="cboLoanProduct" runat="server" CssClass="inputControl" enabled="false" visible="false" ></telerik:RadComboBox>
            </div>
        </div>
    </div> 
    <asp:Repeater ID="rpCheckList" runat="server">
        <HeaderTemplate>
            <table>
                <tr  style="border-bottom: 1px solid #00B4FF; font-weight:bold;">
                    <td style="width:300px; border-bottom: 1px solid #00B4FF; font-weight:bold;">Check List Item</td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF; font-weight:bold;">M?</td>
                    <td style="width:70px; border-bottom: 1px solid #00B4FF; font-weight:bold;"><asp:CheckBox ID="chkPassedAll" runat="server" 
                         AutoPostBack="true" OnCheckedChanged="chkPassedAll_CheckedChanged"
                         ToolTip="Click to tick or untick all checklist items" /></td>
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
                        <asp:CheckBox ID="chkPassed" runat="server" Checked='<%# Bind("passed") %>' />
                    </td>
                    <td style="width:300px; border-bottom: 1px solid #00B4FF">
                        <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("comments") %>' 
                            Width="300" /></td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <div class="subForm">
        <telerik:RadButton ID="btnApprove" Text="Save Checklist for Loan Application" 
            runat="server" OnClick="btnApprove_Click"></telerik:RadButton>
    </div> 
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="100%">
        <Tabs>
            <telerik:RadTab runat="server" Text="Loan Guarantors" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Loan Collaterals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Repayment Schedule"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Supporting Documents"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Credit Manager Comments"  
                 ToolTip="Click to view the check list comments for this loan entered by the credit manager"> 
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
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="EditItem" CommandArgument="surName" HeaderText="View/Edit Guarantor" Text="View/Edit" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="PushButton" CommandName="DeleteItem" CommandArgument="surName" HeaderText="Delete Guarantor" Text="Delete" ItemStyle-Width="55px"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView2" runat="server"> 
           <telerik:RadGrid ID="gridCollateral" runat="server" AutoGenerateColumns="false" OnDetailTableDataBind="gridCollateral_DetailTableDataBind" OnItemCommand="gridCollateral_ItemCommand">
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
            <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCreditManagerNotes" runat="server" Rows="20" TextMode="MultiLine"  Height="277px" Width="746px"></telerik:RadTextBox>
        </telerik:RadPageView> 
    </telerik:RadMultiPage>
</asp:Content>
