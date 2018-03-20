using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.Core.Library.Sites;
using Deviser.Modules.ContactForm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Deviser.Core.Library.Messaging;
using Newtonsoft.Json;
using System;

namespace Deviser.Modules.ContactForm.Controllers
{
    [Module("Contact")]
    public class ContactFormController : ModuleController
    {
        private ILogger<ContactFormController> _logger;

        IContactProvider _contactProvider;

        IScopeService _scopeService;

        IPageManager _pageManager;

        IUserRepository _userProvider;

        IEmailSender _emailsender;

        public ContactFormController(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<ContactFormController>>();
            _contactProvider = serviceProvider.GetService<IContactProvider>();
            _scopeService = serviceProvider.GetService<IScopeService>();
            _pageManager = serviceProvider.GetService<IPageManager>();
            _userProvider = serviceProvider.GetService<IUserRepository>();
            _emailsender = serviceProvider.GetService<IEmailSender>();
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
                    contact.CreatedBy = _userProvider.GetUser(userName);
                    contact.CreatedOn = DateTime.Now;
                    var result = _contactProvider.submitData(contact);

                    var message = JsonConvert.DeserializeObject(contact.Data).ToString();              
                    string subject = "Contact Form";
                  
                    string email ="kowsikanakataj@gmail.com";
                    _emailsender.SendEmailAsync(email, subject, message);
                    if (result)
                        return Ok();
                }        
                
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error occured while posting the message."), ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
