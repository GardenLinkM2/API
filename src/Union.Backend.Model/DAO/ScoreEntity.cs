using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Union.Backend.Model.Models;

namespace Union.Backend.Model.DAO
{
    public class ScoreEntity : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Note).IsRequired();
            builder.Property(s => s.Comment).IsRequired();
            builder.Property(s => s.Rater).IsRequired();
            builder.Property(s => s.Rated).IsRequired();


        }
    }
}
