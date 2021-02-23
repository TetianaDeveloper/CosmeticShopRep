using CosmeticShopWebApp.Models;
using CosmeticShopWebApp.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CosmeticShopWebApp.Pages.Admin
{
    public partial class Orders : System.Web.UI.Page
    {
        private Repository repository = new Repository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int dispatchID;
                if (int.TryParse(Request.Form["dispatch"], out dispatchID))
                {
                    Order myOrder = repository.Orders.Where(o => o.OrderId == dispatchID).FirstOrDefault();
                    if (myOrder != null)
                    {
                        myOrder.Dispatched = true;
                        repository.UpdateOrderDispatched(myOrder.Dispatched, myOrder.OrderId);
                    }
                }
            }
        }

        public IEnumerable<Order> GetOrders([Control] bool showDispatched)
        {
            if (showDispatched)
            {
                return repository.Orders;
            }
            else
            {
                return repository.Orders.Where(o => !o.Dispatched);
            }
        }

        public decimal Total(Order order)
        {
            decimal total = 0;

            foreach (OrderLine ol in order.OrderLines)
            {
                total += ol.Product.Price * ol.Quantity;
            }
            return total;
        }
    }
}