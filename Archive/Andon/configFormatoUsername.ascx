<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configFormatoUsername.ascx.cs" Inherits="KIS.Andon.configFormatoUsername" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <h3><asp:Label runat="server" ID="lblTitleVisUtenti" meta:resourcekey="lblTitleVisUtenti" /></h3>
<asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rbList" AutoPostBack="true" OnSelectedIndexChanged="rbList_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0" Text="<%$resources:liVisUsername %>"></asp:ListItem>
            <asp:ListItem Value="1" Text="<%$resources:liVisNome %>"></asp:ListItem>
            <asp:ListItem Value="2" Text="<%$resources:liVisNomeInizialeC %>"></asp:ListItem>
            <asp:ListItem Value="3" Text="<%$resources:liVisNomeCognome %>"></asp:ListItem>
        </asp:radiobuttonlist>
        </ContentTemplate>
    </asp:UpdatePanel>