using Elfo.Round.ReadCycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
	public static class ReadCycleExtensions
	{
        public static void AddRoundReadCycle(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRoundReadCycle(o => { o.AddFileQueriesAssembly(Assembly.GetExecutingAssembly()); })
                .WithExcelExportation()
                .WithMsSqlServer(o =>
                {
                    o.ConnectionString = configuration.GetConnectionString("Db");
                    o.ProfileConnection = true;
                });
        }

        #region Read

        public static TResult Read<TResult>(this IDbConnection connection, Func<IQueryBuilder, TResult> func)
        {
            TResult result;

            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                result = func(connection.QueryBuilder(trans));
                trans.Commit();
            }

            return result;
        }

        public static async Task<TResult> ReadAsync<TResult>(this IDbConnection connection, Func<IQueryBuilder, Task<TResult>> func)
        {
            TResult result;

            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                result = await func(connection.QueryBuilder(trans));
                trans.Commit();
            }

            return result;
        }

        #endregion

        #region ToListResult

        public static ListResult<T> ToListResult<T>(this IQueryBuilder builder, ListModel model) =>
            builder.Where(model.Filter)
                .Page(model.Paging)
                .OrderBy(model.Sorting?.Fields.ToArray())
                .ToListResult<T>();

        public static Task<ListResult<T>> ToListResultAsync<T>(this IQueryBuilder builder, ListModel model) =>
            builder.Where(model.Filter)
                .Page(model.Paging)
                .OrderBy(model.Sorting?.Fields.ToArray())
                .ToListResultAsync<T>();

        #endregion

        #region ToSelectionResult

        public static List<T> ToSelectionResult<T>(this IQueryBuilder builder, SelectionModel model, string keyColumn, string valueColumn) =>
			model.Key != null
				? builder
					.Where(keyColumn, RuleOperator.IsEqual, model.Key)
					.Take(model.Take ?? 1)
					.ToList<T>()
				: builder
					.Where(valueColumn, RuleOperator.IsLike, model.Term)
					.Take(model.Take ?? 10)
					.ToList<T>();

        public static Task<List<T>> ToSelectionResultAsync<T>(this IQueryBuilder builder, SelectionModel model, string keyColumn, string valueColumn) =>
            model.Key != null
                ? builder
                    .Where(keyColumn, RuleOperator.IsEqual, model.Key)
                    .Take(model.Take ?? 1)
                    .ToListAsync<T>()
                : builder
                    .Where(valueColumn, RuleOperator.IsLike, model.Term)
                    .Take(model.Take ?? 10)
                    .ToListAsync<T>();

        #endregion

        #region ToMultiSelectionResult

        public static List<T> ToMultiSelectionResult<T>(this IQueryBuilder builder, SelectionModel model, string keyColumn, string valueColumn) =>
			model.Keys?.Any() == true
				? builder
					.Where(keyColumn, RuleOperator.In, model.Keys)
					.ToList<T>()
				: builder
					.Where(valueColumn, RuleOperator.IsLike, model.Term)
					.Take(model.Take ?? 10)
					.ToList<T>();

        public static Task<List<T>> ToMultiSelectionResultAsync<T>(this IQueryBuilder builder, SelectionModel model, string keyColumn, string valueColumn) =>
            model.Keys?.Any() == true
                ? builder
                    .Where(keyColumn, RuleOperator.In, model.Keys)
                    .ToListAsync<T>()
                : builder
                    .Where(valueColumn, RuleOperator.IsLike, model.Term)
                    .Take(model.Take ?? 10)
                    .ToListAsync<T>();

        #endregion
    }
}
