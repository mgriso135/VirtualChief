<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" Title="Kaizen Indicator System"
 CodeBehind="managePermessi.aspx.cs" Inherits="KIS.Admin.managePermessi" %>

<%@ Register TagPrefix="permessi" TagName="add" Src="~/Users/addPermesso.ascx" %>
<%@ Register TagPrefix="permessi" TagName="list" Src="~/Users/listPermessi.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
    <li>
						<a href="/Admin/admin.aspx"><asp:Literal runat="server" ID="lblNavAdmin" Text="<%$Resources:lblNavAdmin %>"/></a>
						<span class="divider">/</span>
						<a href="managePermessi.aspx"><asp:Literal runat="server" ID="lblGestPermessi" Text="<%$Resources:lblGestPermessi %>"/></a>
						<span class="divider">/</span>
					</li>
				</ul>
<asp:Label runat="server" ID="lbl1" />
<a href="~/Login/login.aspx" runat="server" id="lnkLogin"><asp:Literal runat="server" ID="lblLogin" Text="<%$Resources:lblLogin %>"/></a>
<br />
<asp:Label runat="server" ID="lblTitolo" style="font-size:24px; font-weight:bold;" Text="<%$Resources:lblGestPermessi %>">
    </asp:Label><br /><br />

    <permessi:add runat="server" id="addPermessi" />
    <permessi:list runat="server" ID="listPermessi" />


</asp:Content>