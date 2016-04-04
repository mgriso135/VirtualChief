<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editUser.aspx.cs"
    Title="Kaizen Indicator System" MasterPageFile="~/Site.master" Inherits="KIS.Users.editUser" %>
<%@ Register TagPrefix="user" TagName="editInfo" Src="~/Users/editUserInfo.ascx" %>
<%@ Register TagPrefix="user" TagName="listGroups" Src="~/Users/listUserGruppi.ascx" %>
<%@ Register TagPrefix="user" TagName="addEmail" Src="~/Users/addUserEmail.ascx" %>
<%@ Register TagPrefix="user" TagName="emails" Src="~/Users/listUserEmails.ascx" %>
<%@ Register TagPrefix="user" TagName="listPhoneNumbers" Src="~/Users/listUserPhoneNumber.ascx" %>
<%@ Register TagPrefix="user" TagName="addPhoneNumbers" Src="~/Users/addUserPhoneNumber.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
        <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageUsers.aspx">Gestione utenti</a>
						<span class="divider">/</span>
                        <a href='<%# Request.RawUrl %>'>Modifica utente</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <user:editInfo runat="server" ID="frmEditUserInfo" />
    <table class="table table-bordered table-condensed table-hover">
        <tr>
            <td>
    <user:addEmail runat="server" id="frmAddEmail" />
    <user:emails runat="server" id="frmEditUserEmails" />
                </td>
            <td>
                <user:addPhoneNumbers runat="server" ID="frmAddPhoneNumbers" />
                <user:listPhoneNumbers runat="server" id="frmListPhoneNumbers" />
            </td>
        </tr>
            </table>
    <user:listGroups runat="server" id="frmManageUserGruppi" />
    </asp:Content>