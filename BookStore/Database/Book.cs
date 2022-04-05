using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database
{
    public class Book : INotifyPropertyChanged, ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public int publicYear { get; set; }
        public string bookCover { get; set; }
        public int purchasePrice { get; set; }
        public int sellingPrice { get; set; }
        public int stockNumer { get; set; }
        public int sellingNumber { get; set; }

        public int category_id { get; set; }

        public Category Category { get; set; }



        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
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

        public Book(int _id, string _name, string _author, int _publicYear, string _cover, int _purchase, int _sellingPrice, int _stockNumber, int _sellingNumber, int _category)
        {
            this.id = _id;
            this.name = _name;
            this.author = _author;
            this.publicYear = _publicYear;
            this.bookCover = _cover;
            this.purchasePrice = _purchase;
            this.sellingPrice = _sellingPrice;
            this.stockNumer = _stockNumber;
            this.sellingNumber = _sellingNumber;
            this.category_id = _category;
        }

        public Book()
        {

        }



    }
}

