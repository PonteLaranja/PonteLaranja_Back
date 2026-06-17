using System;
using System.Collections.Generic;
using API.Domains;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

public partial class PonteLaranjaDbContext : DbContext
{
    public PonteLaranjaDbContext()
    {
    }

    public PonteLaranjaDbContext(DbContextOptions<PonteLaranjaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoTransferencia> EstadoTransferencia { get; set; }

    public virtual DbSet<Item> Item { get; set; }

    public virtual DbSet<TipoItem> TipoItem { get; set; }

    public virtual DbSet<TipoMedida> TipoMedida { get; set; }

    public virtual DbSet<TipoUnidade> TipoUnidade { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Transferencia> Transferencia { get; set; }

    public virtual DbSet<Unidade> Unidade { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoTransferencia>(entity =>
        {
            entity.HasKey(e => e.EstadoTransID).HasName("PK__EstadoTr__7A6BD6E340DC8C21");

            entity.HasIndex(e => e.EstadoTransNome, "UQ__EstadoTr__06DAC07F3F49F02A").IsUnique();

            entity.Property(e => e.EstadoTransID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EstadoTransNome)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemID).HasName("PK__Item__727E83EBA3799E1F");

            entity.ToTable(tb => tb.HasTrigger("trg_SoftDelete_Item"));

            entity.Property(e => e.ItemID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataRecebimento).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Medida).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NomeItem)
                .HasMaxLength(75)
                .IsUnicode(false);

            entity.HasOne(d => d.TipoItem).WithMany(p => p.Item)
                .HasForeignKey(d => d.TipoItemID)
                .HasConstraintName("Fk_TipoItemID");

            entity.HasOne(d => d.TipoMedida).WithMany(p => p.Item)
                .HasForeignKey(d => d.TipoMedidaID)
                .HasConstraintName("Fk_TipoMedidaID");

            entity.HasOne(d => d.Unidade).WithMany(p => p.Item)
                .HasForeignKey(d => d.UnidadeID)
                .HasConstraintName("Fk_UnidadeID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Item)
                .HasForeignKey(d => d.UsuarioID)
                .HasConstraintName("Fk_UsuarioID");
        });

        modelBuilder.Entity<TipoItem>(entity =>
        {
            entity.HasKey(e => e.TipoItemID).HasName("PK__TipoItem__FC89051B9F1A6F7A");

            entity.HasIndex(e => e.NomeTipoItem, "UQ__TipoItem__87A4476E246C5A3A").IsUnique();

            entity.Property(e => e.TipoItemID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeTipoItem)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoMedida>(entity =>
        {
            entity.HasKey(e => e.TipoMedidaID).HasName("PK__TipoMedi__B9250FC809AE17D0");

            entity.HasIndex(e => e.NomeTipoMedida, "UQ__TipoMedi__30C8758022881B18").IsUnique();

            entity.Property(e => e.TipoMedidaID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeTipoMedida)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUnidade>(entity =>
        {
            entity.HasKey(e => e.TipoUnidadeID).HasName("PK__TipoUnid__3940ED86AE5E0268");

            entity.HasIndex(e => e.NomeTipoUnidade, "UQ__TipoUnid__144BF083EABEB00C").IsUnique();

            entity.Property(e => e.TipoUnidadeID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeTipoUnidade)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.TipoUsuarioID).HasName("PK__TipoUsua__7F22C702ED22EA2F");

            entity.HasIndex(e => e.NomeTipoUsuario, "UQ__TipoUsua__C6FB90A8836561A7").IsUnique();

            entity.Property(e => e.TipoUsuarioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeTipoUsuario)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transferencia>(entity =>
        {
            entity.HasKey(e => e.TransferenciaID).HasName("PK__Transfer__E5B4F5F2BAC12617");

            entity.Property(e => e.TransferenciaID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataEnvio).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.EstadoTransferencia).WithMany(p => p.Transferencia)
                .HasForeignKey(d => d.EstadoTransferenciaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_EstadoTransferenciaID");

            entity.HasOne(d => d.Item).WithMany(p => p.Transferencia)
                .HasForeignKey(d => d.ItemID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_ItemID");

            entity.HasOne(d => d.UnidadeDestino).WithMany(p => p.TransferenciaUnidadeDestino)
                .HasForeignKey(d => d.UnidadeDestinoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_UnidadeDestinoID");

            entity.HasOne(d => d.UnidadeOrigem).WithMany(p => p.TransferenciaUnidadeOrigem)
                .HasForeignKey(d => d.UnidadeOrigemID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_UnidadeOrigemID");
        });

        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.HasKey(e => e.UnidadeID).HasName("PK__Unidade__6EE52BD914FAD027");

            entity.ToTable(tb => tb.HasTrigger("trg_SoftDelete_Unidade"));

            entity.HasIndex(e => e.NomeUnidade, "UQ__Unidade__EE30D366BA4A6922").IsUnique();

            entity.Property(e => e.UnidadeID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.CEP)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NomeUnidade)
                .HasMaxLength(75)
                .IsUnicode(false);

            entity.HasOne(d => d.TipoUnidade).WithMany(p => p.Unidade)
                .HasForeignKey(d => d.TipoUnidadeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_TipoUnidadeID");

            entity.HasOne(d => d.UsuarioResponsavel).WithMany(p => p.Unidade)
                .HasForeignKey(d => d.UsuarioResponsavelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_UsuarioResponsavelID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioID).HasName("PK__Usuario__2B3DE7983F384D10");

            entity.ToTable(tb => tb.HasTrigger("trg_SoftDelete_Usuario"));

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534C43C722D").IsUnique();

            entity.Property(e => e.UsuarioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCriacao).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(32);

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.TipoUsuarioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
