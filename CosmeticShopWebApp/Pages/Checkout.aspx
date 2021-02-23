<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" 
    Inherits="CosmeticShopWebApp.Pages.Checkout" 
     MasterPageFile="~/Pages/Store.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodyContent" runat="server" DataKeyNames="OrderId" >
    <div id="content">

        <div id="checkoutForm" class="checkout" runat="server">
            <h2>Оформить заказ</h2>
            Пожалуйста, введите свои данные, и мы отправим Ваш товар прямо сейчас!

        <div id="errors" data-valmsg-summary="true">
            <ul>
                <li style="display:none"></li>
            </ul>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>

            <h3>Заказчик</h3>
            <div>
                <label for="Name">Имя:</label>
                <asp:TextBox ID = "nameTextBox"  runat = "server"> </asp:TextBox>
                
            </div>
            <h3>Адрес доставки</h3>
            <div>
                <label for="Line1">Адрес 1:</label>
                 <asp:TextBox ID = "line1TextBox"  runat = "server"> </asp:TextBox>
               
            </div>
            <div>
                <label for="Line2">Адрес 2:</label>
                 <asp:TextBox ID = "line2TextBox"  runat = "server"> </asp:TextBox>
    
            </div>
            <div>
                <label for="Line3">Адрес 3:</label>
                 <asp:TextBox ID = "line3TextBox"  runat = "server"> </asp:TextBox>
               
            </div>
            <div>
                <label for="City">Город:</label>
                 <asp:TextBox ID = "cityTextBox"  runat = "server"> </asp:TextBox>
            </div>

            <h3>Детали заказа</h3>
            <asp:CheckBox id="checkbox1" runat="server"/>                   
             Использовать подарочную упаковку?
        
        <p class="actionButtons">
            <button class="actionButtons" type="submit">Обработать заказ</button>
        </p>
        </div>
        <div id="checkoutMessage" runat="server">
            <h2>Спасибо!</h2>
            Спасибо что выбрали наш магазин! Мы постараемся максимально быстро отправить ваш заказ   
        </div>
    </div>
</asp:Content>
