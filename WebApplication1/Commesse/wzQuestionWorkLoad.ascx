<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzQuestionWorkLoad.ascx.cs" Inherits="KIS.Commesse.wzQuestionWorkLoad1" %>
<asp:Label runat="server" ID="lbl1"/>

<table runat="server" id="tblShowQuestion">
    <tr>
        <td rowspan="2">
            <asp:HyperLink runat="server" ID="lnkGoBack" NavigateUrl="wzInserisciDataConsegna.aspx">
            <asp:Image runat="server" ID="Image1" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
                </asp:HyperLink>
        </td>
        <td>Verifica il carico di lavoro del reparto</td>
        <td style="text-align:right;">
            <asp:HyperLink runat="server" ID="lnkFwdCheckWorkLoad" NavigateUrl="wzCheckWorkLoadReparto.aspx">
            <asp:Image runat="server" ID="imgGoFwd1" ImageUrl="~/img/iconArrowRight.png" Height="40" />
                </asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            
Pianifica il prodotto in produzione direttamente
                
        </td>
        
        <td>
            <asp:HyperLink runat="server" ID="lnkFwdDeliveryDate" NavigateUrl="wzCheckDeliveryDate.aspx">
            <asp:Image runat="server" ID="imgGoFwd2" ImageUrl="~/img/iconArrowRight.png" Height="40" /></asp:HyperLink></td>
    </tr>
</table>