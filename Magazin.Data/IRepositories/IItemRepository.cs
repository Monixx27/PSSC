using LanguageExt;
using Magazin.Data.Models;
using System.Collections.Generic;

namespace Magazin.Data.IRepositories
{
    public interface IItemRepository
    {
        TryAsync<List<ItemModel>> TryGetExistingItems();
        TryAsync<List<ItemModel>> TryGetItemById(string id);
        TryAsync<Unit> TryUpdateQuantity(List<ItemModel> extItems);
    }
}