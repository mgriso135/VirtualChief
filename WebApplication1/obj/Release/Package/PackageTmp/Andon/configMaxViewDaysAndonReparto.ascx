<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configMaxViewDaysAndonReparto.ascx.cs" Inherits="KIS.Andon.configMaxViewDaysAndonReparto" %>
<h3>Configurazione numero di giorni da visualizzare</h3>
<asp:UpdatePanel runat="server" id="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:DropDownList runat="server" ID="ddlNumDays" OnSelectedIndexChanged="ddlNumDays_SelectedIndexChanged" AutoPostBack="true" />
    </ContentTemplate>
</asp:UpdatePanel>