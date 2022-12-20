using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Data.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public String Name { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        public static ItemModel operator +(ItemModel a, ItemModel b) => new ItemModel { 
            ItemId = a.ItemId,
            Name = a.Name,
            Quantity = a.Quantity > b.Quantity ? a.Quantity + b.Quantity : b.Quantity + a.Quantity,
            Price = a.Price
        };

        public static ItemModel operator -(ItemModel a, ItemModel b) => new ItemModel
        {
            ItemId = a.ItemId,
            Name = a.Name,
            Quantity = a.Quantity > b.Quantity ? a.Quantity - b.Quantity : b.Quantity - a.Quantity,
            Price = a.Price
        };

    }
}
