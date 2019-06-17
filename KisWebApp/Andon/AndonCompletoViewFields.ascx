<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AndonCompletoViewFields.ascx.cs" Inherits="KIS.Andon.AndonCompletoViewFields" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script type="text/javascript">
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    $("#<%=lblInfo.ClientID%>").delay(3000).fadeOut("slow", function () {
                $(this).text('')
            });
        });
    });

     </script> 

        <asp:Label runat="server" ID="lbl1" />
<div class="row-fluid" runat="server" id="frmContainer">
    <div class="span12">
        <asp:Label runat="server" ID="lblInfo" CssClass="text-info" />
        <asp:Repeater runat="server" ID="rptFields" OnItemCommand="rptFields_ItemCommand" OnItemDataBound="rptFields_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                    <th colspan="3"><asp:Label runat="server" ID="lblTHProdotto" meta:resourcekey="lblTHProdotto" /></th>
                            </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
<tr runat="server" id="tr1">
    <td><asp:HiddenField runat="server" ID="idVFieldName" Value='<%# Eval("Key") %>' />
        <asp:ImageButton runat="server" ID="imgUp" CommandName="Up" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconArrowUp.png" Width="10" />
        </td><td>
        <asp:ImageButton runat="server" ID="imgDown" CommandName="Down" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/IconArrowDown.png" Width="10" />
    </td>
    <td><%# (string)GetLocalResourceObject(Eval("Key").ToString()) %>
       
    </td>
    <td>
        <asp:ImageButton runat="server" ID="imgDelete" CommandName="Delete" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconDelete.png" Width="20" />
    </td>
</tr>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
            </asp:Repeater>
        <asp:Repeater runat="server" ID="rptCampiVisualizzabili" OnItemDataBound="rptCampiVisualizzabili_ItemDataBound" OnItemCommand="rptCampiVisualizzabili_ItemCommand">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr2">
    <td>
        <asp:HiddenField runat="server" ID="idPFieldName" Value='<%# Eval("Key") %>' />
        <asp:ImageButton runat="server" ID="imgAdd" CommandName="Add" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconAdd2.png" Width="20" />
    </td>
                    <td></td>
    <td><%# (string)GetLocalResourceObject(Eval("Key").ToString()) %>
    </td>
    <td>
    </td>
</tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <!-- Sezione Tasks -->
        <asp:Repeater runat="server" ID="rptFieldsTasks" OnItemCommand="rptFieldsTasks_ItemCommand" OnItemDataBound="rptFieldsTasks_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                    <th colspan="3"><asp:Label runat="server" ID="lblTHTasks" meta:resourcekey="lblTHTasks" /></th>
                            </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
<tr runat="server" id="tr1">
    <td><asp:HiddenField runat="server" ID="idVFieldTaskName" Value='<%# Eval("Key") %>' />
        <asp:ImageButton runat="server" ID="imgTaskUp" CommandName="Up" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconArrowUp.png" Width="10" />
        </td><td>
        <asp:ImageButton runat="server" ID="imgTaskDown" CommandName="Down" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/IconArrowDown.png" Width="10" />
    </td>
    <td><%# (string)GetLocalResourceObject(Eval("Key").ToString()) %>
    </td>
    <td>
        <asp:ImageButton runat="server" ID="imgTaskDelete" CommandName="Delete" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconDelete.png" Width="20" />
    </td>
</tr>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
            </asp:Repeater>
        <asp:Repeater runat="server" ID="rptCampiTasksVisualizzabili" OnItemDataBound="rptCampiTasksVisualizzabili_ItemDataBound" OnItemCommand="rptCampiTasksVisualizzabili_ItemCommand">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr2">
    <td>
        <asp:HiddenField runat="server" ID="idPFieldTaskName" Value='<%# Eval("Key") %>' />
        <asp:ImageButton runat="server" ID="imgTaskAdd" CommandName="Add" CommandArgument='<%#Eval("key") %>' ImageUrl="~/img/iconAdd2.png" Width="20" />
    </td>
                    <td></td>
    <td><%# (string)GetLocalResourceObject(Eval("Key").ToString()) %>
    </td>
    <td>
    </td>
</tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>