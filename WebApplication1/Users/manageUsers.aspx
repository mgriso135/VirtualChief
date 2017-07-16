<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageUsers.aspx.cs" 
    Title="Kaizen Indicator System" MasterPageFile="~/Site.master" Inherits="KIS.Users.manageUsers" %>

 <%@ Register TagPrefix="user" TagName="list" Src="listUsers.ascx" %>
 <%@ Register TagPrefix="user" TagName="add" Src="addUser.ascx" %>

 <asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="../Admin/admin.aspx"><asp:Literal runat="server" ID="lblNavAdmin" Text="<%$Resources:lblNavAdmin %>" /></a>
						<span class="divider">/</span>
						<a href="manageUsers.aspx"><asp:Literal runat="server" ID="lblGestUsers" Text="<%$Resources:lblGestUsers %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
<user:add runat="server" ID="addUser" />
<br />
<user:list runat="server" id="lstUsers" />

</asp:Content>