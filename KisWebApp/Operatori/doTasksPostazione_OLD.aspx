<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="doTasksPostazione_OLD.aspx.cs" Inherits="KIS.Operatori.doTasksPostazione" %>
<%@ Register TagPrefix="Task" TagName="ElencoAvviati" Src="~/Produzione/listTaskAvviatiPostazioneUtente.ascx" %>
<%@ Register TagPrefix="Task" TagName="ElencoAvviabili" Src="~/Produzione/listTaskAvviabili.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="#"><asp:Literal runat="server" ID="lblWorkplace" Text="<%$Resources:lblNavWorkplace %>" />
                            </a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="checkInPostazione.aspx"><asp:Literal runat="server" ID="lblNavWebGemba" Text="<%$Resources:lblNavWebGemba %>" /></a>
						<span class="divider">/</span>
					</li>
         <li>
						<a href="<%#Request.RawUrl %>"><asp:Literal runat="server" ID="lblNavPostazione" Text="<%$Resources:lblNavPostazione %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3><asp:Label runat="server" ID="lblPostazione" /></h3>
    <asp:Label runat="server" ID="lbl1" />

    <task:ElencoAvviati runat="server" id="frmLstTaskAvviati" />
    <Task:ElencoAvviabili runat="server" id="frmLstTaskAvviabili" />

</asp:Content>
