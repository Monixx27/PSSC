using LanguageExt;
using static Magazin.Data.IRepositories.IDateLivrareRepository;
using Magazin.Domain.FacturaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magazin.Domain.FacturaModels.FacturaStates;

namespace Magazin.Domain
{
    public static class FacturaOperations
    {
        static Random rand = new();
        public static Task<IFacturaStates> GetFacturaValidated(UnvalidatedFacturaState facturaState) =>
             ValidateFactura(facturaState).MatchAsync(
                            Right: validatedFactura => new ValidatedFacturaState(validatedFactura),
                            LeftAsync: errorMessage => Task.FromResult((IFacturaStates)new InvalidFacturaState(facturaState.Factura, errorMessage))
                      );
        public static EitherAsync<string, ValidatedFactura> ValidateFactura(UnvalidatedFacturaState facturaState) =>
            from Tip in FacturaItem.TryParseTipLivrare(facturaState.Factura.TipLivrare)
                                   .ToEitherAsync($"Invalid tip livrare ({facturaState.Factura.Adresa},{facturaState.Factura.Cumparator},{facturaState.Factura.TipLivrare})")
            select (new ValidatedFactura(FillFactura(facturaState)));


        public static FacturaItem FillFactura(UnvalidatedFacturaState facturaState) => new FacturaItem(facturaState.Factura.Adresa, facturaState.Factura.TipLivrare, facturaState.Factura.Cumparator)
        {
            FacturaId = rand.Next(100000),
            Furnizor = "U.P.T",
            Sediu = "Timisoara",
            Data = DateTime.Now,
            Scadenta = DateTime.Now.AddDays(3)
        };

        public static IFacturaStates PublishFactura(IFacturaStates fact) => fact.Match(
                whenInvalidFacturaState: invalidFactura => invalidFactura,
                whenUnvalidatedFacturaState: unvalidatedFactura => unvalidatedFactura,
                whenFailedFactura: faildFactura => faildFactura,
                whenPublishedFacturaState: publishedFactura => publishedFactura,
                whenValidatedFacturaState: PublishFact
                );

        private static IFacturaStates PublishFact(ValidatedFacturaState validFact) => 
            new PublishedFacturaState(new PublishedFactura(validFact.Factura.Factura), DateTime.Now);
    }
}
