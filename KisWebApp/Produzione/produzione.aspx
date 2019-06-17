<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="produzione.aspx.cs" Title="Virtual Chief"
 MasterPageFile="~/Site.master" Inherits="KIS.ProduzioneUI.produzioneUI" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:literal runat="server" id="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>

    <ul class="thumbnails unstyled">
        <li class="span2">
<asp:HyperLink runat="server" ID="lnkNuoveCommesse" NavigateUrl="commesseDaProdurre.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="imgNuoveCommesse" ImageUrl="~/img/iconCart1.jpg" Height="100" Width="100" ToolTip="<%$Resources:lblVisOrdiniNonInseriti %>" />
    <p><asp:literal runat="server" id="lblNuoviOrdini" Text="<%$Resources:lblNuoviOrdini %>" /></p>
</asp:HyperLink>
            </li>

        <li class="span2">
<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="workLoadListReparti.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="Image2" ImageUrl="~/img/iconLabor.png" Height="100" Width="100" ToolTip="<%$Resources:lblTTCapacitaProd %>" />
    <p><asp:literal runat="server" id="lblCapacitaProd" Text="<%$Resources:lblCapacitaProd %>" /></p>
</asp:HyperLink>
            </li>

        <li class="span2">
<asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl="PianoProduzioneCompleto.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="Image3" ImageUrl="~/img/iconProductionPlan.png" Height="100" Width="100" ToolTip="<%$Resources:lblTTPianoProd %>" />
    <p><asp:literal runat="server" id="lblPianoProd" Text="<%$Resources:lblPianoProd %>" /></p>
</asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkProdottiLanciati" NavigateUrl="avanzamentoProduzione.aspx">
        
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgProdottiLanciati" ImageUrl="~/img/iconManufacturing2.png" Height="100" ToolTip="<%$Resources:lblTTAndonCompleto %>" />
        <p><asp:literal runat="server" id="lblAndonGenerale" Text="<%$Resources:lblAndonGenerale %>" /></p>
    </asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkAndonReparto" NavigateUrl="~/Produzione/AndonListReparti.aspx">
        
        <asp:Image CssClass="btn btn-primary" runat="server" ID="Image1" ImageUrl="~/img/iconOrder2.png" Height="100" ToolTip="<%$Resources:lblTTAndonReparto %>" />
        <p><asp:literal runat="server" id="lblAndonReparto" Text="<%$Resources:lblAndonReparto %>" /></p>
    </asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkStoricoProduzione" NavigateUrl="storicoProduzione.aspx">
        
        <asp:Image CssClass="btn btn-primary" BorderStyle="Dashed" BorderColor="Green" runat="server" ID="imgStoricoProduzione" ImageUrl="~/img/iconHistory.png" Height="100" ToolTip="<%$Resources:lblTTStoricoProd %>" />
        <p><asp:literal runat="server" id="lblStoricoProd" Text="<%$Resources:lblStoricoProd %>" /></p>
    </asp:HyperLink>
            </li>  </ul>
</asp:Content>