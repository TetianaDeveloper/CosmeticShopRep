using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CosmeticShopWebApp.Models.Repository
{
    public class SQLHelper
    {
        private SqlConnection connect = null;

        public void OpenConnection(string connectionString)
        {
            connect = new SqlConnection(connectionString);
            connect.Open();
        }

        public void CloseConnection()
        {
            connect.Close();
        }

        public SqlConnection Connection
        {
            get { return connect; }
        }
        public DataTable GetAllProductsAsDataTable()
        {
            DataTable product = new DataTable();
            string sql = "Select * From Product";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                product.Load(dr);
                dr.Close();
            }
            return product;
        }

        public DataTable GetAllCathegoriesAsDataTable()
        {
            DataTable cathegory = new DataTable();
            string sql = "Select * From Cathegory";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                cathegory.Load(dr);
                dr.Close();
            }
            return cathegory;
        }
        public DataTable GetCathegoryByName(string cathegoryName)
        {
            DataTable cathegory = new DataTable();
            string sql = "Select * From Cathegory c Where c.CathegoryName = @cathegoryName";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                cathegory.Load(dr);
                dr.Close();
            }
            return cathegory;
        }
        public DataTable GetAllOrdersAsDataTable()
        {
            DataTable order = new DataTable();
            string sql = "Select * From Orders";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                order.Load(dr);
                dr.Close();
            }
            return order;
        }
        public DataTable GetAllOrderLinesAsDataTable()
        {
            DataTable orderLine = new DataTable();
            string sql = "Select * From OrderLines";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                orderLine.Load(dr);
                dr.Close();
            }
            return orderLine;
        }
        public DataTable GetOrderLinesByOrderID(int orderId)
        {
            DataTable filteredOrderLines = new DataTable();
            string sql = "Select * From OrderLines o Where o.OrderID = @orderId";
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
                SqlParameter idParam = new SqlParameter("@orderId", orderId);
                cmd.Parameters.Add(idParam);

                SqlDataReader dr = cmd.ExecuteReader();
                filteredOrderLines.Load(dr);
                dr.Close();
            }
            return filteredOrderLines;
        }

    }
}