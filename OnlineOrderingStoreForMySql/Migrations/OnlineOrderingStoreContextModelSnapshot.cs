﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineOrderingStore.Data;

namespace OnlineOrderingStore.Migrations
{
    [DbContext(typeof(OnlineOrderingStoreContext))]
    partial class OnlineOrderingStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("OnlineOrderingStore.Models.AdminUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Account")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PassWord")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.Goods", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("GoodsTypeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("PhotoPath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("ReleaseTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("StoreUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodsTypeId");

                    b.HasIndex("StoreUserId");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.GoodsType", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GoodsTypeName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StoreUserId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("StoreUserId");

                    b.ToTable("GoodsTypes");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.GoodsWithOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("BuyCount")
                        .HasColumnType("int");

                    b.Property<bool>("Delivered")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DeliveredTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("GoodsId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("GoodsId");

                    b.HasIndex("OrderId");

                    b.ToTable("GoodsWithOrders");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.OnlineUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("varchar(11) CHARACTER SET utf8mb4")
                        .HasMaxLength(11);

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("varchar(15) CHARACTER SET utf8mb4")
                        .HasMaxLength(15);

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("OnlineUsers");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ConsigneeName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OnlineUserId")
                        .HasColumnType("int");

                    b.Property<decimal>("Pay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("Phone")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OnlineUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.ShoppingCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("BuyCount")
                        .HasColumnType("int");

                    b.Property<Guid>("GoodsID")
                        .HasColumnType("char(36)");

                    b.Property<int>("OnlineUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoodsID");

                    b.HasIndex("OnlineUserId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.StoreType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EditTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TypeName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ID");

                    b.ToTable("StoreTypes");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.StoreUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("varchar(11) CHARACTER SET utf8mb4")
                        .HasMaxLength(11);

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("varchar(15) CHARACTER SET utf8mb4")
                        .HasMaxLength(15);

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<int?>("StoreTypeID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("StoreTypeID");

                    b.ToTable("StoreUsers");
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.Goods", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.GoodsType", "GoodsType")
                        .WithMany("Goods")
                        .HasForeignKey("GoodsTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OnlineOrderingStore.Models.StoreUser", "StoreUser")
                        .WithMany("Goods")
                        .HasForeignKey("StoreUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.GoodsType", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.StoreUser", "StoreUser")
                        .WithMany("GoodsTypes")
                        .HasForeignKey("StoreUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.GoodsWithOrder", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.Goods", "Goods")
                        .WithMany("GoodsWithOrders")
                        .HasForeignKey("GoodsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OnlineOrderingStore.Models.Order", "Order")
                        .WithMany("GoodsWithOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.Order", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.OnlineUser", "OnlineUser")
                        .WithMany("Orders")
                        .HasForeignKey("OnlineUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.ShoppingCart", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.Goods", "Goods")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("GoodsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineOrderingStore.Models.OnlineUser", "OnlineUser")
                        .WithMany()
                        .HasForeignKey("OnlineUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineOrderingStore.Models.StoreUser", b =>
                {
                    b.HasOne("OnlineOrderingStore.Models.StoreType", "StoreType")
                        .WithMany("StoreUsers")
                        .HasForeignKey("StoreTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
