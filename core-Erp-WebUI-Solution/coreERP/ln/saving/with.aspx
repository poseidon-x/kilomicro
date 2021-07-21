<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="True" CodeBehind="with.aspx.cs" Inherits="coreERP.ln.saving.with" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Withdraw from Regular Deposit Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Withdraw from Regular Deposit Account</h3>
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadBinaryImage runat="server" ID="RadBinaryImage1"
                                AutoAdjustImageControlSize="false" Width="216" Height="216" CssClass="img-rounded" />
            </div>
            <div class="subFormLabel">
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
            </div>
            <asp:Panel ID="pnlRegular" runat="server" Visible="true">
                <div class="subFormLabel">
                    Surname
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Other Names
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlJoint" runat="server" Visible="false"> 
                <div class="subFormLabel">
                    Account Name
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtJointAccountName" runat="server" CssClass="form-control"></telerik:RadTextBox>
                </div>
            </asp:Panel>
            <div class="subFormLabel">
                Client Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" Enabled="false" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Amount Withdrawn
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Withdrawal Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Check No
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" CssClass="form-control"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Bank
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboBank" runat="server" CssClass="inputControl"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Currency
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboCur" runat="server" CssClass="inputControl" AutoPostBack="true"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Exchange Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" CssClass="form-control" ID="txtFxRate"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Withdrawal Type
            </div>
            <div class="subFormInput">
               <asp:RadioButtonList ID="chlWTyp" runat="server" CssClass="inputControl" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Interest" Value="I" />
                    <asp:ListItem Text="Principal" Value="P" />
                    <asp:ListItem Text="Both" Value="B" />
                </asp:RadioButtonList>
                
            </div>
            <div class="subFormLabel">
                Naration (Description)
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNaration" runat="server" CssClass="form-control" Rows="1" TextMode="MultiLine"
                         MaxLength="400"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Current Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" 
                    ID="txtPrincBal" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Avaiable Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" 
                    ID="txtAvailPrincBal" runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Current Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtIntBalance" runat="server" 
                        CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Available Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtAvailIntBal" runat="server" 
                        CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Deposit Product
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox Enabled="false" id="cboSavingsType" runat="server" 
                        AutoPostBack="true" OnSelectedIndexChanged="cboSavingsType_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" CssClass="inputControl" ></telerik:RadComboBox>
            </div> 
            <div class="subFormLabel">
                Interest Rate
            </div>
            <div class="subFormInput">
                 <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtInterestRate" runat="server" 
                        CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
        </div>
    </div>
    <div class="subForm">
        <telerik:RadButton ID="btnSave" Text="Save Withdrawal from Regular Account" runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div> 
     
    <telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="1024px">
        <Tabs>
            <telerik:RadTab runat="server" Text="Signatories" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Deposits">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Withdrawals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Accrued Interests">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <telerik:RadMultiPage runat="server" ID="multi1" Width="100%" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageView1" runat="server">  
            <telerik:RadGrid ID="gridDocument" runat="server" AutoGenerateColumns="false">
                <MasterTableView>
                    <Columns>
                        <telerik:GridBoundColumn DataField="fullName" HeaderText="Fullname of Signatory"></telerik:GridBoundColumn>
                        <telerik:GridBinaryImageColumn DataField="image.image1" HeaderText="Signature" ImageWidth="128px" ImageHeight="128px" ResizeMode="Fit"></telerik:GridBinaryImageColumn>
                        <telerik:GridHyperLinkColumn DataTextField="fullName"  HeaderText="Download Signature"
                            DataNavigateUrlFields="image.imageID" DataNavigateUrlFormatString="/ln/loans/image.aspx?id={0}"
                            Target="_blank"></telerik:GridHyperLinkColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView4" runat="server">             
            <telerik:RadGrid ID="gridDep" runat="server" AutoGenerateColumns="false" Width="1000px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="savingDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="savingAmount" HeaderText="Savings Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="checkNo" HeaderText="CheckNo"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="modeOfPayment.modeOfPaymentName" HeaderText="Payment Mode"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView6" runat="server">             
            <telerik:RadGrid ID="gridWith" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="withdrawalDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="interestWithdrawal" HeaderText="Interest Withdrawn" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principalWithdrawal" HeaderText="Principal Withdrawn" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="checkNo" HeaderText="CheckNo"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="modeOfPayment.modeOfPaymentName" HeaderText="Payment Mode"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageView8" runat="server">             
            <telerik:RadGrid ID="gridInt" runat="server" AutoGenerateColumns="false" Width="800px">
                <MasterTableView ShowFooter="true">
                    <Columns>
                        <telerik:GridBoundColumn DataField="interestDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn> 
                        <telerik:GridNumericColumn DataField="interestRate" HeaderText="Interest Rate" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="interestAmount" HeaderText="Interest Amount" DataFormatString="{0:#,###.#0}" Aggregate="Sum" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="principal" HeaderText="Principal" DataFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
