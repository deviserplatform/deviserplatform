﻿// <auto-generated />
using System;
using Deviser.Modules.ContactForm.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Deviser.Modules.ContactForm.Migrations.Postgre
{
    [DbContext(typeof(PostgreSqlContactDbContext))]
    [Migration("20210103190020_InitalSchema")]
    partial class InitalSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Deviser.Modules.ContactForm.Data.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PageModuleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("dmc_Contact");
                });
#pragma warning restore 612, 618
        }
    }
}
