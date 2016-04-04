<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configModoCalcoloTC.ascx.cs" Inherits="KIS.Reparti.configModoCalcoloTC" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rbList" AutoPostBack="true" OnSelectedIndexChanged="rbList_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0">Estendi il calcolo dei tempi oltre l'orario di lavoro programmato</asp:ListItem>
            <asp:ListItem Value="1">Vincola il calcolo dei tempi agli intervalli lavorativi programmati</asp:ListItem>
        </asp:radiobuttonlist>
    </ContentTemplate>
</asp:UpdatePanel>