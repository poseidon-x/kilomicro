<%@ Page Title="" Language="C#" MasterPageFile="~/coreERP.Master" AutoEventWireup="true" CodeBehind="performanceContract.aspx.cs" Inherits="coreERP.hc.ipf.performanceContract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="titlePlaceHolder" runat="server">
    Input Your Performance Contract
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <h3>Performance Contract</h3>
    <telerik:RadSplitter runat="server" ID="splitter1" Width="100%" Height="100%" OnClientLoaded="SplitterLoaded2">
        <telerik:RadPane runat="server" ID="pane1" Width="20%">            
               <telerik:RadTreeView ID="tree" runat="server" OnNodeExpand="tree_NodeExpand"></telerik:RadTreeView>
        </telerik:RadPane>
        <telerik:RadSplitBar runat="server" ID="bar1"></telerik:RadSplitBar>
        <telerik:RadPane runat="server" ID="pane2" Width="70%">
            <div style="width:100%;height:100%">
                <h4>Input Your Performance Contract</h4>
                <table style="width:800px">
                    <tr>
                        <td style="width:200px">Contract Start Date</td>
                        <td style="width:200px">
                            <telerik:RadDatePicker runat="server" ID="dtpStartDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                        </td>
                        <td style="width:200px">Contract End Date</td>
                        <td style="width:200px">
                            <telerik:RadDatePicker runat="server" ID="dtpEndDate" DateInput-DateFormat="dd-MMM-yyyy"></telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Contract Status</td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="cboStatus" DropDownAutoWidth="Enabled" Width="250px"></telerik:RadComboBox>
                        </td>
                        <td></td>
                        <td>
                
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><telerik:RadButton runat="server" ID="btnAddNew" Text="Add New KPI Item"
                             OnClick="btnAddNew_Click">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnlAddEdit" Visible="false">
                    <table style="width:800px">
                        <tr>
                            <td colspan="2"><h5>Add/Edit KPI</h5></td>
                        </tr>
                        <tr>
                            <td>Performance Area (KPA)</td>
                            <td>
                                <telerik:RadComboBox runat="server" ID="cboArea" DropDownAutoWidth="Enabled" Width="250px"></telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>KPI Description</td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txtDesc" TextMode="MultiLine" Rows="4" Width="300px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>KPI Weight</td>
                            <td>
                                <telerik:RadNumericTextBox WrapperCssClass="inputControl" runat="server" ID="txtWeight" Width="50px"></telerik:RadNumericTextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="5"><h5>KPI Targets</h5></td>
                        </tr>
                        <tr>
                            <td>Not Achieved (0)</td>
                            <td>Partly Achieved (1)</td>
                            <td>Achieved-On Target (2)</td>
                            <td>Over Achieved (3)</td>
                            <td>Exceeds Expectations (4)</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txt0" TextMode="MultiLine" Rows="3" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txt1" TextMode="MultiLine" Rows="3" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txt2" TextMode="MultiLine" Rows="3" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txt3" TextMode="MultiLine" Rows="3" Width="200px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox WrapperCssClass="inputControl" runat="server" ID="txt4" TextMode="MultiLine" Rows="3" Width="200px"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <telerik:RadButton runat="server" ID="btnAddEdit" Text="Add KPI"
                            OnClick="btnAddEdit_Click"></telerik:RadButton>
                </asp:Panel>
                <telerik:RadGrid ID="RadGrid1" runat="server" DataSourceID="eds1"
                    GridLines="Both" AllowSorting="true" ShowFooter="true" AllowAutomaticInserts="false"
                    AllowAutomaticDeletes="false" AllowAutomaticUpdates="false"
                    OnItemCommand="RadGrid1_ItemCommand">
                        <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                        <MasterTableView autogeneratecolumns="False" EditMode="InPlace" ShowFooter="true"
                                    datakeynames="performanceContractItemID" datasourceid="eds1" 
                                       CommandItemDisplay="None" Width="960px" allowSorting="true">
                            <DetailTables>
                                <telerik:GridTableView AutoGenerateColumns="false" DataKeyNames="performanceContractTargetID"
                                     AllowSorting="true" CommandItemDisplay="None" DataSourceID="eds3" AllowAutomaticInserts="false"
                                     AllowAutomaticDeletes="false" AllowAutomaticUpdates="false">
                                    <ParentTableRelation>
                                        <telerik:GridRelationFields DetailKeyField="performanceContractItemID"
                                             MasterKeyField="performanceContractItemID" />
                                    </ParentTableRelation>
                                    <Columns>
                                        <telerik:GridDropDownColumn DataField="performanceScoreID" DataSourceID="eds4"
                                             ListTextField="performanceScoreName" ListValueField="performanceScoreID"
                                             HeaderText="Score Category"></telerik:GridDropDownColumn>
                                        <telerik:GridBoundColumn DataField="targetCreteria" HeaderText="Score Criteria"></telerik:GridBoundColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridBoundColumn DataField="performanceContractItemID" DataType="System.Int32" 
                                    DefaultInsertValue="" HeaderText="performanceContractItemID" ReadOnly="True" 
                                    SortExpression="performanceContractItemID" UniqueName="performanceContractItemID" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn DataSourceID="eds2" DataField="performanceAreaID" ListTextField="performanceAreaName"
                                     ListValueField="performanceAreaID" ItemStyle-Width="200px" HeaderText="KPI Area"></telerik:GridDropDownColumn>
                                <telerik:GridBoundColumn DataField="itemDescription" DefaultInsertValue="" 
                                    HeaderText="KPI Description" SortExpression="itemDescription" UniqueName="itemDescription" ItemStyle-Width="220px">
                                </telerik:GridBoundColumn> 
                                <telerik:GridNumericColumn DataField="weight" DataFormatString="{0:#,##0}" HeaderText="KPI Weight"
                                     Aggregate="Sum" FooterAggregateFormatString="{0:#,##0}"></telerik:GridNumericColumn> 
                                <telerik:GridButtonColumn UniqueName="Edit" CommandName="E"
                                    ButtonType="ImageButton" ImageUrl="~/images/edit.jpg" 
                                    ItemStyle-Width="32px" ItemStyle-Height="32px" >
                                </telerik:GridButtonColumn>
                                  <telerik:GridButtonColumn CommandName="D" Text="Delete" UniqueName="DeleteColumn" 
                                    ButtonType="ImageButton" ImageUrl="~/images/delete.jpg" 
                                    ConfirmText="Are you sure you want to delete the selected KPI Item?">
                                  </telerik:GridButtonColumn>
                            </Columns> 
                        </MasterTableView>
                       <ClientSettings> 
                           <Selecting AllowRowSelect="true" />
                           <KeyboardNavigationSettings AllowActiveRowCycle="true" />
                           <Selecting AllowRowSelect="true" />
                       </ClientSettings>
                </telerik:RadGrid>
            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <ef:EntityDataSource ID="eds1" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="performanceContractItems" Where="it.performanceContractID == @performanceContractID">
        <WhereParameters>
            <asp:Parameter Type="Int32" Name="performanceContractID" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds2" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="performanceAreas" > 
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds3" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="performanceContractTargets" Where="it.performanceContractItemID == @performanceContractItemID">
        <WhereParameters>
            <asp:SessionParameter SessionField="performanceContractItemID" Type="Int32" Name="performanceContractItemID" />
        </WhereParameters>
    </ef:EntityDataSource>
    <ef:EntityDataSource ID="eds4" runat="server" 
        ConnectionString="name=coreLoansEntities" DefaultContainerName="coreLoansEntities" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="performanceScores" > 
    </ef:EntityDataSource>
     <script type="text/javascript">
         function SplitterLoaded2(splitter, arg) {
             var pane = splitter.getPaneById('<%= pane2.ClientID %>');
             var height = pane.getContentElement().scrollHeight;
             splitter.set_height(screen.availHeight-200);
             pane.set_height(height);
         }
    </script> 
</asp:Content>
