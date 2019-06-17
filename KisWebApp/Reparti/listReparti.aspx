<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="listReparti.aspx.cs" Inherits="KIS.Reparti.listReparti1" %>
<%@ Register TagPrefix="reparti" TagName="list" Src="listReparti.ascx" %>
<%@ Register TagPrefix="reparti" TagName="add" Src="addReparto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 runat="server" id="lnkShowAddReparti">
    <asp:Literal runat="server" ID="lblAddReparti" Text="<%$Resources:lblAddReparti %>" />
    <asp:ImageButton CssClass="img-rounded" runat="server" ID="showAddReparti" ImageUrl="~/img/iconAdd.jpg" Height="50px" OnClick="showAddReparti_Click" ToolTip="<%$Resources:lblAddReparti %>"/></h3>
    <br />
    <reparti:add runat="server" id="addReparti" visible="false" />
    <br />
    <reparti:list runat="server" id="listReparti" />
</asp:Content>
