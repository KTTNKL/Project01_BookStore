using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
