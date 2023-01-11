using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magazin.Domain.Models.CartItemPublishedEvent;

namespace Magazin.Domain.FacturaModels
{
    public record PublishedFactura(FacturaItem Factura)
    {
    }
}
