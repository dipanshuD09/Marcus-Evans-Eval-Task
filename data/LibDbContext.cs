using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.data
{
    public class LibDbContext : DbContext
    {
        public LibDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(b => b.Author)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(b => b.ISBN)
                    .IsRequired();

                entity.Property(b => b.PublishedDate)
                    .IsRequired();

                entity.Property(b => b.Genre)
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(u => u.Role)
                    .IsRequired();
            });
        }

        public DbSet<Book> books { get; set; }
        public DbSet<User> users { get; set; }
    }
}