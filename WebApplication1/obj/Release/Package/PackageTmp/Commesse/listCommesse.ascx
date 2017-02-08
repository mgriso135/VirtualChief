<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listCommesse.ascx.cs" Inherits="KIS.Commesse.listCommesse" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptCommesse" OnItemDataBound="rptCommesse_ItemDataBound" OnItemCommand="rptCommesse_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <th></th>
                <th>ID</th>
                <th>Data inserimento</th>
                <th>Cliente</th>
                <th>Ragione sociale cliente</th>
                <th>Note</th>
                <th>Status</th>
                <th></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:hyperlink runat="server" ID="lnkWizard" NavigateUrl='<%# "~/Commesse/wzAddPERT.aspx?idCommessa=" + DataBinder.Eval(Container.DataItem, "ID") + "&annoCommessa="+ DataBinder.Eval(Container.DataItem, "Year") %>'>
                <asp:Image runat="server" ID="imgWizard" ImageUrl="/img/iconWizard.png" ToolTip="Prosegui l'inserimento di articoli tramite wizard" Height="40" />
                    </asp:hyperlink>
                <asp:hyperlink runat="server" ID="lnkLinkArticoli" NavigateUrl='<%# "~/Commesse/linkArticoliToCommessa.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno="+ DataBinder.Eval(Container.DataItem, "Year") %>'>
                
                <asp:Image runat="server" ID="imgLnkArticoli" ImageUrl="/img/iconCart1.jpg" ToolTip="Assegna articoli all'ordine" Height="40" />
                </asp:hyperlink></td>
            
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#((DateTime)DataBinder.Eval(Container.DataItem, "DataInserimento")) %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "RagioneSocialeCliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Note") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
            <td>
                <asp:ImageButton runat="server" ID="imgDelete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' Height="40" ImageUrl="/img/iconDelete.png" />
                <asp:HiddenField runat="server" ID="hID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <asp:HiddenField runat="server" ID="hYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>