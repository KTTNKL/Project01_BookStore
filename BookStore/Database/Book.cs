using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database
{
    internal class Book
    {
        public string id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public int publicYear { get; set; }
        public string bookCover { get; set; }
        public int purchasePrice { get; set; }
        public int sellingPrice { get; set; }
        public int stockNumer { get; set; }
        public int sellingNumber { get; set; }

        public Book(string _name, string _author, int _publicYear, string _cover, int _purchase, int _sellingPrice, int _stockNumber, int _sellingNumber)
        {
            this.name = _name;
            this.author = _author;
            this.publicYear = _publicYear;
            this.bookCover = _cover;
            this.purchasePrice = _purchase;
            this.sellingPrice = _sellingPrice;
            this.stockNumer = _stockNumber;
            this.sellingNumber = _sellingNumber;

        }
    }
}

