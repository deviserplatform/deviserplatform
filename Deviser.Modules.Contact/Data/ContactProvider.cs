using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Autofac;
using Deviser.Core.Common.DomainTypes;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data;

namespace Deviser.Modules.ContactForm.Data
{
    public interface IContactProvider: IDataProviderBase
    {
        bool submitData(Contact contact);
    }
    public class ContactProvider: DataProviderBase, IContactProvider
    {
        private readonly ILogger<ContactProvider> _logger;


        public ContactProvider(ILifetimeScope container)
         : base(container)
        {
            _logger = container.Resolve<ILogger<ContactProvider>>();
        }


        public bool submitData(Contact contact)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbContact = Mapper.Map<Core.Data.Entities.Contact>(contact);
                    dbContact.CreatedOn = DateTime.Now;
                    context.Contact.Add(dbContact);
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while posting the data to database."), ex);
                return false;
            }
           
        }
    }
}
