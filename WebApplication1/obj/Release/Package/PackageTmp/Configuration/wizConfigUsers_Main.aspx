<%@ Page Title="" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigUsers_Main.aspx.cs" Inherits="KIS.Configuration.wizConfigUsers_Main" %>
<%@ Register TagPrefix="user" TagName="add" Src="~/Users/addUser.ascx" %>
<%@ Register TagPrefix="user" TagName="list" Src="~/Users/listUsers.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3><asp:Label runat="server" ID="lblTitleGestUtenti" Text="<%$Resources:lblTitleGestUtenti %>" /></h3>
    <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
    <user:add runat="server" ID="frmAddUser" />
    <br />
    <user:list runat="server" ID="frmListUser" />
</asp:Content>
