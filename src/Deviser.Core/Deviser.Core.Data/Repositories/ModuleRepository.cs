using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Module = Deviser.Core.Common.DomainTypes.Module;

namespace Deviser.Core.Data.Repositories
{
    public interface IModuleRepository //: IRepositoryBase
    {
        List<Module> GetModules();
        Module GetModule(Guid moduleId);
        ModuleView GetModuleView(Guid moduleViewId);
        List<ModuleView> GetModuleViews();
        List<ModuleViewType> GetModuleViewType();
        List<ModuleView> GetEditModuleViews(Guid moduleId);
        Module GetModule(string moduleName);
        Module GetModuleByPageModuleId(Guid pageModuleId);
        Module Create(Module dbModule);
        Module UpdateModule(Module dbModule);
        ModuleView CreateModuleView(ModuleView moduleView);
        ModuleView UpdateModuleView(ModuleView moduleView);
    }

    public class ModuleRepository : IModuleRepository
    {
        //Logger
        private readonly ILogger<ModuleRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public ModuleRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<ModuleRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        //Custom Field Declaration
        public List<Module> GetModules()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Module
                    .Include(m => m.ModuleView).ThenInclude(mp => mp.ModuleViewProperties).ThenInclude(p => p.Property).ThenInclude(p => p.OptionList)
                    .ToList();

                return _mapper.Map<List<Module>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public ModuleView GetModuleView(Guid moduleViewId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.ModuleView
                    .FirstOrDefault(m => m.Id == moduleViewId);

                return _mapper.Map<ModuleView>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Get", ex);
            }
            return null;
        }

        public List<ModuleView> GetModuleViews()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.ModuleView
                    .Include(mp => mp.ModuleViewProperties).ThenInclude(p => p.Property).ThenInclude(p => p.OptionList)
                    .Where(m => m.ModuleViewType.ControlType.ToLower() == "view" && m.Module.IsActive) //Selecting View Actions Only
                    .OrderBy(ma => ma.DisplayName)
                    .ToList();

