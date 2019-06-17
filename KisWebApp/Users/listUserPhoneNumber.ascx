<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUserPhoneNumber.ascx.cs" Inherits="KIS.Users.listUserPhoneNumber" %>
<h3><asp:Literal runat="server" ID="lblTHNumTel" Text="<%$Resources:lblPhoneNumbers %>" /></h3>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptUserPhoneNumbers" OnItemDataBound="rptUserPhoneNumbers_ItemDataBound" OnItemCommand="rptUserPhoneNumbers_ItemCommand">
    <HeaderTemplate>
        <table class="table table-bordered table-hover">
            <thead>
        <tr>
            <th></th>
            <th></th>
            <th><asp:Literal runat="server" ID="lblTHNumTel" Text="<%$Resources:lblTHNumTel %>" /></th>
            <th><asp:Literal runat="server" ID="lblTHAmbito" Text="<%$Resources:lblTHAmbito %>" /></th>
            <th><asp:Literal runat="server" ID="lblTHForAlarms" Text="<%$Resources:lblTHForAlarms %>" /></th>
            <th></th>
        </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' /></td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "PhoneNumber") %></td>
            <td><asp:Label runat="server" ID="lblNote" Text='<%#DataBinder.Eval(Container.DataItem, "Note") %>' />
                <asp:TextBox runat="server" ID="txtNote" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "Note") %>' />
            </td>
            <td><asp:CheckBox runat="server" ID="chkAlarm" Checked='<%#DataBinder.Eval(Container.DataItem, "ForAlarm") %>' Enabled="false" /></td>
            <td>
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' Visible="false" />
                <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="30" CommandName="undo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' Visible="false" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>