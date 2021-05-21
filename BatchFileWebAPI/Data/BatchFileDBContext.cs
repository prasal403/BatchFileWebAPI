using BatchFileWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Data
{
    public class BatchFileDBContext:DbContext
    {
        public DbSet<BatchFile> Batches { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<AccessControl> AccessControls { get; set; }
        public DbSet<BatchFilesMetaData> BatchFilesMetaData { get; set; }
        public BatchFileDBContext(DbContextOptions<BatchFileDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverterReadUsers = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<AccessControl>().Property(nameof(AccessControl.ReadUsers)).HasConversion(splitStringConverterReadUsers);
            var splitStringConverterReadGroups = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            builder.Entity<AccessControl>().Property(nameof(AccessControl.ReadGroups)).HasConversion(splitStringConverterReadGroups);
        }
    }
}
