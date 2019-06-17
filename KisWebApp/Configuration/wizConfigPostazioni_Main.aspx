<%@ Page Title="" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigPostazioni_Main.aspx.cs" Inherits="KIS.Configuration.wizConfigPostazioni_Main" %>
<%@ Register TagPrefix="postazioni" TagName="add" Src="~/Postazioni/addPostazione.ascx" %>
<%@ Register TagPrefix="postazioni" TagName="list" Src="~/Postazioni/viewElencoPostazioni.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
    <postazioni:add runat="server" ID="frmAddPostazione" />
    <postazioni:list runat="server" ID="frmListPostazioni" />
</asp:Content>
