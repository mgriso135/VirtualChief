<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addNewItemToProductionPlan.ascx.cs" Inherits="KIS.Produzione.addNewItemToProductionPlan" %>

<asp:Label runat="server" ID="lbl1" />
<h2>Aggiungi un nuovo prodotto in coda al piano di produzione</h2>
Matricola / Batch ID:&nbsp;<asp:TextBox runat="server" ID="matricola" />
<br />
Modello:&nbsp;<asp:DropDownList runat="server" ID="ddlProcessiVarianti" />
<br />
<asp:ImageButton runat="server" ID="imgSaveProduct" ImageUrl="/img/iconSave.jpg" OnClick="imgSaveProduct_Click" Height="40" />