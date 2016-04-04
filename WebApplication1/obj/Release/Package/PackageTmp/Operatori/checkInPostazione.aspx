<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="checkInPostazione.aspx.cs" Inherits="KIS.Operatori.checkInPostazione" %>
<%@ Register TagPrefix="user" TagName="loginPostazione" Src="~/Operatori/userLoginPostazione.ascx" %>
<%@ Register TagPrefix="user" TagName="postazioniAttive" Src="~/Operatori/operatorePostazioniAttive.ascx" %>
<%@ Register TagPrefix="user" TagName="listTaskAvviati" Src="~/Produzione/listTaskAvviatiUtente.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="#">WorkPlace</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="checkInPostazione.aspx">Web Gemba</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <user:listTaskAvviati runat="server" id="frmListTaskAvviatiUtenti" />
    <div class="row-fluid">
        <span class="span6">
        <user:postazioniAttive runat="server" id="frmPostazioniAttive" />
            </span>
        <span class="span6">
            <user:loginPostazione runat="server" id="frmLoginPostazione" />
        </span>
    </div>
    <table>
        <tr style="vertical-align:top">
            <td>
    
    </td><td>
    
            </td></tr>
        </table>
</asp:Content>
