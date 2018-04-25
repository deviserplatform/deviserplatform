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
using System.Linq;

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

        private const string _modulePath = "~/Modules/Contact/Views/ContactForm";

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
            var moduleProperties = GetModuleProperties(PageModuleId);
            string viewTemplate = moduleProperties.Get("cf_view_template")?.ToString();
            var viewPath = $"{_modulePath}/ViewTemplates/{viewTemplate}.cshtml";
            return View(viewPath);
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> Submit([FromBody]Contact contact)
        {
            try
            {
                if (contact != null)
                {
                    var userName = HttpContext.User.Identity.Name;
                    contact.CreatedBy = _userProvider.GetUser(userName);
                    contact.CreatedOn = DateTime.Now;
                    var result = _contactProvider.submitData(contact);
                    System.Collections.Generic.ICollection<Property> pageModuleProperties = GetModuleProperties(contact.PageModuleId);

                    string adminEmail = pageModuleProperties.Get("cf_admin_email")?.ToString();
                    string fromEmail = pageModuleProperties.Get("from")?.ToString();
                    string subject = pageModuleProperties.Get("subject")?.ToString();
                    string adminEmailTemplate = pageModuleProperties.Get("cf_admin_email_template")?.ToString();
                    string contactEmailTemplate = pageModuleProperties.Get("cf_contact_email_template")?.ToString();
                    dynamic data = JObject.Parse(contact.Data);

                    //Send Email to Admin
                    string message = ViewExtensions.RenderPartial($"{_modulePath}/EmailTemplates/{adminEmailTemplate}.cshtml", data, HttpContext);
                    await _emailsender.SendEmailAsync(adminEmail, subject, message, fromEmail);

                    //Send Email to Contact
                    message = ViewExtensions.RenderPartial($"{_modulePath}/EmailTemplates/{contact}.cshtml", data, HttpContext);
                    await _emailsender.SendEmailAsync(adminEmail, subject, message, fromEmail);

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

        private System.Collections.Generic.ICollection<Property> GetModuleProperties(Guid pageModuleId)
        {
            var pageModule = _moduleManager.GetPageModule(pageModuleId);
            var pageModuleProperties = pageModule.Properties;
            //Copy property options from master data
            if (pageModule.Module.Properties != null && pageModule.Module.Properties.Count > 0)
            {
                foreach (var prop in pageModule.Module.Properties)
                {
                    var propValue = pageModule.Properties.FirstOrDefault(p => p.Id == prop.Id);
                    if (propValue != null)
                    {
                        propValue.OptionListId = prop.OptionListId;
                        propValue.OptionList = prop.OptionList;
                    }
                }
            }

            return pageModuleProperties;
        }
    }
}
