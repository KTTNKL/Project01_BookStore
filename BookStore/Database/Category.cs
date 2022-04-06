using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BookStore.Database
{
    public class Category : INotifyPropertyChanged, ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
