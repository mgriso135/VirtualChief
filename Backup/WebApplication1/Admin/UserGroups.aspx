<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
 CodeBehind="UserGroups.aspx.cs" Inherits="WebApplication1.Admin.UserGroups" %>
 <%@ Register TagPrefix="user" TagName="list" Src="/Admin/listUsers.ascx" %>
 <%@ Register TagPrefix="user" TagName="add" Src="/Admin/addUser.ascx" %>

 <asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<user:add runat="server" ID="addUser" />
<br />
<user:list runat="server" id="lstUsers" />

</asp:Content>