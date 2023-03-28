using CerealDB.Service;
using CerealDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CerealDB
{
    class CerealContext : DbContext
    {
        internal class Cerealdata : IEntityTypeConfiguration<Cereal>
        {
            public void Configure(EntityTypeBuilder<Cereal> builder)
            {
                _ = builder.HasData(CerealService.LoadCerialCSV().ToArray());
            }
        }
        
        internal class Imagedata : IEntityTypeConfiguration<ProductImage>
        {
            public void Configure(EntityTypeBuilder<ProductImage> builder)
            {
                string path = "C:\\Users\\KOM\\Documents\\Cerial\\Cereal Pictures";
                _ = builder.HasData(ImageService.LoadImages(path).ToArray());
            }
        }

        public CerealContext(DbContextOptions<CerealContext> options)
        : base(options) { }

        public DbSet<Cereal> Cereals => Set<Cereal>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Cerealdata());
            modelBuilder.ApplyConfiguration(new Imagedata());
        }
    }
    
}
