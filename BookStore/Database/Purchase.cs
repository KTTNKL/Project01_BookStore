using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database
{
    public class Purchase : INotifyPropertyChanged, ICloneable
    {

        public int id { set; get; }
        public String customerName { set; get; }
        public String tel { set; get; }
        public String address { set; get; }
        public int total { set; get; }
        public String date { set; get; }
        public String status { set; get; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
