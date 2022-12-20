using LanguageExt;
using static LanguageExt.Prelude;
using Magazin.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Magazin.Data.Repositories
{
    class ItemRepository
    {
        private readonly MagazinContext dbContext;

        public ItemRepository(MagazinContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<ItemModel>> TryGetExistingItems() => async () => await dbContext.Items.ToListAsync();

        public TryAsync<Unit> TryUpdateQuantity(List<ItemModel> extItems) => async () =>
        {
            var dbItems = await dbContext.Items.ToListAsync();
            await Task.Run(() => {
                    foreach (var dbItem in dbItems)
                    {
                        foreach (var extItem in extItems)
                        {
                            if (extItem.ItemId == dbItem.ItemId)
                            {
                                dbItem.Price = dbItem.Price - extItem.Price;
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
