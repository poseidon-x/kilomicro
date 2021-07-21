<%@ Page Title="" Language="C#" MasterPageFile="~/ln/ln.master" AutoEventWireup="true" CodeBehind="disburseIL.aspx.cs" Inherits="coreERP.ln.loans.disburseIL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Invoice Loan
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Invoice Loan Disbursement</h3>   
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="imageFrame">
                <telerik:RadRotator Width="177" Height="123" runat="server" ID="rotator1" ItemHeight="123" ItemWidth="177" 
                        FrameDuration="30000"></telerik:RadRotator>
            </div>
            <div class="subFormLabel">
                Account No.
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtAccountNo" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
            </div> 
        </div>
        <div class="subFormColumnMiddle">
            <div class="subFormLabel">
                Surname
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtSurname" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Other Names
            </div>
            <div class="subFormInput">
                <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtOtherNames" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                Client Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboClient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboClient_SelectedIndexChanged" CssClass="inputControl"
                        DropDownAutoWidth="Enabled" Enabled="false" EmptyMessage="Type name or account number of client" 
                        MarkFirstMatch="true" AutoCompleteSeparator="" EnableLoadOnDemand="true" OnItemsRequested="cboClient_ItemsRequested"></telerik:RadComboBox> 
            </div>
            <div class="subFormLabel">
                Supplier Selected
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox ID="cboSupplier" runat="server" CssClass="inputControl" AutoPostBack="true" 
                        OnSelectedIndexChanged="cboSupplier_SelectedIndexChanged" Enabled="false"
                        DropDownWidth="355px" EmptyMessage="Select a Supplier" HighlightTemplatedItems="true"
                        MarkFirstMatch="true" AutoCompleteSeparator=""></telerik:RadComboBox>
            </div>
        </div>
        <div class="subFormColumnRight">
            <div class="subFormLabel">
                Disbursement Date
            </div>
            <div class="subFormInput">
                <telerik:RadDatePicker ID="dtApprDate" CssClass="inputControl" runat="server" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
            </div>
            <div class="subFormLabel">
                    Mode of Payment
            </div>
            <div class="subFormInput">
                    <telerik:RadComboBox id="cboPaymentType" CssClass="inputControl" runat="server"></telerik:RadComboBox>
             </div>
             <div class="subFormLabel">
                    Check No.
                </div>
            <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtCheckNo" runat="server" CssClass="inputControl"></telerik:RadTextBox>
            </div>
            <div class="subFormLabel">
                    Bank
                </div>
            <div class="subFormInput">
                    <telerik:RadComboBox ID="cboBank" CssClass="inputControl" runat="server"></telerik:RadComboBox>
            </div>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlAddEdit" Visible="false" >
        <div class="subForm">
            <div class="subFormColumnLeft">
                <div class="subFormLabel">
                    Invoice Amount
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtAmount" runat="server" CssClass="inputControl" AutoPostBack="true" 
                            OnTextChanged="txtAmount_TextChanged" Enabled="false"> </telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                    Invoice No
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtInvoiceNo" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
                </div>
            </div>
            <div class="subFormColumnMiddle">
                <div class="subFormLabel">
                    Invoice Description
                </div>
                <div class="subFormInput">
                    <telerik:RadTextBox WrapperCssClass="inputControl" ID="txtDesc" runat="server" TextMode="MultiLine" Rows="1" CssClass="inputControl" Enabled="false"></telerik:RadTextBox>
                </div>
                <div class="subFormLabel">
                    Invoice Date
                </div>
                <div class="subFormInput">
                    <telerik:RadDatePicker ID="dtAppDate" runat="server" CssClass="inputControl" DateInput-DateFormat="dd-MMM-yyyy" Enabled="false"></telerik:RadDatePicker>
                </div>
                <div class="subFormLabel">
                     Withholding Tax
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" Enabled="false" ID="txtWith" runat="server" CssClass="inputControl"></telerik:RadNumericTextBox>
                </div>
            </div>
            <div class="subFormColumnRight">
                <div class="subFormLabel">
                    Disbursement Rate
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDisbRate" runat="server" CssClass="inputControl" AutoPostBack="true" value="62" 
                            OnTextChanged="txtDisbRate_TextChanged" Enabled="false"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                    Interest Rate
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtRate" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadNumericTextBox>
                </div>
                <div class="subFormLabel">
                     Disbursement Amount
                </div>
                <div class="subFormInput">
                    <telerik:RadNumericTextBox WrapperCssClass="inputControl" ID="txtDisbAmt" runat="server" CssClass="inputControl" Enabled="false"></telerik:RadNumericTextBox>
                </div>
            </div>
        </div>  
    </asp:Panel>
    <div class="subForm"> 
        <telerik:RadButton ID="btnSave" runat="server" Text="Disburse Invoice Loans" OnClick="btnSave_Click"></telerik:RadButton> 
    </div>  
    <telerik:RadGrid runat="server" ID="grid" AutoGenerateColumns="false" AllowAutomaticDeletes="false" AllowAutomaticInserts="false" AllowAutomaticUpdates="false"
         OnItemCommand="grid_ItemCommand">
        <MasterTableView DataKeyNames="invoiceLoanID">
            <Columns>
                <telerik:GridDropDownColumn DataField="clientID" DataSourceID="EntityDataSource3" ListTextField="clientName"
                     ListValueField="clientID" HeaderText="Client Name" Visible="false"></telerik:GridDropDownColumn>
                <telerik:GridDropDownColumn DataField="supplierID" DataSourceID="EntityDataSource2" ListTextField="supplierName"
                     ListValueField="supplierID" HeaderText="Supplier Name" Visible="false"></telerik:GridDropDownColumn>
                <telerik:GridBoundColumn DataField="invoiceNo" HeaderText="Invoice No" SortExpression="invoiceNo"></telerik:GridBoundColumn>
                <telerik:GridNumericColumn DataField="invoiceAmount" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="Invoice Amount"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="withHoldingTax" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="With. Tax"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="interestAmount" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="Interest"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="processingFee" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="Proc. Fee"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="proposedAmount" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="Disb. Amount"></telerik:GridNumericColumn>
                <telerik:GridNumericColumn DataField="ceilRate" DataFormatString="{0:#,##0.#0;(#,##0.#0);0}" HeaderText="Disb. Rate"></telerik:GridNumericColumn>
                <telerik:GridDateTimeColumn DataField="invoiceDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Invoice Date"></telerik:GridDateTimeColumn>
                <telerik:GridBoundColumn DataField="invoiceDescription" HeaderText="Invoice Description" ItemStyle-Width="350px"></telerik:GridBoundColumn>       
               <telerik:GridButtonColumn UniqueName="MyEdit" CommandName="MyEdit"
                    ButtonType="ImageButton" ImageUrl="~/images/edit.jpg"  HeaderImageUrl="~/images/edit.jpg" 
                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                </telerik:GridButtonColumn> 
            </Columns>
        </MasterTableView> 
    </telerik:RadGrid>
    <ef:EntityDataSource ID="EntityDataSource2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="suppliers">
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="EntityDataSource3" runat="server" 
        ConnectionString="name=reportEntities" DefaultContainerName="reportEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="vwClients">
    </ef:EntityDataSource>
</asp:Content>
