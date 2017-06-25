<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listArticoliCommessa.ascx.cs" Inherits="KIS.Commesse.listArticoliCommessa" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptArticoliCommessa" OnItemDataBound="rptArticoliCommessa_ItemDataBound" OnItemCommand="rptArticoliCommessa_ItemCommand">
    <HeaderTemplate>
        <table class="table table-condensed table-hover table-striped">
            <thead>
            <tr>
                <th></th>
                <th><asp:label runat="server" id="lblIDCommessa" meta:resourcekey="lblIDCommessa" /></th>
                <th colspan="2"><asp:label runat="server" id="lblProcesso" meta:resourcekey="lblProcesso" /></th>
                <th><asp:label runat="server" id="lblQuantita" meta:resourcekey="lblQuantita" /></th>
                <th><asp:label runat="server" id="lblStatus" meta:resourcekey="lblStatus" /></th>
                <th><asp:label runat="server" id="lblDataConsegna" meta:resourcekey="lblDataConsegna" /></th>
                <th></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:HiddenField runat="server" ID="hID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <asp:HiddenField runat="server" ID="hYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                <asp:HyperLink runat="server" ID="lnkViewCommessa" NavigateUrl='<%# "/Produzione/statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>'>
                    <asp:Image runat="server" id="imgView" ImageUrl="/img/iconView.png" Height="40" />
                </asp:HyperLink>
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
            <td>
                <asp:label runat="server" ID="lblDataPC" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "dataPrevistaConsegna")).ToString("dd/MM/yyyy") %>' />
                <asp:Calendar runat="server" ID="calEditDataPC" Visible="false" BackColor="White" />
                <asp:ImageButton runat="server" ImageUrl="/img/edit.png" Height="30" ID="imgEdit" CommandName="editData" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
                <asp:ImageButton runat="server" ImageUrl="/img/iconSave.jpg" Height="30" ID="imgSavePC" CommandName="savePC" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' Visible="false" />
                <asp:ImageButton runat="server" ImageUrl="/img/iconUndo.png" Height="30" ID="imgResetPC" CommandName="undoPC" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="imgDepianificazione" ImageUrl="/img/iconUndo.png" Width="40" Height="40" CommandName="depianifica" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' ToolTip="<%$Resources:lblDePianifica %>" />
                <asp:ImageButton runat="server" ID="imgDelete" ImageUrl="/img/iconDelete.png" Height="40" ToolTip="Cancella il record" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>