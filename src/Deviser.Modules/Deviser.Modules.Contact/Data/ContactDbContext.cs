using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Modules.ContactForm.Data
{
    public class ContactDbContext : ModuleDbContext
    {   

        //public ContactDbContext()
        //{
        //    ModuleMetaInfo = Global.ModuleMetaInfo;
        //}

        public ContactDbContext(DbContextOptions<ContactDbContext> options)
            : base(options)
        {            
            
        }

        //public ContactDbContext(IServiceProvider serviceProvider)
        //    : base(serviceProvider)
        //{

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PageModuleId).IsRequired();
                entity.Property(e => e.Data).IsRequired();
                entity.Property(e => e.CreatedOn).IsRequired();
                entity.Property(e => e.CreatedBy);
            });
        }

        public virtual DbSet<Contact> Contact { get; set; }
    }
}
