using Manhwa.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class ExpActionConfiguration : IEntityTypeConfiguration<ExpAction>
    {
        public void Configure(EntityTypeBuilder<ExpAction> builder)
        {
            builder.ToTable("exp_action");

            builder.HasKey(e => e.ExpActionId);
            builder.Property(e => e.ExpActionId)
                   .HasColumnName("exp_action_id")
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.IdAct)
                   .HasColumnName("id_act")
                   .IsRequired();

            builder.Property(e => e.Act)
                   .HasColumnName("act")
                   .HasColumnType("text")
                   .IsRequired();

            builder.Property(e => e.ExpValue)
                   .HasColumnName("exp_value")
                   .IsRequired();
        }
    }
}
