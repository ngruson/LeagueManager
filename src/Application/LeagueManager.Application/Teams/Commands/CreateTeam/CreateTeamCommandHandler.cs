﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.Teams.Commands.CreateTeam
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;

        public CreateTeamCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = context.Teams.SingleOrDefault(x => x.Name == request.Name);
            if (team != null)
                throw new TeamAlreadyExistsException(request.Name);

            Country country = null;
            if (!string.IsNullOrEmpty(request.Country))
            {
                country = await context.Countries.SingleOrDefaultAsync(x => x.Name == request.Country);
            }

            team = new Team
            {
                Name = request.Name,
                Country = country
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}