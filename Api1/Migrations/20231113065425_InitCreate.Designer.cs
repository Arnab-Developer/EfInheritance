﻿// <auto-generated />
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api1.Migrations
{
    [DbContext(typeof(Api1Context))]
    [Migration("20231113065425_InitCreate")]
    partial class InitCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Model.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnimalType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Animal");

                    b.HasDiscriminator<string>("AnimalType").HasValue("Animal");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Model.Cat", b =>
                {
                    b.HasBaseType("Model.Animal");

                    b.HasDiscriminator().HasValue("Cat");
                });

            modelBuilder.Entity("Model.Dog", b =>
                {
                    b.HasBaseType("Model.Animal");

                    b.HasDiscriminator().HasValue("Dog");
                });
#pragma warning restore 612, 618
        }
    }
}
