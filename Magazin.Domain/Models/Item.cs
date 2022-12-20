using LanguageExt;
using System;
using static LanguageExt.Prelude;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Magazin.Domain.Models
{
    public class Item
    {
        private static readonly Regex ValidPattern = new("^i[0-9]{3}$");
        public Item(int ItemId, String Name, int Quantity, decimal? Price)
        {
            this.ItemId = ItemId;
            this.Name = Name;
            this.Quantity = Quantity;
            this.Price = Price;
        }

        public int ItemId { get; }
        public string Name { get; }
        public int Quantity { get; }
        public decimal? Price { get; }

        public override string ToString()
        {
            return $"Id:{ItemId}, Name:{Name}, Quantity:{Quantity}, Price:{Price}";
        }

        public static Option<decimal> TryParseQty(string quantity, decimal max_qty)
        {
            if (decimal.TryParse(quantity, out decimal numericQty) && IsValid(numericQty))
            {
                if (numericQty > max_qty)
                    return Some<decimal>(max_qty);
                else
                    return Some<decimal>(numericQty);
            }
            else
            {
                return None;
            }
        }
        public static Option<Item> TryParseItem(Item item)
        {
            if (item.Quantity != 0 && item.Price > 0.00m)
            {
                return item;
            }
            else
            {
                return None;
            }
        }

        public static Option<string> TryParseId(string id)
        {
            if (ValidPattern.IsMatch(id))
            {
                return Some<string>(id);
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal value) => value > 0.0m;

    }
}
