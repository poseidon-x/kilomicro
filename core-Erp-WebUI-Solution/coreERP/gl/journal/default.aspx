<%@ Page Title="" Language="C#" MasterPageFile="~/gl/gl.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="coreERP.gl.journal._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
Journal Entry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server"> 
    <h3>Journal Entry (Local)</h3>
                 
        <div style="vertical-align:middle; float:right;padding-right:25%;padding-top:150px">
            <h4 runat="server" id="lblHeader">Jounral Entry Transactions (Local)</h4>
            <br />
	        <asp:RadioButton ID="rdbNew" runat="server" Text="Add New" GroupName="JEL"
	         OnCheckedChanged  ="rdbNew_CheckedChanged" AutoPostBack="true" />
	        <br /> 
	        <asp:RadioButton ID="rdbEdit" runat="server" Text="Edit Unposted" GroupName="JEL" 
	        OnCheckedChanged  ="rdbEdit_CheckedChanged" AutoPostBack="true"  />	     
            <telerik:RadComboBox ID="cboJnlTmp" runat="server" Height="200px" Width="200px"
                    DropDownWidth=" 500px" EmptyMessage="Enter a Batch to Edit" HighlightTemplatedItems="true"
                  AutoPostBack="true" OnItemsRequested="cboJnlTmp_ItemsRequested"
                  EnableLoadOnDemand="true" OnSelectedIndexChanged="cboJnlTmp_SelectedIndexChanged"
                    DataTextField="batch_no" DataValueField="batch_no"> 
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 100px;">
                                    Batch #</td>
                                <td style="width: 100px;">
                                    Originator</td>
                                <td style="width: 100px;">
                                    Date</td>
                                <td style="width: 100px;">
                                    Credit</td>
                                <td style="width: 100px;">
                                    Debit</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 100px;" >
                                    <%# Eval("batch_no")%>
                                </td>
                                <td style="width: 100px;" >
                                    <%# Eval("creator")%>
                                </td>
                                <td style="width: 100px;" >
                                    <%# ((DateTime)Eval("tx_date")).ToString("dd-MMM-yyyy")%>
                                </td>
                                <td style="width: 100px;">
                                    <%# ((double)Eval("crdt_amt")).ToString("#,##0.#0")%>
                                </td>  
                                <td style="width: 100px;">
                                    <%# ((double)Eval("dbt_amt")).ToString("#,##0.#0")%>
                                </td>  
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>    
	        <br />
	        <asp:RadioButton ID="rdbEdit2" runat="server" Text="Edit Posted" GroupName="JEL"
	        OnCheckedChanged  ="rdbEdit2_CheckedChanged" AutoPostBack="true"  />    
            <telerik:RadComboBox ID="cboJnl" runat="server" Height="400px" Width="200px"
                    DropDownWidth="500px" EmptyMessage="Enter a Batch to Edit" HighlightTemplatedItems="true"
                    AutoPostBack="true" EnableLoadOnDemand="true"
                 OnItemsRequested="cboJnl_ItemsRequested" OnSelectedIndexChanged="cboJnl_SelectedIndexChanged"
                    DataTextField="batch_no" DataValueField="batch_no" OnTextChanged="cboJnl_TextChanged"> 
                    <HeaderTemplate>
                        <table style="width: 500px">
                            <tr> 
                                <td style="width: 100px;">
                                    Batch #</td>
                                <td style="width: 100px;">
                                    Originator</td>
                                <td style="width: 100px;">
                                    Date</td>
                                <td style="width: 100px;">
                                    Credit</td>
                                <td style="width: 100px;">
                                    Debit</td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 500px">
                            <tr>
                                <td style="width: 100px;" >
                                    <%# Eval("batch_no")%>
                                </td>
                                <td style="width: 100px;" >
                                    <%# Eval("creator")%>
                                </td>
                                <td style="width: 100px;" >
                                    <%# ((DateTime)Eval("tx_date")).ToString("dd-MMM-yyyy")%>
                                </td>
                                <td style="width: 100px;">
                                    <%# ((double)Eval("crdt_amt")).ToString("#,##0.#0")%>
                                </td>  
                                <td style="width: 100px;">
                                    <%# ((double)Eval("dbt_amt")).ToString("#,##0.#0")%>
                                </td>  
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>    
	        <br />
            
	    </div>      
</asp:Content>
