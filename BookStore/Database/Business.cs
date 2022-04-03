using System;
using System.Collections.Generic;
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
            MessageBox.Show(category_name);
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
    }
}
