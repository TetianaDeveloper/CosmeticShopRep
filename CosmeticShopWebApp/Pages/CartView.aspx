<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CartView.aspx.cs" Inherits="CosmeticShopWebApp.Pages.CartView" 
    MasterPageFile="~/Pages/Store.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodyContent" runat="server">
    <div id="content">
        <h2>Ваша корзина</h2>
        <table id="cartTable">
            <thead>
                <tr>
                    <th>Кол-во товаров</th>
                    <th>Название</th>
                    <th>Цена</th>
                    <th>Общая стоимость</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Repeater1" ItemType="CosmeticShopWebApp.Models.CartLine"
                    SelectMethod="GetCartLines" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td><%# Item.Quantity %></td>
                            <td><%# Item.Product.Name %></td>
                            <td><%# Item.Product.Price.ToString("c")%></td>
                            <td><%# ((Item.Quantity * 
                                Item.Product.Price).ToString("c"))%></td>
                             <td>
                                <button type="submit" class="actionButtons" name="remove"
                                    value="<%#Item.Product.ProductId %>">
                                    Удалить</button>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="3">Итого:</td>
                    <td><%= CartTotal.ToString("c") %></td>
                </tr>
            </tfoot>
        </table>
       <p class="actionButtons">
            <a href="<%= ReturnUrl %>">Продолжить покупки</a>
            <a href="<%= CheckoutUrl %>">Оформить заказ</a>
        </p>
    </div>
</asp:Content>
