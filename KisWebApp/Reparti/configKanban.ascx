<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configKanban.ascx.cs" Inherits="KIS.Reparti.configKanban" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rb1" AutoPostBack="true" OnSelectedIndexChanged="rb1_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0" Text="<%$Resources:lblKBNonEnabled %>"></asp:ListItem>
            <asp:ListItem Value="1" Text="<%$Resources:lblKBEnabled %>"></asp:ListItem>
        </asp:RadioButtonList>
    </ContentTemplate>
</asp:UpdatePanel>