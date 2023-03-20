﻿using Elfo.Round.ReadCycle;
using MediatR;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetGroup
    {
        public class Query : IRequest<GetGroup.Result>
        {
            public short IdGroup { get; set; }
        }

        [DataSource("GetGroups", DataSourceType.FileQuery)]
        public class Result
        {
            [Column] public string IdGroup { get; set; }
            [Column] public string Name { get; set; }
            public IEnumerable<int> IdUserList { get; set; }
            public IEnumerable<string> Permissions { get; set; }
        }
    }
}
