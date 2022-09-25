﻿// <auto-generated />
using DiscordBot.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace discordBot.Migrations
{
    [DbContext(typeof(SqliteContext))]
    [Migration("20220920001804_SqliteMigration")]
    partial class SqliteMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("DiscordBot.Models.BankAccount", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Guns")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Horses")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Tigers")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Whores")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("DiscordBot.Models.Tab", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Drinks")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Shots")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Tabs");
                });
#pragma warning restore 612, 618
        }
    }
}