<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="userWorkLoad.ascx.cs" Inherits="WebApplication1.Produzione.userWorkLoad" %>


<asp:Label runat="server" ID="lbl" />
<asp:chart ID="Chart1" runat="server" width="1000px" onload="Chart1_Load">

<ChartAreas>
    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
  </ChartAreas>
</asp:chart>