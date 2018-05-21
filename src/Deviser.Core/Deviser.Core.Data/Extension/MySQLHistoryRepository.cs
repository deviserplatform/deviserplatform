using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
//using MySql.Data.EntityFrameworkCore;
using System;
using System.Text;

namespace Deviser.Core.Data.Extension
{
    public class MySQLHistoryRepository : HistoryRepository
    {
        protected override string ExistsSql
        {
            get
            {                
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT 1 FROM information_schema.tables ").AppendLine("WHERE table_name = '").Append(this.SqlGenerationHelper.EscapeLiteral(this.TableName)).Append("' AND table_schema = DATABASE()");
                return stringBuilder.ToString();
            }
        }

        public MySQLHistoryRepository([NotNull] HistoryRepositoryDependencies dependencies) : base(dependencies)
        {
        }

        public override string GetBeginIfExistsScript(string migrationId)
        {
            ThrowIf.Argument.IsNull(migrationId, "migrationId");
            return (new StringBuilder()).Append("IF EXISTS(SELECT * FROM ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema)).Append(" WHERE ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName)).Append(" = '").Append(this.SqlGenerationHelper.EscapeLiteral(migrationId)).AppendLine("')").Append("BEGIN").ToString();
        }

        public override string GetBeginIfNotExistsScript(string migrationId)
        {
            ThrowIf.Argument.IsNull(migrationId, "migrationId");
            return (new StringBuilder()).Append("IF NOT EXISTS(SELECT * FROM ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema)).Append(" WHERE ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName)).Append(" = '").Append(this.SqlGenerationHelper.EscapeLiteral(migrationId)).AppendLine("')").Append("BEGIN").ToString();
        }

        public override string GetCreateIfNotExistsScript()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("  IF EXISTS(SELECT 1 FROM information_schema.tables ");
            stringBuilder.AppendLine("  WHERE table_name = '").Append(this.SqlGenerationHelper.EscapeLiteral(this.TableName)).AppendLine("' AND table_schema = DATABASE()) ").AppendLine("BEGIN").AppendLine(this.GetCreateScript()).AppendLine("END;");
            return stringBuilder.ToString();
        }

        public override string GetEndIfScript()
        {
            return String.Concat("END;", Environment.NewLine);
        }

        protected override bool InterpretExistsResult(object value)
        {
            return value != DBNull.Value;
        }
    }

    internal static class ThrowIf
    {
        internal static class Argument
        {
            public static void IsEmpty(string argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
                if (argument.Trim().Length == 0)
                {
                    throw new ArgumentException(String.Format("{0} cannot be empty", argumentName));
                }
            }

            public static void IsNull(object argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class NotNullAttribute : Attribute
    {
        public NotNullAttribute()
        {
        }
    }
}
