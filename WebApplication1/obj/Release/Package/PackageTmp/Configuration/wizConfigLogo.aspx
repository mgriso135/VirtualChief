﻿<%@ Page Title="KIS Configuration Wizard : Logo" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigLogo.aspx.cs" Inherits="KIS.Configuration.wizConfigLogo" %>
<%@ Register TagPrefix="WizConfig" TagName="Logo" Src="~/Admin/configLogo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <WizConfig:Logo runat="server" ID="frmConfigLogo" />
</asp:Content>
