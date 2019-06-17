<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageFestivita.aspx.cs" Title="Virtual Chief"
 MasterPageFile="~/Site.master" Inherits="KIS.Reparti.manageFestivita" %>

<%@ Register TagPrefix="festivita" TagName="add" Src="~/Reparti/addFestivita.ascx" %>
<%@ Register TagPrefix="festivita" TagName="list" Src="~/Reparti/elencoFestivita.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"><h4>
    <asp:Literal runat="server" ID="lblAddFestivita" Text="<%$Resources:lblAddFestivita %>" /></h4>
    <festivita:add runat="server" ID="addFest" /><br />
    <br />
    <asp:Literal runat="server" ID="lblElencoFestivita" Text="<%$Resources:lblElencoFestivita %>" />
    <festivita:list runat="server" id="listFest" />
</asp:Content>