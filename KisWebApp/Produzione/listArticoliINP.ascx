<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listArticoliINP.ascx.cs" Inherits="KIS.Produzione.listArticoliINP" %>


<asp:ScriptManager runat="server" ID="ScriptMan1" />
<asp:UpdatePanel runat="server" ID="updProduction" UpdateMode="Conditional">

    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
<asp:Label runat="server" ID="lblDataUpdate" />
<asp:Repeater runat="server" ID="rptArticoli" OnItemDataBound="rptArticoli_ItemDataBound" OnItemCommand="rptArticoli_ItemCommand">
    <HeaderTemplate>
                <table class="table table-condensed">
                    <thead>
                       <tr style="font-size:18px; font-family:Calibri">
                    <th></th>
                           <th></th>
                           <th></th>
                           <th></th>
                           <th><asp:Literal runat="server" ID="lblKanbanCard" Text="<%$Resources:lblKanbanCard %>" /><br />
                               <asp:LinkButton ID="lnkKanbanCardUp" runat="server" onclick="lnkKanbanCardUp_Click">
      <asp:Image ID="imgKanbanCardUp" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkKanbanCardDown" runat="server" onclick="lnkKanbanCardDown_Click">
      <asp:Image ID="imgKanbanCardDown" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                           </th>
                    <th><asp:Literal runat="server" ID="lblProdotto1" Text="<%$Resources:lblProdotto %>" /><br />
                        <asp:LinkButton ID="lnkArticoloUp" runat="server" onclick="lnkArticoloUp_Click">
      <asp:Image ID="Image3" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkArticoloDown" runat="server" onclick="lnkArticoloDown_Click">
      <asp:Image ID="Image4" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    </th>
                    <th><asp:Literal runat="server" ID="lblCliente" Text="<%$Resources:lblCliente %>" /><br />
                        <asp:LinkButton ID="lnkClienteUp" runat="server" onclick="lnkClienteUp_Click">
      <asp:Image ID="Image5" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkClienteDown" runat="server" onclick="lnkClienteDown_Click">
      <asp:Image ID="Image6" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    </th>
                    <th><asp:Literal runat="server" ID="lblOrdine" Text="<%$Resources:lblOrdine %>" />
                        <br />
                        <asp:LinkButton ID="lnkCommessaUp" runat="server" onclick="lnkCommessaUp_Click">
      <asp:Image ID="Image7" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkCommessaDown" runat="server" onclick="lnkCommessaDown_Click">
      <asp:Image ID="Image8" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    </th>
                           <th><asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblTHExternalID %>" />
                               <asp:LinkButton ID="lnkExternalIDUp" runat="server" onclick="lnkExternalIDUp_Click">
      <asp:Image ID="Image19" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkExternalIDDown" runat="server" onclick="lnkExternalIDDown_Click">
      <asp:Image ID="Image20" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                           </th>
                    <th><asp:Literal runat="server" ID="lblProdotto" Text="<%$Resources:lblProdotto %>" /><br />
                        <asp:LinkButton ID="lnkProcessoUp" runat="server" onclick="lnkProcessoUp_Click">
      <asp:Image ID="Image9" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkProcessoDown" runat="server" onclick="lnkProcessoDown_Click">
      <asp:Image ID="Image10" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton></th>
                    <th><asp:Literal runat="server" ID="lblQuantita" Text="<%$Resources:lblQuantita %>" /><br />
                        <asp:LinkButton ID="lnkQuantitaUp" runat="server" onclick="lnkQuantitaUp_Click">
      <asp:Image ID="Image11" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkQuantitaDown" runat="server" onclick="lnkQuantitaDown_Click">
      <asp:Image ID="Image12" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton></th>
                    <th><asp:Literal runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" /><br />
                        <asp:LinkButton ID="lnkMatricolaUp" runat="server" onclick="lnkMatricolaUp_Click">
      <asp:Image ID="Image13" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkMatricolaDown" runat="server" onclick="lnkMatricolaDown_Click">
      <asp:Image ID="Image14" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton></th>
                    <th><asp:Literal runat="server" ID="lblStatus" Text="<%$Resources:lblStatus %>" /><br />
                        <asp:LinkButton ID="lnkStatusUp" runat="server" onclick="lnkStatusUp_Click">
      <asp:Image ID="Image15" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkStatusDown" runat="server" onclick="lnkStatusDown_Click">
      <asp:Image ID="Image16" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton></th>
                    <th><asp:Literal runat="server" ID="lblDataFineProd" Text="<%$Resources:lblDataFineProd %>" /><br /><asp:LinkButton ID="lnkDataFineProdUp" runat="server" onclick="lnkDataFineProdUp_Click">
      <asp:Image ID="imgDataFineProdUp" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkDataFineProdDown" runat="server" onclick="lnkDataFineProdDown_Click">
      <asp:Image ID="imgDataFineProdDown" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    </th>
                    <th><asp:Literal runat="server" ID="lblDataConsegna" Text="<%$Resources:lblDataConsegna %>" /><br />
                        <asp:LinkButton ID="lnkDataConsegnaUp" runat="server" onclick="lnkDataConsegnaUp_Click">
      <asp:Image ID="Image1" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkDataConsegnaDown" runat="server" onclick="lnkDataConsegnaDown_Click">
      <asp:Image ID="Image2" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    </th>
                    <th><asp:Literal runat="server" ID="lblReparto" Text="<%$Resources:lblReparto %>" /><br />
                        <asp:LinkButton ID="lnkRepartoUp" runat="server" onclick="lnkRepartoUp_Click">
      <asp:Image ID="Image17" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                        <asp:LinkButton ID="lnkRepartoDown" runat="server" onclick="lnkRepartoDown_Click">
      <asp:Image ID="Image18" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton></th>
                           
                           <th></th>
                </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-size:14px; font-family:Calibri">
                    <td><asp:HyperLink Width="40" runat="server" ID="lnkStatoArticolo" NavigateUrl='<%# "statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>' Target="_blank">
                            <asp:Image runat="server" ID="imgStatoArticolo" ImageUrl="~/img/iconView.png" ToolTip="<%$Resources:lblTTStatoAvanzamento %>" />
                        </asp:HyperLink></td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgPrintOrdini" ImageUrl="~/img/iconBarcode.png" Width="20" Height="20" CommandName="printOrdini" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + ";" + DataBinder.Eval(Container.DataItem, "Year") %>' ToolTip="<%$Resources:lblTTStampaBarcodeMultipli %>" /><br />
                        <asp:ImageButton runat="server" ID="imgPrintOrdini2" ImageUrl="~/img/iconBarcode.png" Height="20" CommandName="printOrdini" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + ";" + DataBinder.Eval(Container.DataItem, "Year") %>' ToolTip="<%$Resources:lblTTStampaBarcodeMultipli %>" />
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgPrintOrdiniSingolo" ImageUrl="~/img/iconBarcode.png" Width="40" Height="40" CommandName="printOrdiniSingolo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + ";" + DataBinder.Eval(Container.DataItem, "Year") %>' ToolTip="<%$Resources:lblTTStampaBarcodeSingolo %>" />
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imgPrintOrdiniSingoloA3" ImageUrl="~/img/iconA3.gif" Width="40" Height="40" CommandName="printOrdiniSingoloA3" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") + ";" + DataBinder.Eval(Container.DataItem, "Year") %>' ToolTip="<%$Resources:lblTTStampaBarcodeSingoloA3 %>" />
                    </td>
                    <td><asp:hyperlink runat="server" ID="lnkPrintKanbanCard" NavigateUrl='<%# "https://app.kanbanbox.com/printer/print_ekanban/"+DataBinder.Eval(Container.DataItem, "KanbanCardID") %>' ToolTip="<%$Resources:lblTTStampaKanban %>" Target="_blank">
                        <asp:Image runat="server" ID="imgPrintKanbanCard" ImageUrl="~/img/kanban.gif" Height="40" />
                        </asp:hyperlink>
                        <%#DataBinder.Eval(Container.DataItem, "KanbanCardID") %></td>
                <td><asp:HiddenField runat="server" ID="lblIDArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <asp:HiddenField runat="server" ID="lblAnnoArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "RagioneSocialeCliente") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Commessa") %>/<%#DataBinder.Eval(Container.DataItem, "AnnoCommessa") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "ProductExternalID") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                <td><asp:label runat="server" ID="lblStatus" Text='<%#DataBinder.Eval(Container.DataItem, "Status") %>' /></td>
                <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaFineProduzione")).ToString("dd/MM/yyyy HH:mm:ss") %></td>
                <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "RepartoNome") %></td>
                    
                    <td>
                        <asp:HyperLink Width="40" runat="server" ID="lnkRipianifica" NavigateUrl="~/Commesse/wzAssociaPERTReparto.aspx" Target="_blank">
                            <asp:Image runat="server" ID="imgRipianifica" ImageUrl="~/img/iconChangePlan.png" Height="40" ToolTip="<%$Resources:lblRipianifica %>" Visible="false" />
                        </asp:HyperLink>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></tbody>
                </table>
            </FooterTemplate>
</asp:Repeater>

        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="60000" />
        </ContentTemplate>
    </asp:UpdatePanel>