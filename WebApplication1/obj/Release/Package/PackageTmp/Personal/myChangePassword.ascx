<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myChangePassword.ascx.cs" Inherits="KIS.Personal.myChangePassword" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblChangePassword">
    <tr>
        <td>Vecchia password:</td>
        <td><asp:TextBox runat="server" ID="txtOldPassword" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ID="valOldPassword"
                ErrorMessage="* Campo obbligatorio" ForeColor="Red" ControlToValidate="txtOldPassword"
                ValidationGroup="changePassword"
                />
        </td>
    </tr>
    <tr>
        <td>Nuova password:</td>
        <td><asp:TextBox runat="server" ID="txtNewPassword1" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ID="valNewPassword1"
                ErrorMessage="* Campo obbligatorio" ForeColor="Red" ControlToValidate="txtNewPassword1"
                ValidationGroup="changePassword"
                />
        </td>
    </tr>
    <tr>
        <td>Ripeti la nuova password:</td>
        <td><asp:Textbox runat="server" ID="txtNewPassword2" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ID="valNewPassword2"
                ErrorMessage="* Campo obbligatorio" ForeColor="Red" ControlToValidate="txtNewPassword2"
                ValidationGroup="changePassword"
                />
            <asp:CompareValidator id="valComparePassword" runat="server"
        ControlToValidate="txtNewPassword1"
        ControlToCompare="txtNewPassword2"
        Type="String"
        Operator="Equal"
        ErrorMessage="(*) Errore. I due campi NUOVA PASSWORD devono contenere il medesimo valore!"
        ForeColor="Red" />
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ValidationGroup="changePassword" ImageUrl="~/img/iconSave.jpg" ID="imgSavePassword" Height="30" OnClick="imgSavePassword_Click" />
            <asp:ImageButton runat="server" ImageUrl="~/img/iconUndo.png" Height="30" ID="imgUndoPassword" OnClick="imgUndoPassword_Click" />
        </td>
    </tr>
</table>

                </ContentTemplate>
    </asp:UpdatePanel>