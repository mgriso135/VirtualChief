<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzEditPERT.ascx.cs" Inherits="KIS.Commesse.wzEditPERT1" %>
<%@ Register TagPrefix="processovariante" TagName="addTempo" Src="~/Processi/addTempoCiclo.ascx" %>

<asp:ScriptManager runat="server" ID="scriptMan1" EnablePageMethods="true" EnableScriptGlobalization="true" />

<table class="table table-hover table-condensed table-striped" runat="server" id="tblAddTask">
    <tr>
        <td>
<b><asp:Label runat="server" ID="lblAddNewTask" Text="<%$Resources:lblAddNewTask %>" /></b>&nbsp;
            <img ID="imgAddTaskPert" src="/img/iconAdd.jpg" OnClick="addDefaultTask()" style="height:40px; cursor: pointer" Height="40" />
            </td>
            </tr>
    <tr>
        <td>
<b><asp:Label runat="server" ID="lblLinkTask" Text="<%$Resources:lblLinkTask %>" /></b>&nbsp;
<asp:DropDownList runat="server" ID="ddlTasks" />
<img src="/img/iconSave.jpg" style="height:40px; cursor: pointer" onclick="linkExistingTask()" />
            </td>
        </tr>
</table>

<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript" ></script>
<script src="/Scripts/jquery.svg.js" type="text/javascript" ></script>
<script src="/Scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript" ></script>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Label runat="server" ID="lbl1" />
        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="10000" />
        </ContentTemplate>
    </asp:UpdatePanel>

