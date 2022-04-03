using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Book> ReadAllBook()
        {
            var sql = "select * from Book";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();
            // xu li

            List<Book> books = new List<Book>();

            while (reader.Read())
            {
                Book book = new Book()
                {
                    id = (int)reader["book_id"],
                    name = (string)reader["book_name"],
                    author = (string)reader["book_author"],
                    publicYear = (int)reader["book_year"],
                    bookCover = (string)reader["book_cover"],
                    purchasePrice = (int)reader["book_buying_price"],
                    sellingPrice = (int) reader["book_selling_price"],
                    stockNumer = (int)reader["book_stock"],
                    sellingNumber = (int)reader["book_sold"],
                    category_id = (int)reader["book_category"]
                };

                books.Add(book);
                //Debug.WriteLine(bookId);
            }

            reader.Close();
            return books;
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

            reader.Close();
            return result;
        }

        public Book GetBookById(int id)
        {
            var sql = "select * from Book where book_id=@BookId";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add("BookId", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();

            Book result = null;

            if (reader.Read()) // ORM - Object relational mapping
            {
                var bookID = (int)reader["book_id"];
                var bookName = (string)reader["book_name"];
                var bookAuthor = (string)reader["book_author"];
                var bookYear = (int)reader["book_year"];
                var bookCover = (string)reader["book_cover"];
                var bookBuyingPrice = (int)reader["book_buying_price"];
                var bookSellingPrice = (int)reader["book_selling_price"];
                var bookStock = (int)reader["book_stock"];
                var bookSold = (int)reader["book_sold"];
                var bookCategory = (int)reader["book_category"];

                result = new Book()
                {
                    id = bookID,
                    name = bookName,
                    author = bookAuthor,
                    publicYear = bookYear,
                    bookCover = bookCover,
                    purchasePrice = bookBuyingPrice,
                    sellingPrice = bookSellingPrice,
                    stockNumer = bookStock,
                    sellingNumber = bookSold,
                    category_id = bookCategory

                };
            }
            reader.Close();
            return result;
        }

        public void DeleteBookById(int id)
        {
            var sql = "delete from Book where book_id=@BookId";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("BookId", SqlDbType.Int).Value = id;
            var reader = command.ExecuteReader();
            reader.Close();
        }

        public void DeleteCategoryById(int id)
        {
            var sql = "delete from Category where category_id=@CategoryId";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("CategoryId", SqlDbType.Int).Value = id;
            var reader = command.ExecuteReader();
            reader.Close();
        }

        public void UpdateBook(int id, string bookName, string bookAuthor, int bookYear, string bookCover, int bookBuyingPrice, int bookSellingPrice, int bookStock, int bookSold, int bookCategory)
        {
            var sql = "update Book set book_name=@BookName, book_author=@BookAuthor, book_year=@BookYear, book_cover=@BookCover, book_buying_price=@BookBuyingPrice, book_selling_price=@BookSellingPrice, book_stock=@BookStock, book_sold=@BookSold, book_category=@BookCategory where book_id=@BookId";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("BookName", SqlDbType.NText).Value = bookName;
            command.Parameters.Add("BookAuthor", SqlDbType.NText).Value = bookAuthor;
            command.Parameters.Add("BookYear", SqlDbType.Int).Value = bookYear;
            command.Parameters.Add("BookCover", SqlDbType.NText).Value = bookCover;
            command.Parameters.Add("BookBuyingPrice", SqlDbType.Int).Value = bookBuyingPrice;
            command.Parameters.Add("BookSellingPrice", SqlDbType.Int).Value = bookSellingPrice;
            command.Parameters.Add("BookStock", SqlDbType.Int).Value = bookStock;
            command.Parameters.Add("BookSold", SqlDbType.Int).Value = bookSold;
            command.Parameters.Add("BookCategory", SqlDbType.Int).Value = bookCategory;
            command.Parameters.Add("BookId", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();
            reader.Close();
        }
    }

}
