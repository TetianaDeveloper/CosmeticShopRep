using CosmeticShopWebApp.Models;
using CosmeticShopWebApp.Models.Repository;
using CosmeticShopWebApp.Pages.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CosmeticShopWebApp.Pages
{
    public partial class Listing : System.Web.UI.Page
    {
        private Repository repository = new Repository();
        private int pageSize = 4;
        private int tmp = 0;
        protected int CurrentPage
        {
            get
            {
                int page;
                page = int.TryParse(Request.QueryString["page"], out page) ? page : 1;
                return page > MaxPage ? MaxPage : page;
            }
        }
        // свойство, возвращающее наибольший номер допустимой страницы
        protected int MaxPage
        {
            get
            {
                int prodCount = FilterProducts().Count();
                return (int)Math.Ceiling((decimal)prodCount / pageSize);
            }
        }
        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ??
                Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) ? page : 1;
        }
        public IEnumerable<Product> GetProducts()
        {
                return FilterProducts()
                    .OrderBy(p => p.ProductId)
                    .Skip((CurrentPage - 1) * pageSize)
                    .Take(pageSize);
        }
        protected IEnumerable<Product> FilterProducts()
        {
            IEnumerable<Product> products = repository.Products;
            IEnumerable<Cathegory> cathegories = repository.Cathegories;
            string currentCategoryName = (string)RouteData.Values["category"] ??
                Request.QueryString["category"];
            Cathegory cathegory = repository.getCathegoryByName(currentCategoryName);
            IEnumerable<Product> filteredProducts = repository.GetProductsByCathegory(cathegory);

            return currentCategoryName == null ? products : filteredProducts;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                int selectedProductId;
                string tm = Request.Form["add"];
                if (int.TryParse(Request.Form["add"], out selectedProductId))
                {
                    Product selectedProduct = repository.Products
                        .Where(p => p.ProductId == selectedProductId).FirstOrDefault();

                    if (selectedProduct != null)
                    {
                        SessionHelper.GetCart(Session).AddItem(selectedProduct, 1);
                        SessionHelper.Set(Session, SessionKey.RETURN_URL,
                            Request.RawUrl);

                        Response.Redirect(RouteTable.Routes
                            .GetVirtualPath(null, "cart", null).VirtualPath);
                    }
                }
                else
                {
                    string t = Request.Form["searchBtn"];
                    if (int.TryParse(Request.Form["searchBtn"], out selectedProductId))
                    {
                        string tmp = Request.Form["searchTextBox"];
                    }

                }
            }
        }

        protected void Unnamed_TextChanged(object sender, EventArgs e)
        {
            TextBox text = (TextBox)sender;
        }
    }
}