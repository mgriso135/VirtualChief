<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  
CodeBehind="showProcesso.aspx.cs" Inherits="KIS.Processi.showProcesso" ValidateRequest="false" %>
<%@Register TagPrefix="kpi" TagName="add" src="/kpi/addKPIForm.ascx" %>
<%@Register TagPrefix="kpi" TagName="list" src="/kpi/listKPIs.ascx" %>
<%@Register TagPrefix="pert" TagName="showPert" src="/Processi/showPert.ascx" %>

<%@Register TagPrefix="variante" TagName="edit" src="/Processi/editVariante.ascx" %>
<%@ Register TagPrefix="processovariante" TagName="addSubProcess" Src="~/Processi/linkSubProcessVariante.ascx" %>

<%@Reference Control="/Processi/boxProcesso.ascx" %>



<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx">Process Manager</a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>">Dettaglio processo</a>
						<span class="divider">/</span>
					</li>
				</ul>
<asp:Label runat="server" ID="lblErr" />

<asp:Label runat="server" ID="lblLinkFatherProc" />
<asp:Label runat="server" ID="lblProcID" Visible="false" />
<table class="table table-condensed">
<tr>
    <td>Nome:</td>
    <td><asp:label runat="server" id="lblProcName" style="font-size:20px;font-weight:bold;"/>
        <asp:TextBox runat="server" ID="inputProcName" style="font-size:20px;" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcName" ForeColor="Red" ErrorMessage="Campo obbligatorio" />
    </td>
    <td rowspan="5">
        <asp:ImageButton runat="server" Enabled="false" ID="newRevision" ImageUrl="/img/iconNewRevision.png" Height="100px" OnClick="newRevision_Click" ToolTip="Crea una nuova revisione del processo vuota" />
        <br />
        <asp:ImageButton runat="server" Visible="false" Enabled="false" ID="newRevisionCopy" ImageUrl="/img/iconCopyFolder.png" Height="100px" OnClick="newRevisionCopy_Click" ToolTip="Crea una nuova revisione del processo, copiando quella corrente" />
    </td>
</tr>
<tr>
    <td>
Descrizione:
    </td>
    <td><asp:label runat="server" id="lblProcDesc" style="font-size:16px;" /><asp:TextBox runat="server" ID="inputProcDesc" style="font-size:14px;" TextMode="MultiLine" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcDesc" ForeColor="Red" ErrorMessage="Campo obbligatorio" />
    </td>
</tr>
    <tr>
        <td colspan="2">
Revisione <asp:Label runat="server" ID="lblRevisione" style="font-size: 16px;" /> del <asp:Label runat="server" ID="lblDataRevisione" style="font-size: 16px;" />
            </td>
        </tr>
    <tr>
<td>Tipo di diagramma:</td>
<td style="font-size: 16px;">
<asp:label runat="server" id="vsm" />
<asp:DropDownList runat="server" ID="inputVSM">
<asp:ListItem Value="1">VSM</asp:ListItem>
<asp:ListItem Value="0">Pert</asp:ListItem>
</asp:DropDownList>
</td>
        <td rowspan="3">
            <kpi:add runat="server" ID="Add" Visible="false" />
        <kpi:list runat="server" ID="ListKPI" Visible="false" />
            
        </td>
        </tr>
    <tr>
        <td colspan="2">
<asp:imagebutton runat="server" id="imgEdit" ImageUrl="/img/edit.png" Width="50px" onClick="imgEdit_Click" AlternateText="Modifica" ToolTip="Modifica"/>
<asp:imagebutton runat="server" id="imgDeleteProcess" ImageUrl="/img/iconDelete.png" Width="50px" onClick="imgDeleteProcess_Click" AlternateText="Cancella il processo" ToolTip="Cancella il processo"/>
<asp:imagebutton runat="server" id="imgSave" ImageUrl="/img/iconSave.jpg" Width="50px" OnClick="imgSave_Click" AlternateText="Salva le modifiche" ToolTip="Salva le modifiche" />
<asp:imagebutton runat="server" id="imgCancel" ImageUrl="/img/iconCancel.jpg" Width="50px" OnClick="imgCancel_Click" AlternateText="Non salvare le modifiche" ToolTip="Non salvare le modifiche" />
  </td></tr>
