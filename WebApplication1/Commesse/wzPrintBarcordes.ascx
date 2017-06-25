<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzPrintBarcordes.ascx.cs" Inherits="KIS.Commesse.wzPrintBarcordes" %>

<asp:Label runat="server" ID="lbl1" />
<div class="accordion" id="frmPrintBarcodes" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
<asp:Literal runat="server" ID="lblStampaBarcodes" Text="<%$Resources:lblStampaBarcodes%>" />
          </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
<asp:ImageButton runat="server" ID="imgPrintOrdini" ImageUrl="/img/iconBarcodeDoppio.png" width="40" Height="40" OnClick="imgPrintOrdini_Click" ToolTip="<%$Resources:lblStampaFogliMultipliA4 %>" />
          <asp:Label runat="server" ID="lblStampaFogliMultipliA4" Text="<%$Resources:lblStampaFogliMultipliA4 %>" />
          <br />
<asp:ImageButton runat="server" ID="imgPrintOrdiniSingolo" ImageUrl="/img/iconBarcode.png" Height="40" OnClick="imgPrintOrdiniSingolo_Click" ToolTip="<%$Resources:lblStampaFogliSingoliA4 %>" />
          <asp:Label runat="server" ID="lblStampaFogliSingoliA4" Text="<%$Resources:lblStampaFogliSingoliA4 %>" />
          <br />
<asp:ImageButton runat="server" ID="imgPrintOrdiniSingoloA3" ImageUrl="/img/iconA3.gif" Height="40" CommandName="printOrdiniSingoloA3" OnClick="imgPrintOrdiniSingoloA3_Click" ToolTip="<%$Resources:lblStampaFogliSingoliA3 %>Stampa barcode foglio singolo formato A3" />
          <asp:Label runat="server" ID="Label1" Text="<%$Resources:lblStampaFogliSingoliA3 %>" />
</div>
                </div>
            </div>
    </div>