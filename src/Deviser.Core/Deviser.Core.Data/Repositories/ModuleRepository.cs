using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Common.DomainTypes;
using Microsoft.Extensions.Logging;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Module = Deviser.Core.Common.DomainTypes.Module;

namespace Deviser.Core.Data.Repositories
{
    public interface IModuleRepository : IRepositoryBase
    {
        List<Module> Get();
        Module Get(Guid moduleId);
        ModuleAction GetModuleAction(Guid moduleActionId);
        List<ModuleAction> GetModuleActions();
        List<ModuleActionType> GetModuleActionType();
        List<ModuleAction> GetEditModuleActions(Guid moduleId);
        Module Get(string moduleName);
        Module GetModuleByPageModuleId(Guid pageModuleId);
        Module Create(Module dbModule);
        Module Update(Module dbModule);
        ModuleAction CreateModuleAction(ModuleAction moduleAction);
        ModuleAction UpdateModuleAction(ModuleAction moduleAction);
    }

    public class ModuleRepository : RepositoryBase, IModuleRepository
    {
        //Logger
        private readonly ILogger<ModuleRepository> _logger;

        //Constructor
        public ModuleRepository(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<ModuleRepository>>();
        }

        //Custom Field Declaration
        public List<Module> Get()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Module
                        .Include(m => m.ModuleAction).ThenInclude(mp => mp.ModuleActionProperties).ThenInclude(p => p.Property).ThenInclude(p => p.OptionList)
                        .ToList();

                    return Mapper.Map<List<Module>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public ModuleAction GetModuleAction(Guid moduleActionId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ModuleAction
                        .FirstOrDefault(m => m.Id == moduleActionId);

                    return Mapper.Map<ModuleAction>(result);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public List<ModuleAction> GetModuleActions()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ModuleAction
                        .Include(mp => mp.ModuleActionProperties).ThenInclude(p => p.Property).ThenInclude(p => p.OptionList)
                        .Where(m => m.ModuleActionType.ControlType.ToLower() == "view" && m.Module.IsActive) //Selecting View Actions Only
                        .OrderBy(ma => ma.DisplayName)
                        .ToList();

                    return Mapper.Map<List<ModuleAction>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ModuleActions", ex);
            }
            return null;
        }

        public List<ModuleActionType> GetModuleActionType()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ModuleActionType
                        .OrderBy(cd => cd.Id)
                        .ToList();

                    return Mapper.Map<List<ModuleActionType>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Module Action Type", ex);
            }
            return null;
        }

        public List<ModuleAction> GetEditModuleActions(Guid moduleId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ModuleAction
                        .Include(mp => mp.ModuleActionProperties).ThenInclude(p => p.Property)
                        .Where(m => m.ModuleId == moduleId && m.ModuleActionType.ControlType.ToLower() == "edit") //Selecting View Actions Only
                        .ToList();

                    return Mapper.Map<List<ModuleAction>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ModuleActions", ex);
            }
            return null;
        }

        public Module Get(Guid moduleId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Module
                              .Where(e => e.Id == moduleId)
                              .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) // ("ModuleActions.ModuleActionType")                              
                              .FirstOrDefault();

                    return Mapper.Map<Module>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public Module Get(string moduleName)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Module
                              .Where(e => e.Name == moduleName)
                              .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) //("ModuleActions.ModuleActionType")                              
                              .FirstOrDefault();

                    return Mapper.Map<Module>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public Module GetModuleByPageModuleId(Guid pageModuleId)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.Module
                              .Where(e => e.PageModule.Any(pm => pm.Id == pageModuleId))
                              .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) //("ModuleActions.ModuleActionType")
                              .FirstOrDefault();

                    return Mapper.Map<Module>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public Module Create(Module module)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbModule = Mapper.Map<Entities.Module>(module);
                    dbModule.CreatedDate = dbModule.LastModifiedDate = DateTime.Now;
                    var result = context.Module.Add(dbModule).Entity;
                    context.SaveChanges();
                    return Mapper.Map<Module>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }
        public Module Update(Module module)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbModule = Mapper.Map<Entities.Module>(module);
                    var moduleActions = dbModule.ModuleAction;
                    dbModule.ModuleAction = null;
                    foreach (var moduleAction in moduleActions)
                    {

                        if (context.ModuleAction.Any(pc => pc.Id == moduleAction.Id))
                        {                           

                            UpdateModuleActionProperties(context, moduleAction);

                            //content exist, therefore update the content 
                            context.ModuleAction.Update(moduleAction);
                        }
                        else
                        {
                            moduleAction.ModuleId = dbModule.Id;
                            context.ModuleAction.Add(moduleAction);
                        }
                    }
                    //No need to delte a row from dbo.ModuleAction since the key s referred in dbo.pagemodule.

                    //var toDelete = context.ModuleAction.Where(dbModuleAction => dbModuleAction.ModuleId == dbModule.Id &&
                    //!moduleActions.Any(moduleAction => moduleAction.Id != dbModuleAction.Id)).ToList();

                    //context.ModuleAction.RemoveRange(toDelete);

                    var result = context.Module.Update(dbModule).Entity;
                    context.SaveChanges();
                    return Mapper.Map<Module>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        public ModuleAction CreateModuleAction(ModuleAction moduleAction)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbmoduleAction = Mapper.Map<Entities.ModuleAction>(moduleAction);
                    var result = context.ModuleAction.Add(dbmoduleAction).Entity;
                    context.SaveChanges();
                    return Mapper.Map<ModuleAction>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }

        public ModuleAction UpdateModuleAction(ModuleAction moduleAction)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbmoduleAction = Mapper.Map<Entities.ModuleAction>(moduleAction);
                    var result = context.ModuleAction.Update(dbmoduleAction).Entity;

                    UpdateModuleActionProperties(context, dbmoduleAction);

                    context.SaveChanges();
                    return Mapper.Map<ModuleAction>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        private void UpdateModuleActionProperties(DeviserDbContext context, Entities.ModuleAction moduleAction)
        {   
            if (moduleAction.ModuleActionProperties != null && moduleAction.ModuleActionProperties.Count > 0)
            {

                var currentTypeProperties = context.ModuleActionProperty.Where(ctp => ctp.ModuleActionId == moduleAction.Id).ToList();

                var toRemoveFromClient = moduleAction.ModuleActionProperties.Where(clientProp => context.ModuleActionProperty.Any(dbProp =>
                 clientProp.ModuleActionId == dbProp.ModuleActionId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                List<Entities.ModuleActionProperty> toRemoveFromDb = null;

                if (currentTypeProperties != null && currentTypeProperties.Count > 0)
                {
                    toRemoveFromDb = currentTypeProperties.Where(dbProp => !moduleAction.ModuleActionProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                }

                if (toRemoveFromClient != null && toRemoveFromClient.Count > 0)
                {
                    foreach (var contentTypeProp in toRemoveFromClient)
                    {
                        //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                        moduleAction.ModuleActionProperties.Remove(contentTypeProp);
                    }
                }

                if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                {
                    //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                    context.ModuleActionProperty.RemoveRange(toRemoveFromDb);
                }


                if (moduleAction.ModuleActionProperties != null && moduleAction.ModuleActionProperties.Count > 0)
                {
                    foreach (var moduleProp in moduleAction.ModuleActionProperties)
                    {
                        moduleProp.Property = null;
                        context.ModuleActionProperty.Add(moduleProp);
                    }
                }
            }
        }

    }

}//End namespace
