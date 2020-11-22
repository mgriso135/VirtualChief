<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="KIS.HPDefault" %>
<%@ Register Src="~/whatsnew.ascx" TagPrefix="Homepage" TagName="Whatsnew" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
				<ul class="breadcrumb hidden-phone">
					<li>
						<a>Home</a>
						<span class="divider">/</span>
					</li>
				</ul>
<div class="page-header">
					<asp:label runat="server" ID="lblBenvenuto" meta:resourcekey="lblBenvenuto" />
    <asp:label runat="server" ID="lbl1" meta:resourcekey="lbl1"/>
				</div>

    <script>
        function loadKPIPanel() {

            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 2);
            var lastMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            lastMonth.setMonth(lastMonth.getMonth() - 1);


        $.ajax({
        
                        url: "../Analysis/GlobalKPIs/GetGlobalKPIs2",
                        type: 'GET',
                        dataType: "html",
            data: {
                startPeriod: lastMonth.getFullYear() + "-" + (lastMonth.getMonth() + 1) + "-" + lastMonth.getDate(),
                endPeriod:currentDate.getFullYear() + "-"+ (currentDate.getMonth() + 1)+ "-"+ currentDate.getDate(),
                periodType:1
                        },
                    success: function (result) {
                        $("#KPIPanel").html(result);
                    },
                    error: function (result) {
                        alert("Error " + result);
                        console.log(result);
                    },
                    warning: function (result) {
                        alert("Warning");
                        console.log(result);
                    }
            });
        }

        function loadWarningsKPIPanel() {

            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 2);
            var lastMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            lastMonth.setMonth(lastMonth.getMonth() - 1);


        $.ajax({
        
                        url: "../Analysis/GlobalKPIs/GetWarningsKPIs",
                        type: 'GET',
                        dataType: "html",
            data: {
                        },
                    success: function (result) {
                        $("#WarningKPIPanel").html(result);
                    },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
            });
        }

        function loadNonCompliancesKPIsPanels() {

            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 2);
            var lastMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            lastMonth.setMonth(lastMonth.getMonth() - 1);
        $.ajax({
        
                        url: "../Analysis/GlobalKPIs/GetNonCompliancesKPIs",
                        type: 'GET',
                        dataType: "html",
            data: {
                        },
                    success: function (result) {
                        $("#NonCompliancesKPIPanel").html(result);
                    },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
            });
        }

        function loadImprovementActionsKPIsPanels() {

            var currentDate = new Date();
            currentDate.setDate(currentDate.getDate() + 2);
            var lastMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            lastMonth.setMonth(lastMonth.getMonth() - 1);
        $.ajax({
        
                        url: "../Analysis/GlobalKPIs/GetImprovementActionsKPIs",
                        type: 'GET',
                        dataType: "html",
            data: {
                        },
                    success: function (result) {
                        $("#IAKPIPanel").html(result);
                    },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
            });
        }

        function loadTipsPanel() {

            var currentDate = new Date();
            var lastMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            lastMonth.setMonth(lastMonth.getMonth() - 1);


        $.ajax({
        
                        url: "../Home/Default/HomeRandomTips",
                        type: 'GET',
                        dataType: "html",
            data: {
                        },
                    success: function (result) {
                        $("#frmTips").html(result);
                    },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
            });
        }

        loadTipsPanel();
        loadKPIPanel();
        loadWarningsKPIPanel();
        loadNonCompliancesKPIsPanels();
        loadImprovementActionsKPIsPanels();

        </script>
    <div>
    <div id="KPIPanel" >
        </div>
        <div>
            <div id="WarningKPIPanel" /></div>
        <div id="NonCompliancesKPIPanel"></div>
        <div id="IAKPIPanel"></div>
        <div id="frmTips" />
    </div>
</asp:Content>
