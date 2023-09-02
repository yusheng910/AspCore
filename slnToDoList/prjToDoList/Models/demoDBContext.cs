using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjToDoList.Models;

public partial class demoDBContext : DbContext
{
    public demoDBContext()
    {
    }

    public demoDBContext(DbContextOptions<demoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tToDoItem> tToDoItems { get; set; }

    public virtual DbSet<tUser> tUsers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=107.173.111.196,1433;Initial Catalog=demoDB;Persist Security Info=True;User ID=sa;Password=Vida@1103;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tToDoItem>(entity =>
        {
            entity.HasKey(e => e.fTaskId);

            entity.ToTable("tToDoItem");

            entity.Property(e => e.fAddedDate).HasPrecision(0);
            entity.Property(e => e.fTaskContent).HasMaxLength(100);

            entity.HasOne(d => d.fUser).WithMany(p => p.tToDoItems)
                .HasForeignKey(d => d.fUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tToDoItem_tUser");
        });

        modelBuilder.Entity<tUser>(entity =>
        {
            entity.HasKey(e => e.fUserId);

            entity.ToTable("tUser");

            entity.Property(e => e.fEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.fMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.fPassword)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.fUserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
