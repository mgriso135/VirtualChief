<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AndonRepartoGANTT.aspx.cs" Inherits="KIS.Produzione.AdonRepartoGANTT" %>
<%@ Register TagPrefix="warning" TagName="listOpen" Src="~/Produzione/listWarningApertiReparto.ascx" %>
<%@ Register TagPrefix="postazioni" TagName="listUtentiLoggati" Src="~/Postazioni/listPostazioniOperatoriLoggatiReparto.ascx" %>
<%@ Register TagPrefix="produttivita" TagName="show" Src="~/Produzione/showProduttivitaReparto.ascx" %>
<%@ Register TagPrefix="gantt" TagName="show" Src="~/Produzione/AndonRepartoGANTT.ascx" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="scriptMan1" runat="server" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx">Produzione</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="AndonListReparti.aspx">Andon Reparto GANTT</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="<%#Request.RawUrl %>">Andon</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3>Andon reparto</h3>
    <h3><asp:Label runat="server" ID="lblReparto" /></h3>
    <div class="row-fluid">
    <warning:listOpen runat="server" id="frmListWarningAperti" />
        </div>
    <div class="row-fluid">
    <produttivita:show runat="server" id="frmShowProduttiva" />
        </div>
    <div class="row-fluid">
        <div class="span12">
    
        
        <gantt:show runat="server" id="frmGANTT" />
        
        </div>
        </div>


    <div class="row-fluid">
        <div class="span9">
            <asp:UpdatePanel runat="server" ID="updStatoArticoli" UpdateMode="Conditional">
        <ContentTemplate>
            <h5>Articoli da avviare</h5>
            <asp:Label runat="server" ID="lbl1" />

    <asp:Repeater runat="server" ID="rptElencoArticoliNP" OnItemDataBound="rptElencoArticoliNP_ItemDataBound">
        <HeaderTemplate>
            <table>
                <tr style="font-size:18px; font-family:Calibri">
                    <td></td>
                    <td><asp:LinkButton runat="server" ID="btnSortCommessa">Commessa</asp:LinkButton></td>
                    <td>
                        <asp:LinkButton runat="server" ID="btnSortID">Articolo</asp:LinkButton>
                    </td>
                    <td><asp:LinkButton runat="server" ID="btnSortCliente">Cliente</asp:LinkButton></td>
                    <td><asp:LinkButton runat="server" ID="btnSortAnnoCommessa">Tipo prodotto</asp:LinkButton></td>
                    <td><asp:LinkButton runat="server" ID="btnSortMatricola">Matricola</asp:LinkButton></td>
                    <td><asp:LinkButton runat="server" ID="btnSortStatus">Status</asp:LinkButton></td>
                    <td><asp:LinkButton runat="server" ID="btnSortDataFP">Data inizio produzione</asp:LinkButton></td>
                    <td><asp:linkbutton runat="server" ID="btnSortDataPC">Data consegna</asp:linkbutton></td>
                    <td><asp:linkbutton runat="server" ID="btnSortRitardo">Ritardo (ore)</asp:linkbutton></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1" style="font-size:14px; font-family:Calibri">
                <td><asp:HyperLink runat="server" ID="lnkStatoArticolo" NavigateUrl='<%# "statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>' Target="_blank">
                            <asp:Image runat="server" ID="imgStatoArticolo" ImageUrl="/img/iconView.png" Height="40" ToolTip="Visualizza lo stato di avanzamento dell'articolo" />
                        </asp:HyperLink></td>
                <td><asp:Label runat="server" ID="lblIDCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td>
                <td><asp:HiddenField runat="server" ID="lblIDArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <asp:HiddenField runat="server" ID="lblAnnoArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                <td><asp:Label runat="server" ID="lblCliente" /></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Proc.Process.ProcessName") %> - <%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td> 
                <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
                <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "LateStart")).ToString("dd/MM/yyyy HH:mm:ss") %></td>
                <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
                <td><%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "Ritardo")).TotalHours.ToString() %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
            <asp:Timer runat="server" ID="TimeCheck" OnTick="TimeCheck_Tick" Interval="300000" />
            </ContentTemplate>
        </asp:UpdatePanel>

        </div>
        <div class="span3">
            <br /><br /><br /><br /><postazioni:listUtentiLoggati runat="server" ID="frmLstUtentiPostazioni" /></div>
    </div>    
    <table>
        <tr>
            <td>

   
    

    


                </td>

            <td style="vertical-align: top;">
                <br />
                <br />
                <br />
                <br /> <br />
                <br />
                <br />
                <br />
                 

            </td>

            </tr>
        </table>

</asp:Content>
