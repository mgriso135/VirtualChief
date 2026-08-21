<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="menuGruppi.aspx.cs" Inherits="KIS.Admin.menuGruppi" %>
<%@ Register TagPrefix="menuGruppi" TagName="listVoci" Src="~/Admin/MenuGruppiList.ascx" %>
<%@ Register TagPrefix="menuGruppi" TagName="addVoce" Src="~/Admin/MenuGruppiAddVoce.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h3><asp:Label runat="server" ID="lblGruppo" /></h3>
    <asp:Label runat="server" ID="lbl1" />

    <menuGruppi:listVoci runat="server" id="frmListVoci" />
    <menuGruppi:addVoce runat="server" id="frmAddVoce" />

    
</asp:Content>
