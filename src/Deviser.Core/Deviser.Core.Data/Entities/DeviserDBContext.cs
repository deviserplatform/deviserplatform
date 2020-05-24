using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using Deviser.Core.Data.Entities;

namespace Deviser.Core.Data
{
    public partial class DeviserDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
        UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public DeviserDbContext(DbContextOptions<DeviserDbContext> options)
            : base(options)
        {
        }

        //public DeviserDbContext(DbContextOptions options)
        //    : base(options)
        //{

        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DeviserWI;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.HasMany(e => e.UserClaims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.UserLogins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.UserRoles)
                .WithOne(u=>u.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable("User");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                entity.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                entity.ToTable("UserRole");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(e => e.UserRoles)
                .WithOne(ur=>ur.Role)
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

                entity.ToTable("Role");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaim");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaim");
            });

            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    entity.HasOne(d => d.Role).WithMany(r => r.UserRoles).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Restrict);
            //});

            //modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            //{
                
            //});

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogin");
            });

            modelBuilder.Entity<Layout>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.CreatedDate);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastModifiedDate);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ModuleView>(entity =>
            {
                entity.HasIndex(e => e.ModuleId).HasName("IX_FK_ModuleViews_Modules");

                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.ControllerNamespace).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.IsDefault).IsRequired().HasDefaultValue(false);

                entity.HasOne(d => d.ModuleViewType).WithMany(p => p.ModuleView).HasForeignKey(d => d.ModuleViewTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Module).WithMany(p => p.ModuleView).HasForeignKey(d => d.ModuleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModuleViewType>(entity =>
            {
                entity.Property(e => e.ControlType).HasMaxLength(50);
            });

            modelBuilder.Entity<Page>((System.Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Page>>)((Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Page> entity) =>
            {
                entity.HasIndex(e => e.ParentId).HasName("IX_FK_Pages_Pages");

                entity.Property(e => e.CreatedDate);

                entity.Property(e => e.EndDate);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.IsSystem).HasDefaultValue(false);

                entity.Property(e => e.LastModifiedDate);

                entity.Property(e => e.ThemeSrc).HasMaxLength(200);

                entity.Property(e => e.StartDate);

                entity.HasOne(d => d.Layout).WithMany(p => p.Page).HasForeignKey(d => d.LayoutId);

                entity.HasOne(d => d.PageType).WithMany(p => p.Page).HasForeignKey(d => d.PageTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Parent).WithMany(p => p.ChildPage).HasForeignKey(d => d.ParentId);

                entity.Ignore(e => e.IsBreadCrumb);
            }));


            modelBuilder.Entity<PageContent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.InheritViewPermissions).HasDefaultValue(true);

                entity.Property(e => e.InheritEditPermissions).HasDefaultValue(true);

                entity.Ignore(e => e.HasEditPermission);

                entity.Property(e => e.LastModifiedDate);

                entity.HasOne(d => d.Page).WithMany(p => p.PageContent).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ContentType).WithMany(p => p.PageContents).HasForeignKey(d => d.ContentTypeId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<PageContentTranslation>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.CreatedDate);

                entity.Property(e => e.CultureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.LastModifiedDate);

                entity.HasOne(d => d.PageContent).WithMany(p => p.PageContentTranslation).HasForeignKey(d => d.PageContentId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PageModule>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasIndex(e => e.ModuleId).HasName("IX_FK_PageModule_Modules");

                entity.HasIndex(e => e.PageId).HasName("IX_FK_PageModule_Module");

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.InheritViewPermissions).HasDefaultValue(true);

                entity.Property(e => e.InheritEditPermissions).HasDefaultValue(true);

                entity.Ignore(e => e.HasEditPermission);

                entity.HasOne(d => d.Module).WithMany(p => p.PageModule).HasForeignKey(d => d.ModuleId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Page).WithMany(p => p.PageModule).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ModuleView).WithMany(p => p.PageModules).HasForeignKey(d => d.ModuleViewId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PageTranslation>(entity =>
            {
                entity.HasKey(e => new { e.PageId, e.Locale });

                entity.Property(e => e.Locale).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Keywords).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.URL).HasMaxLength(255);

                entity.HasOne(d => d.Page).WithMany(p => p.PageTranslation).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PageType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AdminPage>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.HasOne(a => a.Page).WithOne(p => p.AdminPage).HasForeignKey<AdminPage>(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Module).WithMany(p => p.AdminPage).HasForeignKey(d => d.ModuleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.Property(e => e.SettingName).HasMaxLength(50);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.LastModifiedDate);

            });

            modelBuilder.Entity<LayoutType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<ContentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsActive).HasDefaultValue(true);

            });
            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Ignore(e => e.Value);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.OptionList).WithMany(p => p.Properties).HasForeignKey(d => d.OptionListId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OptionList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<LayoutTypeProperty>(entity =>
            {
                entity.HasOne(d => d.LayoutType).WithMany(p => p.LayoutTypeProperties).HasForeignKey(d => d.LayoutTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Property).WithMany(p => p.LayoutTypeProperties).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.Restrict);

                entity.HasKey(d => new { d.LayoutTypeId, d.PropertyId });
            });

            modelBuilder.Entity<ContentTypeProperty>(entity =>
            {
                entity.HasOne(d => d.ContentType).WithMany(p => p.ContentTypeProperties).HasForeignKey(d => d.ContentTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Property).WithMany(p => p.ContentTypeProperties).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.Restrict);

                entity.HasKey(d => new { d.ContentTypeId, d.PropertyId });
            });

            modelBuilder.Entity<PagePermission>(entity =>
            {
                entity.HasOne(d => d.Page).WithMany(p => p.PagePermissions).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Permission).WithMany(p => p.PagePermissions).HasForeignKey(d => d.PermissionId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Role).WithMany(p => p.PagePermissions).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModulePermission>(entity =>
            {
                entity.HasOne(d => d.PageModule).WithMany(p => p.ModulePermissions).HasForeignKey(d => d.PageModuleId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Permission).WithMany(p => p.ModulePermissions).HasForeignKey(d => d.PermissionId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Role).WithMany(p => p.ModulePermissions).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ContentPermission>(entity =>
            {
                entity.HasOne(d => d.PageContent).WithMany(p => p.ContentPermissions).HasForeignKey(d => d.PageContentId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Permission).WithMany(p => p.ContentPermissions).HasForeignKey(d => d.PermissionId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Role).WithMany(p => p.ContentPermissions).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModuleViewProperty>(entity =>
            {
                entity.HasOne(d => d.ModuleView).WithMany(p => p.ModuleViewProperties).HasForeignKey(d => d.ModuleViewId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Property).WithMany(p => p.ModuleViewProperties).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.Restrict);

                entity.HasKey(d => new { ModuleViewId = d.ModuleViewId, d.PropertyId });
            });
        }

        public virtual DbSet<AdminPage> AdminPage { get; set; }
        public virtual DbSet<ContentPermission> ContentPermission { get; set; }
        public virtual DbSet<ContentType> ContentType { get; set; }
        public virtual DbSet<ContentTypeProperty> ContentTypeProperty { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Layout> Layout { get; set; }
        public virtual DbSet<LayoutType> LayoutType { get; set; }
        public virtual DbSet<LayoutTypeProperty> LayoutTypeProperty { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleView> ModuleView { get; set; }
        public virtual DbSet<ModuleViewType> ModuleViewType { get; set; }
        public virtual DbSet<ModulePermission> ModulePermission { get; set; }
        public virtual DbSet<ModuleViewProperty> ModuleViewProperty { get; set; }
        public virtual DbSet<OptionList> OptionList { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<PageContent> PageContent { get; set; }
        public virtual DbSet<PageModule> PageModule { get; set; }
        public virtual DbSet<PageType> PageType { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<PagePermission> PagePermission { get; set; }
        public virtual DbSet<PageTranslation> PageTranslation { get; set; }
        public virtual DbSet<PageContentTranslation> PageContentTranslation { get; set; }
        public virtual DbSet<SiteSetting> SiteSetting { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}