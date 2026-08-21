<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUserGruppi.ascx.cs" Inherits="KIS.Users.listUserGruppi" %>

<asp:Label runat="server" ID="lbl1" />

<h3><asp:Label runat="server" ID="lblNome" /></h3>
<asp:Repeater runat="server" ID="rptGruppi" OnItemDataBound="rptGruppi_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-condensed table-striped table-hover">
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:CheckBox runat="server" ID="ck" AutoPostBack="true" OnCheckedChanged="ck_CheckedChanged" />
                <asp:HiddenField runat="server" id="grp" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "Nome") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>