<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="coreERP.ln.deposit.add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Additional Deposit into Client Investment Account
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Additional Deposit into Client Investment Account</h3>    
    <div class="subForm">
        <div class="subFormColumnLeft">
            <<div >
                <telerik:RadBinaryImage ID="RadBinaryImage1" Height="216" Width="216" AlternateText="Client Photo"
                         runat="server" ResizeMode="Fit"></telerik:RadBinaryImage>         
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
                Amount Deposited
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmountInvested" runat="server" CssClass="form-control"
                     OnTextChanged="txtAmountInvested_Changed" AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Deposit Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtAppDate" AutoPostBack="true"  OnSelectedDateChanged="dtAppDate_changed" CssClass="inputControl" runat="server" 
                    DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                Payment Type
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboPaymentType" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                Check No
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" CssClass="form-control" runat="server"></telerik:RadTextBox>
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
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtNaration" runat="server" CssClass="form-control" Rows="1" TextMode="MultiLine"
                         MaxLength="400"></telerik:RadTextBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Principal Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtPrincBal" 
                    runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                Interest Balance
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ReadOnly="true" ID="txtIntBalance" 
                    runat="server" CssClass="form-control"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Investment Product
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox Enabled="false" id="cboDepositType" runat="server" 
                        AutoPostBack="true" 
                        DropDownAutoWidth="Enabled" CssClass="inputControl" ></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Annual Interest Rate
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRateA" runat="server" ReadOnly="true" CssClass="form-control" NumberFormat-AllowRounding="false"
                     AutoPostBack="true"></telerik:RadNumericTextBox>
            </div>
            <div class="subFormLabel">
                 Period of Deposit
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboPeriod" runat="server" CssClass="inputControl" AutoPostBack="true"
                    DropDownWidth="355px" EmptyMessage="Select deposit period" HighlightTemplatedItems="true"
                    MarkFirstMatch="true" AutoCompleteSeparator=""
                    ToolTip="Select deposit period" ></telerik:RadComboBox>
            </div>
            <div class="subFormLabel">
                 Additional Interest Expected
            </div>
            <div class="subFormInput">
                <telerik:RadNumericTextBox WrapperCssClass="inputControl" AutoPostBack="true" ID="addIntExpec" runat="server" CssClass="form-control" NumberFormat-AllowRounding="false"
                     ></telerik:RadNumericTextBox>
            </div>
        </div>
    </div>
    <div class="subForm">
         <telerik:RadButton ID="btnSave" Text="Save Deposit into Investment Account" 
             runat="server" OnClick="btnSave_Click"></telerik:RadButton>
    </div> 
<telerik:RadTabStrip ID="tab1" MultiPageID="multi1" runat="server" Align="Left" SelectedIndex="0" Width="1024px">
        <Tabs>
            <telerik:RadTab runat="server" Text="Signatories" Selected="True">   
            </telerik:RadTab>
            <telerik:RadTab runat="server" Text="Deposits">   
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
                        <telerik:GridBoundColumn DataField="depositDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}"></telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="depositAmount" HeaderText="Deposit Amount" Aggregate="Sum" DataFormatString="{0:#,###.#0}" FooterAggregateFormatString="{0:#,###.#0}"></telerik:GridNumericColumn>
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
