﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PanaderiaCore.Data;

#nullable disable

namespace PanaderiaCore.Migrations
{
    [DbContext(typeof(PanaderiaDbContext))]
    partial class PanaderiaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PanaderiaCore.Models.Compra", b =>
                {
                    b.Property<int>("IdFactura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFactura"));

                    b.Property<string>("ClienteEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdFactura");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("PanaderiaCore.Models.DetalleCompra", b =>
                {
                    b.Property<int>("IdDetalle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalle"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IdFactura")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductoNombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdDetalle");

                    b.HasIndex("IdFactura");

                    b.HasIndex("ProductoNombre");

                    b.ToTable("DetalleCompras");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Inventario", b =>
                {
                    b.Property<string>("ProductoNombre")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<DateTime>("UltimaActualizacion")
                        .HasColumnType("datetime2");

                    b.HasKey("ProductoNombre");

                    b.ToTable("Inventarios");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Producto", b =>
                {
                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Nombre");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Reabastecimiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductoNombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ProductoNombre");

                    b.ToTable("Reabastecimientos");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Usuario", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("PanaderiaCore.Models.DetalleCompra", b =>
                {
                    b.HasOne("PanaderiaCore.Models.Compra", "Compra")
                        .WithMany("Detalles")
                        .HasForeignKey("IdFactura")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PanaderiaCore.Models.Producto", "Producto")
                        .WithMany("Detalles")
                        .HasForeignKey("ProductoNombre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Compra");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Inventario", b =>
                {
                    b.HasOne("PanaderiaCore.Models.Producto", "Producto")
                        .WithOne("Inventario")
                        .HasForeignKey("PanaderiaCore.Models.Inventario", "ProductoNombre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Reabastecimiento", b =>
                {
                    b.HasOne("PanaderiaCore.Models.Producto", "Producto")
                        .WithMany("Reabastecimientos")
                        .HasForeignKey("ProductoNombre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Compra", b =>
                {
                    b.Navigation("Detalles");
                });

            modelBuilder.Entity("PanaderiaCore.Models.Producto", b =>
                {
                    b.Navigation("Detalles");

                    b.Navigation("Inventario");

                    b.Navigation("Reabastecimientos");
                });
#pragma warning restore 612, 618
        }
    }
}
