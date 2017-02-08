<%@ Page Title="" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizCustomerReport.aspx.cs" Inherits="KIS.Configuration.wizCustomerReport" %>
<%@ Register TagPrefix="wizConfig" TagName="CustomerReport" Src="~/Admin/configOrderStatusBase.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Configurazione Customer Report Generico</h3>
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <asp:Label runat="server" ID="lbl1" />
    <wizConfig:CustomerReport runat="server" ID="frmConfigCustomerReport" />
</asp:Content>
