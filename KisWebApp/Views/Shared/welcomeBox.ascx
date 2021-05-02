<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="welcomeBox.ascx.cs" Inherits="KIS.Login.welcomeBoxMVC" %>

<section>
    <asp:Label runat="server" ID="lblInfoLogin" />
    <div class="dropdown">
  <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
    <span class="material-icons" style="cursor: pointer;">settings</span>
  </button>
  <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
    <li><a class="dropdown-item" href="#">Action</a></li>
    <li><a class="dropdown-item" href="#">Another action</a></li>
    <li><a class="dropdown-item" href="#">Something else here</a></li>
  </ul>
</div>

</section>