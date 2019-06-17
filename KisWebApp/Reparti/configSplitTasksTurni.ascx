<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configSplitTasksTurni.ascx.cs" Inherits="KIS.Reparti.configSplitTasksTurni" %>

<asp:Label runat="server" ID="lbl1" />
<h3><asp:Literal runat="server" ID="lblOpzioniInserimento" Text="<%$Resources:lblOpzioniInserimento %>" /></h3>
<p>
<asp:RadioButtonList CssClass="radio" runat="server" ID="splitTasks" OnSelectedIndexChanged="splitTasks_SelectedIndexChanged" AutoPostBack="true">
    <asp:ListItem Value="0" Text="<%$Resources:lblStessoTurno %>"></asp:ListItem>
    <asp:ListItem Value="1" Text="<%$Resources:lblSuddividi %>"></asp:ListItem>
</asp:RadioButtonList>
    </p>