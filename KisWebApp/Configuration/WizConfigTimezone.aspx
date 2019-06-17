<%@ Page Title="Virtual Chief Configuration Wizard : Timezone" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="WizConfigTimezone.aspx.cs" Inherits="KIS.Configuration.WizConfigTimezone" %>
<%@ Register TagPrefix="wizconfig" TagName="timezone" Src="~/Configuration/WizConfigTimezone.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Scriptmanager runat="server" id="scriptMan1" />
    <wizconfig:timezone runat="server" ID="frmTimezone" />
</asp:Content>
