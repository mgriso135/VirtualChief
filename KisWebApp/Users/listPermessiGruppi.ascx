
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listPermessiGruppi.ascx.cs" Inherits="KIS.Users.listPermessiGruppi" %>

<h1><asp:Label runat="server" ID="lblNome"></asp:Label></h1>
<asp:Label runat="server" ID="lbl1" />

<asp:Repeater runat="server" ID="rptPermessi" OnItemDataBound="rptPermessi_ItemDataBound">
    <HeaderTemplate><table class="table table-hover table-striped">
        <thead>
        <tr>
            <th></th>
            <th></th>
            <th><asp:Literal runat="server" ID="lblRead" Text="<%$Resources:lblRead %>" /></th>
            <th><asp:Literal runat="server" ID="lblWrite" Text="<%$Resources:lblWrite %>" /></th>
            <th><asp:Literal runat="server" ID="lblExecute" Text="<%$Resources:lblExecute %>" /></th>
        </tr>
            </thead><tbody>
            </HeaderTemplate>
    <ItemTemplate>
        
        <tr runat="server" id="tr1">
            <td><%# DataBinder.Eval(Container.DataItem, "NomePermesso") %>
                <td><%# DataBinder.Eval(Container.DataItem, "PermessoDesc") %></td>
                <asp:HiddenField runat="server" ID="idPerm" Value='<%# DataBinder.Eval(Container.DataItem, "IdPermesso") %>' />
            </td>
            <td><asp:CheckBox runat="server" ID="ckR" Checked='<%# DataBinder.Eval(Container.DataItem, "R") %>' AutoPostBack="true" OnCheckedChanged="ckR_CheckedChanged" /></td>
            <td><asp:CheckBox runat="server" ID="ckW" Checked='<%# DataBinder.Eval(Container.DataItem, "W") %>' AutoPostBack="true" OnCheckedChanged="ckW_CheckedChanged" /></td>
            <td><asp:CheckBox runat="server" ID="ckX" Checked='<%# DataBinder.Eval(Container.DataItem, "X") %>' AutoPostBack="true" OnCheckedChanged="ckX_CheckedChanged" /></td>
        </tr>
            
    </ItemTemplate>
    <FooterTemplate></tbody></table></FooterTemplate>
</asp:Repeater>