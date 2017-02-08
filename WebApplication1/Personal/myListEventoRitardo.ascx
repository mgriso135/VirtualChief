<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myListEventoRitardo.ascx.cs" Inherits="KIS.Personal.myListEventoRitardo" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<div class="row-fluid">
    <div class="span4">
            <asp:repeater runat="server" ID="rptReparti">
                <HeaderTemplate>
                    <table class="table table-bordered table-condensed table-hover table-striped">
                        <thead>
                        <tr>
                            <td><asp:label runat="server" id="lblReparti" meta:resourcekey="lblReparti" /></td>
                        </tr>
                            </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#DataBinder.Eval(Container.DataItem, "name") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:repeater>
        </div>
        <div class="span4">
            <asp:repeater runat="server" ID="rptCommesse">
                <HeaderTemplate>
                    <table class="table table-bordered table-condensed table-hover table-striped">
                        <thead>
                        <tr>
                            <td><asp:label runat="server" id="lblCommesse" meta:resourcekey="lblCommesse" /></td>
                            <td><asp:label runat="server" id="lblCliente" meta:resourcekey="lblCliente" /></td>
                            <td><asp:label runat="server" id="lblDataInserimento" meta:resourcekey="lblDataInserimento" /></td>
                            <td><asp:label runat="server" id="lblStato" meta:resourcekey="lblStato" /></td>
                        </tr>
                            </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
                        <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataInserimento")).ToString("dd/MM/yyyy") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:repeater>
            </div>
    <div class="span4">
            <asp:repeater runat="server" ID="rptArticolo">
                <HeaderTemplate>
                    <table class="table table-bordered table-condensed table-hover table-striped">
                        <thead>
                        <tr>
                            <td><asp:label runat="server" id="lblArticolo" meta:resourcekey="lblArticolo" /></td>
                            <td><asp:label runat="server" id="lblCliente" meta:resourcekey="lblCliente" /></td>
                            <td><asp:label runat="server" id="Label1" meta:resourcekey="lblDataFineProduzione" /></td>
                            <td><asp:label runat="server" id="lblStato" meta:resourcekey="lblStato" /></td>
                        </tr>
                            </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
                        <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaFineProduzione")).ToString("dd/MM/yyyy") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:repeater>
        </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>