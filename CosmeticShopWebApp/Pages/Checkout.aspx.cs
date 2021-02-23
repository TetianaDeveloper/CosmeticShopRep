using CosmeticShopWebApp.Models;
using CosmeticShopWebApp.Models.Repository;
using CosmeticShopWebApp.Pages.Helpers;
using System;
using System.Collections.Generic;
using System.Web.ModelBinding;


namespace CosmeticShopWebApp.Pages
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            checkoutForm.Visible = true;
            checkoutMessage.Visible = false;

            if (IsPostBack)
            {
                Order myOrder = new Order();
              
                    myOrder.Name = nameTextBox.Text;
                    myOrder.Line1 = line1TextBox.Text;
                    myOrder.Line2 = line2TextBox.Text;
                    myOrder.Line3 = line3TextBox.Text;
                    myOrder.City = cityTextBox.Text;
                    myOrder.GiftWrap = checkbox1.Checked;

                    myOrder.OrderLines = new List<OrderLine>();

                    Cart myCart = SessionHelper.GetCart(Session);

                    foreach (CartLine line in myCart.Lines)
                    {
                        myOrder.OrderLines.Add(new OrderLine
                        {
                            Order = myOrder,
                            Product = line.Product,
                            Quantity = line.Quantity
                        });
                    }

                    new Repository().SaveOrder(myOrder);
                    myCart.Clear();

                    checkoutForm.Visible = false;
                    checkoutMessage.Visible = true;
                
            }
        }
    }
}