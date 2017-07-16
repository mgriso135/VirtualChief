<%@ Page Title="Kaizen Indicator System" Language="C#" AutoEventWireup="true" CodeBehind="avanzamentoProduzione.aspx.cs" Inherits="KIS.Produzione.avanzamentoProduzione" %>
<%@Register TagName="listAvviati" TagPrefix="articoli" Src="~/Produzione/articoliStatoI.ascx" %>
<%@ Register TagPrefix="warning" TagName="listOpen" Src="~/Produzione/listWarningAperti.ascx" %>
<%@ Register TagPrefix="postazioni" TagName="listUtentiLoggati" Src="~/Postazioni/listPostazioniOperatoriLoggati.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <title>Kaizen Indicator System</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<meta name="description" content="Kaizen Indicator System" />
    <meta name="author" content="Matteo Griso" />
    <link rel="shortcut icon" type="image/x-icon" href="~/img/favicon.ico" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery-ui-1.10.3.custom/css/redmond/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
	<script src="../Styles/jquery-ui-1.10.3.custom/js/jquery-1.9.1.js"></script>
	<script src="../Styles/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js"></script>
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

    <!-- FINE CSS GIORDANO -->
    </head>
    <body>
        <form id="Form1" runat="server">

    <asp:ScriptManager ID="scriptMan1" runat="server" />
    <ul class="breadcrumb hidden-phone">
                    <li>
						<a href="produzione.aspx">
                            <asp:Label runat="server" ID="lblNavProduzione" meta:resourcekey="lblNavProduzione" />
                            </a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="<%#Request.RawUrl %>">
                            <asp:Label runat="server" ID="lblNavAndonCompleto" meta:resourcekey="lblNavAndonCompleto" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3><asp:Label runat="server" ID="lblNavAndonCompleto1" meta:resourcekey="lblNavAndonCompleto" /></h3>
    
    <div class="row-fluid">
        <warning:listOpen runat="server" id="frmListWarningAperti" />
    </div>
    <div class="row-fluid">
        <div class="span12">
        <articoli:listAvviati runat="server" id="frmArticoliAvviati" />
            </div>
    </div>

    <div class="row-fluid">
        <div class="span9">
            <asp:UpdatePanel runat="server" ID="updStatoArticoli" UpdateMode="Conditional">
        <ContentTemplate>
            <h5><asp:Label runat="server" ID="lblTitleArticoliDaAvviare" meta:resourcekey="lblTitleArticoliDaAvviare" /></h5>
            <asp:Label runat="server" ID="lbl1" />

            
    <asp:Repeater runat="server" ID="rptElencoArticoliNP" OnItemDataBound="rptElencoArticoliNP_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-condensed">
                <thead>
                <tr runat="server" id="trHead">
                    <td></td>
                </tr>
                    </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td><asp:HyperLink runat="server" ID="lnkStatoArticolo" NavigateUrl='<%# "statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>' Target="_blank">
                            <asp:Image runat="server" ID="imgStatoArticolo" ImageUrl="~/img/iconView.png" Width="40" Height="40" ToolTip="<%$resources:lblTTStatoAvanzamento %>" />
                        </asp:HyperLink><asp:HiddenField runat="server" ID="lblIDArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <asp:HiddenField runat="server" ID="lblAnnoArticolo" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' /></td>
                
            </tr>
        </ItemTemplate>
        <FooterTemplate></tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
                
            <asp:Timer runat="server" ID="TimeCheck" OnTick="TimeCheck_Tick" Interval="300000" />
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>

        <div class="span3"><br /><br /><br /><br />
            <postazioni:listUtentiLoggati runat="server" ID="frmLstUtentiPostazioni" />

        </div>
    </div>
            </form>
        </body>
    </html>