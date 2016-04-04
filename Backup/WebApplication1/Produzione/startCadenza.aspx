<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="startCadenza.aspx.cs" Inherits="WebApplication1.Produzione.startCadenza"
MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<br />
<asp:label runat="server" ID="lbl1"/>
<progress id="progBar" style="height: 100px; width: 500px"></progress><div id="avanz" style="font-size:48px; font-weight:bold;" />
<script>
    
    //var startDate;
    function startProgressCadenza() {
        var progBar = document.getElementById("progBar");
        progBar.max = cadenza;
        progBar.value = cadenza;
        
    }

    function updateProgressBar() {
        var progresso = cadenza - 7200 - ((new Date().getTime().toLocaleString() / 1000) - startDate)
        if (progresso > 0) {
            var avanz = document.getElementById("avanz");
            avanz.innerHTML = Math.floor((progresso/cadenza) * 100) + "% - " + Math.floor(progresso/60) + " minuti";
            progBar.value = progresso;
        }
        else {
            alert("STOOOOP!");
            clearInterval(int);
        }
        
    }

    startProgressCadenza();
    var int = setInterval("updateProgressBar()", 1000);
</script>


</asp:Content>