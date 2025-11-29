using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Modelos;

public partial class DbDojankwonContext : DbContext
{
    public DbDojankwonContext()
    {
    }

    public DbDojankwonContext(DbContextOptions<DbDojankwonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<DetallePrestamo> DetallePrestamos { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Examen> Examenes { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Rango> Rangos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=dbDojankwon;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ARTICULO__3214EC07E7180EB2");

            entity.ToTable("ARTICULO");

            entity.Property(e => e.Nombre).HasMaxLength(20);
        });

        modelBuilder.Entity<DetallePrestamo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETALLE___3214EC07016EE2F5");

            entity.ToTable("DETALLE_PRESTAMO");

            entity.HasOne(d => d.IdArticuloNavigation).WithMany(p => p.DetallePrestamos)
                .HasForeignKey(d => d.IdArticulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLE_P__IdArt__4F7CD00D");

            entity.HasOne(d => d.IdPrestamoNavigation).WithMany(p => p.DetallePrestamos)
                .HasForeignKey(d => d.IdPrestamo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLE_P__IdPre__5070F446");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTUDIAN__3214EC075D3460F1");

            entity.ToTable("ESTUDIANTE");

            entity.HasIndex(e => e.Telefono, "UQ__ESTUDIAN__4EC504808583C1BA").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__ESTUDIAN__60695A19DF0989E4").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Apellidos).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(30);
            entity.Property(e => e.Direccion).HasMaxLength(50);
            entity.Property(e => e.Eps)
                .HasMaxLength(50)
                .HasColumnName("EPS");
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombres).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(10);

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ESTUDIANT__IdGru__440B1D61");

            entity.HasOne(d => d.IdRangoNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdRango)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ESTUDIANT__IdRan__4316F928");
        });

        modelBuilder.Entity<Examen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EXAMEN__3214EC0750DF50F5");

            entity.ToTable("EXAMEN");

            entity.Property(e => e.EstudianteId).HasMaxLength(10);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.NotaFinal).HasColumnName("Nota_Final");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Examenes)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EXAMEN__Estudian__47DBAE45");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GRUPO__3214EC0773DB46DF");

            entity.ToTable("GRUPO");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PAGOS__3214EC07E7EAFBAA");

            entity.ToTable("PAGOS");

            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FechaPago).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IdEstudiante).HasMaxLength(10);

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PAGOS__IdEstudia__5441852A");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PRESTAMO__3214EC07227F9F64");

            entity.ToTable("PRESTAMO");

            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.EstudianteId).HasMaxLength(10);
            entity.Property(e => e.FechaDevolucion).HasColumnName("Fecha_Devolucion");
            entity.Property(e => e.FechaPrestamo).HasColumnName("Fecha_Prestamo");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRESTAMO__Estudi__4CA06362");
        });

        modelBuilder.Entity<Rango>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RANGO__3214EC0795CC73AE");

            entity.ToTable("RANGO");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Cc).HasName("PK__USUARIO__32149A65B2E34159");

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Telefono, "UQ__USUARIO__4EC504802F6E89E6").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__USUARIO__60695A190910623E").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__USUARIO__C9F28456766DFD81").IsUnique();

            entity.Property(e => e.Cc)
                .HasMaxLength(10)
                .HasColumnName("CC");
            entity.Property(e => e.Apellidos).HasMaxLength(20);
            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(30);
            entity.Property(e => e.Direccion).HasMaxLength(50);
            entity.Property(e => e.Nombres).HasMaxLength(20);
            entity.Property(e => e.Rol).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(10);
            entity.Property(e => e.UserName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
