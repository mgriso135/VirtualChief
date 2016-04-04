<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productionPlan.aspx.cs" 
Inherits="KIS.Produzione.productionPlan" MasterPageFile="~/Site.Master" %>

<%@ Register TagPrefix="plan" TagName="show" Src="showProductionPlan.ascx" %>
<%@ Register TagPrefix="plan" TagName="addNewItem" Src="addNewItemToProductionPlan.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:Label runat="server" ID="lbl" />
    <h1><asp:Label runat="server" ID="lblTitle" /></h1>
<asp:ImageButton runat="server" ID="showAddItemForm" ImageUrl="/img/iconAdd.jpg" height="40px" OnClick="showAddItemForm_Click"/><br />
<plan:addNewItem runat="server" id="addNewItemForm" />
<asp:ImageButton runat="server" ID="hideAddItemForm" ImageUrl="/img/iconCancel.jpg" height="40px" OnClick="hideAddItemForm_Click"/>
<plan:show runat="server" id="showProductionPlan" />
</asp:Content>