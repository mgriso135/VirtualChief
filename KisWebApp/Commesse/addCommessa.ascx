<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addCommessa.ascx.cs" Inherits="KIS.Commesse.addCommessa" %>

<asp:HyperLink runat="server" ID="lnkAddCommessa" NavigateUrl="~/SalesOrders/SalesOrder/AddSalesOrder?OrderID=-1&OrderYear=-1">
<asp:Image CssClass="img-rounded" runat="server" ID="btnShowFrmAddCommessa" Height="40px" ImageUrl="~/img/iconAdd.jpg" ToolTip="Aggiungi una nuova commessa" />
    </asp:HyperLink>
<asp:Label runat="server" ID="lblTxtAddCommessa" meta:resourcekey="lblAddCommessa" /><br />

<table runat="server" id="frmAddCommessa" class="table table-bordered table-hover">
    <tr><td><asp:Label runat="server" ID="lblCliente" meta:resourcekey="lblCliente" /></td><td>
        <asp:DropDownList runat="server" ID="ddlCliente" />
        <a href="../Clienti/AddCliente.aspx"><asp:Label runat="server" ID="lblAddCliente" meta:resourcekey="lblAddCliente" /></a>
        </td></tr>
    <tr><td><asp:Label runat="server" ID="lblExternalID" meta:resourcekey="lblExternalID" /></td><td><asp:TextBox runat="server" ID="txtExternalID" />
                     </td></tr>
    <tr><td><asp:Label runat="server" ID="lblNote" meta:resourcekey="lblNote" /></td><td><asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" />
                     </td></tr>
    
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="40" ToolTip="<%$Resources:lblTTSave %>" OnClick="btnSave_Click" ValidationGroup="commessa" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="40" ToolTip="<%$Resources:lblTTReset %>" OnClick="btnUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />