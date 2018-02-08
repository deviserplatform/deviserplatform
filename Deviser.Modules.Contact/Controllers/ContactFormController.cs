using System;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Deviser.Core.Common.DomainTypes;
using Microsoft.AspNetCore.Mvc;
using Deviser.Core.Data.DataProviders;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Deviser.Modules.ContactForm.Controllers
{
    [Module("Contact")]
    public class ContactFormController : ModuleController
    {
        private ILogger<ContactFormController> logger;

        IContactProvider ContactProvider;

        IScopeService scopeService;

        IPageManager pageManager;

        public ContactFormController(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetService<ILogger<ContactFormController>>();
            ContactProvider = serviceProvider.GetService<IContactProvider>();
            scopeService = serviceProvider.GetService<IScopeService>();
            pageManager = serviceProvider.GetService<IPageManager>();

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Submit([FromBody]dynamic contactData)
        {
            try
            {
                Contact contact = new Contact();
                contact.PageModuleId = scopeService.ModuleContext.PageModuleId;
                contact.Data = contactData;
                var user = HttpContext.User.Identity.Name;
                var result = ContactProvider.submitData(contact);
                if (result)
                    return Ok();
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
