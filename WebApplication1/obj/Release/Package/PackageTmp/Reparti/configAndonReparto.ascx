<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configAndonReparto.ascx.cs" Inherits="KIS.Reparti.configAndonReparto" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <h3>Andon reparto: modalità di visualizzazione degli utenti nelle postazioni</h3>
<asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rbList" AutoPostBack="true" OnSelectedIndexChanged="rbList_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0">Visualizza solo lo username</asp:ListItem>
            <asp:ListItem Value="1">Visualizza solo il nome</asp:ListItem>
            <asp:ListItem Value="2">Visualizza il nome e l'iniziale del cognome</asp:ListItem>
            <asp:ListItem Value="3">Visualizza nome e cognome</asp:ListItem>
        </asp:radiobuttonlist>
        </ContentTemplate>
    </asp:UpdatePanel>