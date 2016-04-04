<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addNewItemToProductionPlan.ascx.cs" Inherits="WebApplication1.Produzione.addNewItemToProductionPlan" %>
<h2>Aggiungi un nuovo prodotto in coda al piano di produzione</h2>
Matricola / Batch ID:&nbsp;<asp:TextBox runat="server" ID="matricola" />
<br />
<asp:ImageButton runat="server" ID="imgSaveProduct" ImageUrl="/img/iconSave.jpg" OnClick="imgSaveProduct_Click" Height="40" />