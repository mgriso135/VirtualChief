<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="postazioneWorkLoad.ascx.cs" Inherits="KIS.Produzione.postazioneWorkLoad" %>


<asp:Label runat="server" ID="lbl" />

<h3>Postazione <asp:Label runat="server" ID="lblNomePost" /> - carico di lavoro per processo</h3>
<asp:chart ID="Chart1" runat="server" width="1000px" onload="Chart1_Load">
<ChartAreas>
    <asp:ChartArea Name="ChartArea1">
        <AxisY Minimum="0">
        </AxisY>
    </asp:ChartArea>
  </ChartAreas>
</asp:chart>