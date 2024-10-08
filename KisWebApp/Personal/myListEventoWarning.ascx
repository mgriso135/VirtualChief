﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myListEventoWarning.ascx.cs" Inherits="KIS.Personal.myListEventoWarning" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<div class="row-fluid" runat="server" id="tblRitardi">
    <div class="span4">
       
            <asp:repeater runat="server" ID="rptReparti">
                <HeaderTemplate>
                    <table class="table table-bordered table-condensed table-hover table-striped">
                        <thead>
                        <tr>
                            <th><asp:label runat="server" id="lblReparti" meta:resourcekey="lblReparti" /></th>
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
                            <th><asp:label runat="server" id="lblCommesse" meta:resourcekey="lblCommesse" /></th>
                            <th><asp:label runat="server" id="lblCliente" meta:resourcekey="lblCliente" /></th>
                            <th><asp:label runat="server" id="lblDataInserimento" meta:resourcekey="lblDataInserimento" /></th>
                            <th><asp:label runat="server" id="lblStatus" meta:resourcekey="lblStatus" /></th>
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
                            <th><asp:label runat="server" id="lblArticolo" meta:resourcekey="lblArticolo" /></th>
                            <th><asp:label runat="server" id="lblCliente2" meta:resourcekey="lblCliente" /></th>
                            <th><asp:label runat="server" id="lblDataFineProd" meta:resourcekey="lblDataFineProd" /></th>
                            <th><asp:label runat="server" id="lblStatus2" meta:resourcekey="lblStatus" /></th>
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