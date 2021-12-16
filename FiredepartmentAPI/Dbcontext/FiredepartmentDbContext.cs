using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


using System.ComponentModel.DataAnnotations;
using FiredepartmentAPI.DbModels;

namespace FiredepartmentAPI.Dbcontext
{
    public class FiredepartmentDbContext: DbContext
    {

        public DbSet<FireitemModel> FireitemsTable { get; set; }
        public DbSet<InventoryItemModel> InventoryItemsTable { get; set; }
        public DbSet<InventoryEventModel> InventoryEventTable { get; set; }
        public DbSet<StatusChangeModel> StatusChangeTable { get; set; }
        public DbSet<PriorityModel> PriorityTable { get; set; }
        public DbSet<PlaceModel> PlaceTable { get; set; }
        public DbSet<editinforecord> EditinforecordTable { get; set; }

        public FiredepartmentDbContext(DbContextOptions<FiredepartmentDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FireitemModel>().HasOne(p => p.PriorityRef).WithMany(p => p.FireitemList).HasForeignKey(k=>k.StoreId);
            modelBuilder.Entity<InventoryItemModel>().HasOne(p => p.FireitemsRef).WithMany(p => p.InventoryItemList).HasForeignKey(k => k.ItemId);
            modelBuilder.Entity<InventoryItemModel>().HasOne(p => p.InventoryEventRef).WithMany(p => p.InventoryItemList).HasForeignKey(k => k.EventId);
            modelBuilder.Entity<StatusChangeModel>().HasOne(p => p.FireitemRef).WithMany(p => p.lendFixList).HasForeignKey(k => k.ItemId);
            modelBuilder.Entity<InventoryEventModel>().HasOne(p => p.PlaceRef).WithMany(p => p.InventoryEventList).HasForeignKey(k => k.PlaceId);
            modelBuilder.Entity<StatusChangeModel>().HasOne(p => p.PlaceRef).WithMany(p => p.StatusChangeList).HasForeignKey(k => k.PlaceId);
            modelBuilder.Entity<PriorityModel>().HasOne(p => p.PlaceRef).WithMany(p => p.PriorityList).HasForeignKey(k => k.PlaceId);
        
          
        }










    }
}
