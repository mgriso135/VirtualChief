<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editTask.aspx.cs" Inherits="KIS.Processi.editTask" %>
<%@ Register TagPrefix="Task" TagName="edit" Src="~/Processi/editTask.ascx" %>

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
		<!-- Fav and touch icons -->
		<link rel="apple-touch-icon-precomposed" sizes="144x144" href="~/Styles/assets/ico/apple-touch-icon-144-precomposed.png" />
		<link rel="apple-touch-icon-precomposed" sizes="114x114" href="~/Styles/assets/ico/apple-touch-icon-114-precomposed.png" />
		<link rel="apple-touch-icon-precomposed" sizes="72x72" href="~/Styles/assets/ico/apple-touch-icon-72-precomposed.png" />
		<link rel="apple-touch-icon-precomposed" href="~/Styles/assets/ico/apple-touch-icon-57-precomposed.png" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lblTitle" />
    <Task:edit runat="server" id="frmEditTask" />
    </div>
    </form>
</body>
</html>
