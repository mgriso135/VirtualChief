<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  
CodeBehind="showProcesso.aspx.cs" Inherits="KIS.Processi.showProcesso" ValidateRequest="false" %>
<%@Register TagPrefix="pert" TagName="showPert" src="~/Processi/showPert.ascx" %>

<%@Register TagPrefix="variante" TagName="edit" src="~/Processi/editVariante.ascx" %>
<%@ Register TagPrefix="processovariante" TagName="addSubProcess" Src="~/Processi/linkSubProcessVariante.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script>
        $(document).ready(function () {
            function loadParameters() {
                if($('#<%=ProcessID.ClientID%>').val() != "-1" &&
                        $('#<%=ProcessRev.ClientID%>').val() != "-1" &&
                        $('#<%=VariantID.ClientID%>').val() !="-1")
                {
                                    var procID = parseInt($('#<%=ProcessID.ClientID%>').val());
                    var procRev = parseInt($('#<%=ProcessRev.ClientID%>').val());
                var varID = parseInt($('#<%=VariantID.ClientID%>').val());
                $('#imgLoadParams').show();
                $.ajax({
                    url: "../Products/ProductParameters/Index",
                    type: 'GET',
                    dataType: 'html',
                    data:{
                        processID: $('#<%=ProcessID.ClientID%>').val(),
                        processRev: $('#<%=ProcessRev.ClientID%>').val(),
                        variantID: $('#<%=VariantID.ClientID%>').val() 
                    },
                    success: function (result) {
                    $('#divProcessParams').html(result);
                    $('#imgLoadParams').hide();
                }
                
                });
                }
                else
                {
                    $('#imgLoadParams').hide();
                }
            }

            // Show add parameter div
            //lblShowAddParam
            // 
            $('#imgShowAddParams').click(function () {
                if ($('#addParams').is(':visible')) {
                    $('#addParams').fadeOut();
                }
                else
                {
                    $('#addParams').fadeIn();
                }
            });

            $('#imgAddParam').click(function () {
                $('#imgAddParam').fadeOut();
                $('#imgLoadAddParam').fadeIn();
                var procID = parseInt($('#<%=ProcessID.ClientID%>').val());
                var procRev = parseInt($('#<%=ProcessRev.ClientID%>').val());
                var varID = parseInt($('#<%=VariantID.ClientID%>').val());
                var paramName = $('<div/>').text($('#txtParamName').val()).html();
                var paramDescription = $('<div/>').text($('#txtParamDescription').val()).html();
                var paramIsFixed = false;
                if ($('#txtIsFixed').is(":checked")) {
                    var paramIsFixed = true;
                }
                var paramIsRequired = false;
                if ($('#txtIsRequired').is(":checked")) {
                    var paramIsRequired = true;
                }
                var paramCategory = parseInt($('#<%=ddlParamCategory.ClientID%>').val());

                if(procID > -1 && procRev > -1 && varID > -1)
                {
                    if (paramName.length > 0) {
                        if (paramCategory > -1)
                            {
                        $.ajax({
                            url: "../Products/ProductParameters/AddParam",
                            type: 'GET',
                            dataType: 'html',
                            data: {
                                processID: procID,
                                processRev: procRev,
                                variantID: varID,
                                ParamName: paramName,
                                ParamDescription: paramDescription,
                                ParamCategory: paramCategory,
                                ParamIsFixed: paramIsFixed,
                                ParamIsRequired: paramIsRequired
                            },
                            success: function (result) {
                                $('#imgAddParam').fadeIn();
                                $('#imgLoadAddParam').fadeOut();
                                $('#addParams').fadeOut();
                                loadParameters();
                                $('#txtParamName').val('');
                                $('#txtParamDescription').val('');
                                $('#txtIsFixed').attr('checked', false);
                                $('#txtIsRequired').attr('checked', false);
                                $("#<%=ddlParamCategory.ClientID%>").val('-1');
                            },
                            error: function(result)
                            {
                                $('#imgAddParam').fadeIn();
                                $('#imgLoadAddParam').fadeOut();
                                alert("<%=GetLocalResourceObject("lblParamAddError")%>");
                            },
                            warning: function (result)
                            {
                                $('#imgAddParam').fadeIn();
                                $('#imgLoadAddParam').fadeOut();
                                alert("<%=GetLocalResourceObject("lblParamAddError") %>");
                            }
                        });
                            loadParameters();
                        }
                        else
                        {
                            alert("<%=GetLocalResourceObject("lblParamErrCategory") %>");
                            $('#imgAddParam').fadeIn();
                            $('#imgLoadAddParam').fadeOut();
                        }
                    }
                    else {
                        alert("<%=GetLocalResourceObject("lblParamErrName") %>");
                        $('#imgAddParam').fadeIn();
                        $('#imgLoadAddParam').fadeOut();
                    }
                }
            });

            if ($('#<%=ProcessID.ClientID%>').val() != "-1" &&
                        $('#<%=ProcessRev.ClientID%>').val() != "-1" &&
                        $('#<%=VariantID.ClientID%>').val() != "-1") {
                $('#imgLoadAddParam').hide();
                loadParameters();
            }
            else
            {
                $('#imgLoadParams').hide();
                $('#<%=lblShowAddParams.ClientID%>').hide();
                $('#imgShowAddParams').hide();
                $('#addParams').fadeOut();
                $('#imgLoadAddParam').hide();
            }

            $('#imgLoadAddParam').hide();

        });
    </script>

    <asp:HiddenField runat="server" ID="ProcessID" />
    <asp:HiddenField runat="server" ID="ProcessRev" />
    <asp:HiddenField runat="server" ID="VariantID" />

    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx"><asp:literal runat="server" Text="<%$Resources:lblNavProcessMan %>" /></a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>"><asp:literal runat="server" Text="<%$Resources:lblNavDettaglioProc %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
