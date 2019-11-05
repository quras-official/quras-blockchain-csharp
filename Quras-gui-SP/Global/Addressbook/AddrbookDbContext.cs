using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Quras_gui_SP.Global.Addressbook
{
    internal class AddrbookDbContext : DbContext
    {
        public DbSet<Addrbook> Addrbooks { get; set; }

        private readonly string filename;

        public AddrbookDbContext(string filename)
        {
            this.filename = filename;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            SqliteConnectionStringBuilder sb = new SqliteConnectionStringBuilder();
            sb.DataSource = filename;
            optionsBuilder.UseSqlite(sb.ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Addrbook>().ToTable(nameof(Addrbook));
            modelBuilder.Entity<Addrbook>().HasKey(p => p.Address);
            modelBuilder.Entity<Addrbook>().Property(p => p.ContactName).HasColumnType("VarChar").HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Addrbook>().Property(p => p.Address).HasColumnType("VarChar").HasMaxLength(100).IsRequired();
        }
    }
}
