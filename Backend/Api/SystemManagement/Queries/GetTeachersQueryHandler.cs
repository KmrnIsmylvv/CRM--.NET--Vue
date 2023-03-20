using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Round.Identity;
using Elfo.Round.Mvc;
using Elfo.Round.ReadCycle;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetTeachersQueryHandler : IRequestHandler<GetTeachers.Query, List<GetTeachers.Result>>
    {
        readonly IDbConnection connection;
        readonly ISystemUserRepository users;
        readonly IHttpContextAccessor httpContextAccessor;

        public GetTeachersQueryHandler(IDbConnection connection,
            ISystemUserRepository users, IHttpContextAccessor httpContextAccessor)
        {
            this.connection = connection;
            this.users = users;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetTeachers.Result>> Handle(GetTeachers.Query query, CancellationToken cancellationToken)
        {
            var idUser = httpContextAccessor.HttpContext.User.Claims
                .Single(x => x.Type == RoundIdentityClaimType.UserId).Value;

            if (await users.IsTeacher(new SystemUserID(int.Parse(idUser))))
            {
                return await connection.ReadAsync(q => q
                    .Where("idTeacher", RuleOperator.IsEqual, idUser)
                    .ToSelectionResultAsync<GetTeachers.Result>(query, "idTeacher", "teacherName"));
            }

            return await connection.ReadAsync(q => q
             .ToSelectionResultAsync<GetTeachers.Result>(query, "idTeacher", "teacherName"));
        }
    }
}
