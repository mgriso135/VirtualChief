<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTaskAvviatiPostazioneUtente.ascx.cs" Inherits="KIS.Produzione.listTaskAvviatiPostazioneUtente" %>

<h1 runat="server" id="lblTitle">Task avviati da te</h1>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <br />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater runat="server" ID="rptTaskAvviati" OnItemCommand="rptTaskAvviati_ItemCommand" OnItemDataBound="rptTaskAvviati_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-condensed">
                    <thead>
                    <tr>
                        <td></td>
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
                        <td>N° op richiesti</td>
                        <td>N° op attivi</td>
                        <td>Status</td>
                        <td>Operatori attivi</td>
                    </tr>
                        </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-size: 14px; font-family:Calibri;text-align:center">
                    <td><asp:ImageButton ID="btnPause" runat="server" Height="60" ImageUrl="/img/iconPause.png" CommandName="pause" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ToolTip="Metti il task in pausa" /></td>
                    <td><asp:ImageButton ID="btnWarning" runat="server" Height="60" ImageUrl="/img/iconWarning.png" CommandName="warning" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ToolTip="Segnala un problema!" />
                        <asp:Image runat="server" ID="imgWarningCalled" ImageUrl="/img/problemIcon.jpg" Visible="false" Height="80" ToolTip="Hai chiamato uno warning!" />
                    </td>
                    <td><asp:Label runat="server" ID="lblCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td>
                    <td><asp:Label runat="server" ID="lblCliente" /></td>
                    <td><asp:Label runat="server" ID="lblProdotto" /></td>
                    <td><asp:Label runat="server" ID="lblProcesso" /></td>
                    <td><asp:Label runat="server" ID="lblQuantita" />
                        <asp:ImageButton runat="server" Height="20" ImageUrl="~/img/iconQuantity.jpg" ID="imgChangeQty" CommandName="ChangeQty" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ToolTip="Cambia la quantità effettivamente prodotta" />
                        <asp:TextBox runat="server" ID="txtChangeQty" Visible="false" Width="20" />
                        <asp:ImageButton runat="server" ID="txtChangeQtySave" Visible="false" Height="20" CommandName="ChangeQtySave" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ImageUrl="~/img/iconSave.jpg" />
                        <asp:ImageButton runat="server" ID="txtChangeQtyUndo" Visible="false" Height="20" CommandName="ChangeQtyUndo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ImageUrl="~/img/iconUndo.png" />
                    </td>
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
                    <td><asp:ImageButton ID="btnEnd" runat="server" Height="60" ImageUrl="/img/iconComplete.png" CommandName="end" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' ToolTip="ATTIVITA' COMPLETATA" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Timer runat="server" ID="timer1" Interval="300000" OnTick="timer1_Tick" />
    </ContentTemplate>
</asp:UpdatePanel>