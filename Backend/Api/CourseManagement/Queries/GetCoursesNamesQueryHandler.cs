using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Round.Identity;
using Elfo.Round.ReadCycle;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCoursesNamesQueryHandler : IRequestHandler<GetCoursesNames.Query, List<GetCoursesNames.Result>>
    {
        readonly IDbConnection connection;
        readonly ISystemUserRepository users;
        readonly IHttpContextAccessor httpContextAccessor;

        public GetCoursesNamesQueryHandler(IDbConnection connection,
            ISystemUserRepository users, IHttpContextAccessor httpContextAccessor)
        {
            this.connection = connection;
            this.users = users;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<GetCoursesNames.Result>> Handle(GetCoursesNames.Query query, CancellationToken cancellationToken)
        {
            var idUser = httpContextAccessor.HttpContext.User.Claims
                .Single(x => x.Type == RoundIdentityClaimType.UserId).Value;

            if (await users.IsTeacher(new SystemUserID(int.Parse(idUser))))
            {
                return await connection.ReadAsync(q => q
                    .Where("idTeacher", RuleOperator.IsEqual, idUser)
                    .ToSelectionResultAsync<GetCoursesNames.Result>(query, "idCourse", "courseName"));
            }

            return await connection.ReadAsync(q => q
                 .ToSelectionResultAsync<GetCoursesNames.Result>(query, "idCourse", "courseName"));
        }
    }
}
