<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUsers.ascx.cs" Inherits="KIS.Admin.listUsers" %>

<asp:Label runat="server" ID="lblLstUsers" />
<asp:Repeater ID="rptUsers" runat="server" OnItemDataBound="rptUsers_ItemDataBound" OnItemCommand="rptUsers_ItemCommand1">
<headertemplate>
<table class="table table-condensed table-striped table-hover">
    <thead>
<tr>
    <th></th>
    <th></th>
<th><asp:literal runat="server" id="lblUsername" Text="<%$Resources:lblUsername %>" /></th>
<th><asp:literal runat="server" id="lblNome" Text="<%$Resources:lblNome %>" /></th>
<th><asp:literal runat="server" id="lblCognome" Text="<%$Resources:lblCognome %>" /></th>
<th><asp:literal runat="server" id="lblConfigurato" Text="<%$Resources:lblConfigurato %>" /></th>
<th><asp:literal runat="server" id="lblConfigura" Text="<%$Resources:lblConfigura %>" /></th>
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
            <td style="text-align:center;">
                <asp:HiddenField runat="server" ID="hdUsername" Value='<%# DataBinder.Eval(Container.DataItem, "username") %>' />
                <!--
                <asp:HyperLink runat="server" ID="lnkGeneraBarcode" NavigateUrl='<%# "generaBarcode.aspx?id=" +DataBinder.Eval(Container.DataItem, "ID") %>'>
                    <asp:image CssClass="img-rounded" runat="server" ID="imgGeneraBarcode" ImageUrl="/img/iconBarcode.png" Height="40" ToolTip="Genera il cartellino" />
                </asp:HyperLink>
                -->
                <asp:imagebutton CssClass="img-rounded" runat="server" ID="imgGeneraBarcode2" ImageUrl="~/img/iconBarcode.png" Height="40" ToolTip="<%$Resources:lblPrintCard %>" CommandName="printBarcode" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "username") %>' />
            </td>
            <td>
                <asp:HyperLink runat="server" ID="lnkManageHoursRegistration" NavigateUrl='<%#"~/Users/Users/WorkHoursManualRegistration?usr=" +  DataBinder.Eval(Container.DataItem, "username") %>' ToolTip="<%$Resources:lblTTManualHoursRegistration %>">
                    <asp:Image runat="server" ID="imgManualWorkHoursRegistration" ImageUrl="~/img/iconClock.png" Height="40" /> 
                </asp:HyperLink></td>
            <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "username") %>
            </td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "cognome") %></td>
            <td>
                <asp:Image runat="server" ID="imgOk" ImageUrl="~/img/iconComplete.png" Height="20" Visible="false" />
                <asp:Image runat="server" ID="imgKo" ImageUrl="~/img/iconCancel.jpg" Height="20" />
                <asp:Label runat="server" ID="lblImg" />
            </td>
            <td>
                <asp:HyperLink runat="server" ID="lnkEditUsers" Target="_blank" NavigateUrl='<%# "editUser.aspx?id=" + DataBinder.Eval(Container.DataItem, "username") %>'>
                <asp:Image runat="server" ImageUrl="~/img/edit.png" Height="30px" CssClass="img-rounded" />
        </asp:HyperLink>
                </td>
        </tr>
        </ItemTemplate>
        <FooterTemplate></tbody></table></FooterTemplate>
</asp:Repeater>