using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Database
{
    internal class SqlDataAccess
    {
        private SqlConnection _connection;
        public SqlDataAccess(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public bool CanConnect()
        {
            bool result = true;

            try
            {
                _connection.Open();
                _connection.Close();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public void Connect()
        {
            _connection.Open();
        }

        public Category GetCategoryById(int id)
        {
            var sql = "select * from Category where category_id=@CatId";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add("CatId", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();

            Category result = null;

            if (reader.Read()) // ORM - Object relational mapping
            {
                var catId = (int)reader["category_id"];
                var catName = (string)reader["category_name"];

                result = new Category()
                {
                    ID = catId,
                    Name = catName,
                };
            }

            return result;
        }

        public int getIDCategory(string name)
        {
            var sql = "select * from Category where category_name LIKE @CatName";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("CatName", SqlDbType.NText).Value = name;

            var reader = command.ExecuteReader();

            int result = -1;

            if (reader.Read()) // ORM - Object relational mapping
            {
               result = (int)reader["category_id"];
            }
            reader.Close();
            return result;
        }
        public void deleteAllRecord(string table_name)
        {
            string sqlStatement = "DELETE FROM " + table_name;

            try
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, _connection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (SystemException ex)
            {
                MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
            }
        }
        public void insertCategoryRecord(string category_name)
        {

            string query = "INSERT INTO Category(category_name) VALUES('" + category_name + "')";

            SqlCommand command = new SqlCommand(query, _connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
        }

        public void insertBookRecord(string name, string author, int year, string cover, int buying, int selling, int stock, int sold, int category)
        {

            string query = "INSERT INTO Book(book_name, book_author, book_year, book_cover, book_buying_price, book_selling_price, book_stock, book_sold, book_category) VALUES('"
                            + name + "','" + author + "'," + year.ToString() + ",'" + cover + "'," + buying.ToString() + ","
                            + selling.ToString() + "," + stock.ToString() + "," + sold.ToString() + ","
                            + category.ToString() + ")";


            SqlCommand command = new SqlCommand(query, _connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }

        }

    }

}
