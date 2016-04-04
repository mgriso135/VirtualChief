<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTaskAvviabili.ascx.cs" Inherits="KIS.Produzione.listTaskAvviabili" %>

<h3>Task disponibili</h3>

<asp:UpdatePanel UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
<asp:Label runat="server" ID="lbl1" />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater ID="rptTasks" runat="server" OnItemDataBound="rptTasks_ItemDataBound" OnItemCommand="rptTasks_ItemCommand">
            <HeaderTemplate>
                <table class="table table-condensed">
                    <thead>
                    <tr id="tr1">
                        <td></td>
                        <td>Commessa</td>
                        <td>Cliente</td>
                        <td>Prodotto</td>
                        <td>Processo</td>
                        <td>Quantità</td>
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
                    </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:ImageButton ID="btnStart" runat="server" Height="40" ImageUrl="/img/iconPlay.png" CommandName="start" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' /></td>
                    <td><asp:Label runat="server" ID="lblCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td>
                    <td><asp:Label runat="server" ID="lblCliente" /></td>
                    <td><asp:Label runat="server" ID="lblProdotto" /></td>
                    <td><asp:Label runat="server" ID="lblProcesso" /></td>
                    <td><asp:Label runat="server" ID="lblQuantita" /></td>
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
                </tr>
                
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
            
        </asp:Repeater>
        
        <asp:Timer runat="server" ID="timer1" Interval="60000" OnTick="timer1_Tick" />
    </ContentTemplate>
</asp:UpdatePanel>