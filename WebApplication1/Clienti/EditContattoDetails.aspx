<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditContattoDetails.aspx.cs" Inherits="KIS.Clienti.EditContattoDetails" %>
<%@Register TagPrefix="Clienti" TagName="EditContatto" Src="~/Clienti/EditContattoDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lbl1" />
    <Clienti:EditContatto runat="server" ID="frmEditContatto" />
</asp:Content>
