<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUsers.ascx.cs" Inherits="KIS.Admin.listUsers" %>

<asp:Label runat="server" ID="lblLstUsers" />
<asp:Repeater ID="rptUsers" runat="server" OnItemCreated="rptUsers_ItemCreated" OnItemCommand="rptUsers_ItemCommand1">
<headertemplate>
<table class="table table-condensed table-striped table-hover">
    <thead>
<tr>
    <td></td>
    <td></td>
<td>Username</td>
<td>Nome</td>
<td>Cognome</td>
    
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:HyperLink runat="server" ID="lnkEditUsers" NavigateUrl='<%# "editUser.aspx?id=" + DataBinder.Eval(Container.DataItem, "username") %>'>
                <asp:Image runat="server" ImageUrl="/img/edit.png" Height="30px" CssClass="img-rounded" />
        </asp:HyperLink>
                </td>
            <td style="text-align:center;">
                <!--
                <asp:HyperLink runat="server" ID="lnkGeneraBarcode" NavigateUrl='<%# "generaBarcode.aspx?id=" +DataBinder.Eval(Container.DataItem, "ID") %>'>
                    <asp:image CssClass="img-rounded" runat="server" ID="imgGeneraBarcode" ImageUrl="/img/iconBarcode.png" Height="40" ToolTip="Genera il cartellino" />
                </asp:HyperLink>
                -->
                <asp:imagebutton CssClass="img-rounded" runat="server" ID="imgGeneraBarcode2" ImageUrl="/img/iconBarcode.png" Height="40" ToolTip="Genera il cartellino" CommandName="printBarcode" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "username") %>' />
            </td>
            <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "username") %>
            </td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "cognome") %></td>
            
        </tr>
        </ItemTemplate>
        <FooterTemplate></tbody></table></FooterTemplate>
</asp:Repeater>