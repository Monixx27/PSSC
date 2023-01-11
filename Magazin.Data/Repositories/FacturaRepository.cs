using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using Magazin.Data.IRepositories;
using Magazin.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Magazin.Data.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly MagazinContext dbContext;

        public FacturaRepository(MagazinContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<FacturaObjModel>> TryGetExistingFacturi() => async () => (await (from b in dbContext.Facturi
                                                                                              join s in dbContext.Items on b.Item equals s.ItemId
                                                                                              select new { b.FacturaId, s.ItemId, s.Name, b.Price, b.Quantity } into a
                                                                                              group a by a.FacturaId into g
                                                                                              select new { FacturaId = g.Key, Items = g.ToList() }).ToListAsync())
                                                                                            .Select(factura => new FacturaObjModel
                                                                                            {
                                                                                                FacturaId = factura.FacturaId,
                                                                                                Items = factura.Items.Select(item => new ItemModel
                                                                                                {
                                                                                                    ItemId = item.ItemId,
                                                                                                    Name = item.Name,
                                                                                                    Price = item.Price,
                                                                                                    Quantity = item.Quantity
                                                                                                }).ToList()
                                                                                            }).ToList();

        public TryAsync<List<FacturaObjModel>> TryGetFacturaById(int id) => async () => (await (from b in dbContext.Facturi
                                                                                                where b.FacturaId == id
                                                                                                join s in dbContext.Items on b.Item equals s.ItemId
                                                                                                select new { b.FacturaId, s.ItemId, s.Name, b.Price, b.Quantity } into a
                                                                                                group a by a.FacturaId into g
                                                                                                select new { FacturaId = g.Key, Items = g.ToList() }).ToListAsync())
                                                                                                .Select(factura => new FacturaObjModel
                                                                                                {
                                                                                                    FacturaId = factura.FacturaId,
                                                                                                    Items = factura.Items.Select(item => new ItemModel
                                                                                                    {
                                                                                                        ItemId = item.ItemId,
                                                                                                        Name = item.Name,
                                                                                                        Price = item.Price,
                                                                                                        Quantity = item.Quantity
                                                                                                    }).ToList()
                                                                                                }).ToList();


        public TryAsync<int> TryAddingNewFactura(FacturaObjModel factura)
        {
            var dbfactura = factura.Items.Select(item => new FacturaModel
            {
                FacturaId = factura.FacturaId,
                Quantity = item.Quantity,
                Item = item.ItemId,
                Price = item.Price
            }).ToList();

            dbContext.AddRange(dbfactura);
            return async () => await dbContext.SaveChangesAsync();
        }
    }
}
