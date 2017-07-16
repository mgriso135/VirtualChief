<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wlListReparti.ascx.cs" Inherits="KIS.Produzione.wlListReparti" %>
<asp:Label runat="server" ID="lbl1" />

    <asp:Repeater runat="server" ID="rptListReparti" OnItemDataBound="rptListReparti_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-striped table-hover">
                <thead>
                <tr>
                    <td></td>
                    <td style="font-size:14px; font-weight:bold;"><asp:literal runat="server" ID="lblTHReparti" Text="<%$Resources:lblTHReparti %>"/></td>
                    <td style="font-size:14px; font-weight:bold;"><asp:literal runat="server" ID="lblTHDescrizione" Text="<%$Resources:lblTHDescrizione %>"/></td>
                </tr>
                    </thead><tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td><a href="WorkLoadSimReparti.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                    <asp:image ImageUrl="~/img/iconLabor.png" height="30px" runat="server" ID="imgPlan" ToolTip="<%$Resources:lblVisualizzaSim %>" />
                    </a></td>
                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "description") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>