                return _mapper.Map<List<ModuleView>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ModuleViews", ex);
            }
            return null;
        }

        public List<ModuleViewType> GetModuleViewType()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.ModuleViewType
                    .OrderBy(cd => cd.Id)
                    .ToList();

                return _mapper.Map<List<ModuleViewType>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting Module View Type", ex);
            }
            return null;
        }

        public List<ModuleView> GetEditModuleViews(Guid moduleId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.ModuleView
                    .Include(mp => mp.ModuleViewProperties).ThenInclude(p => p.Property)
                    .Where(m => m.ModuleId == moduleId && m.ModuleViewType.ControlType.ToLower() == "edit") //Selecting View Actions Only
                    .ToList();

                return _mapper.Map<List<ModuleView>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ModuleViews", ex);
            }
            return null;
        }

        public Module GetModule(Guid moduleId)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Module
                    .Where(e => e.Id == moduleId)
                    .Include(m => m.ModuleView).ThenInclude(ma=>ma.ModuleViewProperties).ThenInclude(p=>p.Property)
                    .Include(m => m.ModuleView).ThenInclude(ma => ma.ModuleViewType) // ("ModuleViews.ModuleViewType")                              
                    .FirstOrDefault();

                return _mapper.Map<Module>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Get", ex);
            }
            return null;
        }

        public Module GetModule(string moduleName)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Module
                    .Where(e => e.Name == moduleName)
                    .Include(m => m.ModuleView).ThenInclude(ma => ma.ModuleViewType) //("ModuleViews.ModuleViewType")                              
                    .FirstOrDefault();

                return _mapper.Map<Module>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Module
                    .Where(e => e.PageModule.Any(pm => pm.Id == pageModuleId))
                    .Include(m => m.ModuleView).ThenInclude(ma => ma.ModuleViewType) //("ModuleViews.ModuleViewType")
                    .FirstOrDefault();

                return _mapper.Map<Module>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbModule = _mapper.Map<Entities.Module>(module);
                dbModule.CreatedDate = dbModule.LastModifiedDate = DateTime.Now;
                var result = context.Module.Add(dbModule).Entity;
                context.SaveChanges();
                return _mapper.Map<Module>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }
        public Module UpdateModule(Module module)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbModule = _mapper.Map<Entities.Module>(module);
                var moduleViews = dbModule.ModuleView;
                dbModule.ModuleView = null;
                foreach (var moduleView in moduleViews)
                {
                    if (moduleView.ModuleViewType != null)
                    {
                        moduleView.ModuleViewTypeId = moduleView.ModuleViewType.Id;
                        moduleView.ModuleViewType = null;
                    }
                    

                    if (context.ModuleView.Any(pc => pc.Id == moduleView.Id))
                    {                           

                        UpdateModuleViewProperties(context, moduleView);

                        //content exist, therefore update the content 
                        context.ModuleView.Update(moduleView);
                    }
                    else
                    {
                        moduleView.ModuleId = dbModule.Id;
                        context.ModuleView.Add(moduleView);
                    }
                }
                //No need to delete a row from dbo.ModuleView since the key is referred in dbo.PageModule.

                //var toDelete = context.ModuleView.Where(dbModuleView => dbModuleView.ModuleId == dbModule.Id &&
                //!moduleViews.Any(moduleView => moduleView.Id != dbModuleView.Id)).ToList();

                //context.ModuleView.RemoveRange(toDelete);

                var result = context.Module.Update(dbModule).Entity;
                context.SaveChanges();
                return _mapper.Map<Module>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        public ModuleView CreateModuleView(ModuleView moduleView)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbModuleView = _mapper.Map<Entities.ModuleView>(moduleView);
                var result = context.ModuleView.Add(dbModuleView).Entity;
                context.SaveChanges();
                return _mapper.Map<ModuleView>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Create", ex);
            }
            return null;
        }

        public ModuleView UpdateModuleView(ModuleView moduleView)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbModuleView = _mapper.Map<Entities.ModuleView>(moduleView);
                var result = context.ModuleView.Update(dbModuleView).Entity;

                UpdateModuleViewProperties(context, dbModuleView);

                context.SaveChanges();
                return _mapper.Map<ModuleView>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while calling Update", ex);
            }
            return null;
        }

        private void UpdateModuleViewProperties(DeviserDbContext context, Entities.ModuleView moduleView)
        {   
            if (moduleView.ModuleViewProperties != null && moduleView.ModuleViewProperties.Count > 0)
            {

                var currentTypeProperties = context.ModuleViewProperty.Where(ctp => ctp.ModuleViewId == moduleView.Id).ToList();

                var toRemoveFromClient = moduleView.ModuleViewProperties.Where(clientProp => context.ModuleViewProperty.Any(dbProp =>
                 clientProp.ModuleViewId == dbProp.ModuleViewId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                List<Entities.ModuleViewProperty> toRemoveFromDb = null;

                if (currentTypeProperties.Count > 0)
                {
                    toRemoveFromDb = currentTypeProperties.Where(dbProp => !moduleView.ModuleViewProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                }

                if (toRemoveFromClient.Count > 0)
                {
                    foreach (var contentTypeProp in toRemoveFromClient)
                    {
                        //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                        moduleView.ModuleViewProperties.Remove(contentTypeProp);
                    }
                }

                if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                {
                    //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                    context.ModuleViewProperty.RemoveRange(toRemoveFromDb);
                }


                if (moduleView.ModuleViewProperties != null && moduleView.ModuleViewProperties.Count > 0)
                {
                    foreach (var moduleProp in moduleView.ModuleViewProperties)
                    {
                        moduleProp.Property = null;
                        context.ModuleViewProperty.Add(moduleProp);
                    }
                }
            }
        }

    }

}//End namespace
