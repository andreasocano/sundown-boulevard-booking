﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SundownBoulevard.Booking.DAL.Entities;

namespace SundownBoulevard.Booking.DAL.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    [Migration("20210613215424_Order")]
    partial class Order
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Booking", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReservationID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReservationID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.OrderLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Reservation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.Property<Guid>("UID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.TableReservation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ReservationID")
                        .HasColumnType("int");

                    b.Property<int>("TableID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("TableReservations");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Booking", b =>
                {
                    b.HasOne("SundownBoulevard.Booking.DAL.Entities.Reservation", "Reservation")
                        .WithMany()
                        .HasForeignKey("ReservationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Order", b =>
                {
                    b.HasOne("SundownBoulevard.Booking.DAL.Entities.Reservation", "Reservation")
                        .WithMany()
                        .HasForeignKey("ReservationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.OrderLine", b =>
                {
                    b.HasOne("SundownBoulevard.Booking.DAL.Entities.Order", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("SundownBoulevard.Booking.DAL.Entities.Order", b =>
                {
                    b.Navigation("OrderLines");
                });
#pragma warning restore 612, 618
        }
    }
}
