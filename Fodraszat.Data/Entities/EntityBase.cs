using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fodraszat.Data.Entities
{
    public abstract class EntityBase 
    {
        public int Id { get; set; }
        public string? Megjegyzes { get; set; }
        public DateTime ModositasDatum { get; set; }
        public DateTime LetrehozasDatum { get; set; }
    }

    public abstract class EntityBase<TEntity> : EntityBase, IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder) { }
    }
}
