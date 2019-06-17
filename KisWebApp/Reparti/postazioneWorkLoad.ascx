<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="postazioneWorkLoad.ascx.cs" Inherits="KIS.Produzione.postazioneWorkLoad" %>


<asp:Label runat="server" ID="lbl" />

<h3><asp:Literal runat="server" ID="lblPostazione" Text="<%$Resources:lblPostazione %>" />&nbsp;
    <asp:Label runat="server" ID="lblNomePost" />&nbsp;-&nbsp;<asp:Literal runat="server" ID="lblCaricoPerProd" Text="<%$Resources:lblCaricoPerProd %>" /></h3>
<asp:chart ID="Chart1" runat="server" width="1000px" onload="Chart1_Load">
<ChartAreas>
    <asp:ChartArea Name="ChartArea1">
        <AxisY Minimum="0">
        </AxisY>
    </asp:ChartArea>
  </ChartAreas>
</asp:chart>