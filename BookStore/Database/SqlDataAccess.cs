using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                try
                {
                    string? connectionString = AppConfig.ConnectionString2();

                    _connection = new SqlConnection(connectionString);
                    _connection.Open();
                    _connection.Close();
                }
                catch (Exception ex2)
                {
                    result = false;

                }
            }
            return result;
        }

        public void Connect()
        {
            try
            {
                _connection.Open();
            } catch (Exception ex)
            {
                string? connectionString = AppConfig.ConnectionString2();
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
        }

        public List<Category> ReadAllCategory()
        {
            var sql = "select * from Category";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                Category cat = new Category()
                {
                    ID = (int)reader["category_id"],
                    Name = (string)reader["category_name"],
                    Books = new List<Book>()
                };
                categories.Add(cat);
            }
            reader.Close();

            for(int i =0; i< categories.Count; ++i)
            {
                sql = "select * from Book where book_category=@CategoryID";
                command = new SqlCommand(sql, _connection);
                command.Parameters.Add("CategoryID", SqlDbType.Int).Value = categories[i].ID;
                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Book book = new Book()
                    {
                        id = (int)reader["book_id"],
                        name = (string)reader["book_name"],
                        author = (string)reader["book_author"],
                        publicYear = (int)reader["book_year"],
                        bookCover = (string)reader["book_cover"],
                        purchasePrice = (int)reader["book_buying_price"],
                        sellingPrice = (int)reader["book_selling_price"],
                        stockNumer = (int)reader["book_stock"],
                        sellingNumber = (int)reader["book_sold"],
                        category_id = (int)reader["book_category"]
                    };
                    categories[i].Books.Add(book);
                }
                reader.Close();
            }
            return categories;

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

        public List<Book> GetBooksByCategoryId(int id)
        {
            var sql = "select * from Book where book_category=@CategoryID";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("CategoryID", SqlDbType.Int).Value = id;
            var reader = command.ExecuteReader();

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
                    sellingPrice = (int)reader["book_selling_price"],
                    stockNumer = (int)reader["book_stock"],
                    sellingNumber = (int)reader["book_sold"],
                    category_id = (int)reader["book_category"]
                };
                books.Add(book);
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

        public List<Book> ReadAllBookLikeName(string name, int id)
        {
            var sql = "select * from Book where book_name LIKE '%" +name+ "%'and book_category=@CategoryID";

            var command = new SqlCommand(sql, _connection);
            //command.Parameters.Add("BookName", SqlDbType.NText).Value = name;
            command.Parameters.Add("CategoryID", SqlDbType.Int).Value = id;

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
                    sellingPrice = (int)reader["book_selling_price"],
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

        public List<Book> ReadAllBookPrice(int low, int high,int id)
        {
            var sql = "select * from Book where book_selling_price >@low and book_selling_price< @high and book_category=@CategoryID";

            var command = new SqlCommand(sql, _connection);
            //command.Parameters.Add("BookName", SqlDbType.NText).Value = name;
            command.Parameters.Add("low", SqlDbType.Int).Value = low;
            command.Parameters.Add("high", SqlDbType.Int).Value = high;
            command.Parameters.Add("CategoryID", SqlDbType.Int).Value = id;

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
                    sellingPrice = (int)reader["book_selling_price"],
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
        public int countBook()
        {
            var sql = "SELECT COUNT(book_id) AS NumberOfBooks FROM Book";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            int result = -1;

            if (reader.Read()) // ORM - Object relational mapping
            {
                result = (int)reader["NumberOfBooks"];
            }
            reader.Close();
            return result;
        }


        public List<Book> countTop5OutOfStock()
        {
            var sql = "SELECT TOP 5 * FROM Book Where book_stock< 5 ORDER BY  book_stock ASC";
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
                    sellingPrice = (int)reader["book_selling_price"],
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

        public List<Book> countOutOfStock()
        {
            var sql = "SELECT * FROM Book Where book_stock< 5 ORDER BY  book_stock ASC";
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
                    sellingPrice = (int)reader["book_selling_price"],
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

        public void updateNameCategoryByID(int id, string name)
        {
            var sql = "UPDATE Category SET category_name = '" + name + "' WHERE category_id = @id; ";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("id", SqlDbType.Int).Value = id;
            var reader = command.ExecuteReader();
            reader.Close();
        }

        public BindingList<Purchase> ReadAllPurchase()
        {
            var sql = "select * from Purchase";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            BindingList<Purchase> purchases = new BindingList<Purchase>();
            while (reader.Read())
            {
                Purchase pur = new Purchase()
                {
                    id = (int)reader["purchase_id"],
                    customerName = (String)reader["customer_name"],
                    tel = (String)reader["customer_tel"],
                    address = (String)reader["customer_address"],
                    total = (int)reader["purchase_final_total"],
                    date = (String)reader["purchase_created_at"],
                    status = (String)reader["purchase_status"],
                };
                purchases.Add(pur);
            }
            reader.Close();
            return purchases;

        }
        public List<PurchaseDetail> getAllDetailOrder(int purchaseID)
        {
            var sql = "select* from PurchaseDetail where purchase_id=@id";
            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add("id", SqlDbType.Int).Value = purchaseID;

            var reader = command.ExecuteReader();

            List<PurchaseDetail> result = new List<PurchaseDetail>();
            while (reader.Read())
            {
                PurchaseDetail pur = new PurchaseDetail()
                {
                    purchasedetail_id = (int)reader["purchasedetail_id"],
                    purchase_id = (int)reader["purchase_id"],
                    book_id = (int)reader["book_id"],
                    quantity = (int)reader["purchasedetail_quantity"],
                    price = (int)reader["purchasedetail_price"],
                    total = (int)reader["purchasedetail_total"],
                };
                result.Add(pur);
            }
            reader.Close();
            return result;

        }


        public string GetBookNameById(int id)
        {
            var sql = "select * from Book where book_id=@BookId";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add("BookId", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();

            string result = "";

            if (reader.Read()) // ORM - Object relational mapping
            {
                result = (string)reader["book_name"];
            }
            reader.Close();
            return result;
        }


    }

}
