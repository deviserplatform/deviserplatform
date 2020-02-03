﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace Deviser.Modules.ContactForm.Data
{
    public interface IContactProvider
    {
        bool submitData(Contact contact);
    }
    public class ContactProvider: IContactProvider
    {
       
        private readonly ILogger<ContactProvider> _logger;
        private readonly DbContextOptions<ContactDbContext> _dbOptions;
        public ContactProvider(IServiceProvider serviceProvider)
        {           
            _logger = serviceProvider.GetService<ILogger<ContactProvider>>();
            _dbOptions = serviceProvider.GetService<DbContextOptions<ContactDbContext>>();
        }


        public bool submitData(Contact contact)
        {
            try
            {
                using (var context = new ContactDbContext(_dbOptions))
                {    
                    if(contact != null)
                    {
                        context.Contact.Add(contact);
                        context.SaveChanges();
                        return true;
                    }
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Error occured while posting the data to database."), ex);
               
            }
            return false;
        }
    }
}
