<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzListArticoliNCommessa.ascx.cs" Inherits="KIS.Commesse.wzListArticoliNCommessa" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptArticoliStatoN">
    <HeaderTemplate>

    </HeaderTemplate>
    <ItemTemplate>
        <a href="wzEditPERT.aspx?idCommessa=<%= idCommessa.ToString() %>&annoCommessa=<%= annoCommessa.ToString() %>&idProc=<%#DataBinder.Eval(Container.DataItem, "Proc.process.processID") %>&revProc=<%#DataBinder.Eval(Container.DataItem, "Proc.process.revisione") %>&idVariante=<%#DataBinder.Eval(Container.DataItem, "Proc.variant.idVariante") %>&idProdotto=<%#DataBinder.Eval(Container.DataItem, "ID") %>&annoProdotto=<%#DataBinder.Eval(Container.DataItem, "Year") %>&quantita=<%#DataBinder.Eval(Container.DataItem, "Quantita") %>">
        <%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %>
            </a>
        <br />
    </ItemTemplate>
    <FooterTemplate>

    </FooterTemplate>
</asp:Repeater>