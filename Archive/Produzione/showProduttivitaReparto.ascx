<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showProduttivitaReparto.ascx.cs" Inherits="KIS.Produzione.showProduttivitaReparto" %>
<asp:Label runat="server" ID="lbl1" />
<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
<ContentTemplate>
    <table runat="server" id="frmShowDatiProdReparto">
        <tr width="100%">
            <td>
                
                <asp:label runat="server" id="lblFineProgrammate" />
    <asp:label runat="server" id="lblFineAttuale" />
                <br />
                <asp:Label runat="server" ID="intervallo" />
    </td></TR><tr>
            <td style="vertical-align:top">
                <asp:Repeater runat="server" ID="rptElencoArticoliTerminati">
        <HeaderTemplate>
            <table>
                <tr style="font-size:8px; font-family:Calibri">
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHOrdine %>" /></td>
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHProdotto %>" /></td>
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHCliente %>" /></td>
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHNomeProdotto %>" /></td>
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHSerialNumber %>" /></td>
                    <td><asp:literal runat="server" Text="<%$Resources:lblTHDeliveryDate %>" /></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1" style="font-size:10px; font-family:Calibri; background-color:silver">

                <td><%#DataBinder.Eval(Container.DataItem, "Commessa") %>/<%#DataBinder.Eval(Container.DataItem, "AnnoCommessa") %></td>
                <td><asp:HiddenField runat="server" ID="lblIDArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <asp:HiddenField runat="server" ID="lblAnnoArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Proc.Process.ProcessName") %> - <%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td> 
                <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
            </td>
        </tr>
    </table>


    <asp:Timer runat="server" ID="time1" Interval="600000" OnTick="time1_Tick" />
</ContentTemplate>
    </asp:UpdatePanel>