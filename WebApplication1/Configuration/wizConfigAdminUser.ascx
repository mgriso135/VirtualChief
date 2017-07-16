<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wizConfigAdminUser.ascx.cs" Inherits="KIS.Configuration.wizConfigAdminUser1" %>

<div class="row-fluid">
    <div class="span12">
<h3><asp:Literal runat="server" ID="lblTitleAdminCfg" Text="<%$Resources:lblAdminCfg %>" /></h3>
        <asp:Label runat="server" ID="lbl1" />
        <table class="table table-condensed table-hover" runat="server" id="tblInputNewUser">
    <thead>
<tr><td colspan="2" style="font-size:14px; font-weight: bold;"><asp:Literal runat="server" ID="lblNewUser" Text="<%$Resources:lblNewUser %>" /></td></tr>
        </thead>
    <tbody>
<tr>
<td><asp:Label runat="server" ID="lblName" Text="<%$Resources:lblNome %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputNome" />
<asp:RequiredFieldValidator id="valNome" runat="server"
        ControlToValidate="inputNome" 
        ErrorMessage="<%$Resources:lblValNomeReq %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblCognome" Text="<%$Resources:lblCognome %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputCognome" />
<asp:RequiredFieldValidator id="valCognome" runat="server"
        ControlToValidate="inputCognome" 
        ErrorMessage="<%$Resources:lblValCognomeReq %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblUsername" Text="<%$Resources:lblUsername %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputUsername" />
<asp:RequiredFieldValidator id="valUsername" runat="server"
        ControlToValidate="inputUsername" 
        ErrorMessage="<%$Resources:lblValUsernameReq %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblPassword" Text="<%$Resources:lblPassword %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword" runat="server"
        ControlToValidate="inputPassword" 
        ErrorMessage="<%$Resources:lblValPasswordReq %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblRptPassword" Text="<%$Resources:lblPassword2 %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword2" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword2" runat="server"
        ControlToValidate="inputPassword2" 
        ErrorMessage="<%$Resources:lblValPasswordReq %>"
        ForeColor="Red" /><br />
<asp:CompareValidator id="valComparePassword" runat="server"
        ControlToValidate="inputPassword"
        ControlToCompare="inputPassword2"
        Type="String"
        Operator="Equal"
        ErrorMessage="<%$Resources:lblValPasswordMatch %>"
        ForeColor="Red" >
    </asp:CompareValidator>
</td>
</tr>
        <tr>
        <td><asp:literal runat="server" ID="lblIdioma" Text="<%$Resources:lblIdioma %>" /></td>
        <td><asp:DropDownList runat="server" ID="ddlLanguages" AppendDataBoundItems="true">
        <asp:ListItem Value="en">English</asp:ListItem>
        <asp:ListItem Value="es">Español</asp:ListItem>
        <asp:ListItem Value="it">Italiano</asp:ListItem>
        </asp:DropDownList></td>
            </tr>
        </tbody>
    <tfoot>
<tr>
<td colspan="2">
<asp:ImageButton runat="server" ID="btnSaveUser" ImageUrl="../img/iconSave.jpg" Height="40" OnClick="btnSaveUser_Click" CausesValidation="true" />&nbsp;
<asp:ImageButton runat="server" ID="btnUndo" ImageUrl="../img/iconCancel.jpg" Height="40" OnClick="btnUndo_Click" CausesValidation="false" />
</td>
</tr>
<tr><td colspan="2"><asp:Label runat="server" id="lblEsito" /></td></tr>
        </tfoot>
</table>
        </div>
    </div>
