﻿// <auto-generated />
using System;
using BatchFileWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BatchFileWebAPI.Migrations
{
    [DbContext(typeof(BatchFileDBContext))]
    [Migration("20210507085007_AddModelsToDB")]
    partial class AddModelsToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BatchFileWebAPI.Models.AccessControl", b =>
                {
                    b.Property<int>("AclPKID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("BatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReadGroups")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReadUsers")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AclPKID");

                    b.HasIndex("BatchId")
                        .IsUnique();

                    b.ToTable("AccessControls");
                });

            modelBuilder.Entity("BatchFileWebAPI.Models.Attributes", b =>
                {
                    b.Property<int>("AttributePkID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("BatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AttributePkID");

                    b.HasIndex("BatchId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("BatchFileWebAPI.Models.BatchFile", b =>
                {
                    b.Property<Guid>("BatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("BatchPublishedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("BusinessUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ExpiryDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BatchId");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("BatchFileWebAPI.Models.AccessControl", b =>
                {
                    b.HasOne("BatchFileWebAPI.Models.BatchFile", "BatchFile")
                        .WithOne("AccessControl")
                        .HasForeignKey("BatchFileWebAPI.Models.AccessControl", "BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BatchFile");
                });

            modelBuilder.Entity("BatchFileWebAPI.Models.Attributes", b =>
                {
                    b.HasOne("BatchFileWebAPI.Models.BatchFile", "BatchFile")
                        .WithMany("Attributes")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BatchFile");
                });

            modelBuilder.Entity("BatchFileWebAPI.Models.BatchFile", b =>
                {
                    b.Navigation("AccessControl");

                    b.Navigation("Attributes");
                });
#pragma warning restore 612, 618
        }
    }
}