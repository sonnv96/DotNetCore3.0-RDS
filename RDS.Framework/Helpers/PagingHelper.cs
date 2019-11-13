using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDS.Framework.Helpers
{
    public class PagingCommand
    {
        public class Response<T>
        {
            /// <summary>
            /// This is Page Size, start from 1, maximum 100
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// This is current Page, start from 1
            /// </summary>
            public int CurrentPage { get; set; }

            /// <summary>
            /// This is total number of pages
            /// </summary>
            public int TotalPages { get; set; }

            /// <summary>
            /// This is total number of items
            /// </summary>
            public int TotalItems { get; set; }

            /// <summary>
            /// This is list returned items
            /// </summary>
            public List<T> Items { get; set; }

            public void SetExample(T item)
            {
                CurrentPage = 1;
                PageSize = 1;
                TotalPages = 10;
                TotalItems = 10;
                Items = new List<T> { item };
            }
        }
    }


    public static class PagingHelper
    {

        public static async Task<PagingCommand.Response<TResult>> Page<TEntity, TResult>(
            IQueryable<TEntity> query,
            int pageIndex, int pageSize,
            Expression<Func<TEntity, TResult>> selector)
        {
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            var totalItems = query.Count();
            return new PagingCommand.Response<TResult>
            {
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems,
                Items = items,
            };
        }

    }
}
