<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configWizard_TipoPERT.ascx.cs" Inherits="KIS.Admin.configWizard_TipoPERT" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rb1" AutoPostBack="true" OnSelectedIndexChanged="rb1_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="Graph">PERT in formato grafico</asp:ListItem>
            <asp:ListItem Value="Table">PERT in formato tabella</asp:ListItem>
        </asp:RadioButtonList>
    </ContentTemplate>
</asp:UpdatePanel>