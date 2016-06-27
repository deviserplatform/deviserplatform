using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Deviser.Core.Data.Entities
{
    public partial class DeviserDBContext : IdentityDbContext<User, Role, string>
    {


        public DeviserDBContext(DbContextOptions<DeviserDBContext> options)
            : base(options)
        {

        }

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
                entity.ToTable("User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaim");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaim");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRole");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogin");
            });



            modelBuilder.Entity<Layout>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.IsActive).HasDefaultValue(true);

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
                entity.HasIndex(e => e.ModuleId).HasName("IX_FK_ModuleActions_Modules");

                entity.Property(e => e.ActionName).HasMaxLength(50);

                entity.Property(e => e.ControllerName).HasMaxLength(50);

                entity.Property(e => e.ControllerNamespace).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.IsDefault).IsRequired().HasDefaultValue(false);

                entity.HasOne(d => d.ModuleActionType).WithMany(p => p.ModuleAction).HasForeignKey(d => d.ModuleActionTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Module).WithMany(p => p.ModuleAction).HasForeignKey(d => d.ModuleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModuleActionType>(entity =>
            {
                entity.Property(e => e.ControlType).HasMaxLength(50);
            });

            modelBuilder.Entity<Page>((System.Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Page>>)((Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Page> entity) =>
            {
                entity.HasIndex(e => e.ParentId).HasName("IX_FK_Pages_Pages");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.Property(e => e.IsSystem).HasDefaultValue(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SkinSrc).HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Layout).WithMany(p => p.Page).HasForeignKey(d => d.LayoutId);

                entity.HasOne(d => d.Parent).WithMany((System.Linq.Expressions.Expression<System.Func<Page, System.Collections.Generic.IEnumerable<Page>>>)(p => p.ChildPage)).HasForeignKey(d => d.ParentId);

                entity.Ignore(e => e.IsActive);
                entity.Ignore(e => e.IsBreadCrumb);
            }));


            modelBuilder.Entity<PageContent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Page).WithMany(p => p.PageContent).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ContentType).WithMany(p => p.PageContents).HasForeignKey(d => d.ContentTypeId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<PageContentTranslation>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CultureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.PageContent).WithMany(p => p.PageContentTranslation).HasForeignKey(d => d.PageContentId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PageModule>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasIndex(e => e.ModuleId).HasName("IX_FK_PageModule_Modules");

                entity.HasIndex(e => e.PageId).HasName("IX_FK_PageModule_Module");

                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                entity.HasOne(d => d.Module).WithMany(p => p.PageModule).HasForeignKey(d => d.ModuleId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Page).WithMany(p => p.PageModule).HasForeignKey(d => d.PageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ModuleAction).WithMany(p => p.PageModules).HasForeignKey(d => d.ModuleActionId).OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.Property(e => e.SettingName).HasMaxLength(50);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            });

            modelBuilder.Entity<LayoutType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Ignore(e => e.AllowedTypes);
            });

            modelBuilder.Entity<ContentType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.ContentDataType).WithMany(p => p.ContentTypes).HasForeignKey(d => d.ContentDataTypeId).OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Ignore(e => e.Value);

                entity.HasOne(d => d.PropertyOptionList).WithMany(p => p.Properties).HasForeignKey(d => d.PropertyOptionListId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PropertyOptionList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ContentDataType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<LayoutTypeProperty>(entity =>
            {
                entity.HasOne(d => d.LayoutType).WithMany(p => p.LayoutTypeProperties).HasForeignKey(d => d.LayoutTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Property).WithMany(p => p.LayoutTypeProperties).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ContentTypeProperty>(entity =>
            {
                entity.HasOne(d => d.ContentType).WithMany(p => p.ContentTypeProperties).HasForeignKey(d => d.ConentTypeId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Property).WithMany(p => p.ContentTypeProperties).HasForeignKey(d => d.PropertyId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.diagram_id)
                    .HasName("PK__sysdiagr__C2B05B617A77B34C");

                entity.HasIndex(e => new { e.principal_id, e.name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnType("sysname");
            });
        }

        public virtual DbSet<Layout> Layout { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleAction> ModuleAction { get; set; }
        public virtual DbSet<ModuleActionType> ModuleActionType { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<PageContent> PageContent { get; set; }
        public virtual DbSet<PageModule> PageModule { get; set; }
        public virtual DbSet<PageTranslation> PageTranslation { get; set; }
        public virtual DbSet<PageContentTranslation> PageContentTranslation { get; set; }
        public virtual DbSet<SiteSetting> SiteSetting { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LayoutType> LayoutType { get; set; }
        public virtual DbSet<ContentType> ContentType { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<PropertyOptionList> PropertyOptionList { get; set; }
        public virtual DbSet<ContentDataType> ContentDataType { get; set; }
        public virtual DbSet<LayoutTypeProperty> LayoutTypeProperty { get; set; }
        public virtual DbSet<ContentTypeProperty> ContentTypeProperty { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        //public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}