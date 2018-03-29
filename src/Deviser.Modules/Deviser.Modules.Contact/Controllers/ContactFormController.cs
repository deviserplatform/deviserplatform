using Autofac;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Deviser.Core.Library.Controllers;
using Deviser.Core.Library.Extensions;
using Deviser.Core.Library.Messaging;
using Deviser.Core.Library.Modules;
using Deviser.Core.Library.Services;
using Deviser.Modules.ContactForm.Data;
using Deviser.Core.Library.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Deviser.Modules.ContactForm.Controllers
{
    [Module("Contact")]
    public class ContactFormController : ModuleController
    {
        private ILogger<ContactFormController> _logger;

        private readonly IContactProvider _contactProvider;
        private readonly IScopeService _scopeService;
        private readonly IModuleManager _moduleManager;
        private readonly IUserRepository _userProvider;
        private readonly IEmailSender _emailsender;
        private readonly IViewRenderService _viewRenderService;

        public ContactFormController(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<ContactFormController>>();
            _contactProvider = serviceProvider.GetService<IContactProvider>();
            _scopeService = serviceProvider.GetService<IScopeService>();
            _moduleManager = serviceProvider.GetService<IModuleManager>();
            _userProvider = serviceProvider.GetService<IUserRepository>();
            _emailsender = serviceProvider.GetService<IEmailSender>();
            _viewRenderService = serviceProvider.GetService<IViewRenderService>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> Submit([FromBody]Contact contact)
        {
            try
            {  
                if(contact != null)
                {
                    var userName = HttpContext.User.Identity.Name;
                    contact.CreatedBy = _userProvider.GetUser(userName);
                    contact.CreatedOn = DateTime.Now;
                    var result = _contactProvider.submitData(contact);

                    var pageModule = _moduleManager.GetPageModule(contact.PageModuleId);
                    var pageModuleProperties = pageModule.Properties;

                    string email = pageModuleProperties.Get("to")?.ToString();
                    string fromEmail = pageModuleProperties.Get("from")?.ToString();
                    string subject = pageModuleProperties.Get("subject")?.ToString();

                    
                    string templatePath = "~/Modules/Contact/Views/ContactForm/";
                    dynamic data = JObject.Parse(contact.Data);
                    string message = ViewExtensions.RenderPartial($"{templatePath}ContactTemplate.cshtml", data, HttpContext);

                    await _emailsender.SendEmailAsync(email, subject, message, fromEmail);
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
