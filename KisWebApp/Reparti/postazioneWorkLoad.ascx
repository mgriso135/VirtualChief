<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="postazioneWorkLoad.ascx.cs" Inherits="KIS.Produzione.postazioneWorkLoad" %>


<asp:Label runat="server" ID="lbl" />

<h3><asp:Literal runat="server" ID="lblPostazione" Text="<%$Resources:lblPostazione %>" />&nbsp;
    <asp:Label runat="server" ID="lblNomePost" />&nbsp;-&nbsp;<asp:Literal runat="server" ID="lblCaricoPerProd" Text="<%$Resources:lblCaricoPerProd %>" /></h3>
<!-- Google Charts placeholder removed -->