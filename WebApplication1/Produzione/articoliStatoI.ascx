<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="articoliStatoI.ascx.cs" Inherits="KIS.Produzione.articoliStatoI" %>
<%@ Reference Control="~/Produzione/StatoTasksArticolo.ascx" %>


<h5 runat="server" id="lblTitle">Articoli avviati</h5>
<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater runat="server" ID="rptArticoliAvviati" OnItemDataBound="rptArticoliAvviati_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <thead>
                       <tr runat="server" id="trHead_Prod">
                    <th></th>
                </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-size:20px; font-family:Calibri;  border-bottom: 1px dashed blue; border-top: 1px dashed blue;border-left:1px dashed blue; border-right:1px dashed blue; font-weight:bold;">
                    <td><asp:HyperLink runat="server" ID="lnkStatoArticolo" NavigateUrl='<%# "statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>' Target="_blank">
                            <asp:Image runat="server" ID="imgStatoArticolo" ImageUrl="/img/iconView.png" Width="40" ToolTip="Visualizza lo stato di avanzamento dell'articolo" />
                        </asp:HyperLink>
                        <asp:HiddenField runat="server" ID="lblIDArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <asp:HiddenField runat="server" ID="lblAnnoArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                </td>
                </tr>
                <tr style="border-right: 1px dashed blue;border-left: 1px dashed blue;border-bottom: 1px dashed blue;">
                    <td runat="server" id="tr2">

            <asp:Repeater runat="server" ID="rptStatoTasks" OnItemDataBound="rptStatoTasks_ItemDataBound">
            <HeaderTemplate>
                <table width="100%">
                    <tr style="font-family: Calibri; font-size: 18px;" width="100%">
                    
            </HeaderTemplate>
            <ItemTemplate>
                <td runat="server" id="tr1" style="font-family: Calibri; font-size: 14px;">
                    <asp:HiddenField runat="server" ID="hTaskID" Value='<%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                    <asp:Label runat="server" ID="lblTaskFields" />
                </td>
            </ItemTemplate>
            <FooterTemplate>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
                        
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Timer runat="server" ID="timer1" Interval="120000" OnTick="timer1_Tick" />
    </ContentTemplate>
</asp:UpdatePanel>