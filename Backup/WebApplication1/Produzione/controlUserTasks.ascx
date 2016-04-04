<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="controlUserTasks.ascx.cs" Inherits="WebApplication1.Produzione.controlUserTasks" %>

<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript" ></script>
<script src="/Scripts/jquery.eventsource.js" type="text/javascript" ></script>


    <title>Server-Sent Events</title>
    <script>
        function initialize() {
            
            if (window.EventSource == undefined) {
                document.getElementById('targetDiv').innerHTML =
        "Your browser doesn't support Server Side Events.";
                return;
            }

            var source = new EventSource('http://localhost:50573/Produzione/GetDatiCadenza.aspx?usr=' + utente);
            source.onopen = function (event) {
                document.getElementById('targetDiv').innerHTML += 'Connection Opened.<br>';
            };

            source.onerror = function (event) {
                if (event.eventPhase == EventSource.CLOSED) {
                    document.getElementById('targetDiv').innerHTML += 'Connection Closed.<br>';
                }
            };

            source.onmessage = function (event) {
                document.getElementById('targetDiv').innerHTML += event.data + '<br>';
            };
        }

        initialize();
    </script>


    <div id="targetDiv" >
PROVA DI UN EVENTSOURCE
    </div>
