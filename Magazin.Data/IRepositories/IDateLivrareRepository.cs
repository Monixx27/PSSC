using LanguageExt;
using Magazin.Data.Models;
using System.Collections.Generic;

namespace Magazin.Data.IRepositories
{
    public interface IDateLivrareRepository
    {
        TryAsync<int> TryAddingNewFactura(DateLivrareModel date);
        TryAsync<List<DateLivrareModel>> TryGetExistingItems();
        TryAsync<List<DateLivrareModel>> TryGetItemById(string id);
    }
}