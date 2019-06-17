<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageGruppi.aspx.cs"
    MasterPageFile="~/Site.Master" Title="Virtual Chief" Inherits="KIS.Users.manageGruppi" %>
<%@ Register TagPrefix="gruppi" TagName="add" Src="~/Users/addGruppo.ascx" %>
<%@ Register TagPrefix="gruppi" TagName="list" Src="~/Users/listGruppi.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="../Admin/admin.aspx">
                            <asp:Literal runat="server" ID="lblNavAdmin" Text="<%$Resources:lblNavAdmin %>" /></a>
						<span class="divider">/</span>
						<a href="manageGruppi.aspx"><asp:Literal runat="server" ID="lblNavGestGruppi" Text="<%$Resources:lblNavGestGruppi %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <gruppi:add runat="server" id="frmAddGruppo" />
    <gruppi:list runat="server" ID="fromLstGruppi" />
    </asp:Content>