using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStore.Database
{
    internal class Business
    {
        SqlDataAccess _dao;

        public Business(SqlDataAccess dao)
        {
            _dao = dao;
        }
        public void deleteTable(string table_name)
        {
            _dao.deleteAllRecord(table_name);
        }
        public void insertCategory(string category_name)
        {
            _dao.insertCategoryRecord(category_name);
        }

        public void insertBook(string name, string author, int year, string cover, int buying, int selling, int stock, int sold, int category)
        {
            _dao.insertBookRecord(name, author, year, cover, buying, selling, stock, sold, category);
        }
        public int getCategoryID(string category_name)
        {
            return _dao.getIDCategory(category_name);
        }

        public Category GetCategoryById(int id)
        {
            Category result = _dao.GetCategoryById(id);

            return result;
        }

        public Book GetBookById(int id)
        {
            Book result = _dao.GetBookById(id);

            return result;
        }

        public void DeleteBookById(int id)
        {
            _dao.DeleteBookById(id);
        }

        public void DeleteCategoryById(int id)
        {
            _dao.DeleteCategoryById(id);
        }

        public void UpdateBook(int id, string bookName, string bookAuthor, int bookYear, string bookCover, int bookBuyingPrice, int bookSellingPrice, int bookStock, int bookSold, int bookCategory)
        {
            _dao.UpdateBook(id, bookName, bookAuthor, bookYear, bookCover, bookBuyingPrice, bookSellingPrice, bookStock, bookSold, bookCategory);
        }

        public List<Book> ReadAllBook()
        {
            List<Book> books = _dao.ReadAllBook();
            return books;
        }

        public List<Book> ReadAllBookLikeName(string name,int id)
        {
            List<Book> books = _dao.ReadAllBookLikeName(name,id);
            return books;
        }

        public List<Category> ReadAllCategory()
        {
            List<Category> categories = _dao.ReadAllCategory();
            return categories;
        }

        public List<Book> GetBooksByCategoryId(int id)
        {
            List<Book> books = _dao.GetBooksByCategoryId(id);
            return books;
        }


        public int countBook()
        {
            return _dao.countBook();
        }

        public List<Book> countTop5OutOfStock()
        {
            return _dao.countTop5OutOfStock();
        }

        public List<Book> countOutOfStock()
        {
            return _dao.countOutOfStock();
        }

        public List<Book> ReadAllBookPrice(int low, int high,int id)
        {
            return _dao.ReadAllBookPrice(low, high,id);
        }

        public void updateNameCategoryByID(int id, string name)
        {
            _dao.updateNameCategoryByID(id, name);
        }

        public BindingList<Purchase> ReadAllPurchase()
        {
            return _dao.ReadAllPurchase();
        }

        public List<PurchaseDetail> getAllDetailOrder(int id)
        {
            return _dao.getAllDetailOrder(id);
        }

        public string GetBookNameById(int id)
        {
            return _dao.GetBookNameById(id);
        }
    }
}
