<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageStraordinario.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.Reparti.manageStraordinario" %>

<%@ Register TagPrefix="straordinario" TagName="add" Src="~/Reparti/addStraordinario.ascx" %>
<%@ Register TagPrefix="straordinario" TagName="list" Src="~/Reparti/elencoStraordinari.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label runat="server" ID="lbl1" />
    Aggiungi uno straordinario
    <straordinario:add runat="server" ID="addStraord" />
    Elenco straordinari
    <straordinario:list runat="server" id="listStraord" />
</asp:Content>
