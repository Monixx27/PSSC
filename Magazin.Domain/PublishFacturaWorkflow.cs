using LanguageExt;
using Magazin.Data.IRepositories;
using Magazin.Domain.FacturaModels;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using static Magazin.Domain.FacturaModels.FacturaPublishedEvent;
using static Magazin.Domain.FacturaModels.FacturaStates;
using static Magazin.Domain.Models.CartItemPublishedEvent;
using static Magazin.Domain.FacturaOperations;
using Magazin.Domain.CurieratModels;
using static Magazin.Domain.CurieratWorkFlow;
using System;
using static Magazin.Domain.CurieratModels.AWBStates;

namespace Magazin.Domain
{
    public class PublishFacturaWorkflow
    {
        private readonly IItemRepository itemRepository;
        private readonly IFacturaRepository facturaRepository;
        private readonly IDateLivrareRepository dateLivrareRepository;
        private readonly ILogger<PublishFacturaWorkflow> logger;

        public PublishFacturaWorkflow(IItemRepository itemRepository, ILogger<PublishFacturaWorkflow> logger, IDateLivrareRepository dateLivrareRepository, IFacturaRepository facturaRepository)
        {
            this.itemRepository = itemRepository;
            this.logger = logger;
            this.facturaRepository = facturaRepository;
            this.dateLivrareRepository = dateLivrareRepository;
        }

        public async Task<IFacturaPublishedEvent> ExecuteAsync(PublishFacturaCommand command)
        {
            Unvalidatedfactura unvalidatedItems = new Unvalidatedfactura(command.InputData.Adresa, command.InputData.TipLivrare, command.InputData.Cumparator);
            var AWB = ExecuteCurieratWorkflow();
            var result = from publishedFactura in ExecuteWorkflowAsync(new UnvalidatedFacturaState(unvalidatedItems)).ToAsync()
                         let successfulEvent = new FacturaPublishScucceededEvent(publishedFactura.Factura, AWB, command.Publisheditems)
                         select successfulEvent;

            return await result.Match(
                    Left: facturaStates => GenerateFailedEvent(facturaStates) as IFacturaPublishedEvent,
                    Right: publishedFactura => publishedFactura
                );
        }

        private async Task<Either<IFacturaStates, PublishedFacturaState>> ExecuteWorkflowAsync(UnvalidatedFacturaState unvalidatedFactura)
        {
            IFacturaStates factura = await GetFacturaValidated(unvalidatedFactura);
            factura = PublishFactura(factura);

            return factura.Match<Either<IFacturaStates, PublishedFacturaState>>(
                whenInvalidFacturaState: invalidfact => Left(invalidfact as IFacturaStates),
                whenUnvalidatedFacturaState: pubFact => Left(pubFact as IFacturaStates),
                whenFailedFactura: faildFact => Left(faildFact as IFacturaStates),
                whenValidatedFacturaState: validFact => Left(validFact as IFacturaStates),
                whenPublishedFacturaState: pubFact => Right(pubFact)
            );
        }


        private FacturaPublishFaildEvent GenerateFailedEvent(IFacturaStates facturaStates) =>
            facturaStates.Match<FacturaPublishFaildEvent>(
                whenUnvalidatedFacturaState: unvalidatedFactura => new($"Invalid state {nameof(UnvalidatedFacturaState)}"),
                whenInvalidFacturaState: invalidFactura => new(invalidFactura.Reason),
                whenValidatedFacturaState: validatedFactura => new($"Invalid state {nameof(ValidatedFacturaState)}"),
                whenFailedFactura: failedFactura =>
                {
                    logger.LogError(failedFactura.Exception, failedFactura.Exception.Message);
                    return new(failedFactura.Exception.Message);
                },
                whenPublishedFacturaState: publishedFactura => new($"Invalid state {nameof(PublishedFacturaState)}"));
    }
}
