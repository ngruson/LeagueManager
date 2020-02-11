using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Player.Commands.CreatePlayer
{
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, PlayerDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public CreatePlayerCommandHandler(ILeagueManagerDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PlayerDto> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var newPlayer = mapper.Map<Domain.Player.Player>(request); 
            var player = await context.Players.SingleOrDefaultAsync(x => x.FullName == newPlayer.FullName);
            if (player != null)
                throw new PlayerAlreadyExistsException(player.FullName);

            context.Players.Add(newPlayer);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<PlayerDto>(newPlayer);
        }
    }
}