<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="generaBarcode.aspx.cs" Inherits="KIS.Users.generaBarcode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lbl1" /><br />
    <asp:Label runat="server" ID="lblNome" />
    <br />
    <asp:Literal runat="server" ID="lblMatricola1" Text="<%$Resources:lblMatricola %>" />
    :&nbsp;<asp:Label runat="server" ID="lblMatricola" />;&nbsp;
    <asp:Literal runat="server" ID="lblUser" Text="<%$Resources:lblUser %>" />:&nbsp;<asp:Label runat="server" id="lblUsername" />
    <br />
    
</asp:Content>
