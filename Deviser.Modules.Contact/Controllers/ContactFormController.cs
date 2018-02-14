using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.DataProviders;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Deviser.Modules.ContactForm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.Modules.ContactForm.Controllers
{
    [Module("Contact")]
    public class ContactFormController : ModuleController
    {
        private ILogger<ContactFormController> logger;

        IContactProvider ContactProvider;

        IScopeService scopeService;

        IPageManager pageManager;

        IUserProvider userProvider;

        public ContactFormController(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetService<ILogger<ContactFormController>>();
            ContactProvider = serviceProvider.GetService<IContactProvider>();
            scopeService = serviceProvider.GetService<IScopeService>();
            pageManager = serviceProvider.GetService<IPageManager>();
            userProvider = serviceProvider.GetService<IUserProvider>();

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Submit([FromBody]Contact contact)
        {
            try
            {  
                if(contact != null)
                {
                    var userName = HttpContext.User.Identity.Name;
                    contact.CreatedBy = userProvider.GetUser(userName);
                    contact.CreatedOn = DateTime.Now;
                    var result = ContactProvider.submitData(contact);
                    if (result)
                        return Ok();
                }        
                
                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occured while posting the message."), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
