<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUserEmails.ascx.cs" Inherits="KIS.Users.listUserEmails" %>
<h3>Indirizzi e-mail</h3>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptUserEmails" OnItemDataBound="rptUserEmails_ItemDataBound" OnItemCommand="rptUserEmails_ItemCommand">
    <HeaderTemplate>
        <table class="table table-bordered table-hover">
            <thead>
        <tr>
            <td></td>
            <td></td>
            <td><asp:Literal runat="server" ID="lblEmailAddr" Text="<%$Resources:lblEmailAddr %>" /></td>
            <td><asp:Literal runat="server" ID="lblAmbito" Text="<%$Resources:lblAmbito %>" /></td>
            <td><asp:Literal runat="server" ID="lblForAlarm" Text="<%$Resources:lblForAlarm %>" /></td>
            <td></td>
        </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Email") %>' /></td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Email") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Email") %></td>
            <td><asp:Label runat="server" ID="lblNote" Text='<%#DataBinder.Eval(Container.DataItem, "Note") %>' />
                <asp:TextBox runat="server" ID="txtNote" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "Note") %>' />
            </td>
            <td><asp:CheckBox runat="server" ID="chkAlarm" Checked='<%#DataBinder.Eval(Container.DataItem, "ForAlarm") %>' Enabled="false" /></td>
            <td>
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Email") %>' Visible="false" />
                <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="30" CommandName="undo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Email") %>' Visible="false" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>