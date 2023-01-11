using CSharp.Choices;
using Magazin.Domain.CurieratModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magazin.Domain.Models.CartItemPublishedEvent;

namespace Magazin.Domain.FacturaModels
{
    [AsChoice]
    public static partial class FacturaPublishedEvent
    {
        public interface IFacturaPublishedEvent { }

        public record FacturaPublishScucceededEvent : IFacturaPublishedEvent
        {
            public PublishedFactura PubFactura { get; }
            public AWBModel DateLivrare { get; }
            public CartItemsPublishScucceededEvent PublishedItems { get; }
            internal FacturaPublishScucceededEvent(PublishedFactura factura, AWBModel dateLivrare, CartItemsPublishScucceededEvent publishedItems)
            {
                PubFactura = factura;
                DateLivrare = dateLivrare;
                PublishedItems = publishedItems;
            }
        }

        public record FacturaPublishFaildEvent : IFacturaPublishedEvent
        {
            public string Reason { get; }

            internal FacturaPublishFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
