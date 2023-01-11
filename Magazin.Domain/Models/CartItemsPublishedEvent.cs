using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Domain.Models
{
    [AsChoice]
    public static partial class CartItemPublishedEvent
    {
        public interface ICartItemsPublishedEvent { }

        public record CartItemsPublishScucceededEvent : ICartItemsPublishedEvent
        {
            public IEnumerable<PublishedCartItem> Items { get; }
            public DateTime PublishedDate { get; }
            public decimal? Total { get; }

            internal CartItemsPublishScucceededEvent(IEnumerable<PublishedCartItem> items, DateTime publishedDate, decimal? total)
            {
                Items = items;
                PublishedDate = publishedDate;
                Total = total;
            }
        }

        public record CartItemsPublishFaildEvent : ICartItemsPublishedEvent
        {
            public string Reason { get; }

            internal CartItemsPublishFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
