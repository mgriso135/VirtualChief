<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageFestivita.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.Reparti.manageFestivita" %>

<%@ Register TagPrefix="festivita" TagName="add" Src="~/Reparti/addFestivita.ascx" %>
<%@ Register TagPrefix="festivita" TagName="list" Src="~/Reparti/elencoFestivita.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    Aggiungi una festivita
    <festivita:add runat="server" ID="addFest" />
    Elenco festivita
    <festivita:list runat="server" id="listFest" />
</asp:Content>