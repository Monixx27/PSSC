using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Choices;

namespace Magazin.Domain.Models
{
    [AsChoice]
    public static partial class CartItems
    {
        public interface ICartItems { }

        public record UnvalidatedCartItems : ICartItems
        {
            public UnvalidatedCartItems(IReadOnlyCollection<UnValidatedItem> itemsList)
            {
                ItemsList = itemsList;
            }

            public IReadOnlyCollection<UnValidatedItem> ItemsList { get; }
        }

        public record InvalidCartItems : ICartItems
        {
            internal InvalidCartItems(IReadOnlyCollection<UnValidatedItem> itemsList, string reason)
            {
                ItemsList = itemsList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnValidatedItem> ItemsList { get; }
            public string Reason { get; }
        }

        public record FailedCartItems : ICartItems
        {
            internal FailedCartItems(IReadOnlyCollection<UnValidatedItem> itemsList, Exception exception)
            {
                ItemsList = itemsList;
                Exception = exception;
            }

            public IReadOnlyCollection<UnValidatedItem> ItemsList { get; }
            public Exception Exception { get; }
        }

        public record ValidatedCartItems : ICartItems
        {
            internal ValidatedCartItems(IReadOnlyCollection<ValidatedItem> itemsList)
            {
                ItemsList = itemsList;
            }

            public IReadOnlyCollection<ValidatedItem> ItemsList { get; }
        }

        public record CalculatedCartItems : ICartItems
        {
            internal CalculatedCartItems(IReadOnlyCollection<CalculatedItem> itemsList)
            {
                ItemsList = itemsList;
            }

            public IReadOnlyCollection<CalculatedItem> ItemsList { get; }
        }

        public record PublishedCartItems : ICartItems
        {
            internal PublishedCartItems(IReadOnlyCollection<CalculatedItem> itemsList, string csv, DateTime publishedDate)
            {
                ItemsList = itemsList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedItem> ItemsList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}
