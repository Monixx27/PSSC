using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Data.Models
{
    public class FacturaObjModel
    {
        public int FacturaId { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}
