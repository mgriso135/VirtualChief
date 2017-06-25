﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzVarianteDettagli.ascx.cs" Inherits="KIS.Commesse.wzVarianteDettagli" %>
<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblDatiProdotto" class="table table-hover table-condensed table-bordered" width="100%">
    <tr>
        <td style="vertical-align:middle;" rowspan="2">
<asp:ImageButton runat="server" ID="imgEditNomeVariante" ImageUrl="~/img/edit.png" Height="40" OnClick="imgEditNomeVariante_Click" />
<asp:ImageButton runat="server" ID="imgSaveNomeVariante" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="imgSaveNomeVariante_Click" Visible="false" />
<asp:ImageButton runat="server" ID="imgUndoNomeVariante" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="imgUndoNomeVariante_Click" Visible="false" />
            </td>
        <td>
<asp:Label runat="server" ID="lblNomeVariante" />
<asp:TextBox runat="server" ID="txtNomeVariante" Visible="false" />
<asp:RequiredFieldValidator runat="server" ID="val1" ControlToValidate="txtNomeVariante" ForeColor="Red" ErrorMessage="<%$Resources:lblValReqField %>" />
</td>
        
        </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblDescVariante" />
<asp:TextBox runat="server" ID="txtDescVariante" TextMode="MultiLine" Visible="false" />
<asp:RequiredFieldValidator runat="server" ID="val2" ControlToValidate="txtDescVariante" ForeColor="Red" ErrorMessage="<%$Resources:lblValReqField %>" />
        </td>
    </tr>
    </table>