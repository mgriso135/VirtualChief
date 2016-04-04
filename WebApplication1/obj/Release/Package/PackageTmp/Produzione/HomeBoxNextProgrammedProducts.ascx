<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeBoxNextProgrammedProducts.ascx.cs" Inherits="KIS.Produzione.HomeBoxLastProgrammedProducts" %>
<asp:Repeater runat="server" ID="rptNextProgrammedProducts">
    <HeaderTemplate>
        <h3>Piano produzione</h3>
        <ul class="list-group">
            </HeaderTemplate>
    <ItemTemplate>
        <li class="list-group-item">
    <span class="badge">
        <%# DataBinder.Eval(Container.DataItem, "id") %>/<%# DataBinder.Eval(Container.DataItem, "year") %>
    </span>
    <%# DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%# DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %>
  </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>   