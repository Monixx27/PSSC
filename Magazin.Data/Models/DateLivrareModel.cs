using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Data.Models
{
    public class DateLivrareModel
    {
        public int FacturaId { get; set; }
        public String Adresa { get; set; }
        public String TipLivrare { get; set; }
        public String AWB { get; set; }
        public String Furnizor { get; set; }
        public int Sediu { get; set; }
        public string Cumparator { get; set; }
        public DateTime Data { get; set; }
        public DateTime Scadenta { get; set; }
    }
}
