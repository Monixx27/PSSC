using LanguageExt;
using Magazin.Domain.Models;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magazin.Domain.Models.CartItemPublishedEvent;
using System.Text.RegularExpressions;

namespace Magazin.Domain.FacturaModels
{
    
    public class FacturaItem
    {
        public int FacturaId { get; set; }
        public String Adresa { get; set; }
        public String TipLivrare { get; set; }
        public String Furnizor { get; set; }
        public String Sediu { get; set; }
        public string Cumparator { get; set; }
        public DateTime Data { get; set; }
        public DateTime Scadenta { get; set; }
        
        public FacturaItem(String Adresa, String TipLivrare, string Cumparator)
        {
            this.Adresa = Adresa;
            this.TipLivrare = TipLivrare;
            this.Cumparator = Cumparator;
        }

        public static Option<string> TryParseTipLivrare(string tiplivrare)
        {
            if (tiplivrare.Equals("Normala") || tiplivrare.Equals("Express"))
            {
                return Some<string>(tiplivrare);
            }
            else
            {
                return None;
            }
        }

        public static Option<int> TryParseId(int id)
        {
            if (id >= 0)
            {
                return Some<int>(id);
            }
            else
            {
                return None;
            }
        }
    }
}
