using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Seller { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double PriceInDollar { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string Reccuring { get; set; }
        public string Invoice { get; set; } // to be an image later

        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
