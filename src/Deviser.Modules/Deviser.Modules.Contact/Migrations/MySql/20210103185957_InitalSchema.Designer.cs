﻿// <auto-generated />
using System;
using Deviser.Modules.ContactForm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Deviser.Modules.ContactForm.Migrations.MySql
{
    [DbContext(typeof(MySqlContactDbContext))]
    [Migration("20210103185957_InitalSchema")]
    partial class InitalSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Deviser.Modules.ContactForm.Data.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("PageModuleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("dmc_Contact");
                });
#pragma warning restore 612, 618
        }
    }
}
