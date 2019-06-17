<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="viewDetailsTaskProduzione.aspx.cs" Inherits="KIS.Produzione.viewDetailsTaskProduzione" %>
<%@ Register TagPrefix="taskProduzione" TagName="viewDetails" Src="~/Produzione/viewDetailsTask.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:literal runat="server" ID="lblTitoloDettaglioTask" Text="<%$resources:lblTitoloDettaglioTask %>" /></h1>
    <asp:Label runat="server" ID="lbl1" />
    <taskProduzione:viewDetails runat="server" id="frmViewDetails" />
</asp:Content>
