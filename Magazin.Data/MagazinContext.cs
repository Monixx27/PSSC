using Magazin.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Magazin.Data
{
    public class MagazinContext : DbContext
    {
        public MagazinContext(DbContextOptions<MagazinContext> options) : base(options)
        {
        }


        public DbSet<ItemModel> Items { get; set; }
        public DbSet<FacturaModel> Facturi { get; set; }
        public DbSet<DateLivrareModel> DateLivrare { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemModel>().ToTable("Items").HasKey(s => s.ItemId);
            modelBuilder.Entity<FacturaModel>().ToTable("Facturi").HasKey(s => s.FacturaId);
            modelBuilder.Entity<DateLivrareModel>().ToTable("DateLivrare").HasKey(s => s.AWB);
        }
    }
}
