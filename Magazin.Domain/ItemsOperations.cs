using Magazin.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Magazin.Domain.Models.CartItems;
using System.Threading.Tasks;

namespace Magazin.Domain
{
    public static class ItemsOperations
    {
        public static Task<ICartItems> ValidateCartItems(Func<string, Option<ValidatedItem>> checkItemExists, UnvalidatedCartItems cartItems) =>
            cartItems.ItemsList
                      .Select(ValidateCartItem(checkItemExists))
                      .Aggregate(CreateEmptyValatedGradesList().ToAsync(), ReduceValidGrades)
                      .MatchAsync(
                            Right: validatedItems => new ValidatedCartItems(validatedItems),
                            LeftAsync: errorMessage => Task.FromResult((ICartItems)new InvalidCartItems(cartItems.ItemsList, errorMessage))
                      );

        private static Func<UnValidatedItem, EitherAsync<string, ValidatedItem>> ValidateCartItem(Func<string, Option<ValidatedItem>> checkItemExists) =>
            unvalidatedItem => ValidateCartItem(checkItemExists, unvalidatedItem);

        private static EitherAsync<string, ValidatedItem> ValidateCartItem(Func<string, Option<ValidatedItem>> checkItemExists, UnValidatedItem unvalidatedItem) =>
            from Itemid in Item.TryParseId(unvalidatedItem.id)
                                   .ToEitherAsync($"Invalid cart item ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
            from ItemReturned in checkItemExists(Itemid)
                                   .ToEitherAsync($"Invalid cart item ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
            from ItemQty in Item.TryParseQty(unvalidatedItem.quantity, ItemReturned.item.Quantity)
                                   .ToEitherAsync($"Invalid quantity ({unvalidatedItem.id}, {unvalidatedItem.quantity})")
            select (new ValidatedItem(new Item(Itemid, ItemReturned.item.Name, ItemQty, ItemReturned.item.Price)));

        private static Either<string, List<ValidatedItem>> CreateEmptyValatedGradesList() =>
            Right(new List<ValidatedItem>());

        private static EitherAsync<string, List<ValidatedItem>> ReduceValidGrades(EitherAsync<string, List<ValidatedItem>> acc, EitherAsync<string, ValidatedItem> next) =>
            from list in acc
            from nextItem in next
            select list.AppendValidItem(nextItem);

        private static List<ValidatedItem> AppendValidItem(this List<ValidatedItem> list, ValidatedItem validItem)
        {
            list.Add(validItem);
            return list;
        }

        public static ICartItems CalculateItemPrices(ICartItems cartItems) => cartItems.Match(
            whenUnvalidatedCartItems: unvalidaTedItem => unvalidaTedItem,
            whenInvalidCartItems: invalidItem => invalidItem,
            whenFailedCartItems: failedItem => failedItem,
            whenCalculatedCartItems: calculatedItem => calculatedItem,
            whenPublishedCartItems: publishedItem => publishedItem,
            whenValidatedCartItems: CalculateItemPrice
        );

        private static ICartItems CalculateItemPrice(ValidatedCartItems validCartItems) =>
            new CalculatedCartItems(validCartItems.ItemsList
                                                    .Select(CalculateItemPrices)
                                                    .ToList()
                                                    .AsReadOnly());

        private static CalculatedItem CalculateItemPrices(ValidatedItem validItem) =>
            new CalculatedItem(validItem.item, validItem.item.Price*validItem.item.Quantity);

        
        public static ICartItems PublishCartItems(ICartItems cartItems) => cartItems.Match(
            whenUnvalidatedCartItems: unvalidaTedItem => unvalidaTedItem,
            whenInvalidCartItems: invalidItem => invalidItem,
            whenFailedCartItems: failedItem => failedItem,
            whenValidatedCartItems: validatedItem => validatedItem,
            whenPublishedCartItems: publishedItem => publishedItem,
            whenCalculatedCartItems: GenerateExport);

        private static ICartItems GenerateExport(CalculatedCartItems calculatedItem) =>
            new PublishedCartItems(calculatedItem.ItemsList,
                                    calculatedItem.ItemsList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedItem item) =>
            export.AppendLine($"{item.item.ItemId}, {item.item.Name}, {item.item.Price}, {item.item.Quantity}, {item.QuantityxPrice}");


        public static decimal? CalculateCartTotalPrice(IEnumerable<PublishedCartItem> cartItems) => cartItems.Sum(item => item.QuantityxPrice);
    }
}
