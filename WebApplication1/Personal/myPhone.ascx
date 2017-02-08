<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myPhone.ascx.cs" Inherits="KIS.Personal.myPhone" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:ImageButton Height="30" runat="server" ID="imgShowFormAddPhone" ImageUrl="~/img/iconAdd2.png" OnClick="imgShowFormAddPhone_Click" />Aggiungi un numero di telefono
<table runat="server" id="frmAddPhone" class="table table-bordered table-hover table-condensed">
    <tr>
        <td><asp:label runat="server" id="lblPhoneNumber" meta:resourcekey="lblPhoneNumber" /></td>
        <td><asp:label runat="server" id="lblAmbito" meta:resourcekey="lblAmbito" /></td>
        <td><asp:label runat="server" id="lblForAlarm" meta:resourcekey="lblForAlarm" /></td>
    </tr>
    <tr>
        <td><asp:TextBox runat="server" ID="txtPhone" ValidationGroup="addphone" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="addphone" ControlToValidate="txtPhone" ForeColor="Red" ErrorMessage="* Campo obbligatorio" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAmbito" ValidationGroup="addphone" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="addphone" ControlToValidate="txtAmbito" ForeColor="Red" ErrorMessage="* Campo obbligatorio" />
        </td>
        <td><asp:CheckBox runat="server" ID="chkForAlarm" /></td>
        <td>
            <asp:ImageButton runat="server" ValidationGroup="addphone" ImageUrl="~/img/iconSave.jpg" ID="imgSavePhone" Height="30" OnClick="imgSavePhone_Click" />
            <asp:ImageButton runat="server" ImageUrl="~/img/iconUndo.png" Height="30" ID="imgUndoPhone" OnClick="imgUndoPhone_Click" />
        </td>
    </tr>
</table>
        <asp:Label runat="server" ID="lbl1" />

        <asp:Repeater runat="server" ID="rptListPhone" OnItemCommand="rptListPhone_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-condensed">
                    <thead>
                    <tr>
                        <td></td>
                        <td><asp:label runat="server" id="lblPhoneNumber2" meta:resourcekey="lblPhoneNumber" /></td>
                        <td><asp:label runat="server" id="lblAmbito2" meta:resourcekey="lblAmbito" /></td>
                        <td><asp:label runat="server" id="lblForAlarm2" meta:resourcekey="lblForAlarm" /></td>
                    </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' />
                        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' Visible="false" />
                        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="30" CommandName="undo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' Visible="false" />
                    </td>
                    <td><%# DataBinder.Eval(Container.DataItem, "PhoneNumber") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Note") %></td>
                    <td><asp:CheckBox runat="server" id="chkAlarm" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "forAlarm") %>' /></td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PhoneNumber") %>' Visible="false" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        </ContentTemplate>
    </asp:UpdatePanel>