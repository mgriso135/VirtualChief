<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managePostazioniLavoro.aspx.cs"
    MasterPageFile="~/Site.master" Title="Kaizen Indicator System" Inherits="KIS.Postazioni.managePostazioni" %>
<%@ Register TagPrefix="postazioni" TagName="viewElenco" Src="viewElencoPostazioni.ascx" %>
<%@ Register TagPrefix="postazione" TagName="add" Src="/Postazioni/addPostazione.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="managePostazioniLavoro.aspx">Gestione postazioni di lavoro</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:ImageButton runat="server" ID="imgShowAddPostazioni" OnClick="imgShowAddPostazioni_Click" ImageUrl="/img/iconAdd2.png" Height="60px" ToolTip="Aggiungi una nuova postazione di lavoro" />
    <br />
    <postazione:add runat="server" ID="addPostazioni" />

    <postazioni:viewElenco runat="server" id="showElencoPostazioni" />

</asp:Content>