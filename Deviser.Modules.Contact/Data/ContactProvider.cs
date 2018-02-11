using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Deviser.Core.Common.DomainTypes;
using AutoMapper;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Data;

namespace Deviser.Modules.ContactForm.Data
{
    public interface IContactProvider
    {
        bool submitData(Contact contact);
    }
    public class ContactProvider: DataProviderBase, IContactProvider
    {
        private IServiceProvider _serviceProvider;
        private readonly ILogger<ContactProvider> _logger;


        public ContactProvider(IServiceProvider serviceProvider, ILifetimeScope container)
            :base(container)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<ContactProvider>>();
        }


        public bool submitData(Contact contact)
        {
            try
            {
                using (var context = new ContactDbContext())
                {
                    contact.CreatedOn = DateTime.Now;
                    context.Contact.Add(contact);
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
