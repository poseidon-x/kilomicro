<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="coreERP.ln.investment.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Additional Investment into Company Investment Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Additional Investment into Company Investment Account</h3>    
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="216" Height="216" runat="server" ID="rotator1" ItemHeight="216" 
                    ItemWidth="216" FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Account No.
            </div>h
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
                Client Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" Enabled="false" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Amount Invested
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountInvested" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Investment Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" CssClass="inputControl" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox id="cboPaymentType" CssClass="inputControl" runat="server"></telerik:RadComboBox>
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
            <div class="subFormLabel">
                Naration (Description)
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNaration" runat="server" CssClass="inputControl" Rows="1" TextMode="MultiLine"
                         MaxLength="400"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtPrincBal" 
                    runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtIntBalance" 
                    runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Investment Product
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox Enabled="false" id="cboInvestmentType" runat="server" 
                        AutoPostBack="true" OnSelectedIndexChanged="cboInvestmentType_SelectedIndexChanged"
                        DropDownAutoWidth="Enabled" CssClass="inputControl" ></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtInterestRate" 
                        runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Period of Investment
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl"  ReadOnly="true" ID="txtPeriod" runat="server" CssClass="inputControl" NumberFormat-DecimalDigits="0" ></telerik:RadNumericTextBox>
                &nbsp;Months
            </div>
        </div>
    </div>
    <div class="subForm">
         <telerik:RadButton ID="btnSave" Text="Save Investment into Investment Account" 
             runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div> 
<telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="1024px">
        <Tabs>
            <telerik:RadTab runat="server" Text="Signatories" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Investments">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Withdrawals"> 
            </telerik:RadTab> 
            <telerik:RadTab runat="server" Text="Interests">
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
                        <telerik:GridBoundColumn DataField="investmentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="investmentAmount" HeaderText="Investment Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
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
