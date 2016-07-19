<%@ Page Title="Kaizen Indicator System Configuration Wizard" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="MainWizConfig.aspx.cs" Inherits="KIS.Configuration.MainWizConfig" %>
<%@ MasterType VirtualPath="~/Configuration/WizConfig.Master" %>

<%@ Register TagPrefix="Configuration" TagName="Status" Src="~/Configuration/configurationStatus.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <Configuration:Status runat="server" id="frmConfigStatus" />
</asp:Content>
