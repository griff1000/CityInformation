﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCorp.CityApi.Database;

namespace MyCorp.CityApi.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20190629080551_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyCorp.CityApi.Models.Database.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryName")
                        .IsRequired();

                    b.Property<DateTime>("DateEstablished");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("State");

                    b.Property<int>("TouristRating");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
