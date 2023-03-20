using MediatR;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetPermissionsQuery : IRequest<List<string>> { }
}
