﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wizConfigAdminUser.ascx.cs" Inherits="KIS.Configuration.wizConfigAdminUser1" %>

<div class="row-fluid">
    <div class="span12">
<h3>Admin User Configuration</h3>
        <asp:Label runat="server" ID="lbl1" />
        <table class="table table-condensed table-hover" runat="server" id="tblInputNewUser">
    <thead>
<tr><td colspan="2" style="font-size:14px; font-weight: bold;">Nuovo utente</td></tr>
        </thead>
    <tbody>
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
        </tbody>
    <tfoot>
<tr>
<td colspan="2">
<asp:ImageButton runat="server" ID="btnSaveUser" ImageUrl="/img/iconSave.jpg" Height="40" OnClick="btnSaveUser_Click" CausesValidation="true" />&nbsp;
<asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconCancel.jpg" Height="40" OnClick="btnUndo_Click" CausesValidation="false" />
</td>
</tr>
<tr><td colspan="2"><asp:Label runat="server" id="lblEsito" /></td></tr>
        </tfoot>
</table>
        </div>
    </div>