<div id="mainContainer" style="position:relative; height:1000px">

  <asp:label runat="server" ID="svg1">

  
   <script lang="javascript">
       var start_drag = null;
       var drag_taskID = null;
       var offsetx = null;
       var offsety = null;

       var arrTasks;
       var precSucc = new Array();
       var varID, procID, rev;

       function start_dragging(evt, taskID) {
           start_drag = 1;
           drag_taskID = taskID;
           offsetx = evt.layerX - $('#' + taskID).attr('cx');
           offsety = evt.layerY - $('#' + taskID).attr('cy');
           //txtoffsetx = evt.layerX - $('#txt' + taskID).attr('x');
           //txtoffsety = evt.layerY - $('#txt' + taskID).attr('dy');
       }

       function now_drag(evt, taskID) {

           if (start_drag == 1 && drag_taskID == taskID) {
               $('#' + taskID).attr('cx', evt.layerX - offsetx);
               $('#' + taskID).attr('cy', evt.layerY - offsety);
               $('#txt' + taskID).attr('x', evt.layerX - offsetx - 35);
               $('#txt' + taskID).attr('y', evt.layerY - offsety);
               $('#tx' + taskID).attr('x', evt.layerX - offsetx - 40);
               $('#tx' + taskID).attr('y', evt.layerY - offsety - 81);
               $('#lx' + taskID).attr('x', evt.layerX - offsetx);
               $('#lx' + taskID).attr('y', evt.layerY - offsety - 81);
               $('#tTempoCiclo' + taskID).attr('x', evt.layerX - offsetx - 80);
               $('#tTempoCiclo' + taskID).attr('y', evt.layerY - offsety - 30);
               //alert($('#' + taskID).attr('cx'));
           }
       }

       function stop_dragging(evt, taskID) {
           start_drag = 0;
           var popup = window.open("/Processi/updatePERT.aspx?id=" + taskID + "&act=updatepos&posx=" + $('#' + taskID).attr('cx') + "&posy=" + $('#' + taskID).attr('cy') + "&variante=" + varID, null, "height=5,width=5");
           popup.blur();
           window.focus();
           drawLines();
       }

       $(document).ready(function () {
           arrTasks = new Array(1);
           arrTasks[0] = new Array(5);
           procID = getQueryStringValue("idProc");
           varID = getQueryStringValue("idVariante");
           rev = getQueryStringValue("revProc");

           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/loadTempiCiclo",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + "}",
               dataType: "json",
               success: function (response) {

                   arrTasks = response.d;
                   drawTasks();
                   printCycleTimes();
                   drawToolBoxes();
                   printTaskName();
                   drawChainImage();
               },
               error: function (response) {
                   alert("Can't read data " + response.responseText);
               },
               complete: function () {
               }
           });

           testAjax();
       });

       function drawLines() {

           if (precSucc) {
               for (var i = 0; i < precSucc.length; i++) {
                   var old = window.document.getElementById("line" + i);
                   var pause = window.document.getElementById("pause" + i);
                   if (old) {
                       window.document.getElementById("pertGraph").removeChild(old);
                   }
                   if (pause) {
                       window.document.getElementById("pertGraph").removeChild(pause);
                   }
               }
           }

           if (precSucc) {
               var svgNS = "http://www.w3.org/2000/svg";
               // Disegno le linee delle precedenze e scrivo i tempi di pausa
               for (var i = 0; i < precSucc.length; i++) {

                   // Disegno le linee delle precedenze
                   startx = $('#' + precSucc[i][0]).attr('cx');
                   starty = $('#' + precSucc[i][0]).attr('cy');
                   endx = $('#' + precSucc[i][1]).attr('cx');
                   endy = $('#' + precSucc[i][1]).attr('cy');
                   var line = document.createElementNS(svgNS, "line");
                   line.setAttributeNS(null, "x1", parseInt(startx) + 40);
                   line.setAttributeNS(null, "y1", starty);
                   line.setAttributeNS(null, "x2", parseInt(endx) - 40);
                   line.setAttributeNS(null, "y2", endy);
                   line.setAttributeNS(null, "stroke", "black");
                   line.setAttributeNS(null, "id", "line" + i);
                   document.getElementById("pertGraph").appendChild(line);

                   // Scrivo la pausa tra un task ed il successivo
                   var minx = parseInt(endx)-40;
                   var maxx = parseInt(startx)+40;
                   if (maxx < minx)
                   {
                       var swap = minx;
                       minx = maxx;
                       maxx = swap;
                   }

                   var miny = parseInt(endy);
                   var maxy = parseInt(starty);
                   if (maxy < miny)
                   {
                       var swap = miny;
                       miny = maxy;
                       maxy = swap;
                   }

                   var xpos = (maxx - minx) / 2 + minx;
                   var ypos = (maxy - miny) / 2 + miny;
                   // alert(xpos + " " + ypos);

                   var tempoP = parseFloat(precSucc[i][2])/3600;

                   var pauseTxt = document.createElementNS(svgNS, "text");
                   pauseTxt.setAttributeNS(null, "x",xpos);
                   pauseTxt.setAttributeNS(null, "y", ypos - 10);
                   pauseTxt.setAttributeNS(null, "fill", "black");
                   pauseTxt.setAttributeNS(null, "style", "cursor: pointer");
                   pauseTxt.setAttributeNS(null, "id", "pause" + i);
                   pauseTxt.appendChild(document.createTextNode(tempoP.toFixed(1) + "h"));
                   pauseTxt.setAttributeNS(null, "onclick", "openPausePage(evt, \""
                       + precSucc[i][0] + "\", \"" + precSucc[i][3] + "\", \""
                       + precSucc[i][1] + "\", \"" + precSucc[i][4]
                       + "\", \"" + precSucc[i][2] + "\")");
                   document.getElementById("pertGraph").appendChild(pauseTxt);
               }
           }
       }

       function drawToolBoxes() {
           var svgNS = "http://www.w3.org/2000/svg";

           for (var i = 0; i < arrTasks.length; i++) {
               var toolBoxImg = document.createElementNS(svgNS, "image");
               toolBoxImg.setAttributeNS(null, "x", (arrTasks[i][2] - 40));
               toolBoxImg.setAttributeNS(null, "y", (arrTasks[i][3] - 81));
               toolBoxImg.setAttributeNS(null, "width", 40);
               toolBoxImg.setAttributeNS(null, "height", 40);
               toolBoxImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "/img/iconToolbox.jpg");
               toolBoxImg.setAttributeNS(null, "style", "cursor:pointer");
               toolBoxImg.setAttributeNS(null, "id", "tx" + arrTasks[i][0]);
               toolBoxImg.setAttributeNS(null, "onclick", "openToolbar(evt, \"" + arrTasks[i][0] + "\")");
               document.getElementById("pertGraph").appendChild(toolBoxImg);
           }
       }

       function printCycleTimes() {
           clearTempiCiclo();
           var svgNS = "http://www.w3.org/2000/svg";
           for (var i = 0; i < arrTasks.length; i++) {
               // Print valore base del tempo ciclo
               var taskTempoCiclo = document.createElementNS(svgNS, "text");
               taskTempoCiclo.setAttributeNS(null, "x", (arrTasks[i][2] - 90));
               taskTempoCiclo.setAttributeNS(null, "y", arrTasks[i][3] - 30);
               taskTempoCiclo.setAttributeNS(null, "fill", "black");
               taskTempoCiclo.setAttributeNS(null, "id", "tTempoCiclo" + arrTasks[i][0]);
               taskTempoCiclo.appendChild(document.createTextNode(arrTasks[i][4]));
               document.getElementById("pertGraph").appendChild(taskTempoCiclo);
           }
       }

       function loadAddTaskData() {
           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/loadTempiCiclo",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + "}",
               dataType: "json",
               success: function (response) {
                   arrTasks = [];
                   arrTasks = response.d;
                   drawTasks();
                   printCycleTimes();
                   drawToolBoxes();
                   printTaskName();
                   drawChainImage();
               },
               error: function (response) {
               },
               complete: function () {
               }
           });
       }

       function drawTasks() {
           var svgNS = "http://www.w3.org/2000/svg";
           $('#pertGraph').svg();
           var svg = $('#pertGraph').svg("get");
           for (var i = 0; i < arrTasks.length; i++) {
               // draw task circle
               var myCircle = document.createElementNS(svgNS, "circle");
               myCircle.setAttributeNS(null, "id", arrTasks[i][0]);
               myCircle.setAttributeNS(null, "cx", arrTasks[i][2]);
               myCircle.setAttributeNS(null, "cy", arrTasks[i][3]);
               myCircle.setAttributeNS(null, "r", 40);
               myCircle.setAttributeNS(null, "fill", "white");
               myCircle.setAttributeNS(null, "stroke", "black");
               myCircle.setAttributeNS(null, "style", "cursor: move");
               myCircle.setAttributeNS(null, "onmousedown", "start_dragging(evt, \"" + arrTasks[i][0] + "\")");
               myCircle.setAttributeNS(null, "onmouseup", "stop_dragging(evt, \"" + arrTasks[i][0] + "\")");
               myCircle.setAttributeNS(null, "onmousemove", "now_drag(evt, \"" + arrTasks[i][0] + "\")");
               document.getElementById("pertGraph").appendChild(myCircle);
           }
       }

       function printTaskName() {
           var svgNS = "http://www.w3.org/2000/svg";
           $('#pertGraph').svg();
           var svg = $('#pertGraph').svg("get");
           for (var i = 0; i < arrTasks.length; i++) {
               var taskTxt = document.createElementNS(svgNS, "text");
               taskTxt.setAttributeNS(null, "x", (arrTasks[i][2] - 35));
               taskTxt.setAttributeNS(null, "y", arrTasks[i][3]);
               taskTxt.setAttributeNS(null, "fill", "black");
               taskTxt.setAttributeNS(null, "style", "cursor: move");
               taskTxt.setAttributeNS(null, "id", "txt" + arrTasks[i][0]);
               taskTxt.appendChild(document.createTextNode(arrTasks[i][1]));
               taskTxt.setAttributeNS(null, "onmousedown", "start_dragging(evt, \"" + arrTasks[i][0] + "\")");
               taskTxt.setAttributeNS(null, "onmouseup", "stop_dragging(evt, \"" + arrTasks[i][0] + "\")");
               taskTxt.setAttributeNS(null, "onmousemove", "now_drag(evt, \"" + arrTasks[i][0] + "\")");
               document.getElementById("pertGraph").appendChild(taskTxt);
           }
       }

       function drawChainImage() {
           var svgNS = "http://www.w3.org/2000/svg";
           $('#pertGraph').svg();
           var svg = $('#pertGraph').svg("get");
           for (var i = 0; i < arrTasks.length; i++) {
               var linkImg = document.createElementNS(svgNS, "image");
               linkImg.setAttributeNS(null, "x", (arrTasks[i][2]));
               linkImg.setAttributeNS(null, "y", (arrTasks[i][3] - 81));
               linkImg.setAttributeNS(null, "width", 40);
               linkImg.setAttributeNS(null, "height", 40);
               linkImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "/img/iconAddLink.png");
               linkImg.setAttributeNS(null, "style", "cursor:pointer");
               linkImg.setAttributeNS(null, "id", "lx" + arrTasks[i][0]);
               linkImg.setAttributeNS(null, "onclick", "openLinkPage(evt, \"" + arrTasks[i][0] + "\", 0)");
               document.getElementById("pertGraph").appendChild(linkImg);
           }
       }

       function openToolbar(evt, taskID) {
           //window.open("showProcesso.aspx?id=" + taskID + "&variante=" + varID, "_blank", "location=no,menubar=no,location=no,height=600,width=800,menubar=no,status=no");
           __doPostBack(taskID + ";" + varID, "ShowDetails");
       }

       function openLinkPage(evt, taskID, revTask) {
           window.open("/Processi/pertManagePrecedenze2.aspx?id=" + taskID + "&revTaskID=" + revTask + "&variante=" + varID, "_blank", "location=no,menubar=no,location=no,height=600,width=800,menubar=no,status=no");
       }

       function openPausePage(evt, prec, revPrec, succ, revSucc) {
           window.open("/Commesse/wzEditPauseTasks.aspx?prec=" + prec +"&revPrec="+revPrec+"&succ=" + succ + "&revSucc="+revSucc+"&variante=" + varID, "_blank", "location=no,menubar=no,location=no,height=600,width=800,menubar=no,status=no");
       }

       function closeEdit() {
           window.document.getElementById("pertGraph").style.position = "";
           window.document.getElementById("pertGraph").style.zIndex = "100";
           window.document.getElementById("<%=svg1.ClientID%>").style.zIndex = "100";
           window.document.getElementById("<%= pnlEditTask.ClientID %>").style.visibility = "hidden";
           window.document.getElementById("<%= pnlEditTask.ClientID %>").style.zIndex = 0;
           //window.location = window.location;
       }


       function testAjax() {
           var procID = getQueryStringValue("idProc");
           var varID = getQueryStringValue("idVariante");
           var rev = getQueryStringValue("revProc");
           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/loadTempiCiclo",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + "}",
               dataType: "json",
               success: OnSuccess,
               error: function (response) {
                   //alert(response.d);
               },
               complete: function () {
                   for (var i = 0; i < arrTasks.length; i++) {
                       var tc = window.document.getElementById("tTempoCiclo" + arrTasks[i][0]);
                       if (tc) {
                           window.document.getElementById("pertGraph").removeChild(tc);
                       }
                   }
                   printCycleTimes();
                   precedenze();
                   setTimeout(testAjax, 5000);
               }
           });
       }

       function OnSuccess(response) {
           arrTasks = [];
           arrTasks = response.d;
       }

       function precedenze() {
           var procID = getQueryStringValue("idProc");
           var varID = getQueryStringValue("idVariante");
           var rev = getQueryStringValue("revProc");
           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/loadPrecedenze",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + "}",
               dataType: "json",
               success: SuccessPrecedenze,
               error: function (response) {
                   //alert(response.d);
               },
               complete: function () {
               }
           });
       }

       function SuccessPrecedenze(prec) {
           if (precSucc) {
               for (var i = 0; i < precSucc.length * 2; i++) {
                   var tc = window.document.getElementById("line" + i);
                   if (tc) {
                       window.document.getElementById("pertGraph").removeChild(tc);
                   }
               }
           }
           precSucc = [];
           precSucc = prec.d;
           drawLines();
       }

       function getQueryStringValue(key) {
           return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
       }

       function addDefaultTask() {
           var procID = getQueryStringValue("idProc");
           var varID = getQueryStringValue("idVariante");
           var rev = getQueryStringValue("revProc");
           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/addDefaultSubProcess",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + "}",
               dataType: "json",
               success: function (ret) {
                   clearTasks();
                   clearToolBox();
                   clearNomeTask();
                   clearPrecedenze();
                   loadAddTaskData();
               },
               error: function (response) {
                   alert("I couldn't add the task");
               },
               complete: function () {
               }
           });
       }

       function clearTasks() {
           for (var i = 0; i < arrTasks.length; i++) {
               var tc = window.document.getElementById(arrTasks[i][0]);
               if (tc) {
                   window.document.getElementById("pertGraph").removeChild(tc);
               }
           }
       }

       function clearToolBox() {
           for (var i = 0; i < arrTasks.length; i++) {
               var tx = window.document.getElementById("tx" + arrTasks[i][0]);
               if (tx) {
                   window.document.getElementById("pertGraph").removeChild(tx);
               }
           }
       }

       function clearNomeTask() {
           for (var i = 0; i < arrTasks.length; i++) {
               var tc = window.document.getElementById("txt" + arrTasks[i][0]);
               if (tc) {
                   window.document.getElementById("pertGraph").removeChild(tc);
               }
           }
       }

       function clearPrecedenze() {
           for (var i = 0; i < arrTasks.length; i++) {
               var tc = window.document.getElementById("lx" + arrTasks[i][0]);
               if (tc) {
                   window.document.getElementById("pertGraph").removeChild(tc);
               }
           }
       }

       function clearTempiCiclo() {
           for (var i = 0; i < arrTasks.length; i++) {
               var tc = window.document.getElementById("tTempoCiclo" + arrTasks[i][0]);
               if (tc) {
                   window.document.getElementById("pertGraph").removeChild(tc);
               }
           }
       }

       function linkExistingTask() {
           var procID = getQueryStringValue("idProc");
           var varID = getQueryStringValue("idVariante");
           var rev = getQueryStringValue("revProc");
           var ddlTask = document.getElementById("<%=ddlTasks.ClientID%>");
              var taskID = ddlTask.options[ddlTask.selectedIndex].value;
              $.ajax({
                  type: "POST",
                  url: "/Processi/getProcessData.asmx/linkExistingSubProcess",
                  contentType: "application/json; charset=utf-8",
                  data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + ", 'taskID':" + taskID + ", 'taskRev':0 }",
                  dataType: "json",
                  success: function (ret) {
                      clearTasks();
                      clearToolBox();
                      clearNomeTask();
                      clearPrecedenze();
                      loadAddTaskData();
                  },
                  error: function (response) {
                      alert("I couldn't add the task");
                  },
                  complete: function () {
                  }
              });
          }

          function deleteSubProcess() {
              var procID = getQueryStringValue("idProc");
              var varID = getQueryStringValue("idVariante");
              var procRev = getQueryStringValue("revProc");
              var htaskID = document.getElementById('<%=editTaskID.ClientID%>');
           var taskID = htaskID.value;
           var taskRev = 0;
           $.ajax({
               type: "POST",
               url: "/Processi/getProcessData.asmx/deleteSubProcess",
               contentType: "application/json; charset=utf-8",
               data: "{'procID':" + procID + ", 'rev':" + rev + ", 'varID':" + varID + ", 'taskID':" + taskID + ", 'taskRev':0 }",
               dataType: "json",
               success: function (ret) {
                   if (ret.d == true) {
                       clearTasks();
                       clearTempiCiclo();
                       clearToolBox();
                       clearNomeTask();
                       clearPrecedenze();
                       closeEdit();
                       loadAddTaskData();
                   }
                   else {
                       var lblErr = document.getElementById('<%=lblEdit1.ClientID%>');
                          lblErr.innerHTML = '<span style="color: red"><asp:literal runat="server" text="<%$Resources:lblDeleteTempiCiclo%>"/></span><br/>';
                      }
                  },
                  error: function (response) {
                      alert("I couln't remove the task");
                  },
                  complete: function () {
                  }
              });
          }
       </script>

 <svg width="100%" height="1000" xmlns="http://www.w3.org/2000/svg" version="1.1" id="pertGraph" style="border:1px solid black;">
   <rect id='BackDrop' x='-10%' y='-10%' width='110%' height='110%' fill='none' pointer-events='all' />
      </svg>
  </asp:label>

