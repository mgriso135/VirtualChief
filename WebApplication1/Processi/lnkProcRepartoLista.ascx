<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="lnkProcRepartoLista.ascx.cs" Inherits="KIS.Processi.lnkProcRepartoLista" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptReparti" OnItemDataBound="rptReparti_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:CheckBox runat="server" ID="chk" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" /></td>
            <td><asp:HiddenField runat="server" ID="idRep" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <%#DataBinder.Eval(Container.DataItem, "ID") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Name") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Description") %></td>
            <td>
                <asp:HyperLink runat="server" ID="lnkProcPostazione" NavigateUrl='<%# "/Reparti/managePostazioni.aspx?processID=" %>'>
                <asp:Image runat="server" ID="imgLnkProcTaskPostazione" ToolTip="Associa i task del processo alla postazione di lavoro" ImageUrl="/img/iconToolbox.jpg" Height="40" />
                    </asp:HyperLink>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>