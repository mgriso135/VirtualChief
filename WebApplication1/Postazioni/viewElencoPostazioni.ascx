<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewElencoPostazioni.ascx.cs" 
    Inherits="KIS.Postazioni.viewElencoPostazioni" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater ID="rptPostazioni" runat="server" OnItemDataBound="rptPostazioni_ItemDataBound" OnItemCommand="postazione_modify">
<headertemplate>
<table class="table table-condensed table-hover table-striped">
    <thead>
<tr>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></th>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></th>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></th>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"></th>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"><asp:literal runat="server" ID="lblTHPostazione" Text="<%$Resources:lblTHPostazione %>" /></th>
    <th style="font-size: 18px; font-family: Calibri; font-weight:bold; text-align:center"><asp:literal runat="server" ID="lblTHDescrizione" Text="<%$Resources:lblTHDescrizione %>" /></th>
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td>
<a href="/Postazioni/viewPostazione.aspx?pstID=<%# DataBinder.Eval(Container.DataItem, "id") %>" target="_blank">
    <asp:Image runat="server" ID="imgShow" ImageUrl="/img/iconView.png" Height="40px" />
</a>
        </td>
        <td>
            <asp:ImageButton runat="server" ID="imgBarcodePostazione" CommandName="printBarCode" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="<%$Resources:lblTTPrintBarcode %>" ImageUrl="/img/iconBarcode.png" Height="40" />
        </td>
            <td>
                <a href="/Postazioni/editPostazione.aspx?pstID=<%# DataBinder.Eval(Container.DataItem, "id") %>" target="_blank">
                <asp:Image runat="server" ID="editPostazione" ImageUrl="/img/edit.png" Height="40px" ToolTip="<%$Resources:lblTTModificaDati %>" />
                    </a>
        </td>
        <td style="text-align:center;">
        <asp:HiddenField runat="server" ID="ID" />
        <asp:ImageButton runat="server" ID="deletePostazione" ImageUrl="/img/iconDelete.png" Height="40px" ToolTip="<%$Resources:lblTTDelPostazione %>" />
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