using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Dto.Models
{
    public record ItemDto
    {
        public String ItemId { get; set; }
        public String Name { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
    }
}
