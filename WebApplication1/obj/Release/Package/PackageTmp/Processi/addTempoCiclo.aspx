<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addTempoCiclo.aspx.cs" Inherits="KIS.Processi.addTempoCiclo1" %>
<%@ Register TagPrefix="TempoCiclo" TagName="add" Src="~/Processi/addTempoCiclo.ascx" %>
<%@ Register TagPrefix="TempoCiclo" TagName="list" Src="~/Processi/listTempiCiclo.ascx" %>
<%@ Register TagPrefix="Analisys" TagName="TempoTaskProdotto" Src="~/Analysis/DetailTaskProduct.ascx" %>
<%@ Register TagPrefix="Analisys" TagName="TempoTask" Src="~/Analysis/DetailAnalysisTask.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Kaizen Indicator System</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<meta name="description" content="Kaizen People &middot; Kaizen Indicator System" />
    <meta name="author" content="Matteo Griso" />
    <!-- FINE STUDIO GIORDANO -->
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/jquery-ui-1.10.3.custom/css/redmond/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
	<script src="~/Styles/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js"></script>
	<script src="~/Styles/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js"></script>
    	<script>
    	    $(function () {

    	        $("#accordion").accordion();



    	        var availableTags = [
                    "ActionScript",
                    "AppleScript",
                    "Asp",
                    "BASIC",
                    "C",
                    "C++",
                    "Clojure",
                    "COBOL",
                    "ColdFusion",
                    "Erlang",
                    "Fortran",
                    "Groovy",
                    "Haskell",
                    "Java",
                    "JavaScript",
                    "Lisp",
                    "Perl",
                    "PHP",
                    "Python",
                    "Ruby",
                    "Scala",
                    "Scheme"
    	        ];
    	        /*$("#autocomplete").autocomplete({
    	            source: availableTags
    	        });



    	        $("#button").button();
    	        $("#radioset").buttonset();



    	        $("#tabs").tabs();



    	        $("#dialog").dialog({
    	            autoOpen: false,
    	            width: 400,
    	            buttons: [
                        {
                            text: "Ok",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "Cancel",
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
    	            ]
    	        });

    	        // Link to open the dialog
    	        $("#dialog-link").click(function (event) {
    	            $("#dialog").dialog("open");
    	            event.preventDefault();
    	        });

                */

    	        $("#datepicker").datepicker({
    	            inline: true
    	        });

    	        /*
                
                                $("#slider").slider({
                                    range: true,
                                    values: [17, 67]
                                });
                
                
                
                                $("#progressbar").progressbar({
                                    value: 20
                                });
                
                
                                // Hover states on the static widgets
                                $("#dialog-link, #icons li").hover(
                                    function () {
                                        $(this).addClass("ui-state-hover");
                                    },
                                    function () {
                                        $(this).removeClass("ui-state-hover");
                                    }
                                );*/
    	    });
	</script>
   
    		<!-- CSS GIORDANO -->
		<link href="~/Styles/assets/css/bootstrap.css" rel="stylesheet" />
		<link href="~/Styles/assets/css/bootstrap-responsive.css" rel="stylesheet" />
		<!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
		<!--[if lt IE 9]>
		      <script src="~/Styles/assets/js/html5shiv.js"></script>
		    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="scriptMan1" />
    <div>
        <asp:Label runat="server" ID="lblTitle" />
    <TempoCiclo:add runat="server" ID="frmAddTempoCiclo" />
        <br />
        <TempoCiclo:list runat="server" id="frmListTempiCiclo" />
        <br />
        <Analisys:TempoTaskProdotto runat="server" ID="frmStoricoTempiProdotto" />
        <br />
        <Analisys:TempoTask runat="server" ID="frmStoricoTempi" />
    </div>
        
    </form>
    <!-- Le javascript
		    ================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
<!--		<script src="../Styles/assets/js/jquery.js"></script>-->
		<script src="../Styles/assets/js/bootstrap-transition.js"></script>
		<script src="../Styles/assets/js/bootstrap-alert.js"></script>
		<script src="../Styles/assets/js/bootstrap-modal.js"></script>
		<script src="../Styles/assets/js/bootstrap-dropdown.js"></script>
		<script src="../Styles/assets/js/bootstrap-scrollspy.js"></script>
		<script src="../Styles/assets/js/bootstrap-tab.js"></script>
		<script src="../Styles/assets/js/bootstrap-tooltip.js"></script>
		<script src="../Styles/assets/js/bootstrap-popover.js"></script>
		<script src="../Styles/assets/js/bootstrap-button.js"></script>
		<script src="../Styles/assets/js/bootstrap-collapse.js"></script>
		<script src="../Styles/assets/js/bootstrap-carousel.js"></script>
		<script src="../Styles/assets/js/bootstrap-typeahead.js"></script>
</body>
</html>
