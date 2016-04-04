<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managePostazioni.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.Reparti.managePostazioni" %>
<%@ Register TagPrefix="postazione" TagName="add" Src="/Postazioni/addPostazione.ascx" %>
<%@ Register TagPrefix="postazione" TagName="workload" Src="processoWorkLoad.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone" runat="server" id="tblPertNavBar">
					<li>
                        <asp:HyperLink runat="server" ID="lnkManageProcesso" NavigateUrl="/Processi/showProcesso.aspx">
						1. Crea il processo produttivo</asp:HyperLink>
						<span class="divider">/</span>
                        <asp:hyperlink runat="server" ID="lnkProcReparto" NavigateUrl="/Processi/lnkProcessoVarianteReparto.aspx?id=">
                            2. Associa il processo produttivo al reparto
                            </asp:hyperlink>
						<span class="divider">/</span>
                        <a href="#" class="lead"><strong>3. Associa i task alle postazioni di esecuzione</strong></a>
                        <span class="divider">/</span>
            </li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />

    <asp:ImageButton runat="server" ID="imgShowAddPostazioni" OnClick="imgShowAddPostazioni_Click" ImageUrl="/img/iconAdd2.png" Height="60px" ToolTip="Aggiungi una nuova postazione di lavoro" />
    <a href="/Postazioni/managePostazioniLavoro.aspx">
    <asp:Image ID="Image1" ImageUrl="/img/iconManage.png" Height="60px" ToolTip="Gestisci le postazioni di lavoro" runat="server" /></a>
    <br />
    <postazione:add runat="server" ID="addPostazioni" />
    <br />

    <h3><asp:Label runat="server" ID="lblTitle" /></h3>

    <asp:Repeater runat="server" ID="rptTasksPostazioni" OnItemDataBound="rptTasksPostazioni_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
        <thead>
        <tr>
            <td>Task</td>
            <td>Postazione</td>
        </tr>
            </thead>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td>
                    <asp:HiddenField runat="server" ID="taskID" Value='<%#DataBinder.Eval(Container.DataItem, "processID") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "processName") %>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPostazioni" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPostazioni_SelectedIndexChanged" AutoPostBack="true" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <postazione:workload runat="server" ID="caricodilavoro" />
    </asp:Content>