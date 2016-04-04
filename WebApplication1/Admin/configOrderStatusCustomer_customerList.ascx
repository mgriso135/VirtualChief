<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configOrderStatusCustomer_customerList.ascx.cs" Inherits="KIS.Admin.configOrderStatusCustomer_customerList" %>

<div class="row-fluid">
    <div class="span12">
<asp:Label runat="server" ID="lbl1" />
        <asp:Repeater runat="server" ID="rptCustomerList">
            <HeaderTemplate>
                <table class="table table-striped table-condensed table-bordered">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:HyperLink runat="server" ID="lnkCfgCustomer" NavigateUrl='<%# "~/Admin/configOrderStatusCustomer.aspx?id=" + DataBinder.Eval(Container.DataItem, "CodiceCliente") %>' Target="_blank">
                        <%# DataBinder.Eval(Container.DataItem, "RagioneSociale") %></td>
                    </asp:HyperLink>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>