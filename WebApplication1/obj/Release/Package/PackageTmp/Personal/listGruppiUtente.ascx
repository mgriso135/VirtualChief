<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listGruppiUtente.ascx.cs" Inherits="KIS.Personal.listGruppiUtente" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Repeater runat="server" ID="rptGruppi">
            <HeaderTemplate>
                <table class="table table-striped table-condensed table-hover">
                    <thead>
                        <tr>
                            <td><asp:Label runat="server" ID="lblNome" meta:resourcekey="lblNome" /></td>
                            <td><asp:Label runat="server" ID="lblDescrizione" meta:resourcekey="lblDescrizione" /></td>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# DataBinder.Eval(Container.DataItem, "Nome") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Descrizione") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>