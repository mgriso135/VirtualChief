<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configAnticipoMinimo.ascx.cs" Inherits="KIS.Reparti.configAnticipoMinimo" %>

<h3>Anticipo minimo per i task non appartenenti al critical path</h3>
<asp:Label runat="server" ID="lbl1" />
<asp:TextBox Columns="1" runat="server" ID="ore" />H:
<asp:DropDownList runat="server" ID="minuti" />m:
<asp:DropDownList runat="server" ID="secondi" />s
<asp:ImageButton runat="server" ID="btnEdit" OnClick="btnEdit_Click" ImageUrl="/img/edit.png" Height="30px" />
<br />

<asp:RegularExpressionValidator runat="server" ControlToValidate="ore" ErrorMessage="Il campo ore accetta solo valori numerici" ForeColor="Red" ValidationExpression="^\d+$" /><br />
<asp:RangeValidator runat="server" ControlToValidate="ore" MinimumValue="0" MaximumValue="99" ErrorMessage="Errore: il numero di ore deve essere maggiore o uguale a 0" ForeColor="Red" /><br />
<asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="40px" />
<asp:ImageButton runat="server" ID="undo" OnClick="undo_Click" ImageUrl="/img/iconUndo.png" Height="40px" />
