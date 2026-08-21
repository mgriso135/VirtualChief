<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myEmail.ascx.cs" Inherits="KIS.Personal.myEmail" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:ImageButton Height="30" runat="server" ID="imgShowFormAddEmail" ImageUrl="~/img/iconAdd2.png" OnClick="imgShowFormAddEmail_Click" />
        <asp:Literal runat="server" ID="lblAddEmail" Text="<%$Resources:lblAddEmail %>" />
<table runat="server" id="frmAddEmail" class="table table-bordered table-hover table-condensed">
    <thead>
    <tr>
        <th><asp:label runat="server" id="lblEmail" meta:resourcekey="lblEmail" /></th>
        <th><asp:label runat="server" id="lblAmbito" meta:resourcekey="lblAmbito" /></th>
        <th><asp:label runat="server" id="lblAlarmBox" meta:resourcekey="lblAlarmBox" /></th>
    </tr>
        </thead>
    <tbody>
    <tr>
        <td><asp:TextBox runat="server" ID="txtEmail" ValidationGroup="addmail" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="addmail" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="<%$resources:valEmail %>" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtAmbito" ValidationGroup="addmail" />
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="addmail" ControlToValidate="txtAmbito" ForeColor="Red" ErrorMessage="<%$resources:valAmbito %>" />
        </td>
        <td><asp:CheckBox runat="server" ID="chkForAlarm" /></td>
        <td>
            <asp:ImageButton runat="server" ValidationGroup="addmail" ImageUrl="~/img/iconSave.jpg" ID="imgSaveMail" Height="30" OnClick="imgSaveMail_Click" />
            <asp:ImageButton runat="server" ImageUrl="~/img/iconUndo.png" Height="30" ID="imgUndoMail" OnClick="imgUndoMail_Click" />
        </td>
    </tr>
        </tbody>
</table>
        <asp:Label runat="server" ID="lbl1" />

        <asp:Repeater runat="server" ID="rptListMail" OnItemCommand="rptListMail_ItemCommand">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-condensed">
                    <thead>
                    <tr>
                        <th></th>
                        <th><asp:label runat="server" id="lblHeadEmail" meta:resourcekey="lblHeadEmail" /></th>
                        <th><asp:label runat="server" id="lblHeadAmbito" meta:resourcekey="lblHeadAmbito" /></th>
                        <th><asp:label runat="server" id="lblHeadForAlarm" meta:resourcekey="lblHeadForAlarm" /></th>
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