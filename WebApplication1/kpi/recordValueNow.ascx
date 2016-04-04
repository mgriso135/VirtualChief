<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="recordValueNow.ascx.cs" Inherits="KIS.kpi.recordValueNow" %>


<asp:TextBox runat="server" ID="valore" />
<br />
<asp:CompareValidator ID="cv" runat="server" ControlToValidate="valore" Type="Double" Operator="DataTypeCheck" ErrorMessage="Errore: richiesto valore numerico!" ForeColor="Red" />
<br />
<asp:RequiredFieldValidator ID="valReq" runat="server" ControlToValidate="valore" ErrorMessage="Errore: campo richiesto" ForeColor="Red" />
<br />
<asp:ImageButton runat="server" src="/img/iconSave.jpg" ID="btnSaveRecord" OnClick="btnSaveRecord_Click" Height="40px" CausesValidation="true" />
<asp:ImageButton runat="server" src="/img/iconCancel.jpg" ID="btnCancelSaveRecord" OnClick="btnCancelSaveRecord_Click" Height="40px" CausesValidation="false" />
<asp:Label runat="server" ID="lblErr" />