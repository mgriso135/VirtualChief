<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewElencoPostazioni.ascx.cs" 
    Inherits="KIS.Postazioni.viewElencoPostazioni" %>




<asp:Label runat="server" ID="lbl1" />


<asp:Repeater ID="rptPostazioni" runat="server" OnItemDataBound="rptPostazioni_ItemDataBound" OnItemCommand="postazione_modify">
<headertemplate>
<table class="table table-condensed table-hover table-striped">
    <thead>
<tr>

<td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></td>
    <td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></td>
    <td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></td>
    <td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></td>
<td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center">Postazione</td>
    <td style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center">Descrizione</td>
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td>
<a href="viewPostazione.aspx?pstID=<%# DataBinder.Eval(Container.DataItem, "id") %>">
    <asp:Image runat="server" ID="imgShow" ImageUrl="/img/iconView.png" Height="40px" />
</a>
        </td>
        <td>
            <asp:ImageButton runat="server" ID="imgBarcodePostazione" CommandName="printBarCode" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Stampa il codice a barre relativo alla postazione" ImageUrl="/img/iconBarcode.png" Height="40" />
        </td>
            <td>
                <a href="editPostazione.aspx?pstID=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                <asp:Image runat="server" ID="editPostazione" ImageUrl="/img/edit.png" Height="40px" ToolTip="Modifica i dati della postazione" />
                    </a>
        </td>
        <td style="text-align:center;">
        <asp:HiddenField runat="server" ID="ID" />
        <asp:ImageButton runat="server" ID="deletePostazione" ImageUrl="/img/iconDelete.png" Height="40px" ToolTip="Cancella la postazione" />
        </td>
        
        <td style="font-size: 16px; font-family: Calibri;text-align:center;">
            <%# DataBinder.Eval(Container.DataItem, "name") %>
        </td>
        <td style="font-size: 16px; font-family: Calibri;text-align:left;"><%# DataBinder.Eval(Container.DataItem, "desc") %>
        </td>
        </tr>
        </ItemTemplate>
        <FooterTemplate></tbody></table></FooterTemplate>
</asp:Repeater>