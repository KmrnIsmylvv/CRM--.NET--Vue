using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using System.Linq;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using System;
using Elfo.Round.Identity;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCourses.Query, ListResult<GetCourses.Result>>
    {
        readonly IDbConnection connection;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISystemUserRepository users;
        private readonly ICourseRepository courses;

        public GetCoursesQueryHandler(IDbConnection connection, IHttpContextAccessor httpContextAccessor,
            ISystemUserRepository users, ICourseRepository courses)
        {
            this.connection = connection;
            this.httpContextAccessor = httpContextAccessor;
            this.users = users;
            this.courses = courses;
        }
        public async Task<ListResult<GetCourses.Result>> Handle(GetCourses.Query query, CancellationToken cancellationToken)
        {
            var idUser = httpContextAccessor.HttpContext.User.Claims
                .Single(x => x.Type == RoundIdentityClaimType.UserId).Value;

            if (await users.IsTeacher(new SystemUserID(int.Parse(idUser))))
                query.Filter.Add(new Filter("idTeacher", RuleOperator.IsEqual, idUser));

            return await connection.ReadAsync(q => q
                .Where(query.Filter)
                .Page(query.Paging)
                .OrderBy(query.Sorting?.Fields.ToArray())
                .ToListResultAsync<GetCourses.Result>());

        }

    }
}
