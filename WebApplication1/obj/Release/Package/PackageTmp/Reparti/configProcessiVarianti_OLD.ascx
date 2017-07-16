<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configProcessiVarianti_OLD.ascx.cs" Inherits="KIS.Reparti.configProcessiVarianti" %>

<asp:Label runat="server" ID="lbl1" />
<h3><asp:Literal runat="server" ID="lblTitleProcAss" Text="<%$Resources:lblTitleProcAss %>" /></h3>
<asp:Repeater ID="rptMacroProc" runat="server" OnItemDataBound="rptMacroProc_ItemDataBound" OnItemCommand="rptMacroProc_ItemCommand">
<headertemplate>
<table border="1">
<tr>
    <td></td>
<td><asp:Literal runat="server" ID="lblProc" Text="<%$Resources:lblProc %>" /></td>
    <td><asp:Literal runat="server" ID="lblDescr" Text="<%$Resources:lblDescr %>" /></td>
    <td><asp:Literal runat="server" ID="lblVarianti" Text="<%$Resources:lblVarianti %>" /></td>
    <td><asp:Literal runat="server" ID="lblManagePost" Text="<%$Resources:lblManagePost %>" /></td>
</tr>
</headertemplate>

        <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:HiddenField runat="server" ID="processID" Value='<%# DataBinder.Eval(Container.DataItem, "process.processID") %>' />
                <asp:HiddenField runat="server" ID="varianteID" Value='<%# DataBinder.Eval(Container.DataItem, "variant.idVariante") %>' />
                <asp:ImageButton runat="server" ID="delete" CommandName="delete" ImageUrl="/img/iconDelete.png" Height="40px" />
            </td>
        <td>
            <asp:HiddenField runat="server" ID="ID" Value='<%# DataBinder.Eval(Container.DataItem, "process.processID") %>' />
            <%# DataBinder.Eval(Container.DataItem, "process.processName") %>

        </td>
        <td>
            <%# DataBinder.Eval(Container.DataItem, "process.processDescription") %>

        </td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "variant.nomeVariante") %>
            </td>
            <td>
                <asp:HyperLink runat="server" ID="lnkManagePostazione" NavigateUrl="managePostazioni.aspx">
                    <asp:Image runat="server" ID="imgLnkManagePostazione" ImageUrl="/img/iconToolbox.jpg" Height="40px" />
                </asp:HyperLink>
            </td>
        </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table></FooterTemplate>
</asp:Repeater>

<asp:ImageButton runat="server" ID="showAddMacroProc" ImageUrl="/img/iconAdd.jpg" Height="40px" OnClick="showAddMacroProc_Click" />
<asp:Literal runat="server" ID="lblAddProcesso" Text="<%$Resources:lblAddProcesso %>" />
<asp:Repeater runat="server" ID="rptAddProc" OnItemDataBound="rptAddProc_ItemDataBound" OnItemCommand="rptAddProc_ItemCommand">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:HiddenField runat="server" ID="processID" Value='<%#DataBinder.Eval(Container.DataItem, "process.processID") %>' />
                <asp:HiddenField runat="server" ID="varianteID" Value='<%#DataBinder.Eval(Container.DataItem, "variant.idVariante") %>' />
                <%#DataBinder.Eval(Container.DataItem, "process.processName") %>

            </td>
            <td>
               <%#DataBinder.Eval(Container.DataItem, "variant.nomeVariante") %>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="expand" CommandName="expand" ImageUrl="/img/iconExpand.gif" Height="30px" ToolTip="<%$Resouces:lblTTEspandiFigli %>" />
                <asp:ImageButton runat="server" ID="add" CommandName="add" ImageUrl="/img/iconAdd2.png" Height="40px" ToolTip="<%$Resouces:lblTTAddVariante %>" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>