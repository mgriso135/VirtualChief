<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wizConfigReparti_Main.ascx.cs" Inherits="KIS.Configuration.wizConfigReparti_Main1" %>

<h3><asp:Literal runat="server" ID="lblTitleAddReparto" Text="<%$Resources:lblTitleAddReparto %>" /></h3>
<asp:Label runat="server" ID="lbl1"  CssClass="text-info"/>
<table runat="server" id="frmAddReparto" class="table">
    <tr>
        <td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" />:</td>
        <td><asp:TextBox runat="server" ID="nome" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ErrorMessage="<%$Resources:lblValReqField %>" ControlToValidate="nome" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblDesc" Text="<%$Resources:lblDesc %>" />:</td>
        <td><asp:TextBox runat="server" ID="descrizione" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDescrizione" ErrorMessage="<%$Resources:lblValReqField %>" ControlToValidate="descrizione" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblTimezone" Text="<%$Resources:lblTimezone %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlTimezones"/></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="../img/iconSave.jpg" Height="50px" ToolTip="<%$Resources:lblTTSalva %>" ValidationGroup="addReparto" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="../img/iconUndo.png" Height="50px" ToolTip="<%$Resources:lblTTUndo %>" ValidationGroup="addReparto" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptReparti" OnItemDataBound="rptReparti_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-condensed">
            <thead>
                <th><asp:Literal runat="server" ID="lblReparto" Text="<%$Resources:lblReparto %>" /></th>
                <th><asp:Literal runat="server" ID="lblConfigurato" Text="<%$Resources:lblConfigurato %>" /></th>
                <th><asp:Literal runat="server" ID="lblConfigura" Text="<%$Resources:lblConfigura %>" /></th>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hdRepID" Value='<%#DataBinder.Eval(Container.DataItem, "id") %>' />
                <%#DataBinder.Eval(Container.DataItem, "name") %>
            </td>
            <td>
                <asp:Image runat="server" ID="imgOk" ImageUrl="~/img/iconComplete.png" Height="20" />
                <asp:Image runat="server" ID="imgKo" ImageUrl="~/img/iconCancel.jpg" Height="20" />
            </td>
            <td><asp:HyperLink runat="server" ID="lnkCfg" NavigateUrl="~/Configuration/wizConfigReparti_Detail.aspx" Target="_blank">
                <asp:Image runat="server" ID="imgCfg" ImageUrl="~/img/iconConfiguration.png" Height="20" />
                </asp:HyperLink></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>