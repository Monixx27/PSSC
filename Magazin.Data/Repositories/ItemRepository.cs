using LanguageExt;
using static LanguageExt.Prelude;
using Magazin.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Magazin.Data.IRepositories;

namespace Magazin.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly MagazinContext dbContext;

        public ItemRepository(MagazinContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<ItemModel>> TryGetExistingItems() => async () => await dbContext.Items.ToListAsync();

        public TryAsync<List<ItemModel>> TryGetItemById(String id) => async () => await dbContext.Items.Where(item => item.ItemId.Equals(id)).ToListAsync();

        public TryAsync<Unit> TryUpdateQuantity(List<ItemModel> extItems) => async () =>
        {
            var dbItems = await dbContext.Items.ToListAsync();
            await Task.Run(() =>
            {
                foreach (var dbItem in dbItems)
                {
                    foreach (var extItem in extItems)
                    {
                        if (extItem.ItemId == dbItem.ItemId)
                        {
                            if (dbItem.Quantity - extItem.Quantity > 0)
                                dbItem.Quantity = dbItem.Quantity - extItem.Quantity;
                            else
                                dbItem.Quantity = 0;
                        }
                    }
                }
            });

            dbContext.UpdateRange(dbItems);
            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
