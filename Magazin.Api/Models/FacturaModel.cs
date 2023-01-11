using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Magazin.Api.Models
{
    public class FacturaModel
    {
        public int FacturaId { get; set; }
        public int Quantity { get; set; }
        public String ItemId { get; set; }
        public decimal? Price { get; set; }
    }
}
