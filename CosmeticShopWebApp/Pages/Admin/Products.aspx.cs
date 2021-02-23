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
    public partial class Products : System.Web.UI.Page
    {
        private Repository repository = new Repository();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<Product> GetProducts()
        {
            return repository.Products;
        }

        public void UpdateProduct(int ProductID)
        {
            Product myProduct = repository.Products
                .Where(p => p.ProductId == ProductID).FirstOrDefault();
            if (myProduct != null && TryUpdateModel(myProduct,
                new FormValueProvider(ModelBindingExecutionContext)))
            {
             
                repository.SaveProduct(myProduct);
            }
        }

        public void DeleteProduct(int ProductID)
        {
            Product myProduct = repository.Products
               .Where(p => p.ProductId == ProductID).FirstOrDefault();
            if (myProduct != null)
            {
                repository.DeleteProduct(myProduct);
            }
        }

        public void InsertProduct()
        {
            Product myProduct = new Product();
            if (TryUpdateModel(myProduct,
               new FormValueProvider(ModelBindingExecutionContext)))
            {
                repository.SaveProduct(myProduct);
            }
            
        }
    }
}