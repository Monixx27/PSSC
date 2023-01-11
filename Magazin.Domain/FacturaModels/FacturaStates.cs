using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Domain.FacturaModels
{
    [AsChoice]
    public static partial class FacturaStates
    {
        public interface IFacturaStates { }

        public record UnvalidatedFacturaState : IFacturaStates
        {
            public UnvalidatedFacturaState(Unvalidatedfactura factura)
            {
                Factura = factura;
            }

            public Unvalidatedfactura Factura { get; }
        }

        public record InvalidFacturaState : IFacturaStates
        {
            internal InvalidFacturaState(Unvalidatedfactura factura, string reason)
            {
                Factura = factura;
                Reason = reason;
            }

            public Unvalidatedfactura Factura { get; }
            public string Reason { get; }
        }

        public record ValidatedFacturaState : IFacturaStates
        {
            internal ValidatedFacturaState(ValidatedFactura factura)
            {
                Factura = factura;
            }

            public ValidatedFactura Factura { get; }
        }

        public record PublishedFacturaState : IFacturaStates
        {
            internal PublishedFacturaState(PublishedFactura factura, DateTime publishedDate)
            {
                Factura = factura;
                PublishedDate = publishedDate;
            }

            public PublishedFactura Factura { get; }
            public DateTime PublishedDate { get; }
        }

        public record FailedFactura : IFacturaStates
        {
            internal FailedFactura(Unvalidatedfactura factura, Exception exception)
            {
                Factura = factura;
                Exception = exception;
            }

            public Unvalidatedfactura Factura { get; }
            public Exception Exception { get; }
        }
    }
}
