using Elfo.Round.Identity.Impersonation;
using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Elfo.Round.Identity;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetUserFullNamesToImpersonateQueryHandler : IRequestHandler<GetUserFullNamesToImpersonate.Query, List<GetUserFullNamesToImpersonate.Result>>
    {
        readonly IDbConnection connection;
        readonly IHttpContextAccessor httpContextAccessor;

        public GetUserFullNamesToImpersonateQueryHandler(IDbConnection connection, IHttpContextAccessor httpContextAccessor)
        {
            this.connection = connection;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetUserFullNamesToImpersonate.Result>> Handle(GetUserFullNamesToImpersonate.Query query, CancellationToken cancellationToken)
        {
            var filter = new Filter("isEnabled", RuleOperator.IsEqual, true);
            var impersonatedBy = httpContextAccessor.HttpContext.User.GetClaim(RoundImpersonationClaimType.ImpersonatedBy);
            if (impersonatedBy != null)
                filter *= new Filter("username", RuleOperator.IsNotEqual, impersonatedBy);
            else
                filter *= new Filter("username", RuleOperator.IsNotEqual, httpContextAccessor.HttpContext.User.Identity.Name);

            return await connection.ReadAsync(q => q
                .Where(filter)
                .ToMultiSelectionResultAsync<GetUserFullNamesToImpersonate.Result>(query, "idUser", "fullName"));
        }
    }
}
