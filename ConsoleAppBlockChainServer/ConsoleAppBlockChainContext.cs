using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleAppBlockChain
{
    public class ConsoleAppBlockChainContext : DbContext
    {
        public DbSet<Block> Blocks { get; set; }

        public ConsoleAppBlockChainContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-CKC1K66;Database=BlockChain;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>(BlockConfigure);

            modelBuilder.Entity<Block>().OwnsOne(b => b.User);
            modelBuilder.Entity<Block>().OwnsOne(b => b.Transaction);
        }
        
        private void BlockConfigure(EntityTypeBuilder<Block> builder)
        {
            builder.ToTable("blocks", schema: "bcn");

            builder.Property(b => b.Version).HasColumnName("version");
            builder.Property(b => b.CreatedOn).HasColumnName("date_created").HasColumnType("date");
            builder.Property(b => b.Hash).HasColumnName("block_hash");
            builder.Property(b => b.PreviousHash).HasColumnName("block_previous_hash");
        }
    }
}