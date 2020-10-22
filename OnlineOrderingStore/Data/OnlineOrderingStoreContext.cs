using Microsoft.EntityFrameworkCore;
using OnlineOrderingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineOrderingStore.Data
{
    public class OnlineOrderingStoreContext : DbContext
    {
        public OnlineOrderingStoreContext(DbContextOptions<OnlineOrderingStoreContext> options):base(options)
        {

        }

        public DbSet<StoreUser> StoreUsers { get; set; }
        public DbSet<OnlineUser> OnlineUsers { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsType> GoodsTypes { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<GoodsWithOrder> GoodsWithOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //调用ModelBuild扩展方法添加种子数据
            //modelBuilder.Seed();

            modelBuilder.Entity<Goods>()
                .HasOne(g => g.GoodsType)
                .WithMany(g => g.Goods)
                .HasForeignKey(g => g.GoodsTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GoodsWithOrder>()
                .HasOne(g => g.Goods)
                .WithMany(gwo => gwo.GoodsWithOrders)
                .HasForeignKey(gwo => gwo.GoodsId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
