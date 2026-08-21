<%@ Page Title="Virtual Chief" Language="C#"  MasterPageFile="~/Site.master" 
AutoEventWireup="true" CodeBehind="addMacroProcesso.aspx.cs" Inherits="KIS.Processi.addMacroProcesso" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx">
                            <asp:Literal runat="server" ID="lblNavProcessMan" Text="<%$Resources:lblNavProcessMan %>" />
                            </a>
						<span class="divider">/</span>
                        <a href="AddMacroProcesso.aspx">
                            <asp:Literal runat="server" ID="lblNavAddProc" Text="<%$Resources:lblNavAddProc %>" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
<asp:Label runat="server" ID="lbl1" />
    <h3><asp:Literal runat="server" ID="lblTitleAddProc" Text="<%$Resources:lblTitleAddProc %>" /></h3>
<table runat="server" id="frmAddProc" class="table table-bordered">
<tr><td><asp:Literal runat="server" ID="lblNomeProc" Text="<%$Resources:lblNomeProc %>" />
    </td><td><asp:TextBox runat="server" ID="ProcName" /></td>
<td><asp:RequiredFieldValidator runat="server" ControlToValidate="ProcName" ErrorMessage="<%$Resources:lblValReqField %>" ForeColor="Red" /></td>
</tr>
<tr><td><asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblDescProc %>" /></td><td><asp:TextBox runat="server" ID="ProcDesc" 
        Height="50px" Width="250" TextMode="MultiLine" /></td>
<td><asp:RequiredFieldValidator runat="server" ControlToValidate="ProcDesc" ErrorMessage="<%$Resources:lblValReqField %>" ForeColor="Red" /></td>
        </tr>

<tr runat="server" visible="false">
<td><asp:Literal runat="server" ID="lblTipoGrafico" Text="<%$Resources:lblTipoGrafico %>" />
    </td>
<td>
<asp:DropDownList runat="server" ID="vsm">
<asp:ListItem Value="True" Text="Value-Stream Map" />
<asp:ListItem Value="False" Text="PERT" Selected="True" />
</asp:DropDownList>

</td>
</tr>
<tr><td colspan="2">
    <asp:Button runat="server" Text="<%$Resources:lblBtnAddLineaProd %>" ID="btnAddMacroProc" OnClick="btnAddMacroProc_Click" OnClientClick="this.value='processing';this.style.display='hidden';" /></td></tr>
</table>
</asp:Content>