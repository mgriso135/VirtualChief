<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addTempoCiclo.ascx.cs" Inherits="KIS.Processi.addTempoCiclo" %>
Aggiungi tempo ciclo<asp:ImageButton runat="server" ID="imgShowFrmAddTempo" OnClick="imgShowFrmAddTempo_Click" ImageUrl="/img/iconAdd.jpg" Height="30" ToolTip="Aggiungi un tempo ciclo al processo" />
<asp:HiddenField runat="server" ID="taskID" />
<asp:HiddenField runat="server" ID="varID" />
<asp:UpdatePanel runat="server" ID="updAddTempo">
    <ContentTemplate>
<table runat="server" id="frmAddTempo" style="border: 1px dotted green">
    <tr>
        <td>Numero operatori</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlNumOp" Width="80"/>
        </td>
    </tr>
    <tr>
        <td>Setup</td>
        <td>
            ore:<asp:DropdownList runat="server" Width="80" ID="oreSetup"/>mm:<asp:DropdownList runat="server" ID="minSetup" Width="60" />ss:<asp:DropdownList runat="server" ID="secSetup" Width="60"/>
            </td>
    </tr>
    <tr>
        <td>Tempo ciclo</td>
        <td>
            ore:<asp:DropdownList runat="server" Width="80" ID="ore"/>mm:<asp:DropdownList runat="server" ID="minuti" Width="60" />ss:<asp:DropdownList runat="server" ID="secondi" Width="60"/>
            </td>
    </tr>
    <tr>
        <td colspan="2"><asp:CheckBox runat="server" ID="chkDefault" Checked="true" ToolTip="Se selezionato, nel calcolo del percorso critico viene considerato questo tempo ciclo, e nel calcolo delle risorse impegnate vengono considerati questi operatori" />Default</td>
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