<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="linkSubProcessVariante.ascx.cs" Inherits="KIS.Processi.linkSubProcessVariante" %>
<b>Collega un task già esistente alla variante:&nbsp;</b>
<asp:DropDownList runat="server" ID="ddlTasks" />
<asp:ImageButton runat="server" ID="btnLnkTask" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="btnLnkTask_Click" />
<br /><asp:Label runat="server" ID="lbl1" />