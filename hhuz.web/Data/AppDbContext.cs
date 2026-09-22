using hhuz.Models;
using Microsoft.EntityFrameworkCore;

namespace hhuz.web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Profiles> Profiles { get; set; }
    public DbSet<Cvs> Cvs { get; set; }
    public DbSet<Projects> Projects { get; set; }

    public DbSet<Attributes> Attributes { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<AttributeOptions> AttributeOptions { get; set; }
    public DbSet<CandidateAttributeValues> CandidateAttributeValues { get; set; }

    public DbSet<Positions> Positions { get; set; }
    public DbSet<PositionAttributes> PositionAttributes { get; set; }
    public DbSet<PositionProjectTags> PositionProjectTags { get; set; }
    public DbSet<AccessRules> AccessRules { get; set; }

    public DbSet<Tags> Tags { get; set; }
    public DbSet<ProjectTags> ProjectTags { get; set; }

    public DbSet<DiscussionPosts> DiscussionPosts { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<ExternalLogins> ExternalLogins { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users 1 : 1 Profiles
        modelBuilder.Entity<Profiles>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<Profiles>(p => p.UserId);

        modelBuilder.Entity<Profiles>()
            .HasIndex(p => p.UserId)
            .IsUnique();
        
        //Users 1:M Cvs
        modelBuilder.Entity<Cvs>()
            .HasOne(cv=>cv.User)
            .WithMany(u=>u.Cvs)
            .HasForeignKey(cv=>cv.UserId);
        
        //Position 1:M Cvs
        modelBuilder.Entity<Cvs>()
            .HasOne(cv => cv.Position)
            .WithMany(p => p.Cvs)
            .HasForeignKey(cv => cv.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //User 1:M Projects
        modelBuilder.Entity<Projects>()
            .HasOne(project => project.User)
            .WithMany(user => user.Projects)
            .HasForeignKey(project => project.UserId);
        
        //User 1:M CandidateAttributeValues
        modelBuilder.Entity<CandidateAttributeValues>()
            .HasOne(value => value.User)
            .WithMany(user => user.CandidateAttributeValues)
            .HasForeignKey(value => value.UserId);
       
        //Attribute 1:M CandidateAttributeValues
        modelBuilder.Entity<CandidateAttributeValues>()
            .HasOne(value => value.Attribute)
            .WithMany(attribute => attribute.CandidateAttributeValues)
            .HasForeignKey(value => value.AttributeId);
        
        //AttributeOptions 1:M CandidateAttributeValues
        modelBuilder.Entity<CandidateAttributeValues>()
            .HasOne(a => a.AttributeOption)
            .WithMany()
            .HasForeignKey(value => value.AttributeOptionId)
            .OnDelete(DeleteBehavior.SetNull);
        
        //category 1:M Attribute
        modelBuilder.Entity<Attributes>()
            .HasOne(attribute => attribute.Category)
            .WithMany(category => category.Attributes)
            .HasForeignKey(attribute => attribute.CategoryId);
        
        
        //Attribute 1:M AttributeOptions
        modelBuilder.Entity<AttributeOptions>()
            .HasOne(option => option.Attribute)
            .WithMany(attribute => attribute.AttributeOptions)
            .HasForeignKey(option => option.AttributeId);
        
        //Positions 1:M AccessRules
        modelBuilder.Entity<AccessRules>()
            .HasOne(rule => rule.Position)
            .WithMany(position => position.AccessRules)
            .HasForeignKey(rule => rule.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //Position 1:M PositionAttributes
        modelBuilder.Entity<PositionAttributes>()
            .HasOne(pa => pa.Position)
            .WithMany(position => position.PositionAttributes)
            .HasForeignKey(pa => pa.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
       //Attribute 1:M PositionAttribute 
        modelBuilder.Entity<PositionAttributes>()
            .HasOne(pa => pa.Attribute)
            .WithMany(attribute => attribute.PositionAttributes)
            .HasForeignKey(pa => pa.AttributeId);
        
        //Position 1:M PositionProjectTags
        modelBuilder.Entity<PositionProjectTags>()
            .HasOne(ppt => ppt.Position)
            .WithMany(position => position.PositionProjectTags)
            .HasForeignKey(ppt => ppt.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        //Tag 1:M PositionProjectTags
        modelBuilder.Entity<PositionProjectTags>()
            .HasOne(ppt => ppt.Tag)
            .WithMany(tag => tag.PositionProjectTags)
            .HasForeignKey(ppt => ppt.TagId);
        
        //Project 1:M ProjectTags
        modelBuilder.Entity<ProjectTags>()
            .HasOne(pt => pt.Project)
            .WithMany(project => project.ProjectTags)
            .HasForeignKey(pt => pt.ProjectId);
        
        
        //Tag 1:M ProjectTags
        modelBuilder.Entity<ProjectTags>()
            .HasOne(pt => pt.Tag)
            .WithMany(tag => tag.ProjectTags)
            .HasForeignKey(pt => pt.TagId);
        
        //Users 1:M DiscussionPosts
        modelBuilder.Entity<DiscussionPosts>()
            .HasOne(post => post.User)
            .WithMany(user => user.DiscussionPosts)
            .HasForeignKey(post => post.UserId);
        
        
        //Position 1:M DiscussionPosts
        modelBuilder.Entity<DiscussionPosts>()
            .HasOne(post => post.Position)
            .WithMany(position => position.DiscussionPosts)
            .HasForeignKey(post => post.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        //Users 1:M Likes
        modelBuilder.Entity<Likes>()
            .HasOne(like => like.User)
            .WithMany(user => user.Likes)
            .HasForeignKey(like => like.UserId);
        
        //Cv 1:M Likes
        modelBuilder.Entity<Likes>()
            .HasOne(like => like.Cv)
            .WithMany(cv => cv.Like)
            .HasForeignKey(like => like.CvId);
        
        
        modelBuilder.Entity<Likes>()
            .HasIndex(x => new { x.UserId, x.CvId })
            .IsUnique();
        
        //User 1:M ExternalLogins
        modelBuilder.Entity<ExternalLogins>()
            .HasOne(login => login.User)
            .WithMany(user => user.ExternalLogins)
            .HasForeignKey(login => login.UserId);
        
        
        /////////////////////////////////////////////////////////////////////
        //to keep unique and validations
        //Users
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Users>()
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();
        
        //Attribute
        modelBuilder.Entity<Attributes>()
            .HasIndex(a => a.Name)
            .IsUnique();

        modelBuilder.Entity<Attributes>()
            .Property(a => a.Name)
            .HasMaxLength(150)
            .IsRequired();
        
        //CandidateAttributeValues
        modelBuilder.Entity<CandidateAttributeValues>()
            .HasIndex(x => new { x.UserId, x.AttributeId })
            .IsUnique();
        
        //ProjectTags 
        modelBuilder.Entity<ProjectTags>()
            .HasIndex(x => new { x.ProjectId, x.TagId })
            .IsUnique();
        
        //PositionProjectTags
        modelBuilder.Entity<PositionProjectTags>()
            .HasIndex(x => new { x.PositionId, x.TagId })
            .IsUnique();
        
        //PositionAttributes
        modelBuilder.Entity<PositionAttributes>()
            .HasIndex(x => new { x.PositionId, x.AttributeId })
            .IsUnique();
        
        //AttributeOptions
        modelBuilder.Entity<AttributeOptions>()
            .HasIndex(x => new { x.AttributeId, x.Value })
            .IsUnique();
        
        //LoginProvide
        modelBuilder.Entity<ExternalLogins>()
            .HasIndex(x => new { x.LoginProvider, x.Name })
            .IsUnique();
        
        //optimis locking
        modelBuilder.Entity<Cvs>()
            .Property(x => x.Version)
            .IsConcurrencyToken();
        
        modelBuilder.Entity<Positions>()
            .Property(x => x.Version)
            .IsConcurrencyToken();
        
        //TAGS
        modelBuilder.Entity<Tags>()
            .HasIndex(t => t.Name)
            .IsUnique();
        modelBuilder.Entity<Tags>()
            .Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        
        modelBuilder.Entity<Categories>()
            .HasIndex(c => c.Name)
            .IsUnique();
        modelBuilder.Entity<Categories>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        modelBuilder.Entity<Positions>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
    
        

}