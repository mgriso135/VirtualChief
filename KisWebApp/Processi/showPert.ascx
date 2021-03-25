<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showPert.ascx.cs" Inherits="KIS.Processi.showPert" %>
<%@ Register TagPrefix="processovariante" TagName="addTempo" Src="~/Processi/addTempoCiclo.ascx" %>

<asp:ScriptManager runat="server" ID="scriptMan1" EnablePageMethods="true" EnableScriptGlobalization="true" />
<b><asp:Literal runat="server" ID="lblTitleLnkTaskEsistente" Text="<%$Resources:lblTitleLnkTaskEsistente %>" />:&nbsp;</b>
<asp:DropDownList runat="server" ID="ddlTasks" />
<img src="../img/iconSave.jpg" style="height:40px; cursor: pointer" onclick="linkExistingTask()" />
<br /><asp:Label runat="server" ID="Label1" />
<img ID="imgAddTaskPert" src="../img/iconAdd.jpg" OnClick="addDefaultTask()" style="height:40px; cursor: pointer" Height="40" />


<script src="../Scripts/jquery.svg.js" type="text/javascript" ></script>

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
           offsetx = evt.layerX - $('#task' + taskID).attr('cx');
           offsety = evt.layerY - $('#task' + taskID).attr('cy');
       }

       function now_drag(evt, taskID) {

           if (start_drag == 1 && drag_taskID == taskID) {
               $('#task' + taskID).attr('cx', evt.layerX - offsetx);
               $('#task' + taskID).attr('cy', evt.layerY - offsety);
               $('#txt' + taskID).attr('x', evt.layerX - offsetx - 35);
               $('#txt' + taskID).attr('y', evt.layerY - offsety);
               $('#tx' + taskID).attr('x', evt.layerX - offsetx - 40);
               $('#tx' + taskID).attr('y', evt.layerY - offsety - 81);
               $('#lx' + taskID).attr('x', evt.layerX - offsetx);
               $('#lx' + taskID).attr('y', evt.layerY - offsety - 81);
               $('#tTempoCiclo' + taskID).attr('x', evt.layerX - offsetx - 80);
               $('#tTempoCiclo' + taskID).attr('y', evt.layerY - offsety - 30);
               $('#delTask' + taskID).attr('x', evt.layerX - offsetx - 45);
               $('#delTask' + taskID).attr('y', evt.layerY - offsety + 36);
               //alert($('#' + taskID).attr('cx'));
           }
       }

       function stop_dragging(evt, taskID) {
           start_drag = 0;
           var popup = window.open("updatePERT.aspx?id=" + taskID + "&act=updatepos&posx=" + $('#task' + taskID).attr('cx') + "&posy=" + $('#task' + taskID).attr('cy') + "&variante=" + varID, null, "height=5,width=5");
           popup.blur();
           window.focus();
            drawLines();
       }

       $(document).ready(function () {           
           arrTasks = new Array(1);
           arrTasks[0] = new Array(5);
           procID = getQueryStringValue("id");
           varID = getQueryStringValue("variante");
           rev = 0;

           $.ajax({
               type: "POST",
               url: "../Products/Products/loadTempiCiclo",
               //contentType: "application/json; charset=utf-8",
               data: {
                   procID: procID,
                   rev: rev,
                   varID: varID
               },
               // dataType: "json",
               success: function (response) {
                   console.log(response);
                   // console.log("Parsed: " + JSON.parse(response));
                   arrTasks = response;
                   drawTasks();
                   printCycleTimes();
                   drawDeleteIcon();
                   drawToolBoxes();
                   printTaskName();
                   drawChainImage();
               },
               error: function (response) {
                   console.log("loadTempiCiclo error " + response);
               },
               complete: function (response)
               {
                   console.log("loadTempiCiclo complete " + response);
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

           if (precSucc)
               {
           var svgNS = "http://www.w3.org/2000/svg";
               // Disegno le linee delle precedenze e scrivo i tempi di pausa
           for (var i = 0; i < precSucc.length; i++) {

               // Disegno le linee delle precedenze
               startx = $('#task' + precSucc[i][0]).attr('cx');
               starty = $('#task' + precSucc[i][0]).attr('cy');
               endx = $('#task' + precSucc[i][1]).attr('cx');
               endy = $('#task' + precSucc[i][1]).attr('cy');
               var line = document.createElementNS(svgNS, "line");
               line.setAttributeNS(null, "x1", parseInt(startx) + 40);
               line.setAttributeNS(null, "y1", starty);
               line.setAttributeNS(null, "x2", parseInt(endx) - 40);
               line.setAttributeNS(null, "y2", endy);
               line.setAttributeNS(null, "stroke", "black");
               line.setAttributeNS(null, "id", "line" + i);
               document.getElementById("pertGraph").appendChild(line);

               // Scrivo la pausa tra un task ed il successivo
               var minx = parseInt(endx) - 40;
               var maxx = parseInt(startx) + 40;
               if (maxx < minx) {
                   var swap = minx;
                   minx = maxx;
                   maxx = swap;
               }

               var miny = parseInt(endy);
               var maxy = parseInt(starty);
               if (maxy < miny) {
                   var swap = miny;
                   miny = maxy;
                   maxy = swap;
               }

               var xpos = (maxx - minx) / 2 + minx;
               var ypos = (maxy - miny) / 2 + miny;
               // alert(xpos + " " + ypos);

               var tempoP = parseFloat(precSucc[i][2]) / 3600;

               var pauseTxt = document.createElementNS(svgNS, "text");
               pauseTxt.setAttributeNS(null, "x", xpos);
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
               toolBoxImg.setAttributeNS(null, "y", (arrTasks[i][3] - 71));
               toolBoxImg.setAttributeNS(null, "width", 30);
               toolBoxImg.setAttributeNS(null, "height", 30);
               toolBoxImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "iconToolbox.jpg");
               toolBoxImg.setAttributeNS(null, "style", "cursor:pointer");
               toolBoxImg.setAttributeNS(null, "id", "tx" + arrTasks[i][0]);
               toolBoxImg.setAttributeNS(null, "onclick", "openToolbar(evt, " + arrTasks[i][0] + ", 0, " + varID + ")");
               document.getElementById("pertGraph").appendChild(toolBoxImg);
           }
       }

       function printCycleTimes() {
           clearTempiCiclo();
           var svgNS = "http://www.w3.org/2000/svg";
           for (var i = 0; i < arrTasks.length; i++) {
               // Print valore base del tempo ciclo
               var taskTempoCiclo = document.createElementNS(svgNS, "text");
               taskTempoCiclo.setAttributeNS(null, "x", (arrTasks[i][2] - 80));
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
               url: "../Products/Products/loadTempiCiclo",
               // contentType: "application/json; charset=utf-8",
               data: {
               procID: procID,
               rev: rev, 
               varID: varID
           },
               // dataType: "json",
               success: function (response) {
                   arrTasks = [];
                   arrTasks = response;
                   drawTasks();
                   printCycleTimes();
                   drawDeleteIcon();
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
               myCircle.setAttributeNS(null, "id", "task" + arrTasks[i][0]);
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
               linkImg.setAttributeNS(null, "y", (arrTasks[i][3] - 71));
               linkImg.setAttributeNS(null, "width", 30);
               linkImg.setAttributeNS(null, "height", 30);
               linkImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "../img/iconAddLink.png");
               linkImg.setAttributeNS(null, "style", "cursor:pointer");
               linkImg.setAttributeNS(null, "id", "lx" + arrTasks[i][0]);
               linkImg.setAttributeNS(null, "onclick", "openLinkPage(evt, \"" + arrTasks[i][0] + "\", 0)");
               document.getElementById("pertGraph").appendChild(linkImg);
           }
       }

       function drawDeleteIcon() {
           var svgNS = "http://www.w3.org/2000/svg";
           $('#pertGraph').svg();
           var svg = $('#pertGraph').svg("get");
           for (var i = 0; i < arrTasks.length; i++) {
               var delImg = document.createElementNS(svgNS, "image");
               delImg.setAttributeNS(null, "x", (arrTasks[i][2] - 45));
               delImg.setAttributeNS(null, "y", (parseInt(arrTasks[i][3]) + 36));
               delImg.setAttributeNS(null, "width", 20);
               delImg.setAttributeNS(null, "height", 20);
               delImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "../img/iconDelete.png");
               delImg.setAttributeNS(null, "style", "cursor:pointer");
               delImg.setAttributeNS(null, "id", "delTask" + arrTasks[i][0]);
               delImg.setAttributeNS(null, "onclick", "deleteSubProcess(evt, " + parseInt(arrTasks[i][0]) + ")");
               document.getElementById("pertGraph").appendChild(delImg);
           }
       }

       function openToolbar(evt, TaskID, TaskRev, variantID)
       {
           $('#innerPanelEditTask').fadeOut();
           $('#innerPanelEditTask').html('');
           $('#imgLoadToolBox').fadeIn();
           $('#panelEditTask').fadeIn();
           $('#mainContainer').fadeOut();
                $.ajax({
                    url: "../Products/Products/EditTaskPanel",
                    type: 'GET',
                    dataType: 'html',
                    data:{
                        TaskID: TaskID,
                        TaskRev: TaskRev,
                        VariantID: variantID
                    },
                    success: function (result) {
                        $('#innerPanelEditTask').html(result);
                        $('#imgLoadToolBox').fadeOut();
                        $('#innerPanelEditTask').fadeIn();
                },
                    error: function (result) {
                        alert("Error");
                        $('#panelEditTask').fadeOut();
                        $('#mainContainer').fadeIn();
                        $('#imgLoadToolBox').fadeOut();
                    },
                    warning: function (result) {
                        alert("Warning");
                    $('#panelEditTask').fadeOut();
                    $('#mainContainer').fadeIn();
                    $('#imgLoadToolBox').fadeOut();
                },
                
                });
       }

       function openLinkPage(evt, taskID, revTask) {
           window.open("pertManagePrecedenze2.aspx?id=" + taskID + "&revTaskID=" + revTask + "&variante=" + varID, "_blank", "location=no,menubar=no,location=no,height=600,width=800,menubar=no,status=no");
       }

       function openPausePage(evt, prec, revPrec, succ, revSucc) {
           window.open("../Commesse/wzEditPauseTasks.aspx?prec=" + prec + "&revPrec=" + revPrec + "&succ=" + succ + "&revSucc=" + revSucc + "&variante=" + varID, "_blank", "location=no,menubar=no,location=no,height=600,width=800,menubar=no,status=no");
       }

       function closeEdit() {
           window.document.getElementById("pertGraph").style.position = "";
           window.document.getElementById("pertGraph").style.zIndex = "100";
           window.document.getElementById("<%=svg1.ClientID%>").style.zIndex = "100";
           $('#panelEditTask').fadeOut();
           $('#mainContainer').fadeIn();
           $('#innerPanelEditTask').html('');
           $('#innerPanelEditTask').fadeOut();
       }


       function testAjax() {
              var procID = getQueryStringValue("id");
              var varID = getQueryStringValue("variante");
              var rev = 0;
           $.ajax({
               type: "POST",
               url: "../Products/Products/loadTempiCiclo",
               // contentType: "application/json; charset=utf-8",
               data: {
                   procID: procID,
                   rev: rev,
                   varID: varID
               },
               // dataType: "json",
               success: function(response) {
                   arrTasks = response;
               },
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
                      clearNomeTask();
                      printTaskName();
                      clearTempiCiclo();
                      printCycleTimes();
                      precedenze();
                      setTimeout(testAjax, 5000);
                  }
              });  
          }          

          function precedenze() {
              var procID = getQueryStringValue("id");
              var varID = getQueryStringValue("variante");
              var rev = 0;
              $.ajax({
                  type: "POST",
                  url: "../Products/Products/loadPrecedenze",
                  // contentType: "application/json; charset=utf-8",
                  data: {
                      procID: procID,
                      rev: rev,
                      varID: varID
                  },
                  // dataType: "json",
                  success: function(prec) {
                      if (precSucc) {
                          for (var i = 0; i < precSucc.length * 2; i++) {
                              var tc = window.document.getElementById("line" + i);
                              if (tc) {
                                  window.document.getElementById("pertGraph").removeChild(tc);
                              }
                          }
                      }
                      precSucc = [];
                      precSucc = prec;
                      drawLines();
                  },
                  error: function (response) {
                      //alert(response.d);
                  },
                  complete: function () {
                  }
              });
          }

          

          function getQueryStringValue(key) {
              return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
          }
  
          function addDefaultTask() {
              var procID = getQueryStringValue("id");
              var varID = getQueryStringValue("variante");
              var rev = 0;
              $.ajax({
                  type: "POST",
                  url: "../Products/Products/addDefaultSubProcess",
                  // contentType: "application/json; charset=utf-8",
                  data: {
                  procID: procID, 
                  rev: rev, 
                  varID: varID
              },
                 // dataType: "json",
                  success: function (ret) {
                      clearTasks();
                      clearToolBox();
                      clearDelTask();
                      clearNomeTask();
                      clearPrecedenze();
                          loadAddTaskData();
                  },
                  error: function (response) {
                      alert("Non sono riuscito ad aggiungere il task");
                  },
                  complete: function () {
                  }
              });
          }

          function clearTasks() {
              for (var i = 0; i < arrTasks.length; i++) {
                  var tc = window.document.getElementById("task" + arrTasks[i][0]);
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

          function clearDelTask() {
              for (var i = 0; i < arrTasks.length; i++) {
                  var tx = window.document.getElementById("delTask" + arrTasks[i][0]);
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
              var procID = getQueryStringValue("id");
              var varID = getQueryStringValue("variante");
              var rev = 0;
              var ddlTask = document.getElementById("<%=ddlTasks.ClientID%>");
              var taskID = ddlTask.options[ddlTask.selectedIndex].value;
              $.ajax({
                  type: "POST",
                  url: "../Products/Products/linkExistingSubProcess",
                  // contentType: "application/json; charset=utf-8",
                  data: {
                      procID: procID, 
                      rev: rev, 
                      varID: varID, 
                      taskID: taskID, 
                      taskRev: 0
              },
                  // dataType: "json",
                  success: function (ret) {
                      clearTasks();
                      clearToolBox();
                      clearDelTask();
                      clearNomeTask();
                      clearPrecedenze();
                      loadAddTaskData();
                  },
                  error: function (response) {
                      alert("Non sono riuscito ad aggiungere il task");
                  },
                  complete: function () {
                  }
              });
          }

       function deleteSubProcess(evt, taskID) {
           var procID = getQueryStringValue("id");
           var varID = getQueryStringValue("variante");
           var procRev = 0;
           var taskRev = 0;
              $.ajax({
                  type: "POST",
                  url: "../Products/Products/deleteSubProcess",
                  // contentType: "application/json; charset=utf-8",
                  data: {
                      procID: procID,
                      rev: rev,
                      varID: varID,
                      taskID: taskID,
                      taskRev: 0
                  },
                  // dataType: "json",
                  success: function (ret) {
                      if (ret == true) {
                          $('#task' + taskID).remove();
                          $('#txt' + taskID).remove();
                          $('#tx' + taskID).remove();
                          $('#lx' + taskID).remove();
                          $('#tTempoCiclo' + taskID).remove();
                          $('#delTask' + taskID).remove();
                          clearTasks();
                          clearToolBox();
                          clearDelTask();
                          clearNomeTask();
                          clearPrecedenze();
                          loadAddTaskData();
                      }
                      else {
                          alert("Prima di procedere devi cancellare i tempi ciclo associati al task, e tutti i vincoli di precedenza con altri task.");
                      }
                  },
                  error: function (response) {
                      alert("Non sono riuscito a rimuovere il task ");
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
    </div>
    <div ID="panelEditTask" style="width:100%; background: rgba(0, 0, 0, 0.5); display:none; 
width: 100%; height:1000px; z-index:100; vertical-align:middle; border: 1px solid black;">
        <table style="position: relative; top: 0px; left: 20%; background-color: white; border: 1px dashed black;">
            <tr><td style="text-align: right;">
                <img src="../img/iconLoading2.gif" id="imgLoadToolBox" style="min-width: 30px; max-width:60px;" />
                <img src="../img/iconClose.png" style="height: 20px; text-align:right;" onclick="closeEdit();"  />
                </td></tr>
            <tr>
                <td>
                    <span id="innerPanelEditTask"></span>
                </td>
            </tr>
        </table>       
        
        </div>