<span runat="server" ID="pnlEditTask" style="visibility:hidden;width:100%; background: rgba(0, 0, 0, 1.0)" visible="false">
    <asp:HiddenField runat="server" ID="editTaskID" />
    <asp:HiddenField runat="server" ID="editTaskVarianteID" />
    <table style="position: relative; top: 0px; left: 20%; background-color: white; border: 1px dashed black;">
        <tr><td colspan="2" style="text-align: right">
            <img src="/img/iconClose.png" style="height: 20px" onclick="closeEdit();" />
            </td></tr>
        <tr>
            <td><asp:label runat="server" id="lblNome" Text="<%$Resources:lblNome %>"/>:</td>
            <td>
    <asp:TextBox runat="server" ID="editTaskNome" />
                </td>
            </tr>
        <tr>
            <td><asp:label runat="server" id="lblDescrizione" Text="<%$Resources:lblDescrizione %>"/>:</td>
            <td>
    <asp:TextBox runat="server" TextMode="MultiLine" ID="editTaskDesc" />
        </td>
        </tr>
        <td colspan="2">
    <asp:ImageButton runat="server" ID="editTaskSave" ImageUrl="/img/iconSave.jpg" Height="40" OnClick="editTaskSave_Click" />
    <asp:ImageButton runat="server" ID="editTaskUndo" ImageUrl="/img/iconUndo.png" Height="40" OnClick="editTaskUndo_Click" />
            </td>
        <tr>
            <td colspan="2" style="border:1px dashed blue;">
                <processovariante:addTempo runat="server" ID="frmAddTempoCiclo" />
                <asp:UpdatePanel runat="server" ID="updTempi">
                    <ContentTemplate>
                <asp:Repeater runat="server" ID="rptTempi" OnItemCommand="rptTempi_ItemCommand" OnItemDataBound="rptTempi_ItemDataBound">
    <HeaderTemplate>
        <table style="border: 1px dashed blue">
            <thead>
            <tr>
                <th></th>
                <th><asp:label runat="server" id="lblNumOps" Text="<%$Resources:lblNumOps %>"/></th>
                <th><asp:label runat="server" id="lblSetup" Text="<%$Resources:lblSetup %>"/></th>
                <th><asp:label runat="server" id="lblTempoCiclo" Text="<%$Resources:lblTempoCiclo %>"/></th>
                <th><asp:label runat="server" id="lblDefault" Text="<%$Resources:lblDefault %>"/></th>
            </tr>
                </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hNumOp" Value='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' />
                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="/img/iconDelete.png" Height="30px" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="Cancella il tempo ciclo" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %></td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Seconds%>
            </td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Seconds%></td>
            <td><asp:Image runat="server" ID="imgIsDefault" ImageUrl="~/img/iconComplete.png" Width="30" />
                <asp:ImageButton ImageUrl="~/img/iconChoose.png" Width="30" runat="server" ID="imgMakeDefault" CommandName="MakeDefault" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="Rendi il tempo di default"/></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel runat="server" ID="updDeleteTask">
                    <ContentTemplate>
                        <img src="/img/iconDelete.png" style="height: 30px; cursor:pointer;" onclick="deleteSubProcess()" /><asp:label runat="server" id="lblDeleteProcess" Text="<%$Resources:lblDeleteProcess %>"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
        </tr>
        <tr><td colspan="2"><asp:Label runat="server" ID="lblEdit1" /></td></tr>
        </table>
</span>
    </div>