using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace CosmeticShopWebApp.Models.Repository
{
    public class Repository
    {
        private SQLHelper helper;
        private List<Product> products;
        private List<Cathegory> cathegories;
        private List<OrderLine> orderLines;
        private List<Order> orders;
        private List<OrderLine> filteredOrderLines;
        private string connectionString;
        private DataTable productTable;
        private DataTable cathegoryTable;
        private DataTable ordersTable;
        private DataTable orderLinesTable;

        public Repository()
        {
            products = new List<Product>();
            cathegories = new List<Cathegory>();
            orderLines = new List<OrderLine>();
            orders = new List<Order>();

            connectionString =
                WebConfigurationManager.ConnectionStrings["CosmeticShopDB"].ConnectionString;

            helper = new SQLHelper();
            helper.OpenConnection(connectionString);
            productTable = helper.GetAllProductsAsDataTable();
            cathegoryTable = helper.GetAllCathegoriesAsDataTable();
            ordersTable = helper.GetAllOrdersAsDataTable();
            orderLinesTable = helper.GetAllOrderLinesAsDataTable();
            helper.CloseConnection();

            //from DataTable to list products 
            foreach (DataRow row in productTable.Rows)
            {
                Product product = new Product();
                product.ProductId = int.Parse(row["ProductID"].ToString());
                product.Brend = row["Brend"].ToString();
                product.Name = row["Name"].ToString();
                product.Description = row["Description"].ToString();
                product.CathegoryId = int.Parse(row["CathegoryID"].ToString());
                product.Price = decimal.Parse(row["Price"].ToString());
                product.Image = row["Image"].ToString();
                products.Add(product);
            }
            //from DataTable to list cathegories 
            foreach (DataRow row in cathegoryTable.Rows)
            {
                Cathegory cathegory = new Cathegory();
                cathegory.CathegoryId = int.Parse(row["CathegoryID"].ToString());
                cathegory.CathegoryName = row["CathegoryName"].ToString();
                cathegories.Add(cathegory);
            }
            //from DataTable to list orderLines
            foreach (DataRow row in orderLinesTable.Rows)
            {
                OrderLine orderLine = new OrderLine();
                orderLine.OrderLineId = int.Parse(row["OrderLineId"].ToString());
                orderLine.Quantity = int.Parse(row["Quantity"].ToString());
                orderLine.Product = Products
                        .Where(p => p.ProductId == int.Parse(row["ProductID"].ToString())).FirstOrDefault();
                int orderID = int.Parse(row["OrderID"].ToString());
                orderLine.Order = getOrderByID(orderID);
                orderLines.Add(orderLine);
            }
            //from DataTable to list orders
            foreach (DataRow row in ordersTable.Rows)
            {
                Order order = new Order();
                order.OrderId = int.Parse(row["OrderId"].ToString());
                order.Name = row["Name"].ToString();
                order.Line1 = row["Line1"].ToString();
                order.Line2 = row["Line2"].ToString();
                order.Line3 = row["Line3"].ToString();
                order.City = row["City"].ToString();
                order.GiftWrap = bool.Parse(row["GiftWrap"].ToString());
                order.Dispatched = bool.Parse(row["Dispatched"].ToString());
                helper.OpenConnection(connectionString);
                DataTable filteredOrderLinesTable = helper.GetOrderLinesByOrderID(order.OrderId);
                helper.CloseConnection();
                filteredOrderLines = new List<OrderLine>();
                foreach (DataRow dataRow in filteredOrderLinesTable.Rows)
                {
                    OrderLine orderLine = new OrderLine();
                    orderLine.OrderLineId = int.Parse(dataRow["OrderLineId"].ToString());
                    orderLine.Quantity = int.Parse(dataRow["Quantity"].ToString());
                    orderLine.Product = Products
                            .Where(p => p.ProductId == int.Parse(dataRow["ProductID"].ToString())).FirstOrDefault();
                    orderLine.Order = order;
                    filteredOrderLines.Add(orderLine);
                }
                order.OrderLines = filteredOrderLines;
                orders.Add(order);
            }
        }
        public IEnumerable<Product> Products
        {
            get { return products; }
        }

        public IEnumerable<Cathegory> Cathegories
        {
            get { return cathegories; }
        }
        public IEnumerable<Order> Orders
        {
            get { return orders; }
        }


        public Cathegory getCathegoryByName(string name)
        {
            foreach (Cathegory c in cathegories)
                if (name == c.CathegoryName)
                    return c;
            return null;
        }
        public IEnumerable<Product> GetProductsByCathegory(Cathegory c)
        {
            List<Product> filteredProducts = new List<Product>();
            foreach (Product p in Products)
                if (c != null && c.CathegoryId == p.CathegoryId)
                    filteredProducts.Add(p);
            return filteredProducts;
        }

        public Order getOrderByID(int orderID)
        {
            Order order = new Order();
            string sqlExpression = "GetOrderByID";

            helper.OpenConnection(connectionString);
            using (SqlCommand command = new SqlCommand(sqlExpression, helper.Connection))
            {
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = orderID
                };
                // добавляем параметр
                command.Parameters.Add(idParam);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        order.OrderId = reader.GetInt32(0);
                        order.Name = reader.GetString(1);
                        order.Line1 = reader.GetString(2);
                        order.Line2 = reader.GetString(3);
                        order.Line3 = reader.GetString(4);
                        order.City = reader.GetString(5);
                        order.GiftWrap = reader.GetBoolean(6);
                        order.Dispatched = reader.GetBoolean(7);
                    }
                }
                reader.Close();
            }
            helper.CloseConnection();
            return order;
        }
        //Сохранить данные заказа в базе данных
        public void SaveOrder(Order order)
        {
            helper.OpenConnection(connectionString);
            // название процедуры
            string sqlExpression = "InsertIntoOrderAndOrderLine";
            int result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(sqlExpression, helper.Connection))
                {
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter nameParam = new SqlParameter("@name", order.Name);
                    command.Parameters.Add(nameParam);
                    SqlParameter line1Param = new SqlParameter("@line1", order.Line1);
                    command.Parameters.Add(line1Param);
                    SqlParameter line2Param = new SqlParameter("@line2", order.Line2);
                    command.Parameters.Add(line2Param);
                    SqlParameter line3Param = new SqlParameter("@line3", order.Line3);
                    command.Parameters.Add(line3Param);
                    SqlParameter cityParam = new SqlParameter("@city", order.City);
                    command.Parameters.Add(cityParam);
                    SqlParameter giftParam = new SqlParameter("@giftWrap", order.GiftWrap);
                    command.Parameters.Add(giftParam);
                    SqlParameter dispParam = new SqlParameter("@dispatched", order.Dispatched);
                    command.Parameters.Add(dispParam);
                    foreach (OrderLine orderLine in order.OrderLines)
                    {
                        SqlParameter quantityParam = new SqlParameter("@quantity", orderLine.Quantity);
                        command.Parameters.Add(quantityParam);
                        SqlParameter productIdParam = new SqlParameter("@productID", orderLine.Product.ProductId);
                        command.Parameters.Add(productIdParam);
                    }
                    result = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
            helper.CloseConnection();
        }
        public void UpdateOrderDispatched(bool value, int dispId)
        {
            helper.OpenConnection(connectionString);
            // название процедуры
            string sqlExpression = "UpdateOrderDispatched";
            int result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(sqlExpression, helper.Connection))
                {
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter valueParam = new SqlParameter("@value", value);
                    command.Parameters.Add(valueParam);
                    SqlParameter dispIdParam = new SqlParameter("@dispId", dispId);
                    command.Parameters.Add(dispIdParam);
                    result = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
            }
            helper.CloseConnection();
        }
        public void SaveProduct(Product product)
        {
            string sqlExpression = "";
            int result = 0;
            helper.OpenConnection(connectionString);
            if (product.ProductId == 0)
            {
                sqlExpression = "InsertProduct";
                using (SqlCommand command = new SqlCommand(sqlExpression, helper.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter brendParam = new SqlParameter("@brend", product.Brend);
                    command.Parameters.Add(brendParam);
                    SqlParameter nameParam = new SqlParameter("@name", product.Name);
                    command.Parameters.Add(nameParam);
                    SqlParameter discParam = new SqlParameter("@description", product.Description);
                    command.Parameters.Add(discParam);
                    SqlParameter catIdParam = new SqlParameter("@cathegoryId", product.CathegoryId);
                    command.Parameters.Add(catIdParam);
                    SqlParameter priceParam = new SqlParameter("@price", product.Price);
                    command.Parameters.Add(priceParam);
                    SqlParameter imageParam = new SqlParameter("@image", product.Image);
                    command.Parameters.Add(imageParam);
                    result = command.ExecuteNonQuery();
                }
            }
            else
            {
                sqlExpression = "UpdateProduct";
                using (SqlCommand command = new SqlCommand(sqlExpression, helper.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter idParam = new SqlParameter("@productId", product.ProductId);
                    command.Parameters.Add(idParam);
                    SqlParameter brendParam = new SqlParameter("@brend", product.Brend);
                    command.Parameters.Add(brendParam);
                    SqlParameter nameParam = new SqlParameter("@name", product.Name);
                    command.Parameters.Add(nameParam);
                    SqlParameter discParam = new SqlParameter("@description", product.Description);
                    command.Parameters.Add(discParam);
                    SqlParameter catIdParam = new SqlParameter("@cathegoryId", product.CathegoryId);
                    command.Parameters.Add(catIdParam);
                    SqlParameter priceParam = new SqlParameter("@price", product.Price);
                    command.Parameters.Add(priceParam);
                    SqlParameter imageParam = new SqlParameter("@image", product.Image);
                    command.Parameters.Add(imageParam);
                    result = command.ExecuteNonQuery();
                }

            }
            helper.CloseConnection();
        }
        public void DeleteProduct(Product product)
        { 
            string sqlExpression1 = "DeleteOrderLinesByProductID";
            string sqlExpression2 = "DeleteOrder";
            string sqlExpression3 = "DeleteProduct";
            int orderID = 0;
            helper.OpenConnection(connectionString);


            //delete from OrderLines
            using (SqlCommand command = new SqlCommand(sqlExpression1, helper.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter("@productId", product.ProductId);
                command.Parameters.Add(idParam);
                var result = command.ExecuteScalar();  
                if(result != null)
                    orderID = int.Parse(result.ToString());
            }
            //delete from Orders
            if (orderID != 0)
            {
                using (SqlCommand command = new SqlCommand(sqlExpression2, helper.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter idParam = new SqlParameter("@orderId", orderID);
                    command.Parameters.Add(idParam);
                    int result = command.ExecuteNonQuery();
                }
            }
            //delete from Product
            using (SqlCommand command = new SqlCommand(sqlExpression3, helper.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter idParam = new SqlParameter("@productId", product.ProductId);
                command.Parameters.Add(idParam);
                int result = command.ExecuteNonQuery();
            }
            helper.CloseConnection();
        }

    }
}