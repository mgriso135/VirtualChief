﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="linkArticoli.ascx.cs" Inherits="KIS.Commesse.linkArticoli" %>

<table runat="server" id="frmLinkArticolo" class="table table-condensed">
    <tr>
        <td><asp:Label runat="server" ID="lblProdotto" meta:resourcekey="lblProdotto" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlArticoli" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$resources:valDDEligir %>" Value="-1" />
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblQuantita" meta:resourcekey="lblQuantita" /></td>
        <td><asp:TextBox runat="server" ID="txtQuantita" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ForeColor="Red" ControlToValidate="txtQuantita" ErrorMessage="<%$resources:valReqField %>" ValidationGroup="articolo" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblDataConsegnaPrevista" meta:resourcekey="lblDataConsegnaPrevista" />:</td>
        <td><asp:Calendar runat="server" ID="consegnaprevista" /></td>
    </tr>
    <tr><td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="40" ToolTip="<%$resources:lblTTSalva %>" OnClick="btnSave_Click" ValidationGroup="articolo" />
        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="40" ToolTip="<%$resources:lblTTFormReset %>" OnClick="btnUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />