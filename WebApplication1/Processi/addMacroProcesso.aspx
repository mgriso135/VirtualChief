<%@ Page Title="Kaizen Indicator System" Language="C#"  MasterPageFile="~/Site.master" 
AutoEventWireup="true" CodeBehind="addMacroProcesso.aspx.cs" Inherits="KIS.Processi.addMacroProcesso" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx">Process Manager</a>
						<span class="divider">/</span>
                        <a href="AddMacroProcesso.aspx">Aggiungi un processo di primo livello</a>
						<span class="divider">/</span>
					</li>
				</ul>
<asp:Label runat="server" ID="lbl1" />
    <h3>Aggiungi un processo di primo livello</h3>
<table runat="server" id="frmAddProc" class="table table-bordered">
<tr><td>Nome processo</td><td><asp:TextBox runat="server" ID="ProcName" /></td>
<td><asp:RequiredFieldValidator runat="server" ControlToValidate="ProcName" ErrorMessage="Il nome del processo non può essere nullo" ForeColor="Red" /></td>
</tr>
<tr><td>Descrizione processo</td><td><asp:TextBox runat="server" ID="ProcDesc" 
        Height="50px" Width="250" TextMode="MultiLine" /></td>
<td><asp:RequiredFieldValidator runat="server" ControlToValidate="ProcDesc" ErrorMessage="La descrizione del processo non può essere nulla" ForeColor="Red" /></td>
        </tr>

<tr>
<td>Tipo di grafico per i subprocessi:</td>
<td>
<asp:DropDownList runat="server" ID="vsm">
<asp:ListItem Value="True" Text="Value-Stream Map" />
<asp:ListItem Value="False" Text="PERT" Selected="True" />
</asp:DropDownList>

</td>
</tr>
<tr><td colspan="2">
    <asp:Button runat="server" Text="Aggiungi macroprocesso" ID="btnAddMacroProc" OnClick="btnAddMacroProc_Click" OnClientClick="this.value='processing';this.style.display='hidden';" /></td></tr>
</table>
</asp:Content>