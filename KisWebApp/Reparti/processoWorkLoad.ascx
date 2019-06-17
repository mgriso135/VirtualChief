<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="processoWorkLoad.ascx.cs" Inherits="KIS.Produzione.processoWorkLoad" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Label runat="server" ID="lbl" />


<asp:Chart ID="Chart1" runat="server" width="1000px" onload="Chart1_Load">
<ChartAreas>
   <asp:ChartArea Name="ChartArea1"><AxisY Minimum="0">
        </AxisY></asp:ChartArea>
  </ChartAreas>
</asp:Chart>
        <asp:Timer runat="server" ID="timer1" OnTick="Chart1_Load" Interval="10000" />
        </ContentTemplate>
    </asp:UpdatePanel>