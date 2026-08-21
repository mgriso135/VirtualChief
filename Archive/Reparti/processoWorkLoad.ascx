<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="processoWorkLoad.ascx.cs" Inherits="KIS.Produzione.processoWorkLoad" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Label runat="server" ID="lbl" />




   <AxisY Minimum="0">
        </AxisY>
  </ChartAreas>

        <asp:Timer runat="server" ID="timer1" OnTick="Chart1_Load" Interval="10000" />
        </ContentTemplate>
    </asp:UpdatePanel>