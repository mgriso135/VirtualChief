<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditContattoDetails.ascx.cs" Inherits="KIS.Clienti.EditContattoDetails1" %>

<asp:Literal runat="server" ID="jsFireConfirm" />

<script type="text/javascript">
    function FireConfirmDelPhone() {
        if (confirm(Resources.delNumTelefono))
            return true;
        else
            return false;
    }

    function FireConfirmDelMail() {
        if (confirm(Resources.delEmail))
            return true;
        else
            return false;
    }
</script>

<h3><asp:Label runat="server" ID="lblTitleDettagliContatto" meta:resourcekey="lblTitleDettagliContatto" /></h3>
<asp:Label runat="server" ID="lbl1" />

<div class="row-fluid">
    <div class="span12">
        <table runat="server" id="tblContatto">
            <tr>
        <td>
            <asp:Label runat="server" ID="lblNominativo" meta:resourcekey="lblNominativo" />:
            </td>
            <td><asp:TextBox runat="server" ID="txtNominativo" ValidationGroup="contatto" MaxLength="255" />
                <asp:RequiredFieldValidator runat="server" ID="valNome" ValidationGroup="contatto" ControlToValidate="txtNominativo" ForeColor="Red" ErrorMessage="<%$Resources:lblReqField %>" />
            </td>
            </tr>
            <tr>
        <td>
            <asp:Label runat="server" ID="lblLastName" meta:resourcekey="lblLastName" />:
            </td>
            <td><asp:TextBox runat="server" ID="txtLastName" ValidationGroup="contatto" MaxLength="255" />
                <asp:RequiredFieldValidator runat="server" ID="valLastName" ValidationGroup="contatto" ControlToValidate="txtLastName" ForeColor="Red" ErrorMessage="<%$Resources:lblReqField %>" />
            </td>
            </tr>
            <tr>
                <td>
        <asp:Label runat="server" ID="lblRuolo" meta:resourcekey="lblRuolo" />:</td><td><asp:TextBox runat="server" ID="txtRuolo" MaxLength="255" />
            </td>
        </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSave_Click" ValidationGroup="contatto" />
                    <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndo_Click" />
                </td>
            </tr>
        </table>
    </div>
</div>

<div class="row-fluid">
    <div class="span6">
        <h4><asp:Label runat="server" ID="lblNumPhone" meta:resourcekey="lblNumPhone" /></h4>
        <asp:ImageButton runat="server" ID="btnShowAddPhone" ImageUrl="~/img/iconAdd2.png" Height="40" OnClick="btnShowAddPhone_Click" />
        <table runat="server" id="frmAddPhone" visible="false">
            <tr><td><asp:Label runat="server" ID="lblNumPhone2" meta:resourcekey="lblNumPhone2" /></td>
                <td><asp:TextBox runat="server" ID="txtNewPhone" MaxLength="255" ValidationGroup="NewPhone" />
                    <asp:RequiredFieldValidator runat="server" ID="valNewPhone" ControlToValidate="txtNewPhone" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="Red" ValidationGroup="NewPhone" />
                </td>
            </tr>
            <tr>
                <td><asp:Label runat="server" ID="lblNote" meta:resourcekey="lblNote" /></td>
                <td><asp:TextBox runat="server" ID="txtNoteNewPhone" TextMode="MultiLine" MaxLength="255" /></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:ImageButton runat="server" ID="btnNewPhoneSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnNewPhoneSave_Click" ValidationGroup="NewPhone" />
                    <asp:ImageButton runat="server" ID="btnNewPhoneUndo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnNewPhoneUndo_Click" />
                </td>
            </tr>
        </table>

        <asp:Repeater runat="server" ID="rptPhones" OnItemCommand="rptPhones_ItemCommand">
            <HeaderTemplate>
                <table class="table table-condensed table-hover table-striped">
                    <thead>
                        <td>
                            
                        </td>
                        <td><asp:Label runat="server" ID="lblNumPhone3" meta:resourcekey="lblNumPhone2" /></td>
                        <td><asp:Label runat="server" ID="lblNote2" meta:resourcekey="lblNote" /></td>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="20" OnClientClick="return FireConfirmDelPhone()" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Phone") %>' />
                        </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Phone") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Note") %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        
    </div>
    <div class="span6">
        <h4><asp:Label runat="server" ID="lblTitleEmails" meta:resourcekey="lblTitleEmails" /></h4>
        <asp:ImageButton runat="server" ID="btnShowAddEmail" ImageUrl="~/img/iconAdd2.png" Height="40" OnClick="btnShowAddEmail_Click" />
        <table runat="server" id="frmAddEmail" visible="false">
            <tr><td><asp:Label runat="server" ID="lblEmailAddr" meta:resourcekey="lblEmailAddr" /></td>
                <td><asp:TextBox runat="server" ID="txtNewMail" MaxLength="255" ValidationGroup="NewEMail" />
                    <asp:RequiredFieldValidator runat="server" ID="valMail" ControlToValidate="txtNewMail" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="Red" ValidationGroup="NewEMail" />
                </td>
            </tr>
            <tr>
                <td><asp:Label runat="server" ID="lblNote3" meta:resourcekey="lblNote" /></td>
                <td><asp:TextBox runat="server" ID="txtNoteNewMail" TextMode="MultiLine" MaxLength="255" /></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:ImageButton runat="server" ID="btnNewMailSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnNewMailSave_Click" ValidationGroup="NewEMail" />
                    <asp:ImageButton runat="server" ID="btnNewMailUndo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnNewMailUndo_Click" />
                </td>
            </tr>
        </table>

        <asp:Repeater runat="server" ID="rptEmails" OnItemCommand="rptEmails_ItemCommand">
            <HeaderTemplate>
                <table class="table table-condensed table-hover table-striped">
                    <thead>
                        <td>
                            
                        </td>
                        <td><asp:Label runat="server" ID="lblEmailAddr" meta:resourcekey="lblEmailAddr" /></td>
                        <td><asp:Label runat="server" ID="lblNote4" meta:resourcekey="lblNote" /></td>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Height="20" OnClientClick="return FireConfirmDelMail()" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EMail") %>' />
                        </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "EMail") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Note") %>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>