<asp:Label runat="server" ID="lblErr" />

<asp:Label runat="server" ID="lblLinkFatherProc" />
<asp:Label runat="server" ID="lblProcID" Visible="false" />
<table class="table table-condensed">
<tr>
    <td><asp:literal runat="server" Text="<%$Resources:lblNome %>" />:</td>
    <td><asp:label runat="server" id="lblProcName" style="font-size:20px;font-weight:bold;"/>
        <asp:TextBox runat="server" ID="inputProcName" style="font-size:20px;" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcName" ForeColor="Red" ErrorMessage="<%$Resources:lblReqField %>" />
    </td>
    <td rowspan="5">
        <asp:ImageButton runat="server" Enabled="false" ID="newRevision" ImageUrl="~/img/iconNewRevision.png" Height="100px" OnClick="newRevision_Click" ToolTip="<%$Resources:lblTTNuovoProcesso %>" Visible="false" />
        <br />
        <asp:ImageButton runat="server" Visible="false" Enabled="false" ID="newRevisionCopy" ImageUrl="~/img/iconCopyFolder.png" Height="100px" OnClick="newRevisionCopy_Click" ToolTip="Crea una nuova revisione del processo, copiando quella corrente" />
    </td>
</tr>
<tr>
    <td>
<asp:literal runat="server" Text="<%$Resources:lblDescrizione %>" />:
    </td>
    <td><asp:label runat="server" id="lblProcDesc" style="font-size:16px;" /><asp:TextBox runat="server" ID="inputProcDesc" style="font-size:14px;" TextMode="MultiLine" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcDesc" ForeColor="Red" ErrorMessage="<%$Resources:lblReqField %>" />
    </td>
</tr>
    <tr>
        <td colspan="2">
<asp:literal runat="server" Text="<%$Resources:lblRevisione1 %>" />&nbsp;<asp:Label runat="server" ID="lblRevisione" style="font-size: 16px;" />&nbsp;<asp:literal runat="server" Text="<%$Resources:lblRevisione2 %>" />&nbsp;<asp:Label runat="server" ID="lblDataRevisione" style="font-size: 16px;" />
            </td>
        </tr>
    <tr runat="server" visible="false">
<td><asp:Label runat="server" ID="lblTipoDiagramma" style="font-size: 16px;" /><asp:literal runat="server" Text="<%$Resources:lblTipoDiagramma %>" />:</td>
<td style="font-size: 16px;">
<asp:label runat="server" id="vsm" />
<asp:DropDownList runat="server" ID="inputVSM">
<asp:ListItem Value="1" Text="<%$Resources:lblVSM %>"></asp:ListItem>
<asp:ListItem Value="0" Text="<%$Resources:lblPERT %>"></asp:ListItem>
</asp:DropDownList>
</td>
        <td rowspan="3">

            
        </td>
        </tr>
    <tr>
        <td colspan="2">
