<%@ Page Title="Admin section" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
CodeBehind="admin.aspx.cs" Inherits="WebApplication1.kpi.admin" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:Label runat="server" ID="lbl1" />
<a href="~/Login/login.aspx" runat="server" id="lnkLogin">You need to login first</a>
<table runat="server" id="tblOptions" border="0">
<tr>
<td>
<a href="/Admin/UserGroups.aspx" runat="server" id="lnkAdminUsers">
<img src="/img/iconUser.png" runat="server" id="imgAdminUsers" alt="User Admin Section" height="100"/>
</a>
</td>
</tr>
</table>

</asp:Content>

