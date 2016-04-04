<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzPrintBarcordes.ascx.cs" Inherits="KIS.Commesse.wzPrintBarcordes" %>

<asp:Label runat="server" ID="lbl1" />
<div class="accordion" id="frmPrintBarcodes" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
Stampa barcordes
          </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
<asp:ImageButton runat="server" ID="imgPrintOrdini" ImageUrl="/img/iconBarcodeDoppio.png" width="40" Height="40" OnClick="imgPrintOrdini_Click" ToolTip="Stampa barcode su fogli multipli" />Stampa su fogli multipli<br />
<asp:ImageButton runat="server" ID="imgPrintOrdiniSingolo" ImageUrl="/img/iconBarcode.png" Height="40" OnClick="imgPrintOrdiniSingolo_Click" ToolTip="Stampa barcode foglio singolo" />Stampa su foglio singolo A4<br />
<asp:ImageButton runat="server" ID="imgPrintOrdiniSingoloA3" ImageUrl="/img/iconA3.gif" Height="40" CommandName="printOrdiniSingoloA3" OnClick="imgPrintOrdiniSingoloA3_Click" ToolTip="Stampa barcode foglio singolo formato A3" />Stampa su foglio singolo formato A3
</div>
                </div>
            </div>
    </div>