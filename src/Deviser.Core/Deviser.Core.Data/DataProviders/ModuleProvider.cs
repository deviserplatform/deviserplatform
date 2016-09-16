using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Module = Deviser.Core.Data.Entities.Module;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public interface IModuleProvider
    {
        List<Module> Get();
        Module Get(Guid moduleId);
        ModuleAction GetModuleAction(Guid moduleActionId);
        List<ModuleAction> GetModuleActions();
        List<ModuleActionType> GetModuleActionType();
        List<ModuleAction> GetEditModuleActions(Guid moduleId);
        Module Get(string moduleName);
        Module Create(Module module);
        Module Update(Module module);
        ModuleAction CreateModuleAction(ModuleAction moduleAction); 
        ModuleAction UpdateModuleAction(ModuleAction moduleAction);
    }

    public class ModuleProvider : DataProviderBase, IModuleProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;

        //Constructor
        public ModuleProvider(ILifetimeScope container)
            : base(container)
        {
            logger = container.Resolve<ILogger<LayoutProvider>>();
        }

        //Custom Field Declaration
        public List<Module> Get()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<Module> returnData = context.Module
                        .Include(m => m.ModuleAction)//.ThenInclude(ma=>ma.ModuleActionType)
                        
                        .ToList();

                    return new List<Module>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public ModuleAction GetModuleAction(Guid moduleActionId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ModuleAction
                        .Where(m => m.Id == moduleActionId)
                        .FirstOrDefault();
                    return returnData;
                    
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public List<ModuleAction> GetModuleActions()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<ModuleAction> returnData = context.ModuleAction
                        .Include(ma => ma.Module)//.ThenInclude(ma=>ma.ModuleActionType)
                        .Where(m => m.ModuleActionType.ControlType.ToLower() == "view") //Selecting View Actions Only
                        .OrderBy(ma=>ma.DisplayName)
                        .ToList();

                    return new List<ModuleAction>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting ModuleActions", ex);
            }
            return null;
        }

        public List<ModuleActionType> GetModuleActionType()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ModuleActionType
                        .OrderBy(cd => cd.Id)
                        .ToList();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Module Action Type", ex);
            }
            return null;
        }

        public List<ModuleAction> GetEditModuleActions(Guid moduleId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    IEnumerable<ModuleAction> returnData = context.ModuleAction
                        .Include(ma => ma.Module)//.ThenInclude(ma=>ma.ModuleActionType)
                        .Where(m => m.ModuleId== moduleId && m.ModuleActionType.ControlType.ToLower() == "edit") //Selecting View Actions Only
                        .ToList();

                    return new List<ModuleAction>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting ModuleActions", ex);
            }
            return null;
        }

        public Module Get(Guid moduleId)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.Module
                              .Where(e => e.Id == moduleId)
                              .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) // ("ModuleActions.ModuleActionType")
                              .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public Module Get(string moduleName)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.Module
                              .Where(e => e.Name == moduleName)
                              .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) //("ModuleActions.ModuleActionType")
                              .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }
        public Module Create(Module module)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var resultModule = context.Module.Add(module).Entity;
                    context.SaveChanges();
                    return resultModule;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }
        public Module Update(Module module)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var moduleActions = module.ModuleAction;
                    module.ModuleAction = null;                  
                    foreach (var moduleAction in moduleActions)
                    {

                        if (context.ModuleAction.Any(pc => pc.Id == moduleAction.Id))
                        {
                            //content exist, therefore update the content 
                            context.ModuleAction.Update(moduleAction);
                        }
                        else
                        {
                            moduleAction.ModuleId = module.Id;
                            context.ModuleAction.Add(moduleAction);
                        } 
                    }

                    var toDelete = context.ModuleAction.Where(dbModuleAction => dbModuleAction.ModuleId == module.Id &&
                    !moduleActions.Any(moduleAction => moduleAction.Id != dbModuleAction.Id)).ToList();

                    context.ModuleAction.RemoveRange(toDelete);

                    var resultModule = context.Module.Update(module).Entity;
                    context.SaveChanges();
                    return resultModule;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }
        
        public ModuleAction CreateModuleAction(ModuleAction moduleAction)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {   
                    var resultModuleAction = context.ModuleAction.Add(moduleAction).Entity;
                    context.SaveChanges();
                    return resultModuleAction;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }
        public ModuleAction UpdateModuleAction(ModuleAction moduleAction)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var resultModuleAction = context.ModuleAction.Attach(moduleAction).Entity;
                    context.Entry(moduleAction).State = EntityState.Modified;
                    context.SaveChanges();
                    return resultModuleAction;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

    }

}//End namespace
