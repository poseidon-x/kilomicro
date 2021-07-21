<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JDefault.aspx.cs" Inherits="JDefault" MasterPageFile="~/coreERP.Master" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="titlePlaceHolder" runat="server">Welcome</asp:Content>
<asp:Content ID="headContent" ContentPlaceHolderID="headPlaceHolder" runat="server">
    <style type="text/css">
        .wrapper {
         width:95%;
         overflow:hidden;
        }

        .left {
         width:98%;
         padding:10px;
         margin:10px;
        }

        .right {
         width:98%;
         float:right;
         padding:10px;
         margin:10px;
        }
         
        .headerTitle {
            font-family:Calibri;
            font-size:14pt;   
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="mainPlaceHolder" runat="server">
    <div class="subForm">
        <div class="subFormColumnLeft">
            <div class="subFormLabel">
                Display Results for (Branch):
            </div>
            <div class="subFormInput">
                <telerik:RadComboBox runat="server" CssClass="inputControl"
                     ID="cboBranch" AutoPostBack="True" OnClientSelectedIndexChanged="cboBranch_SelectedIndexChanged">
                </telerik:RadComboBox>
            </div>
        </div>
    </div> 
    <div class="wrapper">
        <div class="left">  
            <telerik:RadHtmlChart runat="server" ID="chComp" Width="600px"> 
                <Appearance>
                        <FillStyle BackgroundColor="White"></FillStyle>
                </Appearance>
                <ChartTitle Text="Disbursements vs Receipts">
                        <Appearance Align="Center" BackgroundColor="White" Position="Top"></Appearance>
                </ChartTitle>
                <Legend>
                        <Appearance BackgroundColor="White" Position="Bottom"></Appearance>
                </Legend>
                <PlotArea>
                        <Appearance>
                            <FillStyle BackgroundColor="White"></FillStyle>
                        </Appearance>
                        <XAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickType="Outside" MinorTickType="Outside"
                            Reversed="false"> 
                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0"></LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Month"></TitleAppearance>
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickSize="1" MajorTickType="Outside"
                            MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false"
                            >
                            <LabelsAppearance DataFormatString="${0}" RotationAngle="0"></LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Amount"></TitleAppearance>
                        </YAxis>
                        <Series>
                            <telerik:BarSeries Name="Disbursements" Stacked="false">
                                <Appearance>
                                    <FillStyle BackgroundColor="#c5d291"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="¢{0}" Position="Center">
                                </LabelsAppearance>
                                <TooltipsAppearance BackgroundColor="#c5d291" DataFormatString="¢{0}" Color="White"></TooltipsAppearance>
                            </telerik:BarSeries>
                            <telerik:BarSeries Name="Receipts">
                                <Appearance>
                                    <FillStyle BackgroundColor="#92b622"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="¢{0}" Position="Center"></LabelsAppearance>
                                <TooltipsAppearance BackgroundColor="#92b622" DataFormatString="¢{0}" Color="White"></TooltipsAppearance>
                            </telerik:BarSeries> 
                        </Series>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>
        <div class="right"> 
            <telerik:RadHtmlChart runat="server" ID="chCol"  Width="500px">                
                <Appearance>
                        <FillStyle BackgroundColor="White"></FillStyle>
                </Appearance>
                <ChartTitle Text="Collection Ratio by Month">
                        <Appearance Align="Center" BackgroundColor="White" Position="Top">
                        </Appearance>
                </ChartTitle>
                <Legend>
                        <Appearance BackgroundColor="White" Position="Bottom">
                        </Appearance>
                </Legend>
                <PlotArea>
                        <Appearance>
                            <FillStyle BackgroundColor="White"></FillStyle>
                        </Appearance>
                        <XAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickType="Outside" MinorTickType="Outside"
                            Reversed="false"> 
                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0">
                            </LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Month">
                            </TitleAppearance>
                        </XAxis>
                        <YAxis AxisCrossingValue="0" Color="#b3b3b3" MajorTickSize="1" MajorTickType="Outside"
                            MaxValue="100" MinorTickSize="1" MinorTickType="Outside" MinValue="0" Reversed="false"
                            Step="25">
                            <LabelsAppearance DataFormatString="{0}%" RotationAngle="0">
                            </LabelsAppearance>
                            <MajorGridLines Color="#EFEFEF" Width="1"></MajorGridLines>
                            <MinorGridLines Color="#F7F7F7" Width="1"></MinorGridLines>
                            <TitleAppearance Position="Center" RotationAngle="0" Text="Collection Ratio">
                            </TitleAppearance>
                        </YAxis>
                        <Series>
                            <telerik:LineSeries Name="Collection">
                                <Appearance>
                                    <FillStyle BackgroundColor="#5ab7de"></FillStyle>
                                </Appearance>
                                <LabelsAppearance DataFormatString="{0}%" Position="Above">
                                </LabelsAppearance>
                                <LineAppearance Width="1" />
                                <MarkersAppearance MarkersType="Circle" BackgroundColor="White" Size="8" BorderColor="#5ab7de"
                                    BorderWidth="2"></MarkersAppearance>
                                <TooltipsAppearance DataFormatString="{0}%"></TooltipsAppearance> 
                            </telerik:LineSeries> 
                        </Series>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>
    </div>
 
</asp:Content>