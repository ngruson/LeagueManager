﻿using MediatR;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class GetCompetitionQuery : IRequest<CompetitionDto>
    {
        public string Name { get; set; }
    }
}