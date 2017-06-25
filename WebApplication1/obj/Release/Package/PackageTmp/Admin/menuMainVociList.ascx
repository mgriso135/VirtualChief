<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuMainVociList.ascx.cs" Inherits="KIS.Admin.menuMainVociList" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptMainVoci" OnItemDataBound="rptMainVoci_ItemDataBound" OnItemCommand="rptMainVoci_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <td></td>
                <td>

                </td>
                <td></td>
                <td><asp:label runat="server" id="lblTitleID" meta:resourcekey="lblTitleID" /></td>
                <td><asp:label runat="server" id="lblTitleTitolo" meta:resourcekey="lblTitleTitolo" /></td>
                <td><asp:label runat="server" id="lblTitleDescrizione" meta:resourcekey="lblTitleDescrizione" /></td>
                <td><asp:label runat="server" id="lblTitleURL" meta:resourcekey="lblTitleURL" /></td>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:HyperLink runat="server" ID="lnkExpand" NavigateUrl='<%#"menuShowVoce.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")%>'>
                <asp:Image runat="server" ID="imgView" ImageUrl="/img/iconView.png" Height="40"/>
                </asp:HyperLink>

            </td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="/img/edit.png" ToolTip="<%$resources:lblTTModificaVoce %>" Height="40" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="/img/iconDelete.png" ToolTip="<%$resources:lblTTCancellaVoce %>" Height="40" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>
                
            </td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Titolo") %>' ID="lblTitolo" />
                <asp:TextBox runat="server" ID="txtTitolo" Text='<%#DataBinder.Eval(Container.DataItem, "Titolo") %>' Visible="false" />
            </td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' ID="lblDesc" />
                <asp:TextBox runat="server" ID="txtDesc" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' Visible="false" /></td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "URL") %>' ID="lblURL" />
                <asp:TextBox runat="server" ID="txtURL" Text='<%#DataBinder.Eval(Container.DataItem, "URL") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="imgSave" ImageUrl="/img/iconSave.jpg" ToolTip="<%$resources:lblTTSalvaVoce %>" Height="40" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" />
                <asp:ImageButton runat="server" ID="imgUndo" ImageUrl="/img/iconUndo.png" ToolTip="<%$resources:lblTTReset %>" Height="40" CommandName="undo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>