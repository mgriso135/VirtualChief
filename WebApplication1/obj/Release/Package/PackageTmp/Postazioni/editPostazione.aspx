<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editPostazione.aspx.cs" Title="Kaizen Indicator System"
    MasterPageFile="~/Site.Master" Inherits="KIS.Postazioni.editPostazione" %>

<%@ Register TagPrefix="postazione" TagName="edit" Src="editPostazione.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    		<ul class="breadcrumb hidden-phone">
					<li>
						<a href="managePostazioniLavoro.aspx">
                            <asp:Literal runat="server" ID="lblNavPostazioni" Text="<%$Resources:lblNavPostazioni %>" />
                            </a>
						<span class="divider">/</span>
                        <a href='<%# Request.RawUrl %>'><asp:Literal runat="server" ID="lblNavModPostazione" Text="<%$Resources:lblNavModPostazione %>" /></a>
                        <span class="divider">/</span>
					</li>
				</ul>
    <postazione:edit runat="server" id="editPst" />

</asp:Content>