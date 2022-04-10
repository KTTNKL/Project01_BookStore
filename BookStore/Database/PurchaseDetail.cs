using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database
{
    public class PurchaseDetail
    {
		public string name { set; get; }
		public int purchasedetail_id { set; get; }
		public int purchase_id { set; get; }
		public int book_id { set; get; }
		public int quantity { set; get; }
		public int price { set; get; }
		public int total { set; get; }
	}
}
