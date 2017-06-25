<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageStraordinario.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.Reparti.manageStraordinario" %>

<%@ Register TagPrefix="straordinario" TagName="add" Src="~/Reparti/addStraordinario.ascx" %>
<%@ Register TagPrefix="straordinario" TagName="list" Src="~/Reparti/elencoStraordinari.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label runat="server" ID="lbl1" />
    <h4><asp:Literal runat="server" ID="lblAddStraordinario" Text="<%$Resources:lblAddStraordinario %>" /></h4>
    <straordinario:add runat="server" ID="addStraord" />
    <h4><asp:Literal runat="server" ID="lblListStraordinari" Text="<%$Resources:lblListStraordinari %>" /></h4>
    <straordinario:list runat="server" id="listStraord" />
</asp:Content>
