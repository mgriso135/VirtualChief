<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzCheckDeliveryDate.ascx.cs" Inherits="KIS.Commesse.wzCheckDeliveryDate1" %>

  <script type="text/javascript">
    function FireConfirm() {
        if (confirm('Stai per lanciare il prodotto in produzione. Nessuna ulteriore modifica sarà possibile. Vuoi proseguire?'))
            return true;
        else
            return false;
    }
</script>


<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblDeliveryDate" class="table table-condensed">
    <tr>
        <td>
            <asp:HyperLink runat="server" ID="lnkGoBack">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
                </asp:HyperLink>
        </td>
    <td style="vertical-align: top; text-align:center;">

    Data di fine produzione:&nbsp;<asp:calendar runat="server" id="calFineProd" Width="100px"  />
          <br />
          Ore:<asp:DropDownList runat="server" ID="calOre" CssClass="dropdown" Width="70px" />
          &nbsp;mm:<asp:DropDownList runat="server" ID="calMinuti" CssClass="dropdown"  Width="70px" />
          &nbsp;ss:<asp:DropDownList runat="server" ID="calSecondi" CssClass="dropdown" Width="70px" />
        <br />
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSave_Click" />
</td>
        <td>
            <asp:ImageButton runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" OnClientClick="return FireConfirm();" OnClick="imgGoFwd_Click" />
        </td>
       </tr></table>