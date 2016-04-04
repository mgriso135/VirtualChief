<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Title="Kaizen Indicator System"
    CodeBehind="PermessiGruppi.aspx.cs" Inherits="KIS.Users.PermessiGruppi" %>
<%@ Register TagPrefix="gruppo" TagName="listPermessi" Src="~/Users/listPermessiGruppi.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label runat="server" id="lbl1" />
    
    <gruppo:listPermessi runat="server" ID="lstPermGruppi" />
    </asp:Content>