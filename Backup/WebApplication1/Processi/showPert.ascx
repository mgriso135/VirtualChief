<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showPert.ascx.cs" Inherits="WebApplication1.Processi.showPert" %>

<asp:ImageButton runat="server" ID="imgAddTaskPert" ImageUrl="/img/iconAdd.jpg" OnClick="addTaskPert" Height="40" />

<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript" ></script>
<script src="/Scripts/jquery.svg.js" type="text/javascript" ></script>
<script src="/Scripts/jquery-ui-1.8.23.custom.min.js" type="text/javascript" ></script>

<asp:Label runat="server" ID="lbl1" />



  <asp:label runat="server" ID="svg1">

  

   <script language="javascript">
   
       var start_drag = null;
       var offsetx = null;
       var offsety = null;

       function start_dragging(evt, taskID) {
           start_drag = 1;
           offsetx = evt.layerX - $('#' + taskID).attr('cx');
           offsety = evt.layerY - $('#' + taskID).attr('cy');
           //txtoffsetx = evt.layerX - $('#txt' + taskID).attr('x');
           //txtoffsety = evt.layerY - $('#txt' + taskID).attr('dy');
       }

       function now_drag(evt, taskID) {

           if (start_drag == 1) {
              // alert("ENTRO!!!");
               $('#' + taskID).attr('cx', evt.layerX - offsetx);
               $('#' + taskID).attr('cy', evt.layerY - offsety);
               $('#txt' + taskID).attr('x', evt.layerX - offsetx - 35);
               $('#txt' + taskID).attr('y', evt.layerY - offsety);
               $('#tx' + taskID).attr('x', evt.layerX - offsetx - 20);
               $('#tx' + taskID).attr('y', evt.layerY - offsety - 81);
               //alert($('#' + taskID).attr('cx'));
           }
       }

       function stop_dragging(evt, taskID) {
           start_drag = 0;
           window.open("updatePERT.aspx?id=" + taskID + "&act=updatepos&posx=" + $('#' + taskID).attr('cx') + "&posy=" + $('#' + taskID).attr('cy'));
           for (var i = 0; i < numLinee; i++) {
               $('#line' + i).attr('stroke', 'none');
               $('#line' + i).attr('id', '');
           }
           drawLines();
       }

       $(document).ready(function () {
           var svgNS = "http://www.w3.org/2000/svg";
           $('#pertGraph').svg();
           var svg = $('#pertGraph').svg("get");
           for (var i = 0; i < numTasks; i++) {
               // draw task circle
               var myCircle = document.createElementNS(svgNS, "circle");
               myCircle.setAttributeNS(null, "id", arrTasks[i][0]);
               myCircle.setAttributeNS(null, "cx", arrTasks[i][2]);
               myCircle.setAttributeNS(null, "cy", arrTasks[i][3]);
               myCircle.setAttributeNS(null, "r", 40);
               myCircle.setAttributeNS(null, "fill", "white");
               myCircle.setAttributeNS(null, "stroke", "black");
               myCircle.setAttributeNS(null, "onmousedown", "start_dragging(evt, \"" + arrTasks[i][0] + "\")");
               myCircle.setAttributeNS(null, "onmouseup", "stop_dragging(evt, \"" + arrTasks[i][0] + "\")");
               myCircle.setAttributeNS(null, "onmousemove", "now_drag(evt, \"" + arrTasks[i][0] + "\")");
               document.getElementById("pertGraph").appendChild(myCircle);
               // Print task name
               var taskTxt = document.createElementNS(svgNS, "text");
               taskTxt.setAttributeNS(null, "x", (arrTasks[i][2] - 35));
               taskTxt.setAttributeNS(null, "y", arrTasks[i][3]);
               taskTxt.setAttributeNS(null, "fill", "black");
               taskTxt.setAttributeNS(null, "id", "txt" + arrTasks[i][0]);
               taskTxt.appendChild(document.createTextNode(arrTasks[i][1]));
               document.getElementById("pertGraph").appendChild(taskTxt);
               // adds toolbox icon
               var toolBoxImg = document.createElementNS(svgNS, "image");
               toolBoxImg.setAttributeNS(null, "x", (arrTasks[i][2] - 20));
               toolBoxImg.setAttributeNS(null, "y", (arrTasks[i][3] - 81));
               toolBoxImg.setAttributeNS(null, "width", 40);
               toolBoxImg.setAttributeNS(null, "height", 40);
               toolBoxImg.setAttributeNS("http://www.w3.org/1999/xlink", "href", "iconToolbox.jpg");
               toolBoxImg.setAttributeNS(null, "style", "cursor:pointer");
               toolBoxImg.setAttributeNS(null, "id", "tx" + arrTasks[i][0]);
               toolBoxImg.setAttributeNS(null, "onclick", "openToolbar(evt, \"" + arrTasks[i][0] + "\")");
               document.getElementById("pertGraph").appendChild(toolBoxImg);
           }


           drawLines();

       });

       function drawLines() {
           var svgNS = "http://www.w3.org/2000/svg";
           // Disegno le linee delle precedenze
           for (var i = 0; i < numLinee; i++) {
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
           }
       }

       function openToolbar(evt, taskID) {
           window.open("showProcesso.aspx?id=" + taskID);
       }

  </script>
  <svg width="100%" height="1000" xmlns="http://www.w3.org/2000/svg" version="1.1" id="pertGraph">
   <rect id='BackDrop' x='-10%' y='-10%' width='110%' height='110%' fill='none' pointer-events='all' />


  
  </asp:label>
 