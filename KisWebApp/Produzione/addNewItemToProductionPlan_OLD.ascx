<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addNewItemToProductionPlan_OLD.ascx.cs" Inherits="KIS.Produzione.addNewItemToProductionPlan" %>

<asp:Label runat="server" ID="lbl1" />
<h2><asp:Literal runat="server" ID="lblTitleAddProduct" Text="<%$Resources:lblTitleAddProduct %>" />
    </h2>
<asp:Literal runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" />:&nbsp;<asp:TextBox runat="server" ID="matricola" />
<br />
<asp:Literal runat="server" ID="lblModello" Text="<%$Resources:lblModello %>" />:&nbsp;<asp:DropDownList runat="server" ID="ddlProcessiVarianti" />
<br />
<asp:ImageButton runat="server" ID="imgSaveProduct" ImageUrl="/img/iconSave.jpg" OnClick="imgSaveProduct_Click" Height="40" />