using LanguageExt;
using Magazin.Data.IRepositories;
using Magazin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Data.Repositories
{
    public class DateLivrareRepository : IDateLivrareRepository
    {
        private readonly MagazinContext dbContext;

        public DateLivrareRepository(MagazinContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<DateLivrareModel>> TryGetExistingItems() => async () => await dbContext.DateLivrare.ToListAsync();

        public TryAsync<List<DateLivrareModel>> TryGetItemById(String id) => async () => await dbContext.DateLivrare.Where(data => data.FacturaId.Equals(id)).ToListAsync();

        public TryAsync<int> TryAddingNewFactura(DateLivrareModel date)
        {
            dbContext.AddRange(date);
            return async () => await dbContext.SaveChangesAsync();
        }
    }
}
