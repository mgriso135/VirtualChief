<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configAndonReparto.ascx.cs" Inherits="KIS.Reparti.configAndonReparto" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <h3><asp:literal runat="server" ID="lblTitleAndonRep" Text="<%$Resources:lblTitleAndonRep %>" /></h3>
<asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rbList" AutoPostBack="true" OnSelectedIndexChanged="rbList_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0" Text="<%$Resources:lblVisUsername %>"></asp:ListItem>
            <asp:ListItem Value="1" Text="<%$Resources:lblVisNombre %>"></asp:ListItem>
            <asp:ListItem Value="2" Text="<%$Resources:lblVisNombreYA %>"></asp:ListItem>
            <asp:ListItem Value="3" Text="<%$Resources:lblVisNombreYAppellido %>"></asp:ListItem>
        </asp:radiobuttonlist>
        </ContentTemplate>
    </asp:UpdatePanel>