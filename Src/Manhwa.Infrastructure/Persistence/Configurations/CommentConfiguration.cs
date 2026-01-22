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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.CommentId).HasColumnName("comment_id");
            builder.Property(c => c.Content).HasColumnName("content").HasColumnType("text").IsRequired();

            // Audit Fields
            builder.Property(c => c.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Foreign Keys Mapping
            builder.Property(c => c.UserId).HasColumnName("user_id");
            builder.Property(c => c.StoryId).HasColumnName("story_id");
            builder.Property(c => c.ChapterId).HasColumnName("chapter_id");
            builder.Property(c => c.ParentId).HasColumnName("parent_id");

            // Relationships
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.SetNull); 

            builder.HasOne(c => c.Story)
                   .WithMany(s => s.Comments)
                   .HasForeignKey(c => c.StoryId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(c => c.Chapter)
                   .WithMany(ch => ch.Comments)
                   .HasForeignKey(c => c.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade); 

            // Self-referencing relationship (Replies)
            builder.HasOne(c => c.Parent)
                   .WithMany(p => p.Replies)
                   .HasForeignKey(c => c.ParentId)
                   .OnDelete(DeleteBehavior.SetNull); 
        }
    }
    }
