<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" MasterPageFile="~/Site.Master" Inherits="WebApplication1.Login.login" %>
<%@ Register TagPrefix="login" TagName="box" Src="~/Login/loginbox.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<login:box runat="server" id="loginBox" />

</asp:Content>