<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditContattoDetails.ascx.cs" Inherits="KIS.Clienti.EditContattoDetails1" %>

<script type="text/javascript">
    function FireConfirmDelPhone() {
        if (confirm('Sei sicuro di voler eliminare questo numero di telefono?'))
            return true;
        else
            return false;
    }

    function FireConfirmDelMail() {
        if (confirm('Sei sicuro di voler eliminare questo indirizzo e-mail?'))
            return true;
        else
            return false;
    }
</script>

<h3>Dettagli contatto</h3>
<asp:Label runat="server" ID="lbl1" />

<div class="row-fluid">
    <div class="span12">
        <table runat="server" id="tblContatto">
            <tr>
        <td>
        Nominativo:
            </td>
            <td><asp:TextBox runat="server" ID="txtNominativo" ValidationGroup="contatto" MaxLength="255" />
                <asp:RequiredFieldValidator runat="server" ID="valNome" ValidationGroup="contatto" ControlToValidate="txtNominativo" ForeColor="Red" ErrorMessage="* Campo obbligatorio" />
            </td>
            </tr>
            <tr>
                <td>
        Ruolo:</td><td><asp:TextBox runat="server" ID="txtRuolo" MaxLength="255" />
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
        <h4>Numeri di telefono</h4>
        <asp:ImageButton runat="server" ID="btnShowAddPhone" ImageUrl="~/img/iconAdd2.png" Height="40" OnClick="btnShowAddPhone_Click" />
        <table runat="server" id="frmAddPhone" visible="false">
            <tr><td>Numero di telefono</td>
                <td><asp:TextBox runat="server" ID="txtNewPhone" MaxLength="255" ValidationGroup="NewPhone" />
                    <asp:RequiredFieldValidator runat="server" ID="valNewPhone" ControlToValidate="txtNewPhone" ErrorMessage="* Campo obbligatorio" ForeColor="Red" ValidationGroup="NewPhone" />
                </td>
            </tr>
            <tr>
                <td>Note</td>
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
                        <td>Numero di telefono</td>
                        <td>Note</td>
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
        <h4>E-mails</h4>
        <asp:ImageButton runat="server" ID="btnShowAddEmail" ImageUrl="~/img/iconAdd2.png" Height="40" OnClick="btnShowAddEmail_Click" />
        <table runat="server" id="frmAddEmail" visible="false">
            <tr><td>Indirizzo e-mail</td>
                <td><asp:TextBox runat="server" ID="txtNewMail" MaxLength="255" ValidationGroup="NewEMail" />
                    <asp:RequiredFieldValidator runat="server" ID="valMail" ControlToValidate="txtNewMail" ErrorMessage="* Campo obbligatorio" ForeColor="Red" ValidationGroup="NewEMail" />
                </td>
            </tr>
            <tr>
                <td>Note</td>
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
                        <td>Indirizzo e-mail</td>
                        <td>Note</td>
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