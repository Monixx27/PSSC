using System;
using LanguageExt;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static LanguageExt.Prelude;
using Microsoft.Extensions.Logging;
using Magazin.Data.IRepositories;
using static Magazin.Domain.Models.CartItemPublishedEvent;
using Magazin.Domain.Models;
using static Magazin.Domain.Models.CartItems;
using Magazin.Data.Models;
using static Magazin.Domain.ItemsOperations;
using Magazin.Dto.Events;
using Magazin.Dto.Models;

namespace Magazin.Domain
{
    public class PublishCartWorkflow
    {
        private readonly IItemRepository itemRepository;
        private readonly ILogger<PublishCartWorkflow> logger;

        public PublishCartWorkflow(IItemRepository itemRepository, ILogger<PublishCartWorkflow> logger)
        {
            this.itemRepository = itemRepository;
            this.logger = logger;
        }

        public async Task<ICartItemsPublishedEvent> ExecuteAsync(PublishCartCommand command)
        {
            UnvalidatedCartItems unvalidatedItems = new UnvalidatedCartItems(command.InputCartItems);

            var result = from items in itemRepository.TryGetExistingItems()
                                          .ToEither(ex => new FailedCartItems(unvalidatedItems.ItemsList, ex) as ICartItems)
                         let checkItemExists = (Func<string, Option<ValidatedItem>>)(item => CheckItemExists(items.Select(MapItemModelToValidatedItem).AsEnumerable(), item))
                         from publishedItems in ExecuteWorkflowAsync(unvalidatedItems, checkItemExists).ToAsync()
                         from saveResult in itemRepository.TryUpdateQuantity(publishedItems.ItemsList.Select(MapCalculatedItemToItemModel).ToList())
                                          .ToEither(ex => new FailedCartItems(unvalidatedItems.ItemsList, ex) as ICartItems)
                         let pubItems = publishedItems.ItemsList.Select(item => new PublishedCartItem(item.item,item.QuantityxPrice))
                         let successfulEvent = new CartItemsPublishScucceededEvent(pubItems, publishedItems.PublishedDate, CalculateCartTotalPrice(pubItems))
                         select successfulEvent;

            return await result.Match(
                    Left: cartitems => GenerateFailedEvent(cartitems) as ICartItemsPublishedEvent,
                    Right: publishedItems => publishedItems
                );
        }

        private async Task<Either<ICartItems, PublishedCartItems>> ExecuteWorkflowAsync(UnvalidatedCartItems unvalidatedGrades, Func<string, Option<ValidatedItem>> checkItemExists)
        {
            ICartItems items = await ValidateCartItems(checkItemExists, unvalidatedGrades);
            items = CalculateItemPrices(items);
            items = PublishCartItems(items);

            return items.Match<Either<ICartItems, PublishedCartItems>>(
                whenUnvalidatedCartItems: unvalidatedItems => Left(unvalidatedItems as ICartItems),
                whenCalculatedCartItems: calculatedItems => Left(calculatedItems as ICartItems),
                whenInvalidCartItems: invalidItems => Left(invalidItems as ICartItems),
                whenFailedCartItems: failedItems => Left(failedItems as ICartItems),
                whenValidatedCartItems: validatedItems => Left(validatedItems as ICartItems),
                whenPublishedCartItems: publishedItems => Right(publishedItems)
            );
        }

        private Option<ValidatedItem> CheckItemExists(IEnumerable<ValidatedItem> items, string itemId)
        {
            if (items.Any(s => s.item.ItemId == itemId))
            {
                return Some(items.First(s => s.item.ItemId.Equals(itemId)));
            }
            else
            {
                return None;
            }
        }

        private static ValidatedItem MapItemModelToValidatedItem(ItemModel item) => new ValidatedItem(
           item: new Item(item.ItemId, item.Name, item.Quantity, item.Price)
           );

        private static ItemModel MapCalculatedItemToItemModel(CalculatedItem item) => new ItemModel() {
        
            ItemId=item.item.ItemId,
            Name = item.item.Name,
            Quantity = item.item.Quantity,
            Price = item.item.Price
        };

        private CartItemsPublishFaildEvent GenerateFailedEvent(ICartItems cartItems) =>
            cartItems.Match<CartItemsPublishFaildEvent>(
                whenUnvalidatedCartItems: unvalidatedCartItems => new($"Invalid state {nameof(UnvalidatedCartItems)}"),
                whenInvalidCartItems: invalidCartItems => new(invalidCartItems.Reason),
                whenValidatedCartItems: validatedCartItems => new($"Invalid state {nameof(ValidatedCartItems)}"),
                whenFailedCartItems: failedCartItems =>
                {
                    logger.LogError(failedCartItems.Exception, failedCartItems.Exception.Message);
                    return new(failedCartItems.Exception.Message);
                },
                whenCalculatedCartItems: calculatedCartItems => new($"Invalid state {nameof(CalculatedCartItems)}"),
                whenPublishedCartItems: publishedCartItems => new($"Invalid state {nameof(PublishedCartItems)}"));
    }
}
