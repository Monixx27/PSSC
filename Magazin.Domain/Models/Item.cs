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
        public const string Pattern = "^i[0-9]{3}$";
        private static readonly Regex ValidPattern = new(Pattern);
        public Item(string ItemId, String Name, int Quantity, decimal? Price)
        {
            this.ItemId = ItemId;
            this.Name = Name;
            this.Quantity = Quantity;
            this.Price = Price;
        }

        public string ItemId { get; }
        public string Name { get; }
        public int Quantity { get; }
        public decimal? Price { get; }

        public override string ToString()
        {
            return $"Id:{ItemId}, Name:{Name}, Quantity:{Quantity}, Price:{Price}";
        }

        public static Option<int> TryParseQty(int quantity, int max_qty)
        {
            if (IsValid(quantity))
            {
                if (quantity > max_qty)
                    return Some<int>(max_qty);
                else
                    return Some<int>(quantity);
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
