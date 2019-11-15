using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RDS.Framework.Helpers
{
    public static class Extension
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> query, string orderByMember, string defaultMember, bool direction)
        {
            var member = defaultMember;

            if (!string.IsNullOrEmpty(orderByMember))
            {
                var property = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToUpper() == orderByMember.ToUpper());
                if (property != null)
                    member = property.Name;
            }

            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, member);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction ? "OrderByDescending" : "OrderBy",
                new Type[] { typeof(T), memberAccess.Type },
                query.AsQueryable().Expression,
                Expression.Quote(keySelector));

            return query.AsQueryable().Provider.CreateQuery<T>(orderBy);
        }
    }
}