<asp:imagebutton runat="server" id="imgEdit" ImageUrl="~/img/edit.png" Width="50px" onClick="imgEdit_Click" ToolTip="<%$Resources:lblTTModifica %>"/>
<asp:imagebutton runat="server" id="imgDeleteProcess" ImageUrl="~/img/iconDelete.png" Width="50px" onClick="imgDeleteProcess_Click" ToolTip="<%$Resources:lblDeleteProcess %>"/>
<asp:imagebutton runat="server" id="imgSave" ImageUrl="~/img/iconSave.jpg" Width="50px" OnClick="imgSave_Click" AlternateText="Salva le modifiche" ToolTip="<%$Resources:lblTTSalvaModifiche %>" />
<asp:imagebutton runat="server" id="imgCancel" ImageUrl="~/img/iconCancel.jpg" Width="50px" OnClick="imgCancel_Click" AlternateText="Non salvare le modifiche" ToolTip="<%$Resources:lblTTNONSalvaModifiche %>" />
  </td></tr>
</table>
<br />
    <div class="row">

        <div class="span6">
            <h3>
        <asp:Label runat="server" ID="lblTitoloVarianti"><asp:literal runat="server" ID="lblSelProdotto" Text="<%$Resources:lblSelProdotto %>" /></asp:Label>
                </h3>
    
<asp:Label runat="server" ID="lblVarianti" />
            </div>

        <div class="span6">
            <h3><asp:literal runat="server" ID="lblNuovoProdotto" Text="<%$Resources:lblNuovoProdotto %>" /></h3>
            <table class="table table-bordered table-condensed">
                    <tr>
                        <td>
    <asp:ImageButton runat="server" ID="addVariante" OnClick="addVariante_Click" ImageUrl="~/img/iconAdd.jpg" ToolTip="<%$Resources:lblTTAddProdottoBlank %>" Height="40px" /><asp:literal runat="server" ID="lblAddProdottoBlank" Text="<%$Resources:lblAddProdottoBlank %>" />
                            </td>
    </tr>
                <tr runat="server" id="trCopiaPERT">
                    <td>
            <asp:Label runat="server" ID="lblCopiaPERT">
                
    <asp:literal runat="server" ID="lblCopiaProd" Text="<%$Resources:lblCopiaProd %>" /><asp:DropDownList runat="server" ID="ddlCopiaPERT" /></asp:Label>
            <br /><asp:checkbox runat="server" ID="chkCopia" Checked="true" Enabled="false" /><asp:literal runat="server" ID="lblRiutilizzaTasks" Text="<%$Resources:lblRiutilizzaTasks %>" /><br />
            <asp:checkbox runat="server" ID="chkCopiaTC" Checked="true" /><asp:literal runat="server" ID="lblCopiaTempiCiclo" Text="<%$Resources:lblCopiaTempiCiclo %>" /><br />
            <asp:checkbox runat="server" ID="chkCopiaReparti" Checked="true" OnCheckedChanged="chkCopiaReparti_CheckedChanged" AutoPostBack="true" /><asp:literal runat="server" ID="lblCopiaReparti" Text="<%$Resources:lblCopiaReparti %>" /><br />
            <asp:checkbox runat="server" ID="chkCopiaPostazioni" Checked="true" OnCheckedChanged="chkCopiaPostazioni_CheckedChanged" AutoPostBack="true" /><asp:literal runat="server" ID="lblCopiaPostazioni" Text="<%$Resources:lblCopiaPostazioni %>" /><br />
                        <asp:CheckBox runat="server" ID="chkCopyParameters" Checked="true" /><asp:Literal runat="server" Text="<%$Resources:lblCopyParameters %>" /><br />
                        <asp:CheckBox runat="server" ID="chkCopyWorkInstructions" Checked="true" /><asp:Literal runat="server" Text="<%$Resources:lblCopyWorkInstructions %>" />
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
    <div class="row-fluid">
        <div class="span5">
    <asp:Label runat="server" ID="lblTitoloVariante" Font-Size="16" Font-Bold="true" />
    <asp:ImageButton runat="server" ID="showEditVariante" ImageUrl="~/img/edit.png" OnClick="showEditVariante_Click" Height="40px" ToolTip="<%$Resources:lblTTModificaProcessoProd %>" />
    <asp:ImageButton runat="server" ID="deleteVariante" ImageUrl="~/img/iconDelete.png" OnClick="deleteVariante_Click" Height="40" ToolTip="<%$Resources:lblTTCancellaProcessoProd %>" />
    <br />
    <asp:Label runat="server" ID="lblDescrizioneVariante" />
    <br />
            <asp:Label runat="server" ID="lblExternalIDVal" Text="<%$Resources:lblExternalID %>" />&nbsp;<asp:Label runat="server" ID="lblExternalIDProcessoVariante" />
            <br />
            <asp:Label runat="server" ID="lblMeasurementUnitVal" Text="<%$Resources:lblMeasurementUnit %>" />&nbsp;<asp:Label runat="server" ID="lblMeasurementUnit" />
            <br />
    <asp:Label runat="server" ID="lblLogVariante" />
    <br />
    <variante:edit runat="server" id="editVariante" />
