<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatoTasksArticolo.ascx.cs" Inherits="KIS.Produzione.StatoTasksArticolo" %>
<asp:Label runat="server" ID="lbl1" />
<asp:UpdatePanel runat="server" ID="updStatoTasks" UpdateMode="Conditional">

    <ContentTemplate>
<asp:Label runat="server" ID="lblDataUpdate" />
        <asp:Repeater runat="server" ID="rptTasks" OnItemDataBound="rptTasks_ItemDataBound">
            <HeaderTemplate>
                <table style="border:1px dashed blue">
                    <tr style="font-family: Calibri; font-size: 18px; font-weight: bold;">
                        <td><asp:Literal runat="server" ID="lblTHID" Text="<%$Resources:lblTHID %>" /></td>
                        <td><asp:Literal runat="server" ID="lblTHNome" Text="<%$Resources:lblTHNome %>" /></td>
                        <td><asp:Literal runat="server" ID="lblTHStatus" Text="<%$Resources:lblTHStatus %>" /></td>
                        <td><asp:Literal runat="server" ID="lblTHPostazione" Text="<%$Resources:lblTHPostazione %>" /></td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-family: Calibri; font-size: 14px;">
                    <td><asp:HiddenField runat="server" ID="hTaskID" Value='<%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Status") %></td>
                    <td><asp:Label runat="server" ID="lblPostazione" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="6000000" />
    </ContentTemplate>
</asp:UpdatePanel>