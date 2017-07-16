<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AzioniBarcodeJS.aspx.cs" Inherits="KIS.Operatori.AzioniBarcodeJS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="~/Styles/assets/css/bootstrap.css" rel="stylesheet" />
		<link href="~/Styles/assets/css/bootstrap-responsive.css" rel="stylesheet" />
    <link rel="shortcut icon" type="image/x-icon" href="~/img/favicon.ico" />

    <title>Kaizen Indicator System</title>
    <script lang="javascript">
        var azione1, azione2, id1, id2;

        var tbox = document.getElementById("box1");
        var gettingData = false;

        function controllo1() {
            gettingData = true;
            var imgOK = document.getElementById('<%= imgOK.ClientID %>');
            var imgKO = document.getElementById('<%= imgKO.ClientID %>');
            var imgLoad = document.getElementById('<%= imgLoading.ClientID %>');
            var tblDatiUten = document.getElementById('<%= tblLogUtente.ClientID %>');
            var imgQty = document.getElementById('<%=imgChangeQty.ClientID%>');

            var tbox = document.getElementById("box1");
            var otherBox = document.getElementById("box2");
            var azione = tbox.value.substring(0, 1);

            imgLoad.style.visibility = "hidden";
            imgLoad.style.height = 2;

            if (azione == "I" || azione == "A" || azione == "W" || azione == "F" || azione == "P" || azione == 'B') {
                log.innerHTML = '<asp:literal runat="server" text="<%$Resources:lblReadBarcodeID%>"/>'; // "Ora leggi il barcode sul tuo ID personale / cartellino.";
                var task = tbox.value.substring(1, tbox.value.length);
                if (otherBox.value.length > 0 && otherBox.value.substring(0, 1) == "U") {
                    imgLoad.style.visibility = "visible";
                    imgLoad.style.height = "200px";
                    imgLoad.style.width = "200px";
                    tblDatiUten.style.visibility = "hidden";
                    pback();
                }
                else if (otherBox.value.length == 0) {
                    if (imgOK != null) {
                        imgOK.style.visibility = "hidden";
                    }
                    if (imgKO != null) {
                        imgKO.style.visibility = "hidden";
                    }
                    otherBox.focus();
                }
                else {
                    log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";

                    otherBox.value = "";
                    tbox.value = "";
                }
            }
            else if (azione == "U") {
                var usr = tbox.value.substring(1, tbox.value.length);
                log.innerHTML ='<asp:literal runat="server" text="<%$Resources:lblReadBarcodeTaskAction%>"/>';// "Ora leggi il barcode relativo al task desiderato [INIZIO | PAUSA | FINE | PROBLEMA].";
                if (otherBox.value.length > 0 && (otherBox.value.substring(0, 1) == "I" || otherBox.value.substring(0, 1) == "A" || otherBox.value.substring(0, 1) == "W" || otherBox.value.substring(0, 1) == "F" || otherBox.value.substring(0, 1) == "P" || otherBox.value.substring(0, 1) == 'B')) {
                    imgLoad.style.visibility = "visible";
                    imgLoad.style.height = "200px";
                    imgLoad.style.width = "200px";
                    tblDatiUten.style.visibility = "hidden";
                    pback();
                }
                else if (otherBox.value.length == 0) {
                    if (imgOK != null) {
                        imgOK.style.visibility = "hidden";
                    }
                    if (imgKO != null) {
                        imgKO.style.visibility = "hidden";
                    }

                    otherBox.focus();
                }
                else {
                    log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";
                    otherBox.value = "";
                    tbox.value = "";
                    
                }
            }
            else {
                id1 = -1;
                id2 = -1;
                azione1 = "";
                azione2 = "";
                tbox.value = "";
                otherBox.value = "";
                log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";
            }
        }

        function controllo2() {
            gettingData = true;
            var imgOK = document.getElementById('<%= imgOK.ClientID %>');
            var imgKO = document.getElementById('<%= imgKO.ClientID %>');
            var imgLoad = document.getElementById('<%= imgLoading.ClientID %>');
            var tblDatiUten = document.getElementById('<%= tblLogUtente.ClientID %>');

            var tbox = document.getElementById("box2");
            var otherBox = document.getElementById("box1");
            var azione = tbox.value.substring(0, 1);

            imgLoad.style.visibility = "hidden";
            imgLoad.style.height = 2;

            //log.innerHTML = azione;
            if (azione == "I" || azione == "A" || azione == "W" || azione == "F" || azione == "P" || azione == "B") {
                log.innerHTML = '<asp:literal runat="server" text="<%$Resources:lblReadBarcodeID%>"/>';//"Ora leggi il barcode sul tuo ID personale / cartellino.";
                var task = tbox.value.substring(1, tbox.value.length);
                if (otherBox.value.length > 0 && otherBox.value.substring(0, 1) == "U") {
                    imgLoad.style.visibility = "visible";
                    imgLoad.style.height = "200px";
                    imgLoad.style.width = "200px";
                    tblDatiUten.style.visibility = "hidden";
                    pback();
                }
                else if (otherBox.value.length == 0) {
                    if (imgOK != null) {
                        imgOK.style.visibility = "hidden";
                    }
                    if (imgKO != null) {
                        imgKO.style.visibility = "hidden";
                    }
                    otherBox.focus();
                }
                else {
                    log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";
                    otherBox.value = "";
                    tbox.value = "";
                    otherBox.focus();
                }
            }
            else if (azione == "U") {
                var usr = tbox.value.substring(1, tbox.value.length);
                log.innerHTML = '<asp:literal runat="server" text="<%$Resources:lblReadBarcodeTaskAction%>"/>';//"Ora leggi il barcode relativo al task desiderato [INIZIO | PAUSA | FINE | PROBLEMA].";
                if (otherBox.value.length > 0 && (otherBox.value.substring(0, 1) == "I" || otherBox.value.substring(0, 1) == "A" || otherBox.value.substring(0, 1) == "W" || otherBox.value.substring(0, 1) == "F" || otherBox.value.substring(0, 1) == "P" || otherBox.value.substring(0, 1) == "B")) {
                    imgLoad.style.visibility = "visible";
                    imgLoad.style.height = "200px";
                    imgLoad.style.width = "200px";
                    tblDatiUten.style.visibility = "hidden";
                    pback();
                }
                else if (otherBox.value.length == 0) {
                    if (imgOK != null) {
                        imgOK.style.visibility = "hidden";
                    }
                    if (imgKO != null) {
                        imgKO.style.visibility = "hidden";
                    }
                    otherBox.focus();
                }
                else {
                    log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";
                    otherBox.value = "";
                    tbox.value = "";
                    otherBox.focus();
                }
            }
            else {
                id1 = -1;
                id2 = -1;
                azione1 = "";
                azione2 = "";
                tbox.value = "";
                otherBox.value = "";
                log.innerHTML = "<span style='color: red; font-size: 20px; font-weight:bold;'><asp:literal runat="server" text="<%$Resources:lblInputDataError%>"/></span>";
            }
        }

        function pback() {
            document.getElementById('<%=action.ClientID%>').value = "ManageTasks";
            form1.submit();
        }


        // Gestione cambio quantità prodotta
        function FirePrompt() {
            if (gettingData != true) {
                var retVal = prompt('<asp:literal runat="server" text="<%$Resources:lblQtaProdotta%>"/>', "")
                if (retVal) {
                    document.getElementById('<%=action.ClientID%>').value = "ChangeQuantity";
                    var retInt = parseInt(retVal);
                    if (isNaN(retInt)) {
                        alert('<asp:literal runat="server" text="<%$Resources:lblErroreFormatoQta%>"/>');
                        var tbox = document.getElementById("box1");
                        tbox.focus();
                        return false;
                    }
                    else {
                        document.getElementById('<%=newQtyFld.ClientID%>').value = retInt.toString();
                        form1.submit();
                        return true;
                    }
                }
                else {
                    var tbox = document.getElementById("box1");
                    tbox.focus();
                    return false;
                }
            }
            else
                return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="action" />
    <div>
    <asp:TextBox runat="server" ID="box1" onchange="controllo1()" />
        <asp:TextBox runat="server" ID="box2" onchange="controllo2()" />
        <br/>
        <asp:Image runat="server" ID="imgLoading" ImageUrl="~/img/iconLoading.gif" />
        <br />
        <table>
            <tr>
            <td>
                <asp:Image runat="server" ID="imgOK" Visible="false" ImageUrl="~/img/iconComplete.png" Height="200" />
        <asp:Image runat="server" ID="imgKO" Visible="false" ImageUrl="~/img/iconWarning.png" Height="200" />
            </td>
                <td><p class="lead"><asp:label runat="server" ID="log" /></p></td>
                <td>
                    <asp:ImageButton runat="server" Visible="false" ID="imgChangeQty" OnClientClick="return FirePrompt();" ImageUrl="~/img/iconQuantity.jpg" Height="50" />
                <asp:HiddenField runat="server" ID="newQtyFld" />
                <asp:HiddenField runat="server" ID="hFldTaskID" />
                </td>
                </tr>
            </table>
        <table id="tblLogUtente" runat="server">
            <tr>
                <td style="vertical-align: top;">
                    <asp:Repeater runat="server" ID="rptTaskAvviati" OnItemDataBound="rptTaskAvviati_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHCommessa%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHCliente%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHProdotto%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHProcesso%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHMatricola%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHID%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHName%>"/></th>
                        <th><asp:literal runat="server" text="<%$Resources:lblTHPostazione%>"/></th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:Label runat="server" ID="lblCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td>
                    <td><asp:Label runat="server" ID="lblCliente" /></td>
                    <td><asp:Label runat="server" ID="lblProdotto" /></td>
                    <td><asp:Label runat="server" ID="lblProcesso" /></td>
                    <td><asp:Label runat="server" ID="lblMatricola" /></td>
                    <td><asp:HiddenField runat="server" ID="taskID" Value='<%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                        <%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "PostazioneName") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
                </td>
                <td style="vertical-align:top;">
                    <asp:Repeater runat="server" ID="rptPostazioniAttive" OnItemDataBound="rptPostazioniAttive_ItemDataBound">
    <HeaderTemplate>
        <table>
            <tr>
                <td></td>
                <td></td>
                <td><asp:Label runat="server" ID="lblUtentiInPostazione" Text="<%$Resources:lblUtentiInPostazione %>" /></td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="border: 1px dashed groove; font-size:16px; font-family: Calibri;">
            <td><asp:HiddenField runat="server" ID="id" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <%# DataBinder.Eval(Container.DataItem, "id") %></td>
            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
            <td><asp:Label runat="server" ID="lblUserLogged" /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
                </td>
            </tr>
        </table>

        <asp:Repeater runat="server" ID="rptStatusCommessa" Visible="false" OnItemDataBound="rptStatusCommessa_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-condensed">
                    <thead>
                    <tr>
                        <th><asp:Label runat="server" Text="<%$Resources:lblTHTaskID %>" /></th>
                        <th><asp:Label runat="server" Text="<%$Resources:lblTHNome %>" /></th>
                        <th><asp:Label runat="server" Text="<%$Resources:lblTHPostazione %>" /></th>
                        <th><asp:Label runat="server" Text="<%$Resources:lblTHStato %>" /></th>
                        <th style="text-align:center;"><asp:Label runat="server" Text="<%$Resources:lblTHLavoroPrevisto %>" /></th>
                        <th style="text-align:center;"><asp:Label runat="server" Text="<%$Resources:lblTHLavoroSvolto %>" /></th>
                        <th style="text-align:center;"><asp:Label runat="server" Text="<%$Resources:lblTHRitardo %>" /></th>
                        <th><asp:Label runat="server" Text="<%$Resources:lblTHOperatori %>" /></th>
                    </tr>
                        </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td>
                        <asp:HiddenField runat="server" ID="hTaskID" Value='<%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Name") %>
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "PostazioneName") %>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblStatus" />
                        <%# DataBinder.Eval(Container.DataItem, "Status") %>
                    </td>
                    <td style="text-align:center;"><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroPrevisto")).TotalHours, 1) %></td>
                    <td style="text-align:center;"><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroEffettivo")).TotalHours, 1) %></td>
                    <td style="text-align:center;">
                        <%# Math.Round((((TimeSpan)DataBinder.Eval(Container.DataItem, "Ritardo")).TotalHours), 1) %>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOperatori" />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

    </div>
    </form>
</body>
</html>