</div>
        <div class="span7">
            <img id="imgShowAddParams" src="../img/iconAdd2.png" style="cursor:pointer;min-width:20px; max-width:30px;" /><asp:label ID="lblShowAddParams" runat="server" Text="<%$Resources:lblShowAddParam %>" />
            <div id="addParams" style="display:none;">
                <table>
                    <tr>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblParamName %>" /></td>
                        <td><input type="text" id="txtParamName" /></td>
                        </tr>
                    <tr>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblParamDescription %>" /></td>
                        <td><textarea id="txtParamDescription"></textarea></td>
                        </tr>
                    <tr>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblParamCategory %>" /></td>
                        <td><asp:DropDownList runat="server" ID="ddlParamCategory" AppendDataBoundItems="true" />
                            <asp:HyperLink Target="_blank" runat="server" NavigateUrl="../Products/ProductParametersCategories/Index" ToolTip="<%$Resources:lblAltParamCategoryManagement %>"><asp:image runat="server" ImageUrl="../img/iconCategory.png" style="min-width:10px; max-width:20px;" /></asp:HyperLink>
                        </td>
                        </tr>
                    <tr>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblParamIsFixed %>" /></td>
                        <td><input type="checkbox" id="txtIsFixed" /></td>
                        </tr>
                    <tr>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblParamIsRequired %>" /></td>
                        <td><input type="checkbox" id="txtIsRequired" /></td>
                        </tr>
                    <tr><td colspan="2">
                        <img id="imgAddParam" src="../img/iconSave.jpg" style="max-width:30px; min-width:20px; cursor:pointer;" />
                        <img src="../img/iconLoading.gif" style="min-width:20px; max-width:30px;" id="imgLoadAddParam" />
                        </td></tr>
                    </table>
            </div>
            <div id="divProcessParams" />
            <img src="../img/iconLoading.gif" style="min-width:20px; max-width:30px;" id="imgLoadParams" />
        </div>

    </div>
    <br />
    <div class="row-fluid"><div class="span12">
    <ul class="breadcrumb hidden-phone" runat="server" id="tblPertNavBar">
					<li>
						<a class="lead" href="<%#Request.RawUrl %>"><strong><asp:Literal runat="server" ID="lblNavCreaProc" Text="<%$Resources:lblNavCreaProc %>" /></strong></a>
						<span class="divider">/</span>
                        <asp:HyperLink runat="server" ID="lnkLinkReparto" NavigateUrl="lnkProcessoVarianteReparto.aspx">
                            <asp:Literal runat="server" ID="lblNavAssociaProcRep" Text="<%$Resources:lblNavAssociaProcRep %>" />
                            </asp:HyperLink>
						<span class="divider">/</span>
                        <a href="#"><asp:Literal runat="server" ID="lblNavAssociaProcPost" Text="<%$Resources:lblNavAssociaProcPost %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    </div></div>
    
    <br />

<pert:showPert runat="server" ID="pert" />
</asp:Content>