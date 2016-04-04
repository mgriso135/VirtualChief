<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myEmail.ascx.cs" Inherits="KIS.Personal.myEmail" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:ImageButton Height="30" runat="server" ID="imgShowFormAddEmail" ImageUrl="~/img/iconAdd2.png" OnClick="imgShowFormAddEmail_Click" />Aggiungi un indirizzo e-mail
<table runat="server" id="frmAddEmail" class="table table-bordered table-hover table-condensed">
    <tr>
        <td>E-mail</td>
        <td>Ambito (es: casa, ufficio)</td>
        <td>Per segnalazione allarmi</td>
    </tr>
    <tr>
        <td><asp:TextBox runat="server" ID="txtEmail" ValidationGroup="addmail" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="addmail" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="* Campo obbligatorio" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAmbito" ValidationGroup="addmail" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="addmail" ControlToValidate="txtAmbito" ForeColor="Red" ErrorMessage="* Campo obbligatorio" />
        </td>
        <td><asp:CheckBox runat="server" ID="chkForAlarm" /></td>
        <td>
            <asp:ImageButton runat="server" ValidationGroup="addmail" ImageUrl="~/img/iconSave.jpg" ID="imgSaveMail" Height="30" OnClick="imgSaveMail_Click" />
            <asp:ImageButton runat="server" ImageUrl="~/img/iconUndo.png" Height="30" ID="imgUndoMail" OnClick="imgUndoMail_Click" />
        </td>
    </tr>
</table>
        <asp:Label runat="server" ID="lbl1" />

        <asp:Repeater runat="server" ID="rptListMail" OnItemCommand="rptListMail_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-condensed">
                    <thead>
                    <tr>
                        <td></td>
                        <td>E-mail</td>
                        <td>Ambito</td>
                        <td>Per allarmi</td>
                    </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Email") %>' />
                        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Email") %>' Visible="false" />
                        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="30" CommandName="undo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Email") %>' Visible="false" />
                    </td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Email") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Note") %></td>
                    <td><asp:CheckBox runat="server" id="chkAlarm" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "forAlarm") %>' /></td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Email") %>' Visible="false" />
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