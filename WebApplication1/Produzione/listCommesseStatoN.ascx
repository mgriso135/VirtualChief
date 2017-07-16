<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listCommesseStatoN.ascx.cs" Inherits="KIS.Produzione.listCommesseStatoN" %>

<h3><asp:Literal runat="server" ID="lblTitle" Text="<%$Resources:lblTitle %>" /></h3>
<asp:Label runat="server" ID="lbl1" />

<asp:Repeater runat="server" ID="rptStatoN" OnItemDataBound="rptStatoN_ItemDataBound" OnItemCommand="rptStatoN_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <th></th>
                <th><asp:Literal runat="server" ID="lblID" Text="<%$Resources:lblID %>" /></th>
                <th><asp:Literal runat="server" ID="lblCliente" Text="<%$Resources:lblCliente %>" /></th>
                <th><asp:Literal runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" /></th>
                <th><asp:Literal runat="server" ID="lblTipoProdotto" Text="<%$Resources:lblTipoProdotto %>" /></th>
                <th><asp:Literal runat="server" ID="lblQuantita" Text="<%$Resources:lblQuantita %>" /></th>
                <th><asp:Literal runat="server" ID="lblDataFineProd" Text="<%$Resources:lblDataFineProd %>" /></th>
                <th><asp:Literal runat="server" ID="lblDataConsegna" Text="<%$Resources:lblDataConsegna %>" /></th>
                <th><asp:Literal runat="server" ID="lblLanciaInProd" Text="<%$Resources:lblLanciaInProd %>" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" ID="lnkAssegnaSN" ImageUrl="~/img/iconSerialNumber1.jpg" Height="40" ToolTip="<%$Resources:lblAssegnaMatricola %>" CommandName="AssegnaMatricola" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
            <td><asp:label runat="server" ID="lblMatricola" text='<%#DataBinder.Eval(Container.DataItem, "Matricola") %>' />
                <asp:TextBox runat="server" ID="txtMatricola" Text='<%#DataBinder.Eval(Container.DataItem, "Matricola") %>' Visible="false"/>
                <asp:ImageButton runat="server" ID="btnSaveSN" ImageUrl="~/img/iconSave.jpg" Visible="false" Height="40" ToolTip="<%$Resources:lblSalvaMatricola %>" CommandName="saveSN" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
                <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Visible="false" Height="40" ToolTip="<%$Resources:lblResetMatricola %>Reset matricola" CommandName="resetSN" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
            <td>
                <asp:ImageButton runat="server" ID="btnChangeDataFP" ImageUrl="~/img/iconCalendar.png" Height="40" CommandName="showChangeFP" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' />
                <%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaFineProduzione")).ToString("dd/MM/yyyy HH:mm:ss") %>
                <asp:Calendar runat="server" ID="cal" Visible="false" BackColor="White" /><br />
                <asp:label runat="server" ID="lblHH" Visible="false">HH:</asp:label>
                <asp:DropDownList runat="server" ID="ddlOra" Visible="false" CssClass="dropdown" Width="60px" />
                <asp:label runat="server" ID="lblMM"  Visible="false">mm:</asp:label>
                <asp:DropDownList runat="server" ID="ddlMinuto" Visible="false" CssClass="dropdown" Width="60px" />
                <asp:label runat="server" ID="lblSS" Visible="false">ss</asp:label>
                <asp:DropDownList runat="server" ID="ddlSecondo" Visible="false" CssClass="dropdown" Width="60px" /><br />
                <asp:ImageButton runat="server" ID="saveFP" CommandName="saveFP" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' ImageUrl="~/img/iconSave.jpg" Height="40px" Visible="false" />
                <asp:ImageButton runat="server" ID="undoFP" CommandName="undoFP" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + "/" + DataBinder.Eval(Container.DataItem, "Year") %>' ImageUrl="~/img/iconUndo.png" Height="40px" Visible="false" />
                </td>
            <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
            <td><asp:HyperLink runat="server" ID="lnkLancioProduzione" NavigateUrl='<%# "configuraProcesso.aspx?ID=" + DataBinder.Eval(Container.DataItem, "ID") + "&Year=" + DataBinder.Eval(Container.DataItem, "Year") %>'><asp:Image runat="server" ImageURL="~/img/iconProductionPlan.png" Height="40" /></asp:HyperLink></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
    </table>
    </FooterTemplate>
</asp:Repeater>