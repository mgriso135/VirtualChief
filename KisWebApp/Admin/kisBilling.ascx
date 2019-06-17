<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="kisBilling.ascx.cs" Inherits="KIS.Admin.kisBilling" %>

<asp:Label runat="server" ID="lblExpiryDate" />

<div class="row-fluid">
    <div class="span6">
        <h3><asp:Literal runat="server" ID="lblTOrdiniMese" Text="<%$Resources:lblTOrdiniMese %>" /></h3>
<asp:Repeater runat="server" ID="rptOrdiniMonth">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead class="thead-inverse">
                <th><asp:Literal runat="server" ID="lblTHPeriodo" Text="<%$Resources:lblTHPeriodo %>" /></th>
                <th><asp:Literal runat="server" ID="lblTHNOrdini" Text="<%$Resources:lblTHNOrdini %>" /></th>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "Month") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Value") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
    </div>
    <div class="span6">
        <h3><asp:Literal runat="server" ID="lblTTasksMonth" Text="<%$Resources:lblTTasksMonth %>" /></h3>
        <asp:Repeater runat="server" ID="rptTaskMonth">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead class="thead-inverse">
                <th><asp:Literal runat="server" ID="lblTHPeriodo" Text="<%$Resources:lblTHPeriodo %>" /></th>
                <th><asp:Literal runat="server" ID="lblTHNOrdini" Text="<%$Resources:lblTHNOrdini %>" /></th>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "Month") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Value") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
    </div>
    </div>

<div class="row-fluid">
    <div class="span6">
        <h3><asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblTOrdiniAnno %>" /></h3>
<asp:Repeater runat="server" ID="rptOrdiniYear">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead class="thead-inverse">
                <th><asp:Literal runat="server" ID="lblTHPeriodo" Text="<%$Resources:lblTHPeriodo %>" /></th>
                <th><asp:Literal runat="server" ID="lblTHNOrdini" Text="<%$Resources:lblTHNOrdini %>" /></th>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Value") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
    </div>
    <div class="span6">
        <h3><asp:Literal runat="server" ID="Literal2" Text="<%$Resources:lblTTasksAnno %>" /></h3>
        <asp:Repeater runat="server" ID="rptTasksYear">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead class="thead-inverse">
                <th><asp:Literal runat="server" ID="lblTHPeriodo" Text="<%$Resources:lblTHPeriodo %>" /></th>
                <th><asp:Literal runat="server" ID="lblTHNOrdini" Text="<%$Resources:lblTHNOrdini %>" /></th>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Value") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
    </div>
    </div>