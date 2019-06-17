<%@ Page Title="Virtual Chief" Language="C#" AutoEventWireup="true" CodeBehind="pertManagePrecedenze.aspx.cs" 
     MasterPageFile="~/Site.master" Inherits="KIS.Processi.pertManagePrecedenze" %>

<%@Register TagPrefix="pert" TagName="precedenzePert" src="/Processi/managePrecedenzePERT.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label runat="server" ID="lblErr" />
    <asp:Label runat="server" ID="lblTitle" style="font-size:24px; font-weight:bold;" />
    <pert:precedenzePert runat="server" ID="precedenze" />

</asp:Content>