<%@ Page Title="Virtual Chief Configuration Wizard : Logo" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigLogo.aspx.cs" Inherits="KIS.Configuration.wizConfigLogo" %>
<%@ Register TagPrefix="WizConfig" TagName="wizLogo" Src="~/Configuration/wizConfigLogo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <WizConfig:wizLogo runat="server" id="frmWizConfigLogo" />
</asp:Content>
