<%@ Page Title="KIS Configuration Wizard : Admin User" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigAdminUser.aspx.cs" Inherits="KIS.Configuration.wizConfigAdminUser" %>
<%@ Register TagPrefix="kisConfig" TagName="AdminUser" Src="~/Configuration/wizConfigAdminUser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <kisConfig:AdminUser runat="server" id="frmAdminUser" />
</asp:Content>
