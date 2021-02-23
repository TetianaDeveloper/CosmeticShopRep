using CosmeticShopWebApp.Models;
using CosmeticShopWebApp.Models.Repository;
using CosmeticShopWebApp.Pages.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace CosmeticShopWebApp.Pages
{
    public partial class CartView : System.Web.UI.Page
    {
        private Repository repository = new Repository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {               
                int productId;
                if (int.TryParse(Request.Form["remove"], out productId))
                {
                    Product productToRemove = repository.Products
                        .Where(p => p.ProductId == productId).FirstOrDefault();
                    if (productToRemove != null)
                    {
                        SessionHelper.GetCart(Session).RemoveLine(productToRemove);
                    }
                }
            }
        }
        public IEnumerable<CartLine> GetCartLines()
        {
            return SessionHelper.GetCart(Session).Lines;
        }

        public decimal CartTotal
        {
            get
            {
                return SessionHelper.GetCart(Session).ComputeTotalValue();
            }
        }

        public string ReturnUrl
        {
            get
            {
                return SessionHelper.Get<string>(Session, SessionKey.RETURN_URL);
            }
        }
        public string CheckoutUrl
        {
            get
            {
                return RouteTable.Routes.GetVirtualPath(null, "checkout",
                    null).VirtualPath;
            }
        }
    }
}