<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Admin/Admin.Master" 
    AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="CosmeticShopWebApp.Pages.Admin.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ListView ID="ListView1" ItemType="CosmeticShopWebApp.Models.Product" 
        SelectMethod="GetProducts"
        DataKeyNames="ProductID" 
        UpdateMethod="UpdateProduct" 
        DeleteMethod="DeleteProduct"
        InsertMethod="InsertProduct" 
        InsertItemPosition="LastItem" EnableViewState="false" runat="server">
        <LayoutTemplate>
            <div class="outerContainer">
                <table id="productsTable">
                    <tr>
                        <th>Бренд</th>
                        <th>Название</th>
                        <th>Описание</th>
                        <th>Категория</th>
                        <th>Цена</th>
                        <th>Изображенные</th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder"></tr>
                </table>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Item.Brend %></td>
                <td><%# Item.Name %></td>
                <td class="description"><span><%# Item.Description %></span></td>
                <td><%# Item.CathegoryId %></td>
                <td><%# Item.Price.ToString("c") %></td>
                <td><img src = <%#Item.Image%> ></td>
                <td>
                    <asp:Button ID="Button1" CommandName="Edit" Text="Изменить" runat="server" />
                    <asp:Button ID="Button2" CommandName="Delete" Text="Удалить" runat="server" />
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>
                    <input name="brend" value="<%# Item.Brend %>" /></td>
                <td>
                    <input name="name" value="<%# Item.Name %>" />
                    <input type="hidden" name="ProductID" value="<%# Item.ProductId %>" />
                </td>
                <td>
                    <input name="description" value="<%# Item.Description %>" /></td>
                <td>
                    <input name="cathegoryId" value="<%# Item.CathegoryId %>" /></td>
                <td>
                    <input name="price" value="<%# Item.Price %>" /></td>
                <td>
                    <input name="image" value="<%# Item.Image %>" /></td></td>
                <td>

                    <asp:Button ID="Button3" CommandName="Update" Text="Обновить" runat="server" />
                    <asp:Button ID="Button4" CommandName="Cancel" Text="Отмена" runat="server" />
                </td>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <input name="brend" /></td>
                <td>
                    <input name="name" />
                    <input type="hidden" name="ProductID" value="0" />
                </td>
                <td>
                    <input name="description" /></td>
                <td>
                    <input name="cathegoryId" /></td>
                <td>
                    <input name="price" /></td>
                <td>
                    <input name="image" /></td>
                <td>
                    <asp:Button ID="Button5" CommandName="Insert" Text="Вставить" runat="server" />
                </td>
            </tr>
        </InsertItemTemplate>
    </asp:ListView>
</asp:Content>
