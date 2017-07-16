<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pertManagePrecedenze2.aspx.cs" Inherits="KIS.Processi.pertManagePrecedenze2" %>
<%@Register TagPrefix="pert" TagName="precedenzePert" src="~/Processi/managePrecedenzePERT2.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lblErr" />
    <asp:Label runat="server" ID="lblTitle" style="font-size:24px; font-weight:bold;" />
    <pert:precedenzePert runat="server" ID="precedenze" />
</asp:Content>
