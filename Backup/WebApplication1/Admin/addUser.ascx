<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUser.ascx.cs" Inherits="WebApplication1.Admin.addUser" %>

<asp:ImageButton runat="server" ID="btnUserAdd" ImageUrl="/img/iconUserAdd.jpg" OnClick="btnUserAdd_Click" Height="100" />
<br />
<table border="0" runat="server" id="tblInputNewUser">
<tr><td colspan="2" style="font-size:14px; font-weight: bold;">Nuovo utente</td></tr>
<tr>
<td><asp:Label runat="server" ID="lblName">Nome:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputNome" />
<asp:RequiredFieldValidator id="valNome" runat="server"
        ControlToValidate="inputNome" 
        ErrorMessage="(*) Errore... Il campo NOME è un campo obbligatorio!"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblCognome">Cognome:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputCognome" />
<asp:RequiredFieldValidator id="valCognome" runat="server"
        ControlToValidate="inputCognome" 
        ErrorMessage="(*) Errore... Il campo NOME è un campo obbligatorio!"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblUsername">Username:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputUsername" />
<asp:RequiredFieldValidator id="valUsername" runat="server"
        ControlToValidate="inputUsername" 
        ErrorMessage="(*) Errore... Il campo NOME è un campo obbligatorio!"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblPassword">Password:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword" runat="server"
        ControlToValidate="inputPassword" 
        ErrorMessage="(*) Errore... Il campo NOME è un campo obbligatorio!"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblRptPassword">Ripeti password:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword2" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword2" runat="server"
        ControlToValidate="inputPassword2" 
        ErrorMessage="(*) Errore... Il campo NOME è un campo obbligatorio!"
        ForeColor="Red" /><br />
<asp:CompareValidator id="valComparePassword" runat="server"
        ControlToValidate="inputPassword"
        ControlToCompare="inputPassword2"
        Type="String"
        Operator="Equal"
        ErrorMessage="(*) Errore... I due campi password devono contenere il medesimo valore!"
        ForeColor="Red" >
    </asp:CompareValidator>
</td>
</tr>
<tr>
<td>Tipo di utente:</td>
<td>
<asp:DropDownList ID="tipoUtente" runat="server">
<asp:ListItem runat="server" Value="User" Text="Utente" />
<asp:ListItem runat="server" Value="Admin" Text="Admin" />
</asp:DropDownList>
</td>
</tr>
<tr>
<td colspan="2">
<asp:ImageButton runat="server" ID="btnSaveUser" ImageUrl="/img/iconSave.jpg" Height="40" OnClick="btnSaveUser_Click" CausesValidation="true" />&nbsp;
<asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconCancel.jpg" Height="40" OnClick="btnUndo_Click" CausesValidation="false" />
</td>
</tr>
<tr><td colspan="2"><asp:Label runat="server" id="lblEsito" /></td></tr>
</table>