<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Store.Master" AutoEventWireup="true" 
    CodeBehind="Listing.aspx.cs" Inherits="CosmeticShopWebApp.Pages.Listing" %>
<%@ Import Namespace="System.Web.Routing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodyContent" runat="server">
     <div id="content">
        <asp:Repeater ItemType="CosmeticShopWebApp.Models.Product"
            SelectMethod="GetProducts" runat="server">
        
            <HeaderTemplate>
                 <div class ="search">
                    <tr>
                        <td>
                            <asp:TextBox name="searchTextBox" runat="server" Width="500px" OnTextChanged="Unnamed_TextChanged"/>
                        </td>
                        <td>
                            <button name="searchBtn" type="submit">
                                Поиск
                            </button>
                        </td>
                    </tr>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="item">
                    <img src = <%#Item.Image%> >
                    <h3><%# Item.Brend %></h3>
                    <h3><%# Item.Name %></h3>
                    <%# Item.Description %>
                    <h4><%# Item.Price.ToString("c") %></h4>
                    <button name="add" type="submit" value="<%# Item.ProductId %>">
                        Добавить в корзину
                    </button>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="pager">
        <%
            for (int i = 1; i <= MaxPage; i++)
            {
                string category = (string)Page.RouteData.Values["category"]
                    ?? Request.QueryString["category"];
                
                string path = RouteTable.Routes.GetVirtualPath(null, null,
                    new RouteValueDictionary() { {"category", category}, { "page", i } }).VirtualPath;
                Response.Write(
                    String.Format("<a href='{0}' {1}>{2}</a>",
                        path, i == CurrentPage ? "class='selected'" : "", i));
            }
        %>
    </div>
</asp:Content>
