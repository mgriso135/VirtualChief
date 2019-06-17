<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUser.ascx.cs" Inherits="KIS.Admin.addUser" %>
<asp:Label runat="server" ID="lbl1" />
<asp:ImageButton runat="server" ID="btnUserAdd" ImageUrl="~/img/iconUserAdd.jpg" OnClick="btnUserAdd_Click" CssClass="img-rounded" Height="100" />
<br />
<table class="table table-condensed table-hover" runat="server" id="tblInputNewUser">
    <thead>
<tr><th>
    <asp:literal runat="server" ID="lblTHNewUser" Text="<%$Resources:lblTHNewUser %>" />
    </th></tr>
        </thead>
    <tbody>
<tr>
<td><asp:Label runat="server" ID="lblName" Text="<%$Resources:lblNome %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputNome" />
<asp:RequiredFieldValidator id="valNome" runat="server"
        ControlToValidate="inputNome" 
        ErrorMessage="<%$Resources:lblReqField %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblCognome" Text="<%$Resources:lblCognome %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputCognome" />
<asp:RequiredFieldValidator id="valCognome" runat="server"
        ControlToValidate="inputCognome" 
        ErrorMessage="<%$Resources:lblReqField %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblUsername" Text="<%$Resources:lblUsername %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputUsername" />
<asp:RequiredFieldValidator id="valUsername" runat="server"
        ControlToValidate="inputUsername" 
        ErrorMessage="<%$Resources:lblReqField %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblIdioma" Text="<%$Resources:lblIdioma %>">:</asp:Label></td>
<td><asp:DropDownList runat="server" ID="ddlLanguages" AppendDataBoundItems="true">
        <asp:ListItem Value="">
        </asp:ListItem>
        <asp:ListItem Value="en-US">English (United States)</asp:ListItem>
        <asp:ListItem Value="es-AR">Español (Argentina)</asp:ListItem>
        <asp:ListItem Value="it">Italiano (Italia)</asp:ListItem>
        </asp:DropDownList>
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblPassword" Text="<%$Resources:lblPassword1 %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword" runat="server"
        ControlToValidate="inputPassword" 
        ErrorMessage="<%$Resources:lblReqField %>"
        ForeColor="Red" />
</td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblRptPassword" Text="<%$Resources:lblPassword2 %>">:</asp:Label></td>
<td><asp:TextBox runat="server" ID="inputPassword2" TextMode="Password" />
<asp:RequiredFieldValidator id="valPassword2" runat="server"
        ControlToValidate="inputPassword2" 
        ErrorMessage="<%$Resources:lblReqField %>"
        ForeColor="Red" /><br />
<asp:CompareValidator id="valComparePassword" runat="server"
        ControlToValidate="inputPassword"
        ControlToCompare="inputPassword2"
        Type="String"
        Operator="Equal"
        ErrorMessage="<%$Resources:lblCampiCoincidenti %>"
        ForeColor="Red" >
    </asp:CompareValidator>
</td>
</tr>
<tr>
<td><asp:Literal runat="server" ID="lblTipoUtente" Text="<%$Resources:lblTipoUtente %>" />:</td>
<td>
<asp:DropDownList ID="tipoUtente" runat="server">
<asp:ListItem runat="server" Value="User" Text="<%$Resources:lblUtente %>" />
<asp:ListItem runat="server" Value="Admin" Text="<%$Resources:lblAdmin %>" />
</asp:DropDownList>
</td>
</tr>
        </tbody>
    <tfoot>
<tr>
<td colspan="2">
<asp:ImageButton runat="server" ID="btnSaveUser" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveUser_Click" CausesValidation="true" />&nbsp;
<asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconCancel.jpg" Height="40" OnClick="btnUndo_Click" CausesValidation="false" />
</td>
</tr>
<tr><td colspan="2"><asp:Label runat="server" id="lblEsito" /></td></tr>
        </tfoot>
</table>