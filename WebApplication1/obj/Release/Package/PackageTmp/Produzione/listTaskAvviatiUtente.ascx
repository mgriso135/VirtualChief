<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTaskAvviatiUtente.ascx.cs" Inherits="KIS.Produzione.listTaskAvviatiUtente" %>

<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
        <h1 runat="server" id="lblTitle">Task avviati</h1>
<asp:Label runat="server" ID="lblUser" />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater runat="server" ID="rptTaskAvviati" OnItemDataBound="rptTaskAvviati_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <tr>
                        <td>Commessa</td>
                        <td>Anno</td>
                        <td>Cliente</td>
                        <td>Prodotto</td>
                        <td>Processo</td>
                        <td>Matricola</td>
                        <td>ID</td>
                        <td>Name</td>
                        <td>Early Start</td>
                        <td>Late Start</td>
                        <td>Early Finish</td>
                        <td>Late Finish</td>
                        <td>N° operatori richiesti</td>
                        <td>N° operatori attivi</td>
                        <td>Status</td>
                        <td>Operatori attivi</td>
                        <td>Postazione</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:Label runat="server" ID="lblCommessa" /></td>
                    <td><asp:Label runat="server" ID="lblAnnoCommessa" /></td>
                    <td><asp:Label runat="server" ID="lblCliente" /></td>
                    <td><asp:Label runat="server" ID="lblProdotto" /></td>
                    <td><asp:Label runat="server" ID="lblProcesso" /></td>
                    <td><asp:Label runat="server" ID="lblMatricola" /></td>
                    <td><asp:HiddenField runat="server" ID="taskID" Value='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                        <%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "EarlyStart") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "LateStart") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "EarlyFinish") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "LateFinish") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "NumOperatori") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "UtentiAttivi.Count") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
                    <td><asp:Label runat="server" ID="lblUtentiAttivi" /></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "PostazioneName") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Timer runat="server" ID="timer1" Interval="300000" OnTick="timer1_Tick" />
    </ContentTemplate>
</asp:UpdatePanel>