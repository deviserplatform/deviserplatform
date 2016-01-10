using System;
using System.Collections.Generic;
using System.Linq;
using Deviser.Core.Data.Entities;
using Microsoft.Extensions.Logging;
using Autofac;
using Microsoft.Data.Entity;
using Module = Deviser.Core.Data.Entities.Module;

namespace Deviser.Core.Data.DataProviders
{
    public interface IModuleProvider
    {
        List<Module> Get();
        Module Get(int moduleId);
        Module Get(string moduleName);
        Module Create(Module module);
        Module Update(Module module);

    }

    public class ModuleProvider : DataProviderBase, IModuleProvider
    {
        //Logger
        private readonly ILogger<LayoutProvider> logger;
        private ILifetimeScope container;

        DeviserDBContext context;

        //Constructor
        public ModuleProvider(ILifetimeScope container)
        {
            this.container = container;
            logger = container.Resolve<ILogger<LayoutProvider>>();
            context = container.Resolve<DeviserDBContext>();
        }

        //Custom Field Declaration
        public List<Module> Get()
        {
            try
            {
                IEnumerable<Module> returnData = context.Module
                        .ToList();

                return new List<Module>(returnData);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public Module Get(int moduleId)
        {
            try
            {
                Module returnData = context.Module
                   .Where(e => e.Id == moduleId)
                   .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) // ("ModuleActions.ModuleActionType")
                   .FirstOrDefault();
                return returnData;
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
                Module returnData = context.Module

                   .Where(e => e.Name == moduleName)
                   .Include(m => m.ModuleAction).ThenInclude(ma => ma.ModuleActionType) //("ModuleActions.ModuleActionType")
                   .FirstOrDefault();

                return returnData;
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
                Module resultModule;
                resultModule = context.Module.Add(module, GraphBehavior.SingleObject).Entity;
                context.SaveChanges();
                return resultModule;
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
                Module resultModule;
                resultModule = context.Module.Attach(module, GraphBehavior.SingleObject).Entity;
                context.Entry(module).State = EntityState.Modified;
                context.SaveChanges();
                return resultModule;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

    }

}//End namespace
