using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.Repositories
{
    public static class EFExtensions
    {
  //      public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source,
  //int levelIndex, Expression<Func<TEntity, TEntity>> expression)
  //      {
  //          if (levelIndex < 0)
  //              throw new ArgumentOutOfRangeException(nameof(levelIndex));
  //          var member = (MemberExpression)expression.Body;
  //          var property = member.Member.Name;
  //          var sb = new StringBuilder();
  //          for (int i = 0; i < levelIndex; i++)
  //          {
  //              if (i > 0)
  //                  sb.Append(Type.Delimiter);
  //              sb.Append(property);
  //          }
  //          return source.Where()
  //      }
    }
}
