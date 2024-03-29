﻿// <auto-generated />
using System;
using HogwartsPotions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HogwartsPotions.Migrations
{
    [DbContext(typeof(HogwartsContext))]
    [Migration("20220617130625_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Ingredient", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("PotionID")
                        .HasColumnType("bigint");

                    b.Property<long?>("RecipeID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("PotionID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Ingredient", (string)null);
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Potion", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<long?>("BrewerID")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("RecipeID")
                        .HasColumnType("bigint");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("ID");

                    b.HasIndex("BrewerID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Potion", (string)null);
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Recipe", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<long?>("BrewerID")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("BrewerID");

                    b.ToTable("Recipe", (string)null);
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Room", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Student", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"), 1L, 1);

                    b.Property<byte>("HouseType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("PetType")
                        .HasColumnType("tinyint");

                    b.Property<long?>("RoomID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("RoomID");

                    b.ToTable("Student", (string)null);
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Ingredient", b =>
                {
                    b.HasOne("HogwartsPotions.Models.Entities.Potion", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("PotionID");

                    b.HasOne("HogwartsPotions.Models.Entities.Recipe", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeID");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Potion", b =>
                {
                    b.HasOne("HogwartsPotions.Models.Entities.Student", "Brewer")
                        .WithMany()
                        .HasForeignKey("BrewerID");

                    b.HasOne("HogwartsPotions.Models.Entities.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeID");

                    b.Navigation("Brewer");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Recipe", b =>
                {
                    b.HasOne("HogwartsPotions.Models.Entities.Student", "Brewer")
                        .WithMany()
                        .HasForeignKey("BrewerID");

                    b.Navigation("Brewer");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Student", b =>
                {
                    b.HasOne("HogwartsPotions.Models.Entities.Room", "Room")
                        .WithMany("Residents")
                        .HasForeignKey("RoomID");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Potion", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Recipe", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("HogwartsPotions.Models.Entities.Room", b =>
                {
                    b.Navigation("Residents");
                });
#pragma warning restore 612, 618
        }
    }
}
