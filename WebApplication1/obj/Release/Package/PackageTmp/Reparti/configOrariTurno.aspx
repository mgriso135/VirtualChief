<%@ Page Language="C#" Title="Kaizen Indicator System" AutoEventWireup="true" CodeBehind="configOrariTurno.aspx.cs"
    MasterPageFile="~/Site.Master" Inherits="KIS.Reparti.configOrariTurno" %>

<%@ Register TagPrefix="config" TagName="orariT" Src="configOrariTrn.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <config:orariT runat="server" id="orari" />
</asp:Content>