</table>
<br />
    <div class="row">

        <div class="span6">
            <h3>
        <asp:Label runat="server" ID="lblTitoloVarianti">Seleziona una variante</asp:Label>
                </h3>
    
<asp:Label runat="server" ID="lblVarianti" />
            </div>

        <div class="span6">
            <h3>Nuova variante</h3>
            <table class="table table-bordered table-condensed">
                    <tr>
                        <td>
    <asp:ImageButton runat="server" ID="addVariante" OnClick="addVariante_Click" ImageUrl="/img/iconAdd.jpg" ToolTip="Aggiungi una nuova variante vuota" Height="40px" />Variante vuota
                            </td>
    </tr>
                <tr runat="server" id="trCopiaPERT">
                    <td>
            <asp:Label runat="server" ID="lblCopiaPERT">
                
    Copia PERT esistente<asp:DropDownList runat="server" ID="ddlCopiaPERT" /></asp:Label>
            <br /><asp:checkbox runat="server" ID="chkCopia" Checked="true" Enabled="false" />Riutilizza i tasks esistenti<br />
            <asp:checkbox runat="server" ID="chkCopiaTC" Checked="true" />Copia tempi ciclo<br />
            <asp:checkbox runat="server" ID="chkCopiaReparti" Checked="true" OnCheckedChanged="chkCopiaReparti_CheckedChanged" AutoPostBack="true" />Copia reparti assegnati al prodotto<br />
            <asp:checkbox runat="server" ID="chkCopiaPostazioni" Checked="true" OnCheckedChanged="chkCopiaPostazioni_CheckedChanged" AutoPostBack="true" />Copia le postazioni assegnate ai tasks
                        <br />
                        <asp:ImageButton runat="server" ID="btnCopiaPERT" ImageUrl="~/img/iconSave.jpg" Height="40px" OnClick="btnCopiaPERT_Click" />
                        <asp:Label runat="server" ID="lblCopiaPERTLog" />
                        </td>
                    </tr>
            </table>
        </div>
    </div>
    
    <br />
    <br />
    <asp:Label runat="server" ID="lblTitoloVariante" Font-Size="16" Font-Bold="true" />
    <asp:ImageButton runat="server" ID="showEditVariante" ImageUrl="/img/edit.png" OnClick="showEditVariante_Click" Height="40px" ToolTip="Modifica variante" />
    <asp:ImageButton runat="server" ID="deleteVariante" ImageUrl="/img/iconDelete.png" OnClick="deleteVariante_Click" Height="40" ToolTip="Cancella la variante" />
    <br />
    <asp:Label runat="server" ID="lblDescrizioneVariante" />
    <br />
    <asp:Label runat="server" ID="lblLogVariante" />
    <br />
    <variante:edit runat="server" id="editVariante" />
    <br />
    <ul class="breadcrumb hidden-phone" runat="server" id="tblPertNavBar">
					<li>
						<a class="lead" href="<%#Request.RawUrl %>"><strong>1. Crea il processo produttivo</strong></a>
						<span class="divider">/</span>
                        <asp:HyperLink runat="server" ID="lnkLinkReparto" NavigateUrl="lnkProcessoVarianteReparto.aspx">
                            2. Associa il processo produttivo al reparto
                            </asp:HyperLink>
						<span class="divider">/</span>
                        <a href="#">3. Associa i task alle postazioni di esecuzione</a>
						<span class="divider">/</span>
					</li>
				</ul>

    
    <br />
<asp:panel style="height:200px; position: static;" runat="server" id="containerVSM">
    
<asp:PlaceHolder runat="server" ID="ProcStream" />
</asp:panel>


<pert:showPert runat="server" ID="pert" />




</asp:Content>