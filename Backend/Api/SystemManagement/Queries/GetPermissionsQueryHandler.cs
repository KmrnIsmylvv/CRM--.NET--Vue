using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
	public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, List<string>>
    {
        public Task<List<string>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
        {
			var constantValues = typeof(Permissions).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
								.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();

			var result = new List<string>();
			foreach (var constantValue in constantValues)
			{
				var value = constantValue.GetValue(null);
				if (value != null)
					result.Add(value.ToString());
			}

			return Task.FromResult(result);
		}
    }
}
