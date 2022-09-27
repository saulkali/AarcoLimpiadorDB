using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using limpiarDatos.Models.Entities;

namespace limpiarDatos.Models.Context
{
    public partial class ApiContext : DbContext
    {
        public ApiContext()
        {
        }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Descripcion> Descripcion { get; set; } = null!;
        public virtual DbSet<Marcas> Marcas { get; set; } = null!;
        public virtual DbSet<ModelSubMar> ModelSubMar { get; set; } = null!;
        public virtual DbSet<SubMarca> SubMarca { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // xd para limpiar datos
                optionsBuilder.UseSqlServer("Server= DESKTOP-QCCT1H6;Database=Aarco; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Descripcion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.DescripcionId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcionID");

                entity.Property(e => e.IdModeloSubM).HasColumnName("idModeloSubM");

                entity.HasOne(d => d.IdModeloSubMNavigation)
                    .WithMany(p => p.Descripcion)
                    .HasForeignKey(d => d.IdModeloSubM)
                    .HasConstraintName("FK__Descripci__idMod__3A81B327");
            });

            modelBuilder.Entity<Marcas>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<ModelSubMar>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdSubMarca).HasColumnName("idSubMarca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdSubMarcaNavigation)
                    .WithMany(p => p.ModelSubMar)
                    .HasForeignKey(d => d.IdSubMarca)
                    .HasConstraintName("FK__ModelSubM__idSub__37A5467C");
            });

            modelBuilder.Entity<SubMarca>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdMarcas).HasColumnName("idMarcas");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdMarcasNavigation)
                    .WithMany(p => p.SubMarca)
                    .HasForeignKey(d => d.IdMarcas)
                    .HasConstraintName("FK__SubMarca__idMarc__31EC6D26");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
