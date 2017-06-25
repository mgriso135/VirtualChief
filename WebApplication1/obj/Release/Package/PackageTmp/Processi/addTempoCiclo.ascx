<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addTempoCiclo.ascx.cs" Inherits="KIS.Processi.addTempoCiclo" %>

<asp:Literal runat="server" ID="lblAddTC" Text="<%$Resources:lblAddTC %>" />
<asp:ImageButton runat="server" ID="imgShowFrmAddTempo" OnClick="imgShowFrmAddTempo_Click" ImageUrl="/img/iconAdd.jpg" Height="30" ToolTip="<%$Resources:lblTTAddTC %>" />
<asp:HiddenField runat="server" ID="taskID" />
<asp:HiddenField runat="server" ID="varID" />
<asp:UpdatePanel runat="server" ID="updAddTempo">
    <ContentTemplate>
<table runat="server" id="frmAddTempo" style="border: 1px dotted green">
    <tr>
        <td><asp:Literal runat="server" ID="lblNumOp" Text="<%$Resources:lblNumOp %>" /></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlNumOp" Width="80"/>
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblSetup" Text="<%$Resources:lblSetup %>" /></td>
        <td>
            <asp:Literal runat="server" ID="lblOre" Text="<%$Resources:lblOre %>" />:<asp:DropdownList runat="server" Width="80" ID="oreSetup"/>
            <asp:Literal runat="server" ID="lblMinuti" Text="<%$Resources:lblMinuti %>" />:<asp:DropdownList runat="server" ID="minSetup" Width="60" />
            <asp:Literal runat="server" ID="lblSecondi" Text="<%$Resources:lblSecondi %>" />:<asp:DropdownList runat="server" ID="secSetup" Width="60"/>
            </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblTempoCiclo" Text="<%$Resources:lblTempoCiclo %>" /></td>
        <td>
            <asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblOre %>" />:<asp:DropdownList runat="server" Width="80" ID="ore"/>
            <asp:Literal runat="server" ID="Literal2" Text="<%$Resources:lblMinuti %>" />:<asp:DropdownList runat="server" ID="minuti" Width="60" />
            <asp:Literal runat="server" ID="Literal3" Text="<%$Resources:lblSecondi %>" />:<asp:DropdownList runat="server" ID="secondi" Width="60"/>
            </td>
    </tr>
    <tr>
        <td colspan="2"><asp:CheckBox runat="server" ID="chkDefault" Checked="true" ToolTip="<%$Resources:lblTTDefaultTime %>" /><asp:Literal runat="server" ID="lblDefault" Text="<%$Resources:lblDefault %>" /></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="30" OnClick="btnSave_Click" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconUndo.png" Height="30" OnClick="btnUndo_Click" />
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="lbl1" />
        </ContentTemplate>
    </asp:UpdatePanel>