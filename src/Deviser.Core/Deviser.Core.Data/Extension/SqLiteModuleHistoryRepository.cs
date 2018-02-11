using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deviser.Core.Data.Extension
{
    public class SqliteModuleHistoryRepository : SqliteHistoryRepository
    {
        private readonly string modulePrefix;

        public SqliteModuleHistoryRepository(HistoryRepositoryDependencies dependencies, ICurrentDbContext currentDbContext)
        : base(dependencies)
        {
            var dbContext = currentDbContext.Context as ModuleDbContext;
            modulePrefix = $"_{dbContext.ModuleMetaInfo.ModuleName}";
        }

        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property(h => h.MigrationId).HasColumnName("MigrationId");
        }

        public override IReadOnlyList<HistoryRow> GetAppliedMigrations()
        {
            // Filter applied migrations to ones with the correct tenant prefix
            if (modulePrefix != null)
            {
                return base.GetAppliedMigrations()
                    .Where(r => r.MigrationId.StartsWith(modulePrefix))
                    .Select(r => new HistoryRow(r.MigrationId.Substring(modulePrefix.Length), r.ProductVersion))
                    .ToList();
            }

            return base.GetAppliedMigrations();
        }

        public override string GetInsertScript(HistoryRow row)
        {
            // Add tenant prefix to any new rows
            if (modulePrefix != null)
            {
                row = new HistoryRow(modulePrefix + row.MigrationId, row.ProductVersion);
            }

            return base.GetInsertScript(row);
        }

        public override string GetDeleteScript(string migrationId)
        {
            if (modulePrefix != null)
            {
                return base.GetDeleteScript(modulePrefix + migrationId);
            }
            return base.GetDeleteScript(migrationId);
        }
    }
}
