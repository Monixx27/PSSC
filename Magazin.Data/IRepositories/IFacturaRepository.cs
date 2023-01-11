using LanguageExt;
using Magazin.Data.Models;
using System.Collections.Generic;

namespace Magazin.Data.IRepositories
{
    public interface IFacturaRepository
    {
        TryAsync<int> TryAddingNewFactura(FacturaObjModel factura);
        TryAsync<List<FacturaObjModel>> TryGetExistingFacturi();
        TryAsync<List<FacturaObjModel>> TryGetFacturaById(int id);
    }
}