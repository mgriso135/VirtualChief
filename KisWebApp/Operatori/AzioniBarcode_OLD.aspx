<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AzioniBarcode_OLD.aspx.cs" Inherits="KIS.Operatori.AzioniBarcode" %>
<%@ Register TagPrefix="azioni" TagName="barcode" Src="~/Operatori/AzioniBarcode.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <azioni:barcode runat="server" ID="frmAzioniBarcode" />
</asp:Content>
