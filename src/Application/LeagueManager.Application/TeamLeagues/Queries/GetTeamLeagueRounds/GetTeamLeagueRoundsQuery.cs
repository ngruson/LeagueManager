using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsQuery : IRequest<GetTeamLeagueRoundsVm>
    {
        public string LeagueName { get; set; }
    }
}