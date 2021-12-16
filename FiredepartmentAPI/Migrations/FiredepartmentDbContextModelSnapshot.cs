﻿// <auto-generated />
using System;
using FiredepartmentAPI.Dbcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FiredepartmentAPI.Migrations
{
    [DbContext(typeof(FiredepartmentDbContext))]
    partial class FiredepartmentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FiredepartmentAPI.DbModels.FireitemModel", b =>
                {
                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("InventoryStatus")
                        .HasColumnType("int");

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PresentStatus")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("postscript")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemId");

                    b.HasIndex("StoreId");

                    b.ToTable("FireitemsTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.InventoryEventModel", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InventoryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlaceId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventId");

                    b.HasIndex("PlaceId");

                    b.ToTable("InventoryEventTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.InventoryItemModel", b =>
                {
                    b.Property<int>("InventoryItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("StatusAfter")
                        .HasColumnType("int");

                    b.Property<int>("StatusBefore")
                        .HasColumnType("int");

                    b.HasKey("InventoryItemId");

                    b.HasIndex("EventId");

                    b.HasIndex("ItemId");

                    b.ToTable("InventoryItemsTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.PlaceModel", b =>
                {
                    b.Property<int>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PlaceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("todaysend")
                        .HasColumnType("bit");

                    b.HasKey("PlaceId");

                    b.ToTable("PlaceTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.PriorityModel", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PlaceId")
                        .HasColumnType("int");

                    b.Property<int>("PriorityNum")
                        .HasColumnType("int");

                    b.Property<string>("SubArea")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StoreId");

                    b.HasIndex("PlaceId");

                    b.ToTable("PriorityTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.StatusChangeModel", b =>
                {
                    b.Property<int>("StatusChangId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Beforechange")
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PlaceId")
                        .HasColumnType("int");

                    b.Property<string>("Postscript")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusChangId");

                    b.HasIndex("ItemId");

                    b.HasIndex("PlaceId");

                    b.ToTable("StatusChangeTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.editinforecord", b =>
                {
                    b.Property<int>("editid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("itemid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("newname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("newstore")
                        .HasColumnType("int");

                    b.Property<string>("oldname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("oldstore")
                        .HasColumnType("int");

                    b.HasKey("editid");

                    b.ToTable("EditinforecordTable");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.FireitemModel", b =>
                {
                    b.HasOne("FiredepartmentAPI.DbModels.PriorityModel", "PriorityRef")
                        .WithMany("FireitemList")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PriorityRef");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.InventoryEventModel", b =>
                {
                    b.HasOne("FiredepartmentAPI.DbModels.PlaceModel", "PlaceRef")
                        .WithMany("InventoryEventList")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceRef");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.InventoryItemModel", b =>
                {
                    b.HasOne("FiredepartmentAPI.DbModels.InventoryEventModel", "InventoryEventRef")
                        .WithMany("InventoryItemList")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FiredepartmentAPI.DbModels.FireitemModel", "FireitemsRef")
                        .WithMany("InventoryItemList")
                        .HasForeignKey("ItemId");

                    b.Navigation("FireitemsRef");

                    b.Navigation("InventoryEventRef");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.PriorityModel", b =>
                {
                    b.HasOne("FiredepartmentAPI.DbModels.PlaceModel", "PlaceRef")
                        .WithMany("PriorityList")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceRef");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.StatusChangeModel", b =>
                {
                    b.HasOne("FiredepartmentAPI.DbModels.FireitemModel", "FireitemRef")
                        .WithMany("lendFixList")
                        .HasForeignKey("ItemId");

                    b.HasOne("FiredepartmentAPI.DbModels.PlaceModel", "PlaceRef")
                        .WithMany("StatusChangeList")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FireitemRef");

                    b.Navigation("PlaceRef");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.FireitemModel", b =>
                {
                    b.Navigation("InventoryItemList");

                    b.Navigation("lendFixList");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.InventoryEventModel", b =>
                {
                    b.Navigation("InventoryItemList");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.PlaceModel", b =>
                {
                    b.Navigation("InventoryEventList");

                    b.Navigation("PriorityList");

                    b.Navigation("StatusChangeList");
                });

            modelBuilder.Entity("FiredepartmentAPI.DbModels.PriorityModel", b =>
                {
                    b.Navigation("FireitemList");
                });
#pragma warning restore 612, 618
        }
    }
}
