<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="produzione.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.ProduzioneUI.produzioneUI" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx">Produzione</a>
						<span class="divider">/</span>
					</li>
				</ul>

    <ul class="thumbnails unstyled">
        <li class="span2">
<asp:HyperLink runat="server" ID="lnkNuoveCommesse" NavigateUrl="commesseDaProdurre.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="imgNuoveCommesse" ImageUrl="/img/iconCart1.jpg" Height="100" Width="100" ToolTip="Visualizza le commesse non inserite in produzione" />
    <p>Nuove commesse</p>
</asp:HyperLink>
            </li>

        <li class="span2">
<asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="workLoadListReparti.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="Image2" ImageUrl="/img/iconLabor.png" Height="100" Width="100" ToolTip="Calcolo della capacità produttiva richiesta" />
    <p>Capacità produttiva</p>
</asp:HyperLink>
            </li>

        <li class="span2">
<asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl="PianoProduzioneCompleto.aspx">
    <asp:Image CssClass="btn btn-primary" runat="server" ID="Image3" ImageUrl="/img/iconProductionPlan.png" Height="100" Width="100" ToolTip="Calcolo della capacità produttiva richiesta" />
    <p>Piano produttivo</p>
</asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkProdottiLanciati" NavigateUrl="avanzamentoProduzione.aspx">
        
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgProdottiLanciati" ImageUrl="/img/iconManufacturing2.png" Height="100" ToolTip="Andon completo" />
        <p>Andon completo</p>
    </asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkAndonReparto" NavigateUrl="/Produzione/AndonListReparti.aspx">
        
        <asp:Image CssClass="btn btn-primary" runat="server" ID="Image1" ImageUrl="/img/iconOrder2.png" Height="100" ToolTip="Andon completo" />
        <p>Andon reparto</p>
    </asp:HyperLink>
            </li>

        <li class="span2">
    <asp:HyperLink runat="server" ID="lnkStoricoProduzione" NavigateUrl="storicoProduzione.aspx">
        
        <asp:Image CssClass="btn btn-primary" BorderStyle="Dashed" BorderColor="Green" runat="server" ID="imgStoricoProduzione" ImageUrl="/img/iconHistory.png" Height="100" ToolTip="Visualizza lo storico di produzione" />
        <p>Storico produzione</p>
    </asp:HyperLink>
            </li>  </ul>
</asp:Content>