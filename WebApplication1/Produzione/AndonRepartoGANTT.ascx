<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AndonRepartoGANTT.ascx.cs" Inherits="KIS.Produzione.AndonRepartoGANTT" %>

<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label runat="server" ID="lblData" />
        <br />
        <asp:Label runat="server" ID="lbl1" />

        <asp:Chart runat="server" ID="chart1" Width="1500" Height="500">
            <ChartAreas>
                
            </ChartAreas>
            <Series>

            </Series>
        </asp:Chart>

        <br />
        <br />

        <asp:Timer runat="server" ID="timer1" Interval="120000" OnTick="timer1_Tick" />
        </ContentTemplate>
    </asp:UpdatePanel>