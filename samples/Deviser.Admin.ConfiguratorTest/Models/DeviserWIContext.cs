using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Deviser.Admin.ConfiguratorTest.Models
{
    public partial class DeviserWIContext : DbContext
    {
        public DeviserWIContext()
        {
        }

        public DeviserWIContext(DbContextOptions<DeviserWIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<ContentPermission> ContentPermission { get; set; }
        public virtual DbSet<ContentType> ContentType { get; set; }
        public virtual DbSet<ContentTypeProperty> ContentTypeProperty { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Layout> Layout { get; set; }
        public virtual DbSet<LayoutType> LayoutType { get; set; }
        public virtual DbSet<LayoutTypeProperty> LayoutTypeProperty { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleAction> ModuleAction { get; set; }
        public virtual DbSet<ModuleActionProperty> ModuleActionProperty { get; set; }
        public virtual DbSet<ModuleActionType> ModuleActionType { get; set; }
        public virtual DbSet<ModuleMigrationsHistory> ModuleMigrationsHistory { get; set; }
        public virtual DbSet<ModulePermission> ModulePermission { get; set; }
        public virtual DbSet<OptionList> OptionList { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<PageContent> PageContent { get; set; }
        public virtual DbSet<PageContentTranslation> PageContentTranslation { get; set; }
        public virtual DbSet<PageModule> PageModule { get; set; }
        public virtual DbSet<PagePermission> PagePermission { get; set; }
        public virtual DbSet<PageTranslation> PageTranslation { get; set; }
        public virtual DbSet<PageType> PageType { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PostTags> PostTags { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleClaim> RoleClaim { get; set; }
        public virtual DbSet<SiteSetting> SiteSetting { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DeviserWI;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Data).IsRequired();
            });

            modelBuilder.Entity<ContentPermission>(entity =>
            {
                entity.HasIndex(e => e.PageContentId);

                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.PageContent)
                    .WithMany(p => p.ContentPermission)
                    .HasForeignKey(d => d.PageContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.ContentPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ContentPermission)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ContentTypeProperty>(entity =>
            {
                entity.HasKey(e => new { e.ContentTypeId, e.PropertyId });

                entity.HasIndex(e => e.PropertyId);

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.ContentTypeProperty)
                    .HasForeignKey(d => d.ContentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.ContentTypeProperty)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Layout>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<LayoutType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<LayoutTypeProperty>(entity =>
            {
                entity.HasKey(e => new { e.LayoutTypeId, e.PropertyId });

                entity.HasIndex(e => e.PropertyId);

                entity.HasOne(d => d.LayoutType)
                    .WithMany(p => p.LayoutTypeProperty)
                    .HasForeignKey(d => d.LayoutTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.LayoutTypeProperty)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ModuleAction>(entity =>
            {
                entity.HasIndex(e => e.ModuleActionTypeId);

                entity.HasIndex(e => e.ModuleId)
                    .HasName("IX_FK_ModuleActions_Modules");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.ControllerNamespace).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.HasOne(d => d.ModuleActionType)
                    .WithMany(p => p.ModuleAction)
                    .HasForeignKey(d => d.ModuleActionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleAction)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ModuleActionProperty>(entity =>
            {
                entity.HasKey(e => new { e.ModuleActionId, e.PropertyId });

                entity.HasIndex(e => e.PropertyId);

                entity.HasOne(d => d.ModuleAction)
                    .WithMany(p => p.ModuleActionProperty)
                    .HasForeignKey(d => d.ModuleActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.ModuleActionProperty)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ModuleActionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ControlType).HasMaxLength(50);
            });

            modelBuilder.Entity<ModuleMigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId);

                entity.ToTable("__ModuleMigrationsHistory");

                entity.Property(e => e.MigrationId)
                    .HasMaxLength(150)
                    .ValueGeneratedNever();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<ModulePermission>(entity =>
            {
                entity.HasIndex(e => e.PageModuleId);

                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.PageModule)
                    .WithMany(p => p.ModulePermission)
                    .HasForeignKey(d => d.PageModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.ModulePermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ModulePermission)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OptionList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasIndex(e => e.LayoutId);

                entity.HasIndex(e => e.PageTypeId);

                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_FK_Pages_Pages");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.ThemeSrc).HasMaxLength(200);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId);
            });

            modelBuilder.Entity<PageContent>(entity =>
            {
                entity.HasIndex(e => e.ContentTypeId);

                entity.HasIndex(e => e.PageId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InheritEditPermissions)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.InheritViewPermissions)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.PageContent)
                    .HasForeignKey(d => d.ContentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageContent)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PageContentTranslation>(entity =>
            {
                entity.HasIndex(e => e.PageContentId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.PageContent)
                    .WithMany(p => p.PageContentTranslation)
                    .HasForeignKey(d => d.PageContentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PageModule>(entity =>
            {
                entity.HasIndex(e => e.ModuleActionId);

                entity.HasIndex(e => e.ModuleId)
                    .HasName("IX_FK_PageModule_Modules");

                entity.HasIndex(e => e.PageId)
                    .HasName("IX_FK_PageModule_Module");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InheritEditPermissions)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.InheritViewPermissions)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ModuleAction)
                    .WithMany(p => p.PageModule)
                    .HasForeignKey(d => d.ModuleActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.PageModule)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageModule)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PagePermission>(entity =>
            {
                entity.HasIndex(e => e.PageId);

                entity.HasIndex(e => e.PermissionId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PagePermission)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PagePermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PagePermission)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PageTranslation>(entity =>
            {
                entity.HasKey(e => new { e.PageId, e.Locale });

                entity.Property(e => e.Locale).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Keywords).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageTranslation)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PageType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<PostTags>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.TagId });

                entity.HasIndex(e => e.TagId);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.PostId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasIndex(e => e.OptionListId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.OptionList)
                    .WithMany(p => p.Property)
                    .HasForeignKey(d => d.OptionListId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaim)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SettingName).HasMaxLength(50);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.HasIndex(e => e.TagName)
                    .HasName("AK_Tags_TagName")
                    .IsUnique();

                entity.Property(e => e.TagId).ValueGeneratedNever();

                entity.Property(e => e.TagName).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaim)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId);
            });
        }
    }
}
