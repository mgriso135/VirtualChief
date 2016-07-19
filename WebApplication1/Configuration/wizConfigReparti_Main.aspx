<%@ Page Title="KIS Configuration Wizard : Reparti" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigReparti_Main.aspx.cs" Inherits="KIS.Configuration.wizConfigReparti_Main" %>
<%@ Register TagPrefix="wizConfig" TagName="reparti" Src="~/Configuration/wizConfigReparti_Main.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <wizConfig:reparti runat="server" id="wizReparti" />
</asp:Content>
