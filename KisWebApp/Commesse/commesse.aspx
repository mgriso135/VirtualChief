<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="commesse.aspx.cs" Inherits="KIS.Commesse.commesse" %>
<%@Register TagPrefix="commesse" TagName="Add" Src="~/Commesse/addCommessa.ascx" %>
<%@ Register TagPrefix="commesse" TagName="list" Src="~/Commesse/listCommesse.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="commesse.aspx">
                            <asp:Label runat="server" ID="lblNavCommesse" meta:resourcekey="lblNavCommesse" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h1><asp:Label runat="server" ID="lblTitleCommesse" meta:resourcekey="lblTitleCommesse" /></h1>
    <commesse:Add runat="server" ID="frmAddCommessa" />
    <br />
    <commesse:list runat="server" ID="frmListCommesse" />
</asp:Content>
