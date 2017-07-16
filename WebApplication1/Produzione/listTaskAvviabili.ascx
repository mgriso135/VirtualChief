<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTaskAvviabili.ascx.cs" Inherits="KIS.Produzione.listTaskAvviabili" %>

<h3><asp:literal runat="server" Text="<%$Resources:lblTasksDispo %>" /></h3>

<asp:UpdatePanel UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
<asp:Label runat="server" ID="lbl1" />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater ID="rptTasks" runat="server" OnItemDataBound="rptTasks_ItemDataBound" OnItemCommand="rptTasks_ItemCommand">
            <HeaderTemplate>
                <table class="table table-condensed">
                    <thead>
                    <tr id="tr1">
                        <th></th>
                        <th><asp:literal runat="server" id="lblOrdine" Text="<%$Resources:lblOrdine %>" /></th>
                        <th><asp:literal runat="server" id="lblCliente" Text="<%$Resources:lblCliente %>" /></th>
                        <th><asp:literal runat="server" id="lblProdotto" Text="<%$Resources:lblProdotto %>" /></th>
                        <th><asp:literal runat="server" id="lblTipoProdotto" Text="<%$Resources:lblTipoProdotto %>" /></th>
                        <th><asp:literal runat="server" id="lblQuantita" Text="<%$Resources:lblQuantita %>" /></th>
                        <th><asp:literal runat="server" id="lblMatricola" Text="<%$Resources:lblMatricola %>" /></th>
                        <th><asp:literal runat="server" id="lblID" Text="<%$Resources:lblID %>" /></th>
                        <th><asp:literal runat="server" id="lblName" Text="<%$Resources:lblName %>" /></th>
                        <th><asp:literal runat="server" id="lblEarlyStart" Text="<%$Resources:lblEarlyStart %>" /></th>
                        <th runat="server" visible="false"><asp:literal runat="server" id="lblLateStart" Text="<%$Resources:lblLateStart %>" /></th>
                        <th runat="server" visible="false"><asp:literal runat="server" id="lblEarlyFinish" Text="<%$Resources:lblEarlyFinish %>" /></th>
                        <th><asp:literal runat="server" id="lblLateFinish" Text="<%$Resources:lblLateFinish %>" /></th>
                        <th><asp:literal runat="server" id="lblNOpRichiesti" Text="<%$Resources:lblNOpRichiesti %>" /></th>
                        <th><asp:literal runat="server" id="lblNOpAttivi" Text="<%$Resources:lblNOpAttivi %>" /></th>
                        <th><asp:literal runat="server" id="lblStatus" Text="<%$Resources:lblStatus %>" /></th>
                        <th><asp:literal runat="server" id="lblOpAttivi" Text="<%$Resources:lblOpAttivi %>" /></th>
                    </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:ImageButton ID="btnStart" runat="server" Height="40" ImageUrl="~/img/iconPlay.png" CommandName="start" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' /></td>
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
                    <td runat="server" visible="false"><%#DataBinder.Eval(Container.DataItem, "LateStart") %></td>
                    <td runat="server" visible="false"><%#DataBinder.Eval(Container.DataItem, "EarlyFinish") %></